// See https://aka.ms/new-console-template for more information

using SCL;

Lex lex = new Lex();


string s = File.ReadAllText("source/arith_expr.scl");

var list = lex.Analyze(s);


Par par = new Par();
ASTNode node = par.Parse(list);

Inter inter = new Inter(node);
inter.Evaluate();


Console.WriteLine(list);



//Hello


