using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        public IEnumerable<Lexeme> Lex(string source)
        {
            return new Runner(source).Lex().Where(l => l != null);
        }

        #region Nested type: Lexeme

        public class Lexeme
        {
            public string Source { get; set; }
            public int Position { get; set; }
            public int Length { get; set; }
            public int Line { get; set; }
            public int Column { get; set; }
            public LexemeType Type { get; set; }

            public string Value { get; set; }
        }

        #endregion

        #region Nested type: Runner

        [DebuggerDisplay("{_source.Substring(_pos)}")]
        protected class Runner
        {
            private static readonly string[] Punctuators = new[]
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


            private static readonly Regex MatchOperator = new Regex(string.Join("|",
                                                                                from p in Punctuators
                                                                                orderby p.Length descending
                                                                                select Regex.Escape(p)));


            private static readonly Func<Runner, char, Lexeme>[] _preLookup = new Func<Runner, char, Lexeme>[127];
            private static readonly Func<Runner, char, Lexeme>[] _postLookup = new Func<Runner, char, Lexeme>[127];

            private static readonly Regex MatchDoubleQuotedString =
                new Regex(@"^""([^""\r\n\\]|(\\""|\\'|\\\\|\\b|\\f|\\n|\\r|\\t|\\v|\\0)|(\\x[0-9a-fA-F]{2})|(\\u[0-9a-fA-F]{4})|(\\\r?\n))*""");

            private static readonly Regex MatchSingleQuotedString =
                new Regex(@"'([^'\r\n\\]|(\\""|\\'|\\\\|\\b|\\f|\\n|\\r|\\t|\\v|\\0)|(\\x[0-9a-fA-F]{2})|(\\u[0-9a-fA-F]{4})|(\\\r?\n))*'");

            private static readonly Regex MatchEscapeCharacter = new Regex(@"(\\""|\\'|\\\\|\\b|\\f|\\n|\\r|\\t|\\v|\\0)|(\\x[0-9a-fA-F]{2})|(\\u[0-9a-fA-F]{4})|(\\\r?\n)");
            private readonly string _source;
            private int _column;
            private int _line;
            private int _pos;

            static Runner()
            {
                // Single character operators
                // Notice absence of . since that can be start of a number
                foreach (var op in "()[]{};?:,")
                {
                    _preLookup[op] = (r, c) => r.TryLexSingleCharacterOperator(c);
                }

                // Multi character operators
                // Notice absence of / since that can be the start of a comment
                foreach (var op in "+-*%<>&|^!=")
                {
                    _preLookup[op] = (r, c) => r.TryLexMultiCharacterOperator(c);
                }
                // ...and here comments goes
                _preLookup['/'] = (r, c) => r.TryLexComment(c);


                // Numbers
                // Notice: we also try on '.' which might turn out to be an operator instead of a prefix on a number
                foreach (var d in ".0123456789")
                {
                    _preLookup[d] = (r, c) => r.TryLexNumber(c);
                }

                // Strings
                _preLookup['\''] = (r, c) => r.TryLexSingleQuotedString(c);
                _preLookup['"'] = (r, c) => r.TryLexDoubleQuotedString(c);

                // Fast identification of identifiers
                // This is not complete (an additional ceck is made later) but it covers a wide range of typical cases
                foreach (var prefix in "_$abcdefghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVXYZ")
                {
                    _preLookup[prefix] = (r, c) => r.TryLexIdentifier(c);
                }

                // . wasnt part of a number and / didnt start a comment
                foreach (var op in "./")
                {
                    _postLookup[op] = (r, c) => r.TryLexSingleCharacterOperator(c);
                }
            }

            public Runner(string source)
            {
                _source = source;
            }

            private Lexeme TryLexSingleQuotedString(char c)
            {
                var m = MatchSingleQuotedString.Match(_source, _pos);
                return m.Success ? CreateLexeme(LexemeType.String, UnsecapeString(m.Value), m.Length) : null;
            }

            private Lexeme TryLexDoubleQuotedString(char c)
            {
                var m = MatchDoubleQuotedString.Match(_source, _pos);
                return m.Success ? CreateLexeme(LexemeType.String, UnsecapeString(m.Value), m.Length) : null;
            }

            private Lexeme TryLexMultiCharacterOperator(char c)
            {
                var m = MatchOperator.Match(_source, _pos);
                return m.Success ? CreateLexeme(LexemeType.Operator, m.Length) : null;
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
                    return CreateLexeme(LexemeType.Name, l);
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
                    return CreateLexeme(LexemeType.Number, l);
                }
                return null;
            }

            private Lexeme TryLexSingleCharacterOperator(char c)
            {
                return CreateLexeme(LexemeType.Operator, 1);
            }

            private Lexeme CreateLexeme(LexemeType type, string value, int length)
            {
                var l = new Lexeme
                            {
                                Line = _line,
                                Column = _column,
                                Position = _pos,
                                Type = type,
                                Value = value,
                                Source = _source
                            };
                Advance(length);
                return l;
            }

            private Lexeme CreateLexeme(LexemeType type, int length)
            {
                return CreateLexeme(type, _source.Substring(_pos, length), length);
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

                    yield return TryLookupLex(c, _preLookup)
                                 ?? TryLexIdent(c)
                                 ?? TryLookupLex(c, _postLookup)
                                 ?? LexFail(c);
                }
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
                    return CreateLexeme(LexemeType.Comment, l);
                }

                if (_source[_pos + 1] == '*')
                {
                    var e = _source.IndexOf("*/", _pos + 2);
                    if (e < 0)
                    {
                        return CreateLexeme(LexemeType.Error, "Unterminated comment", _source.Length - _pos);
                    }
                    var l = e + 2 - _pos;
                    return CreateLexeme(LexemeType.Comment, l);
                }
                return null;
            }

            private bool CanReadAtleast(int n)
            {
                return _pos + n < _source.Length;
            }

            private Lexeme TryLexIdent(char c)
            {
                return TryLexIdentifier(c);
            }

            private Lexeme LexFail(char c)
            {
                return CreateLexeme(LexemeType.Error, "Illegal character in input", 1);
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
                    return value;    
                }

                return MatchEscapeCharacter.Replace(value, m =>
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
                                                                               int.Parse(m.Value.Substring(2),
                                                                                         NumberStyles.HexNumber);
                                                                           return c.ToString();
                                                                       case '\r':
                                                                       case '\n':
                                                                           return m.Value.Substring(1);
                                                                   }
                                                                   return m.Value;
                                                               });
            }

            #region Nested type: IdentifierPart

            private static class IdentifierPart
            {
                private static readonly UnicodeCategory[] _unicodeCategories = new[]
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
                    return _unicodeCategories.Contains(char.GetUnicodeCategory(c))
                           || (c == '\u200c') || (c == '\u200d');
                }
            }

            #endregion
        }

        #endregion
    }
}