using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    internal class Var
    {
        public string Name { get; set; }
        public SymbolType Type { get; set; }
        public object Value { get; set; }

        public Var(string name, SymbolType type, object value)
        {
            this.Name = name;
            this.Type = type;
            if (type == SymbolType.DT_LST)
                this.Value = new List<object>();
            else if (type == SymbolType.DT_SET)
                this.Value = new HashSet<object>();
            else if (type == SymbolType.DT_MAP)
                this.Value = new Dictionary<object, object>();
            else
                this.Value = value;
        }
    }
}
