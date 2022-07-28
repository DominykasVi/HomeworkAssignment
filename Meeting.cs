using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler2
{
    public class Meeting
    {
        public int id { get; set; }
        public string name { get; set; }
        public string responsiblePerson { get; set; }
        public string description { get; set; }
        public Category category { get; set; }
        public Type type { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public List<string> people { get; set; }

        public Meeting(string name, string responsiblePerson, string description, Category category, Type type,
            DateTime startDate, DateTime endDate)
        {
            this.name = name;
            this.responsiblePerson = responsiblePerson;
            this.description = description;
            this.category = category;
            this.type = type;
            this.startDate = startDate;
            this.endDate = endDate;
            people = new List<string> { responsiblePerson};
        }

        public string getMeetingInfo()
        {
            string peopleString = "";
            foreach(string person in people)
            {
                peopleString += person + ", ";
            }
            return "Meeting ID:" + id.ToString() + "\n" +
                "Meeting: " + name + "\n" +
                "Meeting is from " + startDate.ToString() + " to " + endDate.ToString() + "\n" +
                "Responsible person: " + responsiblePerson + "\n" +
                "Description: " + description + "\n" +
                "Category: " + category.ToString() + "\n" +
                "Type: " + type.ToString() + "\n" +
                "People in meeting:" + peopleString;
        }
    }

    public enum Category
    {
        CodeMonkey = 0,
        Hub = 1,
        Short = 2,
        TeamBuilding = 3
    }

    public enum Type
    {
        InPerson = 0,
        Live = 1
    }
}
