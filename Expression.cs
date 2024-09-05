using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SCL
{
    internal class Expression
    {
        public List<Symbol> Symbols {get;set;}


        private List<Symbol> rpnSymbols;

        public Expression(List<Symbol> symbols)
        {
            this.Symbols = symbols;
            this.rpnSymbols = ShuntingYard.ConvertToPostfix(symbols);
        }

        public object Evaluate(Scope s, Dictionary<string, ASTNode> fds)
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
                    //This means that this name is a function, not a variable
                    if (fds.ContainsKey(symbol.Value))
                    {
                        string func = symbol.Value;
                        ASTNode fdNode = fds[func];

                        int argCount = fdNode.Parameters.Count;


                        Scope new_scope = new Scope();

                        // Pop arguments from stack in reverse order (right to left)
                        for (int i = argCount - 1; i >= 0; i--)
                        {
                            string name = fdNode.Parameters[i].Name;
                            object val = stack.Pop();
                            Var v = new Var(name, fdNode.Parameters[i].Type, val);
                            new_scope.Add(name, v);
                            
                        }

                        var func_result = fdNode.Evaluate(new_scope, fds);
                        stack.Push(func_result);
                    }
                    
                    //This means that this is a variable
                    else
                    {
                        
                        stack.Push(s[symbol.Value].Value);
                    }

                        



                    
                }
                else if (symbol.Type == SymbolType.CONST)
                {
                    stack.Push(symbol.Value);
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
            foreach (var symbol in Symbols)
            {
                sb.Append(symbol.Value);

            }

            return " (" + sb.ToString() + ")";
            
        }
    }
}
