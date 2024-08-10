﻿using System;
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

                
                if (symbol.Type == SymbolType.S)
                {
                    int expOffset = 0;
                    ASTNode s = new ASTNode(ASTNodeType.S);
                    //This means this is a declaration
                    if (list[i+1].IsDataType())
                    {
                        s.DeclarationType = list[i + 1].Type;
                        s.AssignmentVariable = list[i + 2].Value;

                        if (list[i + 3].Type != SymbolType.EQ)
                            throw new Exception("Expecting assignment operator = ");

                        expOffset = i + 4;
                    }
                    //This means this is an assignment
                    else
                    {
                        s.AssignmentVariable = list[i + 1].Value;
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

                    continue;
                }
                
                if (symbol.Type == SymbolType.C)
                {
                    ASTNode c = new ASTNode(ASTNodeType.C);

                    int j = i + 1;
                    List<Symbol> expList = new List<Symbol>();
                    while (list[j].Type != SymbolType.BRACE_START)
                        expList.Add(list[j++]);
                    
                    c.Exp = new Expression(expList);
                    c.Parent = parent;
                    parent.AddChild(c);
                    parentStack.Push(parent); //Remember the previous parent
                    //Reassign the parent
                    parent = c;

                    //Fast forward to beginning of the brace 
                    while (list[i].Type != SymbolType.BRACE_START)
                        i++;

                    //All subsequent statements within the braces are the children of this node c
                    //int endBraceIndex = FindEndBoundryIndex(list, j);




                    continue;
                }
                
                
                if (symbol.Type == SymbolType.L)
                {

                }
                if (symbol.Type == SymbolType.BRACE_END) //This means you are out of the previous scope, time to resume and resume to previous parent
                {
                    if(parentStack.Count>0)
                        parent = parentStack.Pop();
                }


            }


            root.PrintPretty(" ", true);
            return root;
        }



    }
}
