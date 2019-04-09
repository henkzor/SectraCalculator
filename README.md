# SectraCalculator
Programming challenge for Sectra


The calculator, how it works:
- An initial, welcoming and explaining print is made
- A check is made to see if there are any command line arguments that should be used in the program.
- If there is a command line input that directs to a file, the file is read and used as input in the program.
- If there is not, the user is asked to write querys as input.
- The input is validated and if it is invalid it is ignored and logged to a file, otherwise it is used.
- Different actions are carried out depending on what the user input is.
- When the user input is "quit", the program stops and if there are any invalid inputs stored, these are printed.


How to set up the program:
 The calculator is written in C#. With visual studio, open the solution and run it with F5.

 In order to start the program with command line arguments, open the "properties"-window, either by searching for 
it in the quick start (Ctrl+q) or by clicking the "project"-tab and selecting properties in the menu. 
 Then go into "debug" on the left side menu and here in the "Application arguments" textbox you can write the name 
of the file (including filetype ending, like .txt) that you want the program to read your input from. Remember that 
this is case sensitive.
 Place the file in the main folder, the same folder where this README.md file is.
 There is a file called InputQuerys.txt already in the folder that can be used, or you could create a new one.
 

Design choices/assumptions:
- I use nestled methods in many places as i find it more intuitive to have methods grouped with what part of the 
program they are associated with, rather than putting them all at the end. 

- I use the three main methods 'StartProgram()', 'RunProgram()' and 'EndProgram()' as a rough frame for how the 
program is running. I find this makes it easier to get an overview of the program and it helps with where I should
place the different methods etc. In this case both StartProgram() and EndProgram() are much shorter than 
'RunProgram()' and so it might seem a bit redundant to do it like this, but I still find it to be good practice.

- The instruction says "Any name consisting of alphanumeric characters should be allowed as register names."
I have however chosen to not allow register names consisting of only numbers since it to me is non-intuitive and could
lead to situations like the following:
1 add 2
Where this would mean that the value 2 would be stored in a register with the name '1', which would make the following 
query unclear:
a add 1
Would the register 'a' in this case get the value 2, since the register '1' has the value 2, or would 'a' get the value
1, since that is the integer that is written there?

- If the program is asked to print a register that doesn't exist, a message is shown saying that the value does not 
exist. I chose this approach over showing the value 0, as I found it more intuitive.

- If a register is used as a value in a query, and at the time of evaluation that register does not exist, no operation 
is carried out, this is done ahead of quitting the program a giving an error message.

- I made the output for the "print"-action a bit more elaborate compared to what was specified in the instructions as I 
found it more intuitive.

- If one would want to add say, a division-operator, this could be done quite easily by adding another "or"-clause in the
if-statement TakeUserInput method, accepting "DIVIDE" as an operation as well as "ADD", "SUBTRACT" and "MULTIPLY". Then 
also adding another case in the switch-case in the PerformOperation method as well as the one in the LazyEvaluation method, 
essentially copying one of the cases but using the division-operator.
(as I commented there, I know that part of the code could be made much shorter by adding a method that takes a mathematical 
operator as a parameter and uses it in the code in a few places, but I could not find an easy way to do this and let it 
stay this way due to not wanting to delay the hand-in of the project much more)


General thoughts and comments about the project:
During the design of the program I came across many challenges that i hadn't anticipated during the planning stage. This
led to me having to add functionality and validation somewhat unstructured as the design went on. This led to the code 
being less structured and DRY than I would have prefered, I worked on cleaning it up but due to the time factor I chose to 
accept the current state as satisfactory.

I have tried to be thorough while commenting the code where I have thought it necessary, but it is difficult to know how 
much commenting is needed and how much is too much. 

I have included quite a lot of validation but I still feel there could a lot more added, I have however chosen this to be
satisfactory, knowing that one could spend huge amounts of time in order to secure exactly every aspect of the program.