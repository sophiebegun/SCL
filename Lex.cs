using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    internal class Lex
    {
        public List<Symbol> Analyze(string source)
        {
            List<Symbol> result = new List<Symbol>();

            //Break down source into separate lines
            using StringReader sReader = new StringReader(source);

            while (true)
            {
                string line = sReader.ReadLine();
                if(line == null)
                    break;

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                List<Symbol> lineSymbols = AnalyzeLine(line);
                result.AddRange(lineSymbols);
            }
           
            return result;
        }

        private Symbol ParseDoubleToken(string line, int i)
        {
            //If at last symbol then return null;
            if (i == line.Length - 1)
                return null;

            string s = line[i].ToString() + line[i + 1].ToString();

            switch(s)
            {
               case "==": return new Symbol(SymbolType.COMP, s);
               case "!=": return new Symbol(SymbolType.NOT_EQ, s);
               case ">=": return new Symbol(SymbolType.GTE, s);
               case "<=": return new Symbol(SymbolType.LTE, s);
               default: return null;
            }

        }

        //S int hello = 1223

        public List<Symbol> AnalyzeLine(string line)
        {
            List<Symbol> result = new List<Symbol>();

            int i = 0;

            while (i < line.Length)
            {
                if (char.IsWhiteSpace(line[i]))
                {
                    i++;
                    continue;
                }

                if (line[i] == '"')
                {
                    StringBuilder sb = new StringBuilder();
                    i++; // Skip the opening double quote

                    while (i < line.Length && line[i] != '"')
                    {
                        sb.Append(line[i]);
                        i++;
                    }

                    if (i < line.Length && line[i] == '"')
                    {
                        i++; // Skip the closing double quote
                    }

                    result.Add(new Symbol(SymbolType.CONST, sb.ToString()));
                }
                else if (char.IsLetter(line[i]))
                {
                    StringBuilder sb = new StringBuilder();
                    while (i < line.Length && (char.IsLetter(line[i]) || char.IsDigit(line[i])))
                    {
                        sb.Append(line[i]);
                        i++;
                    }

                    Symbol s = Map(sb.ToString());
                    result.Add(s);
                    i--; // Adjust for the outer loop increment
                }
                else if (char.IsDigit(line[i]) || line[i] == '.')
                {
                    StringBuilder sb = new StringBuilder();
                    while (i < line.Length && (char.IsDigit(line[i]) || line[i] == '.'))
                    {
                        sb.Append(line[i]);
                        i++;
                    }
                    result.Add(new Symbol(SymbolType.NUMBER, sb.ToString()));
                    i--; // Adjust for the outer loop increment
                }
                else
                {
                    Symbol dt = ParseDoubleToken(line, i);
                    if (dt != null)
                    {
                        i++;
                        result.Add(dt);
                    }
                    else
                    {
                        string token = line[i].ToString();
                        Symbol s = Map(token);
                        result.Add(s);
                    }
                }
                i++;
            }

            result.Add(new Symbol(SymbolType.EOL));

            return result;
        }

        private Symbol Map(string s)
        {
            
            //if (int.TryParse(s, out int n))
            //    return new Symbol(SymbolType.NUMBER, n.ToString());

            switch (s)
            {
                case "S": return new Symbol(SymbolType.S, s);
                case "C": return new Symbol(SymbolType.C, s);
                case "L": return new Symbol(SymbolType.L, s);
                case "F": return new Symbol(SymbolType.F, s);
                case "I": return new Symbol(SymbolType.I, s);
                case "O": return new Symbol(SymbolType.O, s);
                case "int": return new Symbol(SymbolType.DT_INT, s);
                case "double": return new Symbol(SymbolType.DT_DOUBLE, s);
                case "str": return new Symbol(SymbolType.DT_STR, s);
                case "hset": return new Symbol(SymbolType.DT_SET, s);
                case "lst": return new Symbol(SymbolType.DT_LST, s);
                case "hmap": return new Symbol(SymbolType.DT_MAP, s);
                case "bool": return new Symbol(SymbolType.DT_BOOL, s);

                case "^": return new Symbol(SymbolType.POW, s);
                case "*": return new Symbol(SymbolType.MUL, s);
                case "/": return new Symbol(SymbolType.DIV, s);
                case "+": return new Symbol(SymbolType.PLUS, s);
                case "-": return new Symbol(SymbolType.MINUS, s);
                case "(": return new Symbol(SymbolType.PAREN_START, s);
                case ")": return new Symbol(SymbolType.PAREN_END, s);
                case "[": return new Symbol(SymbolType.BRACK_START, s);
                case "]": return new Symbol(SymbolType.BRACK_END, s);

                case ",": return new Symbol(SymbolType.COM, s);
                case "=": return new Symbol(SymbolType.EQ, s);
                case "<": return new Symbol(SymbolType.LT, s);
                case ">": return new Symbol(SymbolType.GT, s);
                case "true": return new Symbol(SymbolType.TRUE, s);
                case "false": return new Symbol(SymbolType.FALSE, s);
                case "~": return new Symbol(SymbolType.SWIGGLE, s);
                case "#": return new Symbol(SymbolType.HASHTAG, s);
                case ":": return new Symbol(SymbolType.COLON, s);
                case "!": return new Symbol(SymbolType.NOT, s);
                case "|": return new Symbol(SymbolType.PIPE, s);
                case "&": return new Symbol(SymbolType.AMP, s);
                case "{": return new Symbol(SymbolType.BRACE_START, s);
                case "}": return new Symbol(SymbolType.BRACE_END, s);
 
            }

            return new Symbol(SymbolType.NAME, s);
        }

    }
}
