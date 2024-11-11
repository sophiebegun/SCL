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
            if (rpnSymbols == null || rpnSymbols.Count == 0)
                return null;

            Stack<object> stack = new Stack<object>();

            foreach (var symbol in rpnSymbols)
            {
                if (symbol.Type == SymbolType.NUMBER)
                {
                    stack.Push(symbol.Value); // Push as is, to check type later
                }
                else if (symbol.Type == SymbolType.NAME)
                {
                    if (fds.ContainsKey(symbol.Value.ToString()))
                    {
                        ProcessCustomFunctionCall(fds, stack, symbol);
                    }
                    else if (Par.IsBuiltIn(symbol.Value.ToString()))
                    {
                        ProcessInBuiltFunctionCall(s, stack, symbol);
                    }
                    else
                    {
                        if (Symbol.IsComplexType(s[symbol.Value.ToString()].Type))
                            stack.Push(s[symbol.Value.ToString()]);
                        else
                            stack.Push(s[symbol.Value.ToString()].Value);
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
                            stack.Push(Add(leftOperand, rightOperand));
                            break;
                        case SymbolType.MINUS:
                            stack.Push(Subtract(leftOperand, rightOperand));
                            break;
                        case SymbolType.MUL:
                            stack.Push(Multiply(leftOperand, rightOperand));
                            break;
                        case SymbolType.DIV:
                            stack.Push(Divide(leftOperand, rightOperand));
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
                        case SymbolType.NOT_EQ:
                            stack.Push(!Equal(leftOperand, rightOperand));
                            break;
                        case SymbolType.COMP:
                            stack.Push(Equal(leftOperand, rightOperand));
                            break;
                        case SymbolType.GTE:
                            stack.Push(Compare(leftOperand, rightOperand) >= 0);
                            break;
                        case SymbolType.GT:
                            stack.Push(Compare(leftOperand, rightOperand) > 0);
                            break;
                        case SymbolType.LTE:
                            stack.Push(Compare(leftOperand, rightOperand) <= 0);
                            break;
                        case SymbolType.LT:
                            stack.Push(Compare(leftOperand, rightOperand) < 0);
                            break;
                    }
                }
            }

            return stack.Count > 0 ? stack.Pop() : null;
        }

        // Helper methods for arithmetic operations
        private object Add(object left, object right)
        {
            if (left is int && right is int) return (int)left + (int)right;
            return Convert.ToDouble(left) + Convert.ToDouble(right);
        }

        private object Subtract(object left, object right)
        {
            if (left is int && right is int) return (int)left - (int)right;
            return Convert.ToDouble(left) - Convert.ToDouble(right);
        }

        private object Multiply(object left, object right)
        {
            if (left is int && right is int) return (int)left * (int)right;
            return Convert.ToDouble(left) * Convert.ToDouble(right);
        }

        private object Divide(object left, object right)
        {
            if (left is int && right is int) return (int)left / (int)right;
            return Convert.ToDouble(left) / Convert.ToDouble(right);
        }

        private bool Equal(object left, object right)
        {
            if (left is int && right is int) return (int)left == (int)right;
            return Convert.ToDouble(left) == Convert.ToDouble(right);
        }

        private int Compare(object left, object right)
        {
            if (left is int && right is int) return ((int)left).CompareTo((int)right);
            return Convert.ToDouble(left).CompareTo(Convert.ToDouble(right));
        }

        private void ProcessCustomFunctionCall(Dictionary<string, ASTNode> fds, Stack<object> stack, Symbol symbol)
        {
            string func = symbol.Value.ToString();
            ASTNode fdNode = fds[func];

            int argCount = fdNode.Parameters.Count;


            Scope new_scope = new Scope();

            // Pop arguments from stack in reverse order (right to left)
            for (int i = argCount - 1; i >= 0; i--)
            {
                string name = fdNode.Parameters[i].Name;
                object val = stack.Pop();
                if(val is Var)
                    val = ((Var)val).Value;

                Var v = new Var(name, fdNode.Parameters[i].Type, val);
                new_scope.Add(name, v);

            }

            var func_result = fdNode.Evaluate(new_scope, fds);
            stack.Push(func_result);
        }


        object CopyObject(object obj)
        {
            if (obj == null) return null;
            return Convert.ChangeType(obj, obj.GetType());
        }

        void ProcessInBuiltFunctionCall(Scope s, Stack<object> stack, Symbol symbol)
        {
            string func = symbol.Value.ToString();

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


            if (Par.IsBuiltInMethodType(func))
            {
                if (!s.ContainsKey(obj.Name))
                    throw new Exception("Object not found: " + obj.Name);
            }

            if (func == "cat")
            {
                StringBuilder sb = new StringBuilder();
                foreach (object val in args)
                    sb.Append(val.ToString());
                stack.Push(sb.ToString());
            }
            else if (func == "add")
            {
                if (obj.Type == SymbolType.DT_LST)
                {
                    object value = CopyObject(args[0]);
                    obj.AsList().Add(value);
                }
                else if (obj.Type == SymbolType.DT_SET)
                {
                    object value = CopyObject(args[0]);
                    obj.AsHSet().Add(value);
                }
                else if (obj.Type == SymbolType.DT_MAP)
                {

                    object key = CopyObject(args[0]);
                    object value = CopyObject(args[1]);
                    obj.AsHMap().Add(key, value);
                }

            }
            else if (func == "set")
            {
                if (obj.Type == SymbolType.DT_LST)
                {
                    int index = Convert.ToInt32(args[0]);
                    object value = CopyObject(args[1]);
                    obj.AsList()[index] = value;

                    Console.WriteLine("Set index:" + index + " value:" + value);
                }
                else if (obj.Type == SymbolType.DT_SET)
                {
                    throw new Exception("Op not supported on a HSET");
                }
                else if (obj.Type == SymbolType.DT_MAP)
                {

                    object key = args[0];
                    object value = CopyObject(args[1]);
                    obj.AsHMap()[key] =  value;
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
