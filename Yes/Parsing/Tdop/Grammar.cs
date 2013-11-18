using System;
using System.Collections.Generic;

namespace Yes.Parsing.Tdop
{
    public class Grammar<TLexeme, TAst, TAstFactory, TState> : IGrammar<TLexeme, TAst, TAstFactory, TState> where TLexeme : ILexeme
    {
        private readonly Dictionary<string, Rule> _rules = new Dictionary<string, Rule>();

        #region IGrammar<TLexeme,TAst,TAstFactory> Members

        public IRule<TLexeme, TAst, TAstFactory, TState> GetRule(TLexeme lexeme)
        {
            Rule rule;
            return _rules.TryGetValue(GetRuleId(lexeme), out rule) ? rule : null;
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

        public Rule Led(string id, int bp, Func<TState, ITdop<TLexeme, TAst, TAstFactory, TState>, TAst, TAst> led)
        {
            var rule = MakeRule(id, bp);
            rule.Led = led;
            return rule;
        }

        public Rule Nud(string id, Func<TState, ITdop<TLexeme, TAst, TAstFactory, TState>, TLexeme, TAst> nud)
        {
            var rule = MakeRule(id);
            rule.Nud = nud;
            return rule;
        }

        public Rule Std(string id, Func<TState, ITdop<TLexeme, TAst, TAstFactory, TState>, TAstFactory, TAst> reduce)
        {
            var rule = MakeRule(id);
            rule.Std = (state, p) => reduce(state, p, p.Factory);
            return rule;
        }

        public Rule Literal(string id, Func<TState, TAstFactory, TLexeme, TAst> reduce)
        {
            var rule = MakeRule(id);
            rule.Nud = (state, p, l) => reduce(state, p.Factory, l);
            return rule;
        }

        public Rule Prefix(int bp, string id, Func<TState, TAstFactory, TAst, TAst> reduce)
        {
            return Prefix(id, (state, p, l) => reduce(state, p.Factory, p.Expression(state, bp)));
        }

        public Rule Prefix(string id, Func<TState, ITdop<TLexeme, TAst, TAstFactory, TState>, TLexeme, TAst> nud)
        {
            var rule = MakeRule(id);
            rule.Nud = nud;
            return rule;
        }

        public Rule Infix(int bp, string id, Func<TState, TAstFactory, TAst, TAst, TAst> reduce)
        {
            var rule = MakeRule(id, bp);
            rule.Led = (state, p, left) =>
                           {
                               var second = p.Expression(state, bp);
                               return reduce(state, p.Factory, left, second);
                           };
            return rule;
        }

        public Rule InfixR(string id, int bp, Func<TState, TAstFactory, TAst, TAst, TAst> reduce)
        {
            var rule = MakeRule(id, bp);
            rule.Led = (state, p, left) =>
            {
                var second = p.Expression(state, bp-1);
                return reduce(state, p.Factory, left, second);
            };
            return rule;
        }

        #region Nested type: Rule

        public class Rule : IRule<TLexeme, TAst, TAstFactory, TState>
        {
            #region IRule<TLexeme,TAst,TAstFactory> Members

            public string Id { get; set; }
            public int Lbp { get; set; }
            public Func<TState, ITdop<TLexeme, TAst, TAstFactory, TState>, TAst, TAst> Led { get; set; }
            public Func<TState, ITdop<TLexeme, TAst, TAstFactory, TState>, TLexeme, TAst> Nud { get; set; }
            public Func<TState, ITdop<TLexeme, TAst, TAstFactory, TState>, TAst> Std { get; set; }

            #endregion
        }

        #endregion
    }
}