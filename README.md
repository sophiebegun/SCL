# Intro

Building a language is an essential skill that can significantly enhance your coding abilities. Not only does it help you write more efficient code, but it also improves your understanding of existing programming languages. Moreover, it opens the door to innovating on existing languages, creating new ideas that other coders can use. 

Older languages often feature more complicated syntax, making them harder to learn. By designing a simpler syntax, you can make it easier for others to pick up and understand.

A key feature of SCL is that it clearly demonstrates each aspect of **Turing Completeness**. In a Turing complete language, **state**, **conditionality**, and **looping** combine to provide the computational power to express any algorithm. These concepts are crucial because they allow the language to represent computations that any Turing machine can perform. Without any one of these elements, the language would be unable to express certain computations, thus failing to meet the criteria for Turing completeness.

There are four primary components in a programming language:
1. **Lexer**
2. **Parser**
3. **Interpreter**
4. **Compiler**

### Lexer
The **Lexer** is responsible for breaking the code into symbols or tokens. These tokens can represent various elements like operators (`=`, `==`) and variables, and help simplify the parsing process.

### Parser
The **Parser** processes these tokens and creates an **Abstract Syntax Tree (AST)**, a hierarchical representation of the program structure. It ensures that the code follows the grammar of the language.

### Interpreter
An **Interpreter** directly executes the instructions without compiling them into machine code. It processes the code line by line, following the program's flow. While interpreters tend to be slower than **Compilers**, which convert the entire codebase into machine code, they can still provide faster feedback during development.

# Language Features

SCL simplifies some of the core programming constructs with single-letter abbreviations for **state**, **conditionality**, and **looping**:

- **State (S)**: Used to represent variables and data storage.
- **Conditionality (C)**: Used for conditional statements (`if`, `else`).
- **Looping (L)**: Used for loops (`for`, `while`).

### Syntax Examples

- **State**: To create a variable:

- **Conditionality**: For an `if` statement:

- **Looping**: For a loop statement:

### Data Structures

SCL supports several built-in data structures, each with simple syntax:

- **Hashmap (hmap)**: Similar to a dictionary, it stores key-value pairs.

- **Hashset (hset)**: Similar to a set, it stores unique keys.

- **List (lst)**: A simple list structure.

### Example Code

This code demonstrates creating a list, adding items, removing an element, updating an item, and then looping through the list to print each value:
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


### Built-in Functions

SCL includes several built-in functions, all prefixed with `F`:

- **add()**: Adds an element to a list, hashset, or hashmap.

- **get()**: Retrieves an element from a list, hashset, or hashmap.

- **set()**: Assigns a value to an element in a list or hashmap.

- **rem()**: Removes an element from a list, hashset, or hashmap.

### Expressions

Expressions are combinations of variables and values that evaluate to a single result. They can be arithmetic, conditional, or more complex combinations of **state**, **conditionality**, and **looping**. In a **C** (conditional) statement, a boolean expression must be provided, or the code will fail to compile.

# Lexer

The **Lexer** is responsible for breaking down the raw code into tokens, making it easier for the **Parser** to process. The Lexer includes several key functions:

### `Analyze`
Essentially, this method processes a block of text by breaking it down into individual lines and collects the results in a list of symbols. A new list is made to store those symbols and a StringReader is used to read the string from a stream and calls “AnalyzeLine” to process those lines. Those lines are added to a temporary list, which is then added to the main list.  

### `ParseDoubleToken`
This method checks if the characters at a certain index are valid double tokens, meaning they have 2 symbols. It takes a string and an index as parameters, and returns a Symbol. It checks if the index is the last character of the line, and returns null if true. Then, it combines the character at the index with the next character to form a double token. A switch statement is carried out, which returns a new symbol based on token value. 

### `AnalyzeLine`
This function processes a line of text character by character, showing string double tokens, numbers, identifiers, individual characters, accumulating the results in a list of Symbol objects. It skips whitespaces and uses a StringBuilder to collect characters between quotes, adding a “const” symbol to the list. It processes letters and numbers, using another StringBuilder to form tokens, mapping them to corresponding symbols. If a digiti/dot is shown, it collects the number and creates a new “number” symbol. It also checks for double tokens, and adds them to the list. It returns the list of Symbols.

### `Map`
This simply takes a string s as an input and returns the corresponding symbol based on the cases. It uses a switch to match the string against keywords, data types, and operators. If 	it does not match with any of the cases, the function returns the type “name.”






   
