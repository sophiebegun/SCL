# Intro

Building a language is important for your coding skills, helping you write more efficient code and better understand existing languages. Moreover, it allows you to innovate on existing languages, creating new ideas for other coders to use. Some older languages may have a more complicated syntax, which may cause complexities in learning it. Therefore, designing a simpler syntax would facilitate the learning process to others.	

A feature of SCL clearly shows each aspect of Turing Completeness. In a Turing complete language, state, conditionality, and looping combine to give the language the ability to carry out . State holds the data that the memory a program changes or stores, conditionality allows for making decisions based on the current state, and looping enables repetition, which can be used in a quicksort, for example. These three concepts are fundamental because they allow the language to represent algorithms that can compute anything that a Turing machine can compute. Without any one of these elements, the language would be unable to express certain computations, thus failing to meet the criteria for Turing completeness.

There are four components to a computer language, which include the lexer, parser, interpreter, and compiler.. The Lexer is essentially a symbol creator; it breaks the code into symbols, or tokens. Some symbols may include an equal sign (=) or a comparison operator (==). Furthermore, it simplifies the process for the parser by converting raw code into pieces that are simpler to understand. This can be compared to a “split” function, as it breaks up a sentence into it’s own pieces to process each line. The Parser takes the tokens created by the Lexer and creates an AST, an abstract syntax tree. An AST is a structure designed to show how code is layed out; it breaks down the code into parts and shows how they are related. Moreover, each part of a tree has one parent and several children nodes, distinguishing the roots and branches of the tree. Its main purpose is to make sure the code follows the grammar of the language, which was indicated by Lexer. An interpreter directly executes instructions in the code without converting it to machine code, which is binary. It processes each piece of code line by line, following the program’s flow directly. This makes it slower than a compiler, as that takes the entire source code and turns it into machine code. Moreover, a compiler improves the code to make it run faster and use less memory. 

# Language Features

Many other coding languages are known to have key words to denote conditionality, looping, and state. SCL does that but on a simpler level. For example, S represents state, C represents conditionality, and L represents looping. Those 3 symbols are always used throughout the language to initialize variables, loops, or if statements. 

* When creating a variable “S int a = 0” would be an appropriate syntax. 
* When showing an if statement or any sort of conditionality “C(‘boolean expression’)” would be appropriate syntax. 
* When showing a loop statement “L(‘boolean expression’)” would be appropriate syntax.
Moreover, to keep consistency and simplicity throughout the language, denoting a function with “F” and inputs and outputs as “O” and “I” respectively, allow a coder to keep organized.

As any other language, SCL has lists, hashmaps, hashsets, and speciality functions. 
* A hashmap, or hmap in SCL, behaves the same way as a dictionary, including a key and a value 
* **Creating an hmap, like any form of state, would include an “S’ at the beginning to denote that you are using state. After the S would come the datatype, in this case hmap, and then the datatypes of both the key and the value, respectively. Lastly, the name is indicated.  Ex: S hmap int str m* 
* A hashset, or hset in SCL, behaves the same way as a hashset, using a key. 
* **Creating an hset, like any form of state, would include an “S’ at the beginning to denote that you are using state. After the S would come the datatype, in this case hset, and then the datatype used. Lastly, the name is indicated.  Ex: S hset int h* 
* A list, or lst in SCL, behaves the same way as a list.
* **Creating a lst, like any form of state, would include an “S’ at the beginning to denote that you are using state. After the S would come the datatype, in this case lst, and then the datatype used. Lastly, the name is indicated.  Ex: S lst int l*
* **This is some sample code, which creates list l and adds 4 strings to it. Then it removes the third time, “hello3” and updates the second item to “helo2a”. The loop goes through each item in the list and prints each value.*
F add(l,"hello1")
F add(l,"hello2")
F add(l,"hello3")
F add(l,"hello4")
F rem(l,2)
F set(l,1,"hello2a")
S int i = 0
S str s = ""
L (i < count(l))
{
S s = get(l,i)
O s
S i = i + 1
}

 In addition to several key data types, SCL includes specialized functions like add(), get(), set(), rem() to add, receive, assign, and remove elements from all 3 data types above.
** there must be an F in front of all built in functions
* The add() function requires the name of the list,hashset, or hashmap and what you’re adding to it. 
* **In a hashmap you must specify your key as well*
* **Ex: F add(m,2,”hello2”)*
* The get() function allows you to receive an element from a list, hashset, or hashmap.Requires name of the list and position you plan on getting element.
* **Ex: F get(l, i)*
* The set() function acts as an assignment operator, because SCL does not have an “=” like c#, for example.
* **Ex: F set(l, i)*

Expressions are used in any one of these features, and show a combination of variables and values that evaluate to a single value. In SCL, they can be arithmetic or conditional or can consist of more complex state, conditionality, and looping. For example, in a ‘C’ statement there must be a conditional, or boolean expression, or it will not compile.

