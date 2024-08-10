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
    }
}
