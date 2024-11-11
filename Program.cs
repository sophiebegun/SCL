// See https://aka.ms/new-console-template for more information

using SCL;

//Console.WriteLine("C#");
//List<int> l = new List<int> { 300, 200, 100};

//MergeS.MergeSort(l, 0, l.Count - 1);
//MergeS.Print(l);

//Console.ReadLine();


Lex lex = new Lex();


string s = File.ReadAllText("source/count_freq.scl");

var list = lex.Analyze(s);


Par par = new Par();
ASTNode node = par.Parse(list);

Console.WriteLine(list);



Inter inter = new Inter(node, par.FDs);
inter.Evaluate();


//Hello

