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
                        while (stack.Count > 0 && stack.Peek().Type != SymbolType.PAREN_START &&
                               stack.Peek().Type != SymbolType.BRACK_START)
                        {
                            output.Add(stack.Pop());
                        }

                        if (stack.Count > 0 && (stack.Peek().Type == SymbolType.PAREN_START ||
                                                stack.Peek().Type == SymbolType.BRACK_START))
                        {
                            stack.Pop();
                        }

                        if (stack.Count > 0 && stack.Peek().Type == SymbolType.NAME && expectFunction)
                        {
                            output.Add(stack.Pop());
                        }

                        break;

                    case SymbolType.COM:
                        while (stack.Count > 0 && stack.Peek().Type != SymbolType.PAREN_START &&
                               stack.Peek().Type != SymbolType.BRACK_START)
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
                case SymbolType.NOT:
                    return 4;
                case SymbolType.AMP:
                    return 5;
                case SymbolType.PIPE:
                    return 6;
                case SymbolType.COMP:
                case SymbolType.NOT_EQ:
                    return 7;
                default:
                    return 0;
            }
        }
    }
}