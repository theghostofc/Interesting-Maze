using System;
using System.Collections.Generic;
using System.Linq;

namespace InterestingMaze
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] inputs = GetUserInputs();
            Maze m = new Maze(inputs[0], inputs[1], inputs[2]);
            Console.WriteLine(m.Draw());

            List<string> paths = m.Solve();
            Next("Finding ways to go out...");
            if (paths.Any())
            {
                Next("Some of the possible way(s) out:", ConsoleColor.Yellow, false);
                foreach (string str in paths)
                {
                    Console.WriteLine(str);
                    Console.WriteLine("--------------");
                }

                Next("One of the quickest way out is:", ConsoleColor.Yellow);
                Console.WriteLine(paths[0]);
                if (paths[0].Length == 3)
                {
                    Next("You've been lucky, standing on one of the exits of the maze!", ConsoleColor.Green, false);
                }
            }
            else
            {
                Next("Ouch, it's a blocked maze!\nThere seems no way out, better luck next time!", ConsoleColor.Red, false);
            }
        }

        static int[] GetUserInputs()
        {
            bool flag = true;
            string lastError = string.Empty;

            int i = 0;
            int minSize = 2;
            int maxSize = 10;
            int[] inputs = new int[3];

            string[] messages = new string[3];
            messages[0] = "What size your maze would be (Integer > 2 and < 10) : ";
            messages[1] = "Which row would you like to start from (Integer > 0) : ";
            messages[2] = "Which column would you like to start from (Integer > 0) : ";
            string errorMessage = "Please enter an integer value > {0} and < {1}";

            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Maze!!\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(lastError);
                Console.ResetColor();
                Console.Write(messages[i]);
                string input = Console.ReadLine();

                if (!Int32.TryParse(input, out inputs[i]) || inputs[i] <= minSize || inputs[i] >= maxSize)
                {
                    lastError = string.Format(errorMessage, minSize, maxSize);
                    continue;
                }

                lastError = string.Empty;

                i++;
                if (i > 0)
                {
                    minSize = 0;
                    maxSize = inputs[0];
                }

                flag &= i < 3;

            } while (flag);

            Console.Clear();
            Console.WriteLine("Welcome to the Maze!!\nThis is how it looks like...");
            return inputs;
        }

        static void Next(string message, ConsoleColor color = ConsoleColor.Gray, bool keyPress = true)
        {
            if (keyPress)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }

            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
