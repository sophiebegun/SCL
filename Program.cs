// See https://aka.ms/new-console-template for more information

using SCL;

Lex lex = new Lex();


string s = File.ReadAllText("expr.scl");

var list = lex.Analyze(s);
var postFixList = ShuntingYard.ConvertToPostfix(list);


Console.WriteLine(list);



//Hello

//Sample programs converted from C# to SCL


//C#
//public static bool isPrime(int num)
// {
//     // Loop to check for factors of the number
//     for (int i = 2; i < num; i++)
//     {
//         if (num % i == 0)
//         {
//             return false; // If any factor found, the number is not prime, return false
//         }
//     }
//     return true; // If no factors found, the number is prime, return true
// }

//SCl
//F(isPrime int n: bool)
//{
    //L 2 < num
    //{
        //C n % 2 == 0
        //{
            //# false
        //}
    //}
    //# true

//}


//public static int Add(int a, int b)
    //{
    //    //int r = a + b;
    //    //cw(r);
    //}


//F(Add int a, int b: int)
    {
        //s int r
        //a + b
        //O S 
    }