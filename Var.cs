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

        public bool IsComplexType()
        {
            return this.Type == SymbolType.DT_LST || this.Type == SymbolType.DT_SET || this.Type == SymbolType.DT_MAP;
        }


        public List<object> AsList()
        {
            return (List<object>)this.Value;
        }
        public HashSet<object> AsHSet()
        {
            return (HashSet<object>)this.Value;
        }
        public Dictionary<object, object> AsHMap()
        {
            return (Dictionary<object, object>)this.Value;
        }

    }
}
