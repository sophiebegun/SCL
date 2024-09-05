using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    public enum SymbolType
    {
        NONE, //No Symbol
        S, //State
        C, //Conditionality
        L, //Looping
        F, //Function
        I, //Input
        O, //Output
        DT_INT, //Integer
        DT_DOUBLE, //Double
        DT_STR, //String
        DT_SET, //Hashset
        DT_MAP, //Hashmap
        DT_LST, //List
        DT_BOOL, //Boolean
        MUL, //*
        DIV, // /
        PLUS,// +
        MINUS,// -
        POW, // ^
        NAME, //Name of int, str, list...
        NUMBER, //Numeral value
        EOL, //End of line
        EQ, //Equal
        COMP, //Comparison
        NOT_EQ, //Not Equals
        GTE, //Greater than or equal to
        GT, //Greater than
        LTE, //Less than or equal to
        LT, //Less than
        TRUE, //True
        FALSE, //False
        PAREN_START, //(
        PAREN_END, //)
        BRACK_START, //[
        BRACK_END, //]
     
        SWIGGLE, //Break statement
        HASHTAG, //Return statement
        COLON, //Return output function (def/call)
        NOT, //Opposite command
        BRACE_START, //{
        BRACE_END, //}
        COM, //Comma in between parameters/arguments
        CONST, //Reps constant either number or string
        AMP, //&
        PIPE, // |
        
    }

    internal class Symbol
    {
        public SymbolType Type { get; set; }
        public string Value { get; set; }  //Only used for Name & Number

        public Symbol(SymbolType type, string value)
        {
            Type = type;
            Value = value;
        }

        public Symbol(SymbolType type)
        {
            Type = type;
        }


        public bool IsDataType()
        {
            switch (Type)
            {
                case SymbolType.DT_BOOL:
                case SymbolType.DT_INT:
                case SymbolType.DT_DOUBLE:
                case SymbolType.DT_LST:
                case SymbolType.DT_STR:
                case SymbolType.DT_MAP:
                case SymbolType.DT_SET:
                    return true;
            }
            return false;
        }


        public override string ToString()
        {
            if (Type == SymbolType.NAME || Type == SymbolType.NUMBER)
                return Value;
            else
            {
                return Type.ToString();
            }
        }
    }
}
