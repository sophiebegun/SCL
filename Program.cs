// See https://aka.ms/new-console-template for more information

using SCL;

Lex lex = new Lex();


string s = File.ReadAllText("source/SCLIO.scl");

var list = lex.Analyze(s);


Par par = new Par();
ASTNode node = par.Parse(list);

//var postFixList = ShuntingYard.ConvertToPostfix(list);


Console.WriteLine(list);



//Hello

