using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Reflection;
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
            if (rpnSymbols == null)
                return null;

            if (rpnSymbols.Count == 0)
                return null;

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
                        ProcessCustomFunctionCall(fds, stack, symbol);
                    }
                    //This means it's a built in function such as get or add
                    else if (Par.IsBuiltIn(symbol.Value))
                    {
                        ProcessInBuiltFunctionCall(s, stack, symbol);

                    }
                    //This means that this is a variable
                    else
                    {
                        if(s[symbol.Value].IsComplexType())
                            stack.Push(s[symbol.Value]);
                        else
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
                            stack.Push(Convert.ToDouble(leftOperand) + Convert.ToDouble(rightOperand));
                            break;
                        case SymbolType.MINUS:
                            stack.Push(Convert.ToDouble(leftOperand) - Convert.ToDouble(rightOperand));
                            break;
                        case SymbolType.MUL:
                            stack.Push(Convert.ToDouble(leftOperand) * Convert.ToDouble(rightOperand));
                            break;
                        case SymbolType.DIV:
                            stack.Push(Convert.ToDouble(leftOperand) / Convert.ToDouble(rightOperand));
                            break;
                        case SymbolType.POW:
                            stack.Push(Math.Pow(Convert.ToDouble(leftOperand), Convert.ToDouble(rightOperand)));
                            break;
                        case SymbolType.AMP:
                            stack.Push(Convert.ToBoolean(leftOperand) && Convert.ToBoolean(rightOperand));
                            break;
                        case SymbolType.PIPE:
                            stack.Push(Convert.ToBoolean(leftOperand) || Convert.ToBoolean(rightOperand));
                            break;
                        case SymbolType.NOT:
                            stack.Push(!Convert.ToBoolean(rightOperand));
                            break;
                        case SymbolType.EQ:
                            stack.Push(Convert.ToDouble(leftOperand) == Convert.ToDouble(rightOperand));
                            break;
                        case SymbolType.NOT_EQ:
                            stack.Push(Convert.ToDouble(leftOperand) != Convert.ToDouble(rightOperand));
                            break;
                        case SymbolType.GTE:
                            stack.Push(Convert.ToDouble(leftOperand) >= Convert.ToDouble(rightOperand));
                            break;
                        case SymbolType.GT:
                            stack.Push(Convert.ToDouble(leftOperand) > Convert.ToDouble(rightOperand));
                            break;
                        case SymbolType.LTE:
                            stack.Push(Convert.ToDouble(leftOperand) <= Convert.ToDouble(rightOperand));
                            break;
                        case SymbolType.LT:
                            stack.Push(Convert.ToDouble(leftOperand) < Convert.ToDouble(rightOperand));
                            break;
                    }
                }
            }

            if (stack.Count == 0)
                return null;

            object result = stack.Pop();
            return result;

       }

        private void ProcessCustomFunctionCall(Dictionary<string, ASTNode> fds, Stack<object> stack, Symbol symbol)
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

        void ProcessInBuiltFunctionCall(Scope s, Stack<object> stack, Symbol symbol)
        {
            string func = symbol.Value;

            List<object> args = new List<object>();

            Var obj = null;
            while (stack.Count>0)
            {
                object val = stack.Pop();
                if (val is Var)
                {
                    obj = val as Var;
                    break;
                }
                args.Insert(0, val);
            }



            if (!s.ContainsKey(obj.Name))
                throw new Exception("Object not found: " + obj.Name);

            if (func == "add")
            {
                if (obj.Type == SymbolType.DT_LST)
                {
                    object value = args[0];
                    obj.AsList().Add(value);
                }
                else if (obj.Type == SymbolType.DT_SET)
                {
                    object value = args[0];
                    obj.AsHSet().Add(value);
                }
                else if (obj.Type == SymbolType.DT_MAP)
                {

                    object key = args[0];
                    object value = args[1];
                    obj.AsHMap().Add(key, value);
                }

            }
            else if (func == "rem")
            {
                if (obj.Type == SymbolType.DT_LST)
                {
                    int index = Convert.ToInt32(args[0]);
                    obj.AsList().RemoveAt(index);
                }
                else if (obj.Type == SymbolType.DT_SET)
                {
                    object value = args[0];
                    obj.AsHSet().Remove(value);
                }
                else if (obj.Type == SymbolType.DT_MAP)
                {

                    object key = args[0];
                    obj.AsHMap().Remove(key);
                }

            }
            else if (func == "get")
            {
                if (obj.Type == SymbolType.DT_LST)
                {
                    int index = Convert.ToInt32(args[0]);
                    stack.Push(obj.AsList()[index]);
                }
                else if (obj.Type == SymbolType.DT_SET)
                {
                    object key = args[0];
                    obj.AsHSet().TryGetValue(key, out var value);
                    stack.Push(value);
                }
                else if (obj.Type == SymbolType.DT_MAP)
                {
                    object key = args[0];
                    obj.AsHMap().TryGetValue(key, out var value);
                    stack.Push(value);
                }

            }
            else if (func == "count")
            {
                if (obj.Type == SymbolType.DT_LST)
                {
                    stack.Push(obj.AsList().Count);
                }
                else if (obj.Type == SymbolType.DT_SET)
                {
                    stack.Push(obj.AsHSet().Count);
                }
                else if (obj.Type == SymbolType.DT_MAP)
                {
                    stack.Push(obj.AsHMap().Count);
                }

            }


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
