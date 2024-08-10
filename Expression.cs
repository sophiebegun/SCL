using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    internal class Expression
    {
        private List<Symbol> symbols;


        public Expression(List<Symbol> symbols)
        {
            this.symbols = symbols;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var symbol in symbols)
            {
                if (symbol.Value != null)
                    sb.Append(symbol.Value);
                else
                    sb.Append(symbol.Type.ToString());
            }

            return " (" + sb.ToString() + ")";
            
        }
    }
}
