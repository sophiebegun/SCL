// See https://aka.ms/new-console-template for more information

using SCL;

Lex lex = new Lex();


string s = File.ReadAllText("source/func_add.scl");

var list = lex.Analyze(s);


Par par = new Par();
ASTNode node = par.Parse(list);

Console.WriteLine(list);



Inter inter = new Inter(node, par.FDs);
inter.Evaluate();


//Hello


