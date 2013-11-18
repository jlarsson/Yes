using System.Collections.Generic;

namespace Yes.Parsing
{
    public class JavascriptLexemeMapper: ILexemeMapper
    {
        private static readonly HashSet<string> Keywords = new HashSet<string>()
                                                {
                                                    "return",
                                                    "var",
                                                    "if",
                                                    "else",
                                                    "for",
                                                    "while",
                                                    "break",
                                                    "continue",
                                                    "function",
                                                    "new",
                                                    "in",
                                                    "delete",
                                                    "void",
                                                    "instanceof"
                                                };
        public string CommentId(string text)
        {
            return "(comment)";
        }

        public string OperatorId(string text)
        {
            return text;
        }

        public string ErrorId(string text)
        {
            return "(error)";
        }

        public string NumberId(string text)
        {
            return "(number)";
        }

        public string NameId(string text)
        {
            return Keywords.Contains(text) ? text : "(name)";
        }

        public string StringId(string text)
        {
            return "(string)";
        }
    }
}