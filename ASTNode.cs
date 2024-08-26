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


        public void Evaluate(Scope s)
        {
            if (NodeType == ASTNodeType.Root)
            {
                this.Children[0].Evaluate(s);
                return;
            }
            if (IsDeclaration)
            {
                object value = Exp.Evaluate(s);
                Var v = new Var(this.Variable, this.DeclarationType, value);
                s.Add(this.Variable,v);
            }
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
