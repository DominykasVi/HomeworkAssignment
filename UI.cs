using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler2
{
    public class UI
    {
        public UI() { }

        public string login()
        {
            Console.WriteLine("Please enter your name:");
            String input = Console.ReadLine();
            while(input == null)
            {
                Console.WriteLine("Please enter your name");
                input = Console.ReadLine();
            }
            Console.WriteLine("Welcome " + input);
            return input;
        }
        public void printInstructions()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the scheduler app, please type in your command number");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Print all meetings");
            Console.WriteLine("2. Create a meeting");
            Console.WriteLine("3. Delete a meeting");
            Console.WriteLine("4. Add person to a meeting");
            Console.WriteLine("5. Remove a person from a meeting");
            Console.WriteLine("6. Filter meetings");
        }

        public void printFilters()
        {
            Console.Clear();
            Console.WriteLine("Please type filter number");
            Console.WriteLine("1. Description");
            Console.WriteLine("2. Responsible Person");
            Console.WriteLine("3. Category");
            Console.WriteLine("4. Type");
            Console.WriteLine("5. Date");
            Console.WriteLine("6. Number of attendees");
        }

        public string getSearchText()
        {
            return Console.ReadLine();
        }
        public void printMeetings(List<Meeting> meetings)
        {
            if(meetings == null)
            {
                Console.WriteLine("No planned meetings");
            } else
            {
                foreach (Meeting meeting in meetings)
                {
                    Console.WriteLine(meeting.getMeetingInfo());
                    Console.WriteLine();
                }
            }
        }

        public void printText(string message)
        {
            Console.WriteLine(message);
        }

        public int getInput(int lowerBound, int upperBound)
        {
            String input = Console.ReadLine();
            int inputValue;
            while (!int.TryParse(input, out inputValue) || inputValue < lowerBound || inputValue > upperBound)
            {
                //change this
                if (inputValue < lowerBound || inputValue > upperBound)
                {
                    Console.WriteLine("Please input numbers only from the options");
                } else
                {
                    Console.WriteLine("Please input only the number");
                }
                input = Console.ReadLine();
            }
            
            return inputValue;
        }

        public void waitForInput()
        {
            Console.WriteLine("Press enter when finished");
            Console.ReadLine();
        }

        public string getPerson()
        {
            Console.WriteLine("Please enter persons full name");
            String input = Console.ReadLine();
            return input;
        }

        public int getMeetingID(List<Meeting> meetings)
        {
            String input = Console.ReadLine();
            int maxID = meetings.Last().id;
            int inputValue;
            while(!int.TryParse(input, out inputValue) || !meetings.Any(meet => meet.id == inputValue))
            {
                Console.WriteLine("Please enter valid meeting ID");
                input = Console.ReadLine();
            }

            return inputValue;
        }
        
        public Meeting getMeetingInfo(string user)
        {
            Console.WriteLine("Please insert meeting name:");
            String name = Console.ReadLine();

            Console.WriteLine("Please insert meeting description:");
            String description = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Please select meeting category:");
            Category category = getCategory();


            Console.Clear();
            Console.WriteLine("Please select meeting type:");
            Type type = getType();


            DateTime startDate;
            DateTime endDate;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please insert meeting start date (Format yyyy-mm-dd hh:mm):");
                startDate = getDate();

                Console.WriteLine("Please insert meeting end date (Format yyyy-mm-dd hh:mm):");
                endDate = getDate();
                if(startDate > endDate)
                {
                    Console.WriteLine("Start date cannot be later than end date");
                    waitForInput();
                }else
                {
                    break;
                }
            }
                

            return new Meeting(name, user, description, category, type, startDate, endDate);
        }
        public DateTime getDateShort()
        {
            DateTime userDateTime;
            while (!DateTime.TryParse(Console.ReadLine(), out userDateTime) || timeCheckShort(userDateTime))
            {
                Console.WriteLine("You have entered an incorrect value.");
            }
            return userDateTime;

        }
        public static DateTime getDate()
        {
            DateTime userDateTime;
            while(!DateTime.TryParse(Console.ReadLine(), out userDateTime) || timeCheck(userDateTime))
            {
                Console.WriteLine("You have entered an incorrect value.");
            }
            return userDateTime;    
           
        }

        private static bool timeCheckShort(DateTime date)
        {
            if (date.Hour != 0 || date.Minute != 0 || date.Second != 0)
            {
                return true;
            }
            return false;
        }
        private static bool timeCheck(DateTime date)
        {
            if (date.Hour == 0 && date.Minute == 0 && date.Second == 0)
            {
                return true;
            }
            return false;
        }

        public new Type GetType()
        {
            return getType();
        }

        private static Type getType()
        {
            var values = Enum.GetValues(typeof(Type));
            foreach (var value in values)
            {
                Console.WriteLine(value.ToString());

            }
            String type = Console.ReadLine();
            Type selectedType = new Type();
            while (!validateType(type, ref selectedType))
            {
                type = Console.ReadLine();
            }

            return selectedType;
        }

        public Category GetCategory()
        {
            return getCategory();
        }

        private static Category getCategory()
        {
            var values = Enum.GetValues(typeof(Category));
            foreach (var value in values)
            {
                Console.WriteLine( value.ToString());

            }
            String category = Console.ReadLine();
            Category selectedCategory = new Category();
            while (!validateCategory(category, ref selectedCategory))
            {
                category = Console.ReadLine();
            }

            return selectedCategory;
        }

        private static bool validateCategory(string categoryString, ref Category category)
        {
            switch (categoryString)
            {
                case "TeamBuilding":
                    category = Category.TeamBuilding;
                    break;
                case "Short":
                    category = Category.Short;
                    break;
                case "CodeMonkey":
                    category = Category.CodeMonkey;
                    break;
                case "Hub":
                    category = Category.Hub;
                    break;
                default:
                    Console.WriteLine("Please enter the correct category:");
                    return false;
            }
            return true;
        }

        private static bool validateType(string typeString, ref Type type)
        {
            switch (typeString)
            {
                case "InPerson":
                    type = Type.InPerson;
                    break;
                case "Live":
                    type = Type.Live;
                    break;
                default:
                    Console.WriteLine("Please enter the correct type:");
                    return false;
            }
            return true;
        }
    }
}
