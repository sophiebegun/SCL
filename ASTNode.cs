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
        S,
        C,
        L,
        I,
        O

    }

    internal class ASTNode
    {
        public ASTNode Parent { get; set;}
        public ASTNodeType NodeType { get; set; }
        public List<ASTNode> Children { get; set; }
        public List<Symbol> Symbols { get; set; }


        #region "Declaration/Assignments"
        public SymbolType DeclarationType { get; set; }
        public string AssignmentVariable { get; set; }

        #endregion


        public Expression Exp { get; set; }

        public void AddChild(ASTNode n)
        {
            if (Children == null)
                Children = new List<ASTNode>();
            Children.Add(n);
        }









        public ASTNode(List<Symbol> symbols)
        {
            this.Symbols = symbols;
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

            Console.WriteLine(NodeType.ToString());

            if (Children == null)
                return;

            for (int i = 0; i < Children.Count; i++)
                Children[i].PrintPretty(indent, i == Children.Count - 1);
        }
    }
}
