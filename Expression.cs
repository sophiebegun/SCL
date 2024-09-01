using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    internal class Expression
    {
        private List<Symbol> symbols;
        private List<Symbol> rpnSymbols;

        public Expression(List<Symbol> symbols)
        {
            this.symbols = symbols;
            this.rpnSymbols = ShuntingYard.ConvertToPostfix(symbols);
        }

        public object Evaluate(Scope s)
        {
            Stack<object> stack = new Stack<object>();

            foreach (var symbol in rpnSymbols)
            {
                if (symbol.Type == SymbolType.NUMBER)
                {
                    stack.Push(Convert.ToDouble(symbol.Value));
                }
                else if (symbol.Type == SymbolType.NAME)
                {
                    //For now assume that the name is a variable
                    stack.Push(s[symbol.Value].Value);
                }
                else if (symbol.Type == SymbolType.TRUE || symbol.Type == SymbolType.FALSE)
                {
                    stack.Push(symbol.Type == SymbolType.TRUE);
                }
                else
                {
                    var rightOperand = stack.Pop();
                    object leftOperand = null;

                    if (symbol.Type != SymbolType.NOT)
                    {
                        leftOperand = stack.Pop();
                    }

                    switch (symbol.Type)
                    {
                        case SymbolType.PLUS:
                            stack.Push((double)leftOperand + (double)rightOperand);
                            break;
                        case SymbolType.MINUS:
                            stack.Push((double)leftOperand - (double)rightOperand);
                            break;
                        case SymbolType.MUL:
                            stack.Push((double)leftOperand * (double)rightOperand);
                            break;
                        case SymbolType.DIV:
                            stack.Push((double)leftOperand / (double)rightOperand);
                            break;
                        case SymbolType.POW:
                            stack.Push(Math.Pow((double)leftOperand, (double)rightOperand));
                            break;
                        case SymbolType.AMP:
                            stack.Push((bool)leftOperand && (bool)rightOperand);
                            break;
                        case SymbolType.PIPE:
                            stack.Push((bool)leftOperand || (bool)rightOperand);
                            break;
                        case SymbolType.NOT:
                            stack.Push(!(bool)rightOperand);
                            break;
                        case SymbolType.EQ:
                            stack.Push((double)leftOperand == (double)rightOperand);
                            break;
                        case SymbolType.NOT_EQ:
                            stack.Push((double)leftOperand != (double)rightOperand);
                            break;
                        case SymbolType.GTE:
                            stack.Push((double)leftOperand >= (double)rightOperand);
                            break;
                        case SymbolType.GT:
                            stack.Push((double)leftOperand > (double)rightOperand);
                            break;
                        case SymbolType.LTE:
                            stack.Push((double)leftOperand <= (double)rightOperand);
                            break;
                        case SymbolType.LT:
                            stack.Push((double)leftOperand < (double)rightOperand);
                            break;
                    }
                }
            }
            
            object result = stack.Pop();
            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var symbol in symbols)
            {
                sb.Append(symbol.Value);

            }

            return " (" + sb.ToString() + ")";
            
        }
    }
}
