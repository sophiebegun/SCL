using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace SCL
{
    internal class Par
    {
        ASTNode root = null;

        public Dictionary<string, ASTNode> FDs = new Dictionary<string, ASTNode>();




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

        private bool HasParen(int startIndex, List<Symbol> list)
        {
            
            int fi = startIndex;
            while (list[fi].Type != SymbolType.EOL)
            {
                if (list[fi].Type == SymbolType.PAREN_START)
                    return true;
                fi++;


            }

            return false;

        }

        public ASTNode Parse(List<Symbol> list, ASTNode parent)
        {
            Stack<ASTNode> parentStack = new Stack<ASTNode>();

            for (int i = 0; i< list.Count; i++)
            {
                Symbol symbol = list[i];

                if (symbol.Type == SymbolType.EOL)
                    continue;

                ASTNode n = new ASTNode();


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
                    if (HasParen(i, list))
                    {
                        n.NodeType = ASTNodeType.FC;
                        int j = i;
                        List<Symbol> expList = new List<Symbol>();

                        while (list[j].Type != SymbolType.EOL)
                            expList.Add(list[j++]);

                        n.Exp = new Expression(expList);
                        SetParent(parent, n);
                    }
                    else
                    {
                        n.NodeType = ASTNodeType.FD;
                        n.Variable = list[i + 1].Value;

                        int pIndex = i + 2;
                        while (true)
                        {
                            //If at the end
                            if (list[pIndex].Type == SymbolType.COLON || list[pIndex].Type == SymbolType.EOL)
                            {
                                if (list[pIndex].Type == SymbolType.COLON)
                                    n.ReturnType = list[pIndex + 1].Type;
                                else
                                    n.ReturnType = SymbolType.NONE;
                                break;
                            }
                            if (n.Parameters == null)
                                n.Parameters = new List<Parameter>();

                            Parameter p = new Parameter();
                            p.Type = list[pIndex].Type;
                            p.Name = list[pIndex + 1].Value;

                            n.Parameters.Add(p);

                            pIndex += 2;

                            if (list[pIndex].Type == SymbolType.COM)
                                pIndex++;

                        }


                        i = DoCLF(n, list, ref parent, i, parentStack, ASTNodeType.FD);

                        //Add the function def to dictionary of FDs
                        FDs.Add(n.Variable, n);

                    }


                }

                if (symbol.Type == SymbolType.S)
                {
                    int expOffset = 0;

                    n.NodeType = ASTNodeType.S;
                    //This means this is a declaration
                    if (list[i+1].IsDataType())
                    {
                        n.DeclarationType = list[i + 1].Type;

                        //If simple types
                        if (n.DeclarationType ==SymbolType.DT_INT || n.DeclarationType == SymbolType.DT_DOUBLE || n.DeclarationType == SymbolType.DT_STR || n.DeclarationType == SymbolType.DT_BOOL)
                        {
                            if (list[i + 2].Type != SymbolType.NAME)
                                throw new Exception("Expecting name");

                            n.Variable = list[i + 2].Value;

                            if (list[i + 3].Type != SymbolType.EQ)
                                throw new Exception("Expecting assignment operator = ");

                            expOffset = i + 4;
                        }
                        else
                        {
                            if (n.DeclarationType == SymbolType.DT_LST || n.DeclarationType == SymbolType.DT_SET)
                            {
                                n.KeySubtype = list[i + 2].Type;
                                n.Variable = list[i + 3].Value;

                                if(list[i + 3].Type != SymbolType.NAME)
                                    throw new Exception("Expecting name");

                                expOffset = i + 3;
                            }
                            else if(n.DeclarationType == SymbolType.DT_MAP)
                            {

                                n.KeySubtype = list[i + 2].Type;
                                n.ValueSubtype = list[i + 3].Type;
                                n.Variable = list[i + 4].Value;

                                if (list[i + 4].Type != SymbolType.NAME)
                                    throw new Exception("Expecting name");




                                expOffset = i + 4;
                            }
                        }
                        //S lst int list
                        
                    }
                    //This means this is an assignment
                    else 
                    {
                        n.Variable = list[i + 1].Value;
                        if (list[i + 2].Type != SymbolType.EQ)
                            throw new Exception("Expecting assignment operator = ");
                        expOffset = i + 3;
                    }

                    int j = expOffset;
                    List<Symbol> expList = new List<Symbol>();

                    while (list[j].Type != SymbolType.EOL)
                        expList.Add(list[j++]);

                    n.Exp = new Expression(expList);
                    SetParent(parent, n);

                }

                if (symbol.Type == SymbolType.C)
                {
                    n.NodeType = ASTNodeType.C;
                    i = DoCLF(n,list, ref parent, i, parentStack, ASTNodeType.C);
                }
                
                
                if (symbol.Type == SymbolType.L)
                {
                    n.NodeType = ASTNodeType.L;
                    i = DoCLF(n,list, ref parent, i, parentStack, ASTNodeType.L);
                }
                

                if (symbol.Type == SymbolType.I)
                {
                    n.NodeType = ASTNodeType.I;
                    n.Variable = list[i+1].Value;
                    SetParent(parent, n);

                }

                if (symbol.Type == SymbolType.O)
                {
                    n.NodeType = ASTNodeType.O;
                    n.Variable = list[i + 1].Value;
                    SetParent(parent, n);

                    if (list[i+2].Type == SymbolType.COM)
                    {
                        n.IsNL = false;
                    }
                }

                if (symbol.Type == SymbolType.SWIGGLE)
                {
                    n.NodeType = ASTNodeType.Break;
                    SetParent(parent, n);
                }

                if (symbol.Type == SymbolType.HASHTAG)
                {
                    n.NodeType = ASTNodeType.Return;

                    int j = i +1;
                    List<Symbol> expList = new List<Symbol>();
                    while (list[j].Type != SymbolType.EOL)
                        expList.Add(list[j++]);
                    n.Exp = new Expression(expList);
                    SetParent(parent, n);
                }




                
                if (symbol.Type == SymbolType.BRACE_END) //This means you are out of the previous scope, time to resume and resume to previous parent
                {
                    if (parentStack.Count > 0)
                        parent = parentStack.Pop();
                }


                //Fast forward to end of the line.
                while (list[i].Type != SymbolType.EOL)
                    i++;


            }


            root.PrintPretty(" ", true);
            return root;
        }

        private static void SetParent(ASTNode parent, ASTNode n)
        {
            //Set parent references
            n.Parent = parent;
            parent.AddChild(n);
        }

        private static int DoCLF(ASTNode n, List<Symbol> list, ref ASTNode parent, int i, Stack<ASTNode> parentStack, ASTNodeType nt)
        {
            
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
            return i;
        }
    }
}
