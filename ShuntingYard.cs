using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    internal class ShuntingYard
    {
        public static List<Symbol> ConvertToPostfix(List<Symbol> infix)
        {
            Stack<Symbol> stack = new Stack<Symbol>();
            List<Symbol> output = new List<Symbol>();
            bool expectFunction = false;

            foreach (var symbol in infix)
            {
                switch (symbol.Type)
                {
                    case SymbolType.NUMBER:
                    case SymbolType.NAME:
                        output.Add(symbol);
                        expectFunction = true;
                        break;

                    case SymbolType.PAREN_START:
                    case SymbolType.BRACK_START:
                        stack.Push(symbol);
                        expectFunction = false;
                        break;

                    case SymbolType.PAREN_END:
                    case SymbolType.BRACK_END:
                        while (stack.Count > 0 && stack.Peek().Type != SymbolType.PAREN_START && stack.Peek().Type != SymbolType.BRACK_START)
                        {
                            output.Add(stack.Pop());
                        }
                        if (stack.Count > 0 && (stack.Peek().Type == SymbolType.PAREN_START || stack.Peek().Type == SymbolType.BRACK_START))
                        {
                            stack.Pop();
                        }
                        if (stack.Count > 0 && stack.Peek().Type == SymbolType.NAME && expectFunction)
                        {
                            output.Add(stack.Pop());
                        }
                        break;

                    case SymbolType.COM:
                        while (stack.Count > 0 && stack.Peek().Type != SymbolType.PAREN_START && stack.Peek().Type != SymbolType.BRACK_START)
                        {
                            output.Add(stack.Pop());
                        }
                        break;
                    case SymbolType.POW:
                    case SymbolType.MUL:
                    case SymbolType.DIV:
                    case SymbolType.PLUS:
                    case SymbolType.MINUS:
                        while (stack.Count > 0 && Precedence(stack.Peek().Type) >= Precedence(symbol.Type))
                        {
                            output.Add(stack.Pop());
                        }
                        stack.Push(symbol);
                        expectFunction = false;
                        break;
                }
            }

            while (stack.Count > 0)
            {
                output.Add(stack.Pop());
            }

            return output;
        }

        private static int Precedence(SymbolType type)
        {
            switch (type)
            {
                case SymbolType.PLUS:
                case SymbolType.MINUS:
                    return 1;
                case SymbolType.MUL:
                case SymbolType.DIV:
                    return 2;
                case SymbolType.POW:
                    return 3;
                default:
                    return 0;
            }
        }
    }

}
