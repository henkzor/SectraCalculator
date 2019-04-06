using System;
using System.Collections.Generic;
using System.Linq;

namespace SectraCalculator
{
    class Program
    {
        

        static void Main(string[] args)
        {
            //Lite förklarande text för alla delar
            StartProgram();
            RunProgram();
            EndProgram();

            //Connecta till git via VS
            //Skriv att jag väljer att göra lite tydligare outputs för jag tycker det är mer givande, eller strunta i det?
            //Konstigt att även en siffra kan vara register, är det så dom menar?
            //Kolla att första input-parametern är en string, gör den till en int
            //Kolla att sista input-parametern är en int, om den är sträng så lägg till den för lazy evaluation
            //Lägg in typ "see instructions" med readkey
            //Dela upp sträng med mellanslag som delningsoperator
            //Lazy evaluation, vad är alternativet?
            //Alla bokstäver och siffror godkänns som variabelnamn, ÅÄÖ?
            //Case insensitive
            //Logga inkorrekt input, logga var?
            //Filhantering
            //Behövs alla förklarande kommentarer jag lagt in?
            //Lägga över förklaring i README?
            //Gör ett antagande om negativa nummer och förklara det i README
            //Vill jag göra validering i en egen metod eller i takeuserinput() ?
            //Göra en klass för register med prop name och value
            //Lazily evaluation, om man har querys med 
            //a add c
            //a multply b
            //a subtract d
            //Så spelar det ju stor roll i vilken ordning dessa görs, ska man lägga dem i inputordning... typ?

            //Felhantering:
            /*
             * Variabel som inte finns
             * Action som inte finns
             * 
             * 
             */

        }
        private static void StartProgram()
        {
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
            List<Query> queryList = new List<Query>();
            List<Register> registerList = new List<Register>();
            Query currentQuery = new Query();
            bool shouldLoopContinue = true;

            while(shouldLoopContinue)
            {
                TakeUserInput();

                PerformOperation();

            }


            // I use nestled methods here as I find it more intuitive to have methods
            // grouped with what part of the program they are associated with, rather 
            // than putting them all at the end

            void TakeUserInput()
            {
                string userInput = "";
                bool correctInput = false;
                string[] inputParts = null;
                string errorMessage = "";

                while (!correctInput)
                {
                    userInput = Console.ReadLine().ToUpper();

                    if (userInput == "")
                    {
                        errorMessage = "Input can't be empty";
                    }

                    
                    inputParts = userInput.Split(" ");

                    //Dela upp dessa if-satser i fler så att man kan ge mer specifika felmeddelanden
                    
                    if (inputParts.Length == 1)
                    {
                        if ( inputParts[0] == "QUIT")
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
                            correctInput = true;

                            currentQuery.Operation = inputParts[0];
                        }
                        else
                        {
                                
                            errorMessage = "Invalid input";
                        }
                        //Kolla här om inputparts[1] är en existerande register, annars ge fel, 
                        //Gör antagande om att om man vill printa en icke existerande variabel så ger jag hellre 
                        //fel än att printa 0, vilket skulle vara alternativet
                    }
                    else if (inputParts.Length == 3)
                    {
                        if (inputParts[0].All(char.IsLetterOrDigit))
                        {
                            if (inputParts[1] == "ADD" || 
                                inputParts[1] == "SUBTRACT" || 
                                inputParts[1] == "MULTIPLY")
                            {
                                int i;
                                if (int.TryParse(inputParts[2], out i))
                                {
                                    //Lägg in koll så att tredje delen av arrayen också är ok
                                    //den ska vara siffra eller register
                                    correctInput = true;

                                    Register tempRegister = new Register();

                                    tempRegister.Name = inputParts[0];

                                    currentQuery.Register = tempRegister;
                                    currentQuery.Operation = inputParts[1];
                                    currentQuery.Value = int.Parse(inputParts[2]);
                                }
                                else
                                {
                                    errorMessage = "Value must be integer or an already registered register";
                                }
                            }
                        }
                        else
                        {
                            errorMessage = "Register must be an alphanumerical character";

                        }

                     }
                    else
                    {
                        errorMessage = "Too many input arguments";
                    }
                    
                    if (!correctInput)
                    {
                        Console.WriteLine("Incorrect input, try again!");
                        Console.WriteLine(errorMessage); 
                        //Logga felaktiga inputen
                    }
                    
                }
            }
            void PerformOperation()
            {
                bool registerExists = false;
                if (registerList.Any(r => r.Name == currentQuery.Register.Name))
                {
                    //currentQuery.Register.Value = registerList.Find(r => r.Name == currentQuery.Register.Name).Value;
                    registerExists = true;
                }

                switch (currentQuery.Operation)
                {

                    //Kolla om register i query finns i listan, isåfall updpatera värdet där, annars gör nytt
                    case "ADD":
                        if (registerExists)
                        {
                            registerList.Find(r => r.Name == currentQuery.Register.Name).Value += currentQuery.Value;
                        }
                        else
                        {
                            currentQuery.Register.Value += currentQuery.Value;
                            registerList.Add(currentQuery.Register);
                        }
                        break;

                    case "SUBTRACT":
                        if (registerExists)
                        {
                            registerList.Find(r => r.Name == currentQuery.Register.Name).Value -= currentQuery.Value;
                        }
                        else
                        {
                            currentQuery.Register.Value -= currentQuery.Value;
                            registerList.Add(currentQuery.Register);
                        }
                        break;

                    case "MULTIPLY":
                        if (registerExists)
                        {
                            registerList.Find(r => r.Name == currentQuery.Register.Name).Value *= currentQuery.Value;
                        }
                        else
                        {
                            currentQuery.Register.Value *= currentQuery.Value;
                            registerList.Add(currentQuery.Register);
                        }
                        break;

                    case "PRINT":
                        //Enligt uppgiften ska man bara printa value och inte name på register
                        Console.WriteLine($"Register: {registerList.Find(r => r.Name == currentQuery.Register.Name).Name}");
                        Console.WriteLine($"Value: {registerList.Find(r => r.Name == currentQuery.Register.Name).Value}");

                        break;

                    case "QUIT":
                        shouldLoopContinue = false;
                        break;

                    default:
                        break;
                }
                
                
            }
        }

        private static void EndProgram()
        {
            Console.WriteLine("Thanks for using the calculator");
        }
    }
}
