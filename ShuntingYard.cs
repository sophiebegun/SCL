namespace SCL
{
    internal class ShuntingYard
    {
        public static List<Symbol> ConvertToPostfix(List<Symbol> infix)
        {
            Stack<Symbol> stack = new Stack<Symbol>();
            List<Symbol> output = new List<Symbol>();
            bool isFunctionCall = false; // This flag helps identify when we're processing a function call

            foreach (var symbol in infix)
            {
                switch (symbol.Type)
                {
                    case SymbolType.NUMBER:
                    case SymbolType.CONST:
                    case SymbolType.TRUE:
                    case SymbolType.FALSE:
                        output.Add(symbol);  // Numbers, constants, and literals go directly to output
                        break;

                    case SymbolType.NAME:
                        // Check if the NAME is followed by parentheses, indicating a function
                        if (IsNextSymbolFunction(symbol, infix))
                        {
                            stack.Push(symbol);  // Function name goes on the stack to be processed later
                            isFunctionCall = true;
                        }
                        else
                        {
                            output.Add(symbol);  // It's a variable or constant, so go directly to output
                        }
                        break;

                    case SymbolType.PAREN_START:
                        stack.Push(symbol);
                        break;

                    case SymbolType.PAREN_END:
                        // Pop the stack until an opening parenthesis is found
                        while (stack.Count > 0 && stack.Peek().Type != SymbolType.PAREN_START)
                        {
                            output.Add(stack.Pop());
                        }

                        // Pop the opening parenthesis
                        if (stack.Count > 0 && stack.Peek().Type == SymbolType.PAREN_START)
                        {
                            stack.Pop();
                        }

                        // If it was a function call, pop the function name and add it to the output
                        if (isFunctionCall && stack.Count > 0 && stack.Peek().Type == SymbolType.NAME)
                        {
                            output.Add(stack.Pop());
                            isFunctionCall = false;
                        }

                        break;

                    case SymbolType.COM:
                        // Handle function argument separation
                        while (stack.Count > 0 && stack.Peek().Type != SymbolType.PAREN_START)
                        {
                            output.Add(stack.Pop());
                        }
                        break;

                    case SymbolType.POW:
                    case SymbolType.MUL:
                    case SymbolType.DIV:
                    case SymbolType.PLUS:
                    case SymbolType.MINUS:
                    case SymbolType.NOT:
                    case SymbolType.AMP:
                    case SymbolType.PIPE:
                    case SymbolType.COMP:
                    case SymbolType.NOT_EQ:
                    case SymbolType.GTE:
                    case SymbolType.GT:
                    case SymbolType.LTE:
                    case SymbolType.LT:
                        // Pop all operators from the stack with greater or equal precedence
                        while (stack.Count > 0 && Precedence(stack.Peek().Type) >= Precedence(symbol.Type))
                        {
                            output.Add(stack.Pop());
                        }
                        stack.Push(symbol);
                        break;
                }
            }

            // Pop any remaining operators from the stack
            while (stack.Count > 0)
            {
                if (stack.Peek().Type == SymbolType.PAREN_START)
                {
                    stack.Pop();
                    continue;
                }
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
                case SymbolType.NOT:
                    return 4;
                case SymbolType.AMP:
                    return 5;
                case SymbolType.PIPE:
                    return 6;
                case SymbolType.COMP:
                case SymbolType.NOT_EQ:
                case SymbolType.GTE:
                case SymbolType.GT:
                case SymbolType.LTE:
                case SymbolType.LT:
                    return 7;
                default:
                    return 0;
            }
        }

        // Helper method to check if a NAME is a function by looking ahead in the infix list
        private static bool IsNextSymbolFunction(Symbol symbol, List<Symbol> infix)
        {
            int index = infix.IndexOf(symbol);
            if (index >= 0 && index < infix.Count - 1)
            {
                // If the next symbol is an opening parenthesis, it's a function call
                return infix[index + 1].Type == SymbolType.PAREN_START;
            }
            return false;
        }
    }
}
