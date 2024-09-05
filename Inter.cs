using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    internal class Inter 
    {
        private Scope state = new Scope();
        Dictionary<string, ASTNode> fds;
        private ASTNode root;
        public Inter(ASTNode root, Dictionary<string, ASTNode> fds)
        {
            this.root = root;
            this.fds = fds;
        }

        public void Evaluate()
        {
            this.root.Evaluate(state, fds);
        }



    }
}
