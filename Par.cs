using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    internal class Par
    {
        ASTNode root = null;




        /*
 *
 *
 * S int i = 0
   S i = i + 10
   L i<=10
   {
    O i
    S i=i+1
   }

   C i<=10
   {
    O i
    S i=i+1
   }



   L i<=10
   {
    O i
    S i=i+1
    C i<=10
    {
        O i
        S i=i+1
        C i<=10
        {
           O i
           S i=i+1
           C i<=10
           {
              O i
              S i=i+1
           }
        }
    }
           
   }



   L i<=10
        {
            O i
            S i=i+1
            C i<=10
        }

   L i<=10
        {
            O i
            S i=i+1
            C i<=10
            L i<=10
                {
                    O i
                    S i=i+1
                    C i<=10
                }
            L i<=10
               {
                   O i
                   S i=i+1
                   C i<=10
               }
        }
   
 */


        public int FindEndBoundryIndex(List<Symbol> list, int startIndex)
        {
            int counter = 0;
            for (int i = startIndex; i < list.Count; i++)
            {
                if (list[i].Type == SymbolType.BRACE_START)
                    counter++;
                else if (list[i].Type == SymbolType.BRACE_END)
                {
                    counter--;
                    if (counter == 0)
                        return i;
                }
            }

            throw new Exception("End brace not found");
        }


        public ASTNode Parse(List<Symbol> list)
        {
            root = new ASTNode(ASTNodeType.Root);
            ASTNode parent = root;
            return Parse(list, parent);

        }

        public ASTNode Parse(List<Symbol> list, ASTNode parent)
        {
            Stack<ASTNode> parentStack = new Stack<ASTNode>();

            for (int i = 0; i< list.Count; i++)
            {
                Symbol symbol = list[i];

                // A function definition
                //                F Add int a, int b: int
                //                {
                //                    S int r = a + b
                //                     #r
                //                }


                //A function call
                // Add(a, b) //A function call without return statement
                // S r = Add(a, b) //Function call with a return statement

                if (symbol.Type == SymbolType.F)
                {

                    ASTNode f = new ASTNode(ASTNodeType.F);

                    f.Variable = list[i + 1].Value;

                    int pIndex = i + 2;
                    while (true)
                    {
                        //If at the end
                        if (list[pIndex].Type == SymbolType.COLON || list[pIndex].Type == SymbolType.EOL)
                        {
                            if (list[pIndex].Type == SymbolType.COLON)
                                f.ReturnType = list[pIndex + 1].Type;
                            else
                                f.ReturnType = SymbolType.NONE;
                            break;
                        }
                        if (f.Parameters == null)
                            f.Parameters = new List<Parameter>();
                        
                        Parameter p = new Parameter();
                        p.Type = list[pIndex].Type;
                        p.Name = list[pIndex + 1].Value;

                        f.Parameters.Add(p);

                        pIndex += 2;

                        if (list[pIndex].Type == SymbolType.COM)
                            pIndex++;
                        



                    }


                    f.Parent = parent;
                    parent.AddChild(f);

                }

                if (symbol.Type == SymbolType.S)
                {
                    int expOffset = 0;
                    ASTNode s = new ASTNode(ASTNodeType.S);
                    //This means this is a declaration
                    if (list[i+1].IsDataType())
                    {
                        s.DeclarationType = list[i + 1].Type;
                        s.Variable = list[i + 2].Value;

                        if (list[i + 3].Type != SymbolType.EQ)
                            throw new Exception("Expecting assignment operator = ");

                        expOffset = i + 4;
                    }
                    //This means this is an assignment
                    else
                    {
                        s.Variable = list[i + 1].Value;
                        if (list[i + 2].Type != SymbolType.EQ)
                            throw new Exception("Expecting assignment operator = ");
                        expOffset = i + 3;
                    }

                    int j = expOffset;
                    List<Symbol> expList = new List<Symbol>();

                    while (list[j].Type != SymbolType.EOL)
                        expList.Add(list[j++]);

                    s.Exp = new Expression(expList);

                    s.Parent = parent;
                    parent.AddChild(s);

                    //Fast forward to end of the line.
                    while (list[i].Type != SymbolType.EOL)
                        i++;

                    
                }
                
                if (symbol.Type == SymbolType.C)
                {
                    i = DoCL(list, ref parent, i, parentStack, ASTNodeType.C);
                    
                }
                
                
                if (symbol.Type == SymbolType.L)
                {
                    i = DoCL(list, ref parent, i, parentStack, ASTNodeType.L);
                   

                }
                

                if (symbol.Type == SymbolType.I)
                {

                    ASTNode input = new ASTNode(ASTNodeType.I);
                    input.Variable = list[i+1].Value;

                    input.Parent = parent;
                    parent.AddChild(input);

                }

                if (symbol.Type == SymbolType.O)
                {

                    ASTNode output = new ASTNode(ASTNodeType.O);
                    output.Variable = list[i + 1].Value;

                    output.Parent = parent;
                    parent.AddChild(output);

                }

                if (symbol.Type == SymbolType.SWIGGLE)
                {

                    ASTNode b = new ASTNode(ASTNodeType.Break);
                  
                    b.Parent = parent;
                    parent.AddChild(b);

                }

                if (symbol.Type == SymbolType.HASHTAG)
                {

                    ASTNode r = new ASTNode(ASTNodeType.Return);

                    r.Parent = parent;
                    parent.AddChild(r);

                }





                //if ()
                //{

                //}

                if (symbol.Type == SymbolType.BRACE_END) //This means you are out of the previous scope, time to resume and resume to previous parent
                {
                    if (parentStack.Count > 0)
                        parent = parentStack.Pop();
                }


            }


            root.PrintPretty(" ", true);
            return root;
        }

        private static int DoCL(List<Symbol> list, ref ASTNode parent, int i, Stack<ASTNode> parentStack, ASTNodeType nt)
        {
            ASTNode n = new ASTNode(nt);

            int j = i + 1;
            List<Symbol> expList = new List<Symbol>();
            while (list[j].Type != SymbolType.BRACE_START)
                expList.Add(list[j++]);
                    
            n.Exp = new Expression(expList);
            n.Parent = parent;
            parent.AddChild(n);
            parentStack.Push(parent); //Remember the previous parent
            //Reassign the parent
            parent = n;

            //Fast forward to beginning of the brace 
            while (list[i].Type != SymbolType.BRACE_START)
                i++;
            return i;
        }
    }
}
