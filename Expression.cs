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
            Stack<double> stack = new Stack<double>();
            foreach (var symbol in rpnSymbols)
            {
                if (symbol.Type == SymbolType.NUMBER)
                {
                    stack.Push(Convert.ToDouble(symbol.Value));
                }
                else
                {
                    double rightOperand = stack.Pop();
                    double leftOperand = stack.Pop();

                    switch (symbol.Type)
                    {
                        case SymbolType.PLUS:
                            stack.Push(leftOperand + rightOperand);
                            break;
                        case SymbolType.MINUS:
                            stack.Push(leftOperand - rightOperand);
                            break;
                        case SymbolType.MUL:
                            stack.Push(leftOperand * rightOperand);
                            break;
                        case SymbolType.DIV:
                            stack.Push(leftOperand / rightOperand);
                            break;
                        case SymbolType.POW:
                            stack.Push(Math.Pow(leftOperand, rightOperand));
                            break;
                    }
                }
            }
            double result = stack.Pop();
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
