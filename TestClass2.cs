using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace InClassPractice
{
    class App
    {
        private List<string> task = new List<string>();
        private List<bool> isActioned = new List<bool>();

        private int selectedTask = 0;

        const int pageLength = 20;
        public App()
        {
            Console.OutputEncoding = Encoding.Unicode;
            
            ReadListFromFile();
            Console.WriteLine("\n\n");
            Run();
        }

        public void Run()
        {
            bool isTaskEmpty = !task.Any();
            bool isActEmpty = !isActioned.Any();
            if (isTaskEmpty && isActEmpty)
            {
                Console.Write("\nThe list is empty! ");
                InputTaskToList();
               
            }
            else
            {
                bool quit;
                do
                {

                    RemoveFirstActionedItems();
                    PrintTaskList();
                    var key = RunInputCycle();
                    quit = HandleUserInput(key);
                } while (!quit);

                WriteListToFile();
                Console.WriteLine(); // Cleans up "press any key to quit..." is on its own line
            }
        }

        private void RemoveFirstActionedItems()
        {            
                while (isActioned[0])
                {
                    task.RemoveAt(0);
                    isActioned.RemoveAt(0);
                    selectedTask -= 1;
                }
                if (selectedTask < 0)
                {
                    selectedTask = 0;
                }            
        }

        private ConsoleKey RunInputCycle()
        {
            ConsoleKey key;

            PrintUsageOptions();
            key = GetInputFromUser();

            return key;
        }

        private bool HandleUserInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.A:
                    InputTaskToList();
                    break;
                case ConsoleKey.D:
                    DeleteCurrentlySelectedTask();
                    break;
                case ConsoleKey.N:
                    SelectNextPage();
                    break;
                case ConsoleKey.DownArrow:
                    SelectNextUnactionedTask();
                    break;
                case ConsoleKey.Enter:
                    WorkOnSelectedTask();
                    break;
                case ConsoleKey.Q:
                    return true;
                    
            }
            return false;
        }

        private void SelectNextPage()
        {
            var page = GetPage();
            selectedTask = FirstElementInPage(page + 1);
            SelectNextUnactionedTask();
        }

        private void WorkOnSelectedTask()
        {
            bool valid = false;
            do
            {
                Console.Clear();
                Console.WriteLine($"Working on: {task[selectedTask]}");
                Console.WriteLine("r: re-enter | c: complete | q: cancel");
                Console.Write("Input: ");

                var key = GetInputFromUser();

                switch (key)
                {
                    case ConsoleKey.R:                       
                        ReenterTask();
                        valid = true;
                        break;
                    case ConsoleKey.C:
                        DeleteCurrentlySelectedTask();
                        valid = true;
                        break;
                    case ConsoleKey.Q:
                        valid = true;
                        break;

                }
            } while (!valid);
        }

        private void ReenterTask()
        {
            AddTaskToList(task[selectedTask]);
            DeleteCurrentlySelectedTask();
        }

        private void DeleteCurrentlySelectedTask()
        {
            isActioned[selectedTask] = true;
            SelectNextUnactionedTask();
        }

        private void SelectNextUnactionedTask()
        {
            bool overflowed = false;
            do
            {
                selectedTask += 1;

                if (selectedTask >= isActioned.Count)
                {
                    selectedTask = 0;
                    overflowed = true;
                }
            } while (!overflowed && isActioned[selectedTask]);


        }

        private ConsoleKey GetInputFromUser()
        {
            return Console.ReadKey().Key;
        }

        private void PrintUsageOptions()
        {
            Console.WriteLine("a: add | n: next page | d: delete | Enter: action | \u2193: select | q: quit");
            Console.Write("Input: ");
        }

        private void InputTaskToList()
        {
           // Console.Clear();
            Console.Write("\nAdd a new task (empty to cancel): ");

            var input = Console.ReadLine();

            AddTaskToList(input);
        }

        private void AddTaskToList(string input)
        {
           if(!string.IsNullOrWhiteSpace(input))
            {
                task.Add(input);
                isActioned.Add(false);
            }
        }

        private void PrintTaskList()
        {
            Console.Clear();
            int page = GetPage();
            int startingPoint = FirstElementInPage(page);

            int endingPoint = FirstElementInPage(page + 1);
            for (int i = startingPoint; (i < endingPoint) && (i < task.Count); ++i)
            {
                if (isActioned[i])
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else if (i == selectedTask)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine(task[i]);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine();
        }

        private static int FirstElementInPage(int page)
        {
            return page * pageLength;
        }

        private int GetPage()
        {
            return selectedTask / pageLength;
        }

        private void ReadListFromFile()
        {
           
            try
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\Ryan Newman\Desktop\List.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        string list = sr.ReadLine();
                        if (!list.Any())
                        {
                            InputTaskToList();

                        }

                        var input = sr.ReadLine();

                            var splits = input.Split(new char[] { '\x1e' });

                            if (splits.Length == 2)
                            {

                                task.Add(splits[0]);
                                isActioned.Add(bool.Parse(splits[1]));
                            }
                        
                    }
                }
            }
            catch (FileNotFoundException)
            {
                ;
            }            
        }
        private void WriteListToFile()
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Users\Ryan Newman\Desktop\List.txt"))
            {
                for(var i = 0;i < task.Count; ++i)
                {
                    sw.WriteLine($"{task[i]}\x1e{isActioned[i]}");
                }
            }
        }

     }
}

