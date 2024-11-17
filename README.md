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
This function processes a block of text, breaking it into individual lines and collecting them into a list of symbols. It uses a `StringReader` to read the text from a stream and processes each line with the `AnalyzeLine` function.

### `ParseDoubleToken`
This function checks if two consecutive characters form a valid double token (e.g., `==`). It combines the characters and returns the corresponding symbol.

### `AnalyzeLine`
This function processes a single line of text, character by character. It identifies strings, numbers, and symbols, skipping whitespaces and adding constants when necessary. The function collects tokens into a list, which is returned to the main Lexer.

### `Map`
The `Map` function takes a string as input and returns the corresponding symbol. It uses a switch statement to match the string against keywords, data types, and operators. If no match is found, it defaults to returning the type `name`.






   
