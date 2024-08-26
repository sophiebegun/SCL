using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    internal class Inter 
    {
        private Stack<Scope> state = new Stack<Scope>();
        private ASTNode root;
        public Inter(ASTNode root)
        {
            this.root = root;
            Scope s = new Scope();
            state.Push(s);

        }

        public void Evaluate()
        {
            this.root.Evaluate(state.Peek());
        }



    }
}
