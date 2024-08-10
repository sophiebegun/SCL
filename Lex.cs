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

        //S int hello = 1223
        public List<Symbol> AnalyzeLine(string line)
        {
            List<Symbol> result = new List<Symbol>();

            //Check for Pipe
            string[] arPipe = line.Split('|');



            //string[] ar = arPipe[0].Split(' ');
            //foreach (string s in ar)
            //{

            //    Symbol sym = Map(s);
            //    result.Add(sym);  
            //}


            int i=0;

            while (i < line.Length)
            {
                if (char.IsWhiteSpace(line[i]))
                {
                    i++;
                    continue;
                }

                string token = line[i].ToString();

                if (char.IsLetter(line[i]))
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
                    Symbol s = Map(token);
                    result.Add(s);
                }
                i++;
            }


            //This means there is a constant after the pipe
            if (arPipe.Length > 1)
            {
                result.Add(new Symbol(SymbolType.CONST, arPipe[1]));
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
                case "S": return new Symbol(SymbolType.S);
                case "C": return new Symbol(SymbolType.C);
                case "L": return new Symbol(SymbolType.L);
                case "F": return new Symbol(SymbolType.F);
                case "I": return new Symbol(SymbolType.I);
                case "O": return new Symbol(SymbolType.O);
                case "int": return new Symbol(SymbolType.DT_INT);
                case "str": return new Symbol(SymbolType.DT_STR);
                case "hset": return new Symbol(SymbolType.DT_SET);
                case "lst": return new Symbol(SymbolType.DT_LST);
                case "hmap": return new Symbol(SymbolType.DT_MAP);
                case "bool": return new Symbol(SymbolType.DT_BOOL);

                case "^": return new Symbol(SymbolType.POW);
                case "*": return new Symbol(SymbolType.MUL);
                case "/": return new Symbol(SymbolType.DIV);
                case "+": return new Symbol(SymbolType.PLUS);
                case "-": return new Symbol(SymbolType.MINUS);
                case "(": return new Symbol(SymbolType.PAREN_START);
                case ")": return new Symbol(SymbolType.PAREN_END);
                case "[": return new Symbol(SymbolType.BRACK_START);
                case "]": return new Symbol(SymbolType.BRACK_END);

                case ",": return new Symbol(SymbolType.COM);
                case "=": return new Symbol(SymbolType.EQ);
                case "==": return new Symbol(SymbolType.COMP);
                case ">=": return new Symbol(SymbolType.GTE);
                case "<=": return new Symbol(SymbolType.LTE);
                case "<": return new Symbol(SymbolType.LT);
                case ">": return new Symbol(SymbolType.GT);
                case "true": return new Symbol(SymbolType.TRUE);
                case "false": return new Symbol(SymbolType.FALSE);
                case "~": return new Symbol(SymbolType.SWIGGLE);
                case "#": return new Symbol(SymbolType.HASHTAG);
                case ":": return new Symbol(SymbolType.COLON);
                case "!": return new Symbol(SymbolType.NOT);
                case "{": return new Symbol(SymbolType.BRACE_START);
                case "}": return new Symbol(SymbolType.BRACE_END);

            }

            return new Symbol(SymbolType.NAME, s);
        }

    }
}
