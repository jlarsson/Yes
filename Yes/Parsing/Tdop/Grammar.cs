using System;
using System.Collections.Generic;

namespace Yes.Parsing.Tdop
{
    public class Grammar<TLexeme, TAst, TAstFactory> : IGrammar<TLexeme, TAst, TAstFactory> where TLexeme : ILexeme
    {
        private readonly Dictionary<string, Rule> _rules = new Dictionary<string, Rule>();

        #region IGrammar<TLexeme,TAst,TAstFactory> Members

        public IRule<TLexeme, TAst, TAstFactory> GetRule(TLexeme lexeme)
        {
            Rule rule;
            return _rules.TryGetValue(GetRuleId(lexeme), out rule) ? rule : null;
            //return _rules[GetRuleId(lexeme)];
        }

        #endregion

        protected virtual string GetRuleId(TLexeme lexeme)
        {
            return lexeme.Id;
        }

        public Rule MakeRule(string id, int bp = 0)
        {
            Rule rule;
            if (!_rules.TryGetValue(id, out rule))
            {
                rule = new Rule
                           {
                               Id = id,
                               Lbp = bp
                           };
                _rules.Add(id, rule);
            }
            else
            {
                rule.Lbp = Math.Max(rule.Lbp, bp);
            }
            return rule;
        }

        public Rule Led(string id, int bp, Func<ITdop<TLexeme, TAst, TAstFactory>, TAst, TAst> led)
        {
            var rule = MakeRule(id, bp);
            rule.Led = led;
            return rule;
        }

        public Rule Nud(string id, Func<ITdop<TLexeme, TAst, TAstFactory>, TLexeme, TAst> nud)
        {
            var rule = MakeRule(id);
            rule.Nud = nud;
            return rule;
        }

        public Rule Std(string id, Func<ITdop<TLexeme, TAst, TAstFactory>, TAstFactory, TAst> reduce)
        {
            var rule = MakeRule(id);
            rule.Std = p => reduce(p, p.Factory);
            return rule;
        }

        public Rule Literal(string id, Func<TAstFactory, TLexeme, TAst> reduce)
        {
            var rule = MakeRule(id);
            rule.Nud = (p, l) => reduce(p.Factory, l);
            return rule;
        }

        public Rule Prefix(int bp, string id, Func<TAstFactory, TAst, TAst> reduce)
        {
            return Prefix(id, (p, l) => reduce(p.Factory, p.Expression(bp)));
        }

        public Rule Prefix(string id, Func<ITdop<TLexeme, TAst, TAstFactory>, TLexeme, TAst> nud)
        {
            var rule = MakeRule(id);
            rule.Nud = nud;
            return rule;
        }

        public Rule Infix(int bp, string id, Func<TAstFactory, TAst, TAst, TAst> reduce)
        {
            var rule = MakeRule(id, bp);
            rule.Led = (p, left) =>
                           {
                               var second = p.Expression(bp);
                               return reduce(p.Factory, left, second);
                           };
            return rule;
        }

        public Rule InfixR(string id, int bp, Func<TAstFactory, TAst, TAst, TAst> reduce)
        {
            var rule = MakeRule(id, bp);
            rule.Led = (p, left) =>
            {
                var second = p.Expression(bp-1);
                return reduce(p.Factory, left, second);
            };
            return rule;
        }

        #region Nested type: Rule

        public class Rule : IRule<TLexeme, TAst, TAstFactory>
        {
            #region IRule<TLexeme,TAst,TAstFactory> Members

            public string Id { get; set; }
            public int Lbp { get; set; }
            public Func<ITdop<TLexeme, TAst, TAstFactory>, TAst, TAst> Led { get; set; }
            public Func<ITdop<TLexeme, TAst, TAstFactory>, TLexeme, TAst> Nud { get; set; }
            public Func<ITdop<TLexeme, TAst, TAstFactory>, TAst> Std { get; set; }

            #endregion
        }

        #endregion
    }
}