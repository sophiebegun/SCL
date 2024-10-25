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
            if (type == SymbolType.DT_LST && value == null)
                this.Value = new List<object>();
            else if (type == SymbolType.DT_SET && value == null)
                this.Value = new HashSet<object>();
            else if (type == SymbolType.DT_MAP && value == null)
                this.Value = new Dictionary<object, object>();
            else
            {
                if(type == SymbolType.DT_INT)
                    this.Value = Convert.ToInt32(value);
                else if(type == SymbolType.DT_BOOL)
                    this.Value = Convert.ToBoolean(value);
                else if (type == SymbolType.DT_DOUBLE)
                    this.Value = Convert.ToDouble(value);
                else
                    this.Value = value;
            }
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
