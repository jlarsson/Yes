using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Yes.Parsing.Tdop;

namespace Yes.Parsing
{
    public class JavascriptLexer
    {
        #region LexemeType enum

        public enum LexemeType
        {
            Comment,
            Operator,
            Error,
            Number,
            Name,
            String
        }

        #endregion

        public IEnumerable<Lexeme> Lex(string source, ILexemeMapper lexemeMapper = null)
        {
            return new Runner(source, lexemeMapper ?? new JavascriptLexemeMapper()).Lex().Where(l => l != null);
        }

        #region Nested type: Lexeme

        [DebuggerDisplay("{Id} '{Text}'")]
        public class Lexeme: ILexeme
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public object Value { get; set; }
            public string Source { get; set; }
            public int Position { get; set; }
            public int Length { get; set; }
            public int Line { get; set; }
            public int Column { get; set; }
        }

        #endregion

        public class Trie<TChar, TValue>
        {
            private class Node
            {
                public TChar Char { get; set; }
                public TValue Value { get; set; }
                public Dictionary<TChar, Node> Children { get; protected set; }

                public Node()
                {
                    Children = new Dictionary<TChar, Node>();
                }
            }

            private readonly Node _root = new Node();

            public void Add(IEnumerable<TChar> path, TValue value)
            {
                var node = _root;
                foreach (var c in path)
                {
                    Node childNode;
                    if (!node.Children.TryGetValue(c, out childNode))
                    {
                        childNode = new Node {Char = c};
                        node.Children.Add(c, childNode);
                    }
                    node = childNode;
                }
                node.Value = value;
            }

            public TValue Find(IEnumerable<TChar> path)
            {
                var node = _root;
                foreach (var c in path)
                {
                    Node childNode;
                    if (!node.Children.TryGetValue(c, out childNode))
                    {
                        break;
                    }
                    node = childNode;
                }
                return node.Value;
            }
        }

        #region Nested type: Runner

        [DebuggerDisplay("{_source.Substring(_pos)}")]
        protected class Runner
        {
            private static readonly string[] Operators = new[]
                                                             {
                                                                 "=",
                                                                 "{", "}", "(", ")", "[", "]",
                                                                 ".", ";", ",", "<", ">", "<=",
                                                                 ">=", "==", "!=", "===", "!==",
                                                                 "+", "-", "*", "%", "++", "--",
                                                                 "<<", ">>", ">>>", "&", "|", "^",
                                                                 "!", "~", "&&", "||", "?", ":",
                                                                 "=", "+=", "-=", "*=", "%=", "<<=",
                                                                 ">>=", ">>>=", "&=", "|=", "^=",
                                                                 "/", "/=",
                                                                 ","
                                                             };

            private static readonly Trie<char, string> OperatorTrie = new Trie<char, string>();

            private static readonly Func<Runner, char, Lexeme>[] PreLookup = new Func<Runner, char, Lexeme>[127];
            private static readonly Func<Runner, char, Lexeme>[] PostLookup = new Func<Runner, char, Lexeme>[127];

            private static readonly Regex MatchDoubleQuotedString =
                new Regex(
                    @"^""([^""\r\n\\]|(\\""|\\'|\\\\|\\b|\\f|\\n|\\r|\\t|\\v|\\0)|(\\x[0-9a-fA-F]{2})|(\\u[0-9a-fA-F]{4})|(\\\r?\n))*""");

            private static readonly Regex MatchSingleQuotedString =
                new Regex(
                    @"'([^'\r\n\\]|(\\""|\\'|\\\\|\\b|\\f|\\n|\\r|\\t|\\v|\\0)|(\\x[0-9a-fA-F]{2})|(\\u[0-9a-fA-F]{4})|(\\\r?\n))*'");

            private static readonly Regex MatchEscapeCharacter =
                new Regex(
                    @"(\\""|\\'|\\\\|\\b|\\f|\\n|\\r|\\t|\\v|\\0)|(\\x[0-9a-fA-F]{2})|(\\u[0-9a-fA-F]{4})|(\\\r?\n)");

            private readonly string _source;
            private readonly ILexemeMapper _lexemeMapper;
            private int _column;
            private int _line;
            private int _pos;

            static Runner()
            {
                foreach (var @operator in Operators)
                {
                    OperatorTrie.Add(@operator, @operator);
                    PreLookup[@operator[0]] = (r, c) => r.TryLexOperator();
                }
                // '/' might be start of comment...
                PreLookup['/'] = (r, c) => r.TryLexComment(c) ?? r.TryLexOperator();

                // Numbers
                // Notice: we also try on '.' which might turn out to be an operator instead of a prefix on a number
                foreach (var d in ".0123456789")
                {
                    PreLookup[d] = (r, c) => r.TryLexNumber(c);
                }

                // Strings
                PreLookup['\''] = (r, c) => r.TryLexSingleQuotedString();
                PreLookup['"'] = (r, c) => r.TryLexDoubleQuotedString();

                // Fast identification of identifiers
                // This is not complete (an additional ceck is made later) but it covers a wide range of typical cases
                foreach (var prefix in "_$abcdefghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVXYZ")
                {
                    PreLookup[prefix] = (r, c) => r.TryLexIdentifier(c);
                }

                // '.' might be operator and not part of number
                foreach (var op in ".")
                {
                    PostLookup[op] = (r, c) => r.TryLexSingleCharacterOperator();
                }
            }

            public Runner(string source, ILexemeMapper lexemeMapper)
            {
                _source = source;
                _lexemeMapper = lexemeMapper;
            }

            private Lexeme TryLexOperator()
            {
                var v = OperatorTrie.Find(
                    Enumerable.Range(0, _source.Length - _pos).Select(i => _source[_pos + i])
                    );
                if (v != null)
                {
                    return CreateLexeme(_lexemeMapper.OperatorId, v.Length);
                }
                return null;
            }

            private Lexeme TryLexComment(char c)
            {
                if (c != '/')
                {
                    return null;
                }
                if (!CanReadAtleast(2))
                {
                    return null;
                }
                if (_source[_pos + 1] == '/')
                {
                    // Single line comment
                    var e = _source.IndexOf('\n', _pos + 2);
                    if (e < 0)
                    {
                        e = _source.Length;
                    }
                    var l = e - _pos;
                    return CreateLexeme(_lexemeMapper.CommentId, l);
                }

                if (_source[_pos + 1] == '*')
                {
                    var e = _source.IndexOf("*/", _pos + 2, StringComparison.Ordinal);
                    if (e < 0)
                    {
                        return CreateLexeme(_lexemeMapper.ErrorId, "Unterminated comment", _source.Length - _pos);
                    }
                    var l = e + 2 - _pos;
                    return CreateLexeme(_lexemeMapper.CommentId, l);
                }
                return null;
            }

            private Lexeme TryLexSingleQuotedString()
            {
                var m = MatchSingleQuotedString.Match(_source, _pos);
                return m.Success ? CreateLexeme(_lexemeMapper.StringId, UnsecapeString(m.Value), m.Length) : null;
            }

            private Lexeme TryLexDoubleQuotedString()
            {
                var m = MatchDoubleQuotedString.Match(_source, _pos);
                return m.Success ? CreateLexeme(_lexemeMapper.StringId, UnsecapeString(m.Value), m.Length) : null;
            }

            private Lexeme TryLexIdentifier(char c)
            {
                if (char.IsLetter(c) || (c == '$' || (c == '_')))
                {
                    var p = _pos + 1;
                    while ((p < _source.Length) && IdentifierPart.IsIdentifierPart(_source[p]))
                    {
                        ++p;
                    }
                    var l = p - _pos;
                    return CreateLexeme(_lexemeMapper.NameId, l);
                }
                return null;
            }

            private Lexeme TryLexNumber(char c)
            {
                var p = _pos;

                while ((p < _source.Length) && char.IsDigit(_source[p]))
                {
                    ++p;
                }
                if ((p < _source.Length) && ('.' == _source[p]))
                {
                    ++p;

                    while ((p < _source.Length) && char.IsDigit(_source[p]))
                    {
                        ++p;
                    }
                }

                var e = p;
                if ((e < _source.Length) && (('e' == _source[e]) || ('E' == _source[e])))
                {
                    ++e;
                    if ((e < _source.Length) && (('+' == _source[e]) || ('-' == _source[e])))
                    {
                        ++e;
                    }
                    while ((e < _source.Length) && char.IsDigit(_source[e]))
                    {
                        ++e;
                        p = e;
                    }
                }

                var l = p - _pos;

                if ((l > 1) || char.IsDigit(c))
                {
                    return CreateLexeme(_lexemeMapper.NumberId, l);
                }
                return null;
            }

            private Lexeme TryLexSingleCharacterOperator()
            {
                return CreateLexeme(_lexemeMapper.OperatorId, 1);
            }

            private Lexeme CreateLexeme(Func<string, string> getId, string value, int length)
            {
                var l = new Lexeme
                            {
                                Id = getId(value),
                                Text = value,
                                Source = _source,
                                Position = _pos,
                                Length = length,
                                Line = _line,
                                Column = _column
                            };
                Advance(length);
                return l;
            }

            private Lexeme CreateLexeme(Func<string,string> getId, int length)
            {
                return CreateLexeme(getId, _source.Substring(_pos, length), length);
            }

            private void Advance(int count)
            {
                while (count > 0)
                {
                    var c = _source[_pos++];
                    if (c == '\n')
                    {
                        ++_line;
                        _column = 0;
                    }
                    else
                    {
                        _column = 0;
                    }
                    --count;
                }
            }

            public IEnumerable<Lexeme> Lex()
            {
                while (_pos < _source.Length)
                {
                    var c = _source[_pos];
                    if (char.IsWhiteSpace(c))
                    {
                        Advance(1);
                        continue;
                    }

                    yield return TryLookupLex(c, PreLookup)
                                 ?? TryLexIdent(c)
                                 ?? TryLookupLex(c, PostLookup)
                                 ?? LexFail();
                }
            }

            private bool CanReadAtleast(int n)
            {
                return _pos + n < _source.Length;
            }

            private Lexeme TryLexIdent(char c)
            {
                return TryLexIdentifier(c);
            }

            private Lexeme LexFail()
            {
                return CreateLexeme(_lexemeMapper.ErrorId, "Illegal character in input", 1);
            }

            private Lexeme TryLookupLex(char c, Func<Runner, char, Lexeme>[] lookup)
            {
                if (c >= lookup.Length)
                {
                    return null;
                }
                var f = lookup[c];
                if (f == null)
                {
                    return null;
                }
                return f(this, c);
            }

            private string UnsecapeString(string value)
            {
                var escapePos = value.IndexOf('\\');
                if (escapePos < 0)
                {
                    // No escapes, fast bail out
                    return value.Substring(1, value.Length - 2);
                }

                return MatchEscapeCharacter.Replace(value.Substring(1, value.Length - 2), m =>
                                                                                              {
                                                                                                  switch (m.Value[1])
                                                                                                  {
                                                                                                      case '"':
                                                                                                          return "\"";
                                                                                                      case '\'':
                                                                                                          return "'";
                                                                                                      case '\\':
                                                                                                          return "\\";
                                                                                                      case 'b':
                                                                                                          return "\b";
                                                                                                      case 'f':
                                                                                                          return "\f";
                                                                                                      case 'n':
                                                                                                          return "\n";
                                                                                                      case 'r':
                                                                                                          return "\r";
                                                                                                      case 't':
                                                                                                          return "\t";
                                                                                                      case 'v':
                                                                                                          return "\v";
                                                                                                      case '0':
                                                                                                          return "\0";
                                                                                                      case 'x':
                                                                                                      case 'u':
                                                                                                          var c =
                                                                                                              (char)
                                                                                                              int.Parse(
                                                                                                                  m.
                                                                                                                      Value
                                                                                                                      .
                                                                                                                      Substring
                                                                                                                      (2),
                                                                                                                  NumberStyles
                                                                                                                      .
                                                                                                                      HexNumber);
                                                                                                          return
                                                                                                              c.ToString();
                                                                                                      case '\r':
                                                                                                      case '\n':
                                                                                                          return
                                                                                                              m.Value.
                                                                                                                  Substring
                                                                                                                  (1);
                                                                                                  }
                                                                                                  return m.Value;
                                                                                              });
            }

            #region Nested type: IdentifierPart

            private static class IdentifierPart
            {
                private static readonly UnicodeCategory[] UnicodeCategories = new[]
                                                                                   {
                                                                                       UnicodeCategory.UppercaseLetter,
                                                                                       UnicodeCategory.LowercaseLetter,
                                                                                       UnicodeCategory.TitlecaseLetter,
                                                                                       UnicodeCategory.ModifierLetter,
                                                                                       UnicodeCategory.OtherLetter,
                                                                                       UnicodeCategory.LetterNumber,
                                                                                       UnicodeCategory.NonSpacingMark,
                                                                                       UnicodeCategory.
                                                                                           SpacingCombiningMark,
                                                                                       UnicodeCategory.
                                                                                           DecimalDigitNumber,
                                                                                       UnicodeCategory.
                                                                                           ConnectorPunctuation
                                                                                   };

                public static bool IsIdentifierPart(char c)
                {
                    return UnicodeCategories.Contains(char.GetUnicodeCategory(c))
                           || (c == '\u200c') || (c == '\u200d');
                }
            }

            #endregion
        }

        #endregion
    }
}