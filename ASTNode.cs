using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    public enum ASTNodeType
    {
        Root,
        S, //State
        C, //Conditionality
        L, //Loop
        FC, //Function Call
        FD, //Function Def
        I, //Input
        O, //Output
        Break,
        Return

    }

    internal class ASTNode
    {
        public ASTNode Parent { get; set;}
        public ASTNodeType NodeType { get; set; }
        public List<ASTNode> Children { get; set; }
        public List<Symbol> Symbols { get; set; }


        #region "Declaration/Assignments"

        public SymbolType DeclarationType { get; set; } = SymbolType.NONE;
        public string Variable { get; set; } = "";

        //Used for lst, hmaps. 
        public SymbolType KeySubtype { get; set; } = SymbolType.NONE;

        //Used for hsets only.
        public SymbolType ValueSubtype { get; set; } = SymbolType.NONE;

        #endregion

        #region "Function Parameters"

        public List<Parameter> Parameters { get; set; }

        public SymbolType ReturnType { get; set; }

        #endregion

        #region "Output"
        public bool IsNL { get; set; } = true;
        #endregion

        public bool IsDeclaration
        {
            get
            {
                if(DeclarationType!= SymbolType.NONE && NodeType == ASTNodeType.S)
                    return true;
                return false;
            }
        }


        public Expression Exp { get; set; }

        public void AddChild(ASTNode n)
        {
            if (Children == null)
                Children = new List<ASTNode>();
            Children.Add(n);
        }


        public object Evaluate(Scope s, Dictionary<string, ASTNode> fds)
        {
            //Root Condition || FD
            if (this.NodeType == ASTNodeType.Root || this.NodeType == ASTNodeType.FD)
            {
                for (int j = 0; j < this.Children.Count; j++)
                {
                    var r = this.Children[j].Evaluate(s, fds);
                    //If return statement then return r
                    if (this.Children[j].NodeType == ASTNodeType.Return) 
                        return r;
                }
                return null;
            }

            //Everything else
            if (this.NodeType == ASTNodeType.S)
            {
                object value = this.Exp.Evaluate(s, fds);
                if (value is bool && this.DeclarationType != SymbolType.DT_BOOL)
                    throw new Exception("Trying to assign a Boolean evaluation to a " + this.DeclarationType.ToString() + " type");


                if (this.IsDeclaration)
                {
                    if (value is double && (this.DeclarationType != SymbolType.DT_INT && this.DeclarationType != SymbolType.DT_DOUBLE))
                        throw new Exception("Trying to assign a double evaluation to a " + this.DeclarationType.ToString() + " type");
                    Var v = new Var(this.Variable, this.DeclarationType, value);
                    s.Add(this.Variable, v);
                }
                else
                {
                    if (value is double && (s[this.Variable].Type != SymbolType.DT_INT && s[this.Variable].Type != SymbolType.DT_DOUBLE))
                        throw new Exception("Trying to assign a double evaluation to a " + this.DeclarationType.ToString() + " type");

                    s[this.Variable].Value = value;
                }
            }
            else if (this.NodeType == ASTNodeType.C)
            {
                object value = this.Exp.Evaluate(s, fds);
                if (!(value is bool))
                    throw new Exception("C must evaluate to a bool.");
                if ((bool)value)
                {
                    for (int j = 0; j < this.Children.Count; j++)
                        this.Children[j].Evaluate(s, fds);
                }
            }
            else if (this.NodeType == ASTNodeType.L)
            {
                while (true)
                {
                    object value = this.Exp.Evaluate(s, fds);
                    if (!(value is bool))
                        throw new Exception("L must evaluate to a bool.");

                    if ((bool)value)
                    {
                        for (int j = 0; j < this.Children.Count; j++)
                            this.Children[j].Evaluate(s, fds);
                    }
                    else
                        break;
                }
            }
            else if (this.NodeType == ASTNodeType.O)
            {
                object value = s[this.Variable].Value;
                if (this.IsNL)
                    Console.WriteLine(value);
                else
                    Console.Write(value);


            }
            else if (this.NodeType == ASTNodeType.Return)
            {
                //This means that the function returns void.
                if (this.Exp == null)
                    return null;

                if (this.Exp.Symbols.Count == 0)
                    return null;

                object value = this.Exp.Evaluate(s, fds);
                return value;
            }
            return null;


        }







        public ASTNode(List<Symbol> symbols)
        {
            this.Symbols = symbols;
        }

        public ASTNode()
        {
            
        }


        public ASTNode(ASTNodeType type)
        {
            NodeType = type;
        }

        public void PrintPretty(string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }

            string d = "";
            if (IsDeclaration)
            {
                d = " dec ";
            }

            string v = "";

            if (Variable !="")
                v = ' ' + Variable + ' ';
            if (Exp == null)
                Console.WriteLine(NodeType.ToString() + d + v);
            else
                Console.WriteLine(NodeType.ToString() + d + v  + Exp.ToString());

            if (Children == null)
                return;

            for (int i = 0; i < Children.Count; i++)
                Children[i].PrintPretty(indent, i == Children.Count - 1);
         }
    }
}
