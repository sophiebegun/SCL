using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    public class Parameter
    {

        public SymbolType Type { get; set; }

        //Used for lst, hmaps. 
        public SymbolType KeySubtype { get; set; } = SymbolType.NONE;

        //Used for hsets only.
        public SymbolType ValueSubtype { get; set; } = SymbolType.NONE;

        public string Name { get; set; }


       
    }
}
