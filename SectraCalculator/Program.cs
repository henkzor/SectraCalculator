using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SectraCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            StartProgram();
            RunProgram();
            EndProgram();
        }

        private static void StartProgram()
        {
            //Empties the .txt-file used for logging incorrect input before running the program
            File.WriteAllText(@".\..\..\..\..\Incorrect input.txt", String.Empty);

            InitialPrint();

            void InitialPrint()
            {
                Console.WriteLine("Welcome to the calculator!");
                Console.WriteLine("Please enter a command in the form of:");
                Console.WriteLine("<register> <operation> <value>");
                Console.WriteLine();
                Console.WriteLine("An example: A add 2");
            }
        }

        private static void RunProgram()
        {
            //A list for the Querys that are going to be evaluated lazily
            List<Query> queryList = new List<Query>();

            //A list for the different Registers used and their values
            List<Register> registerList = new List<Register>();

            bool shouldLoopContinue = true;
            string[] inputArgsList = null;

            //Variables used for determining if there is a file used for user input and for
            //handling that input
            var fileInputArgs = Environment.GetCommandLineArgs();
            bool isInputFromFile = (fileInputArgs.Length == 2);
            int fileInputListCounter = 0;
            if (isInputFromFile)
            {
                try
                {
                    inputArgsList = File.ReadAllLines($@".\..\..\..\..\{fileInputArgs[1]}");
                }
                catch
                {
                    Console.WriteLine("Could not find file!");
                    Console.WriteLine("Write your querys below:");
                    isInputFromFile = false;
                }
            }

            //Loop that continously takes input and performs actions until "quit" is the input
            while (shouldLoopContinue)
            {
                Query currentQuery = new Query();

                TakeUserInput(currentQuery);

                HandleInput(currentQuery);

            }

            //A method that takes input and validates it, returns a valid query
            void TakeUserInput(Query currentQuery)
            {
                bool correctInput = false;
                string userInput = "";
                string[] inputParts = null;
                string errorMessage = "";
                currentQuery.IsValueARegister = false;

                while (!correctInput)
                {
                    //Logic for handling the file input querys
                    if (isInputFromFile)
                    {
                        if (fileInputListCounter >= inputArgsList.Length)
                        {
                            userInput = "quit";
                        }
                        else
                        {
                            userInput = inputArgsList[fileInputListCounter];
                            fileInputListCounter++;
                        }
                    }
                    else
                    {
                        userInput = Console.ReadLine();
                    }

                    // Using the ToUpper() method makes the entire program case insensitive
                    inputParts = userInput.ToUpper().Split(" ");

                    //Validation that checks if the user input is valid, prints an error message if not
                    if (userInput == "")
                    {
                        errorMessage = "Input can't be empty";
                    }
                    else if (inputParts.Length == 1)
                    {
                        if (inputParts[0] == "QUIT")
                        {
                            correctInput = true;
                            currentQuery.Operation = inputParts[0];
                        }
                        else
                        {
                            errorMessage = "Invalid input";
                        }
                    }
                    else if (inputParts.Length == 2)
                    {
                        if (inputParts[0] == "PRINT")
                        {
                            if (IsValidRegisterName(inputParts[1]))
                            {
                                correctInput = true;
                                Register tempRegister = new Register();
                                tempRegister.Name = inputParts[1];

                                currentQuery.Register = tempRegister;
                                currentQuery.Operation = inputParts[0];
                            }
                            else
                            {
                                errorMessage = "Invalid register name";
                            }
                        }
                        else
                        {
                            errorMessage = "Invalid input";
                        }
                    }
                    else if (inputParts.Length == 3)
                    {
                        if (IsValidRegisterName(inputParts[0]))
                        {
                            if (inputParts[1] == "ADD" ||
                                inputParts[1] == "SUBTRACT" ||
                                inputParts[1] == "MULTIPLY")
                            {
                                if (inputParts[2].All(char.IsLetterOrDigit))
                                {
                                    if (!(inputParts[0] == inputParts[2]))
                                    {
                                        correctInput = true;

                                        Register tempRegister = new Register();
                                        tempRegister.Name = inputParts[0];

                                        currentQuery.Register = tempRegister;
                                        currentQuery.Operation = inputParts[1];

                                        if (inputParts[2].Any(x => char.IsLetter(x)))
                                        {
                                            currentQuery.IsValueARegister = true;
                                            currentQuery.ValueRegister = inputParts[2];
                                        }
                                        else
                                        {
                                            currentQuery.ValueInt = int.Parse(inputParts[2]);
                                        }
                                    }
                                    else
                                    {
                                        errorMessage = "Register and value cannot be identical";
                                    }
                                }
                                else
                                {
                                    errorMessage = "Value must be integer or a register";
                                }
                            }
                            else
                            {
                                errorMessage = "Invalid input";
                            }
                        }
                        else
                        {
                            errorMessage = "Register must be alphanumerical and contain at least one letter";
                        }
                    }
                    else
                    {
                        errorMessage = "Too many input arguments";
                    }
                    if (!correctInput)
                    {
                        if (!isInputFromFile)
                        {
                            Console.WriteLine($"{errorMessage}, try again!");
                        }

                        using (StreamWriter incorrectInputSW = File.AppendText(@".\..\..\..\..\Incorrect input.txt"))
                            incorrectInputSW.WriteLine(userInput);
                    }

                    bool IsValidRegisterName(string inputString)
                    {
                        return (inputString.All(char.IsLetterOrDigit) &&
                                        inputString.Any(x => char.IsLetter(x)));
                    }
                }
            }
            //A method that takes a valid query, decides how to handle it, and handles it
            void HandleInput(Query currentQuery)
            {
                //Checks whether the value of current query is a register while the operation is not print,               
                //and if so, puts it in the queryList for lazy evaluation
                if (currentQuery.IsValueARegister && currentQuery.Operation != "PRINT")
                {
                    queryList.Add(currentQuery);
                }
                else
                {
                    //Determines wether the register in the current query already exists in the registerList,
                    //this affects how the query is handled later
                    bool registerExists = false;
                    if (currentQuery.Operation != "QUIT")
                    {
                        if (registerList.Any(r => r.Name == currentQuery.Register.Name))
                        {
                            registerExists = true;
                        }
                    }

                    PerformOperation(currentQuery);

                    void PerformOperation(Query inputQuery)
                    {
                        switch (inputQuery.Operation)
                        {
                            //The three first cases, "ADD", "SUBTRACT" and "MULTIPLY" are very similar and it should
                            //probably be possible to write a method that takes the mathematical operator as an input
                            //and only changes in the few places where it is needed, this would make it possible to reduce
                            //the amount of code-lines drastically. However I did not manage to figure out how to do
                            //this so I decided to keep it like this. It still works, eh?
                            //The same is true in the LazyEvaluation method.
                            case "ADD":
                                if (registerExists)
                                {
                                    registerList.Find(r => r.Name == inputQuery.Register.Name).Value += inputQuery.ValueInt;
                                }
                                else
                                {
                                    inputQuery.Register.Value += inputQuery.ValueInt;
                                    registerList.Add(inputQuery.Register);
                                }
                                break;

                            case "SUBTRACT":
                                if (registerExists)
                                {
                                    registerList.Find(r => r.Name == inputQuery.Register.Name).Value -= inputQuery.ValueInt;
                                }
                                else
                                {
                                    inputQuery.Register.Value -= inputQuery.ValueInt;
                                    registerList.Add(inputQuery.Register);
                                }
                                break;

                            case "MULTIPLY":
                                if (registerExists)
                                {
                                    registerList.Find(r => r.Name == inputQuery.Register.Name).Value *= inputQuery.ValueInt;
                                }
                                else
                                {
                                    inputQuery.Register.Value *= inputQuery.ValueInt;
                                    registerList.Add(inputQuery.Register);
                                }
                                break;

                            case "PRINT":
                                CheckQLForRegister(inputQuery.Register);

                                Console.WriteLine($"Register: {inputQuery.Register.Name}");
                                if (!registerList.Any(r => r.Name == inputQuery.Register.Name))
                                {
                                    Console.WriteLine("Value does not exist");
                                }
                                else
                                {
                                    Console.WriteLine($"Value: {registerList.Find(r => r.Name == inputQuery.Register.Name).Value}");
                                }

                                break;

                            case "QUIT":
                                shouldLoopContinue = false;
                                break;


                                //A method called before print, determining whether lazy evaluation should be used                                
                                void CheckQLForRegister(Register inputRegister)
                                {
                                    if (queryList.Any(q => q.Register.Name == inputRegister.Name))
                                    {
                                        //Listing all occurences of the register in the left hand side of the QueryList and 
                                        //performing lazy evaluation for each of them
                                        List<Query> tempQueryList = queryList.FindAll(q => q.Register.Name == inputRegister.Name);
                                        foreach (var tempQuery in tempQueryList)
                                        {
                                            //Checking if the right hand side of the query exists as the left hand side somewhere
                                            //else in the QueryList and in that case, iteratively uses the CheckQLForRegister
                                            //method to perform those operations first.
                                            if (queryList.Any(q => q.Register.Name == tempQuery.ValueRegister))
                                            {
                                                CheckQLForRegister(queryList.Find(q => q.Register.Name == tempQuery.ValueRegister).Register);
                                            }
                                            LazyEvaluation(tempQuery);
                                        }
                                    }
                                    void LazyEvaluation(Query lazyQuery)
                                    {
                                        switch (lazyQuery.Operation)
                                        {
                                            case "ADD":
                                                //A check to see if the register already exists in the registerList
                                                if (registerList.Any(r => r.Name == lazyQuery.Register.Name))
                                                {
                                                    //A check to see if the register on the right hand side of the equation
                                                    //exists in the registerList. If it doesn't, no action is taken.
                                                    if (registerList.Any(r => r.Name == lazyQuery.ValueRegister))
                                                    {
                                                        registerList.Find(r => r.Name == lazyQuery.Register.Name).Value +=
                                                        registerList.Find(r => r.Name == lazyQuery.ValueRegister).Value;
                                                    }
                                                }
                                                else
                                                {
                                                    Register tempRegister = new Register()
                                                    {
                                                        Name = lazyQuery.Register.Name
                                                    };

                                                    if (registerList.Any(r => r.Name == lazyQuery.ValueRegister))
                                                    {
                                                        tempRegister.Value = registerList.Find(r => r.Name == lazyQuery.ValueRegister).Value;
                                                    }

                                                    registerList.Add(tempRegister);
                                                }
                                                break;

                                            case "SUBTRACT":
                                                if (registerList.Any(r => r.Name == lazyQuery.Register.Name))
                                                {
                                                    if (registerList.Any(r => r.Name == lazyQuery.Register.Name))
                                                    {
                                                        registerList.Find(r => r.Name == lazyQuery.Register.Name).Value -=
                                                        registerList.Find(r => r.Name == lazyQuery.ValueRegister).Value;
                                                    }
                                                }
                                                else
                                                {
                                                    Register tempRegister = new Register()
                                                    {
                                                        Name = lazyQuery.Register.Name
                                                    };
                                                    if (registerList.Any(r => r.Name == lazyQuery.ValueRegister))
                                                    {
                                                        tempRegister.Value = -registerList.Find(r => r.Name == lazyQuery.ValueRegister).Value;
                                                    }
                                                    registerList.Add(tempRegister);
                                                }

                                                break;

                                            case "MULTIPLY":
                                                if (registerList.Any(r => r.Name == lazyQuery.Register.Name))
                                                {
                                                    if (registerList.Any(r => r.Name == lazyQuery.Register.Name))
                                                    {
                                                        registerList.Find(r => r.Name == lazyQuery.Register.Name).Value *=
                                                        registerList.Find(r => r.Name == lazyQuery.ValueRegister).Value;
                                                    }
                                                }
                                                else
                                                {
                                                    Register tempRegister = new Register()
                                                    {
                                                        Name = lazyQuery.Register.Name,
                                                        Value = 0
                                                    };

                                                    registerList.Add(tempRegister);
                                                }
                                                break;
                                        }
                                    }
                                }
                        }
                    }
                }
            }
        }

        private static void EndProgram()
        {
            Console.WriteLine("Thanks for using the calculator");

            //A check to see if the file containing incorrect input has something in it
            if (new FileInfo(@".\..\..\..\..\Incorrect input.txt").Length != 0)
            {
                string[] incorrectInputQueryArray = File.ReadAllLines(@".\..\..\..\..\Incorrect input.txt");
                Console.WriteLine();
                Console.WriteLine("The following input querys were invalid and were ignored");

                foreach (string incorrectInputQuery in incorrectInputQueryArray)
                {
                    Console.WriteLine(incorrectInputQuery);
                }
            }
        }
    }
}
