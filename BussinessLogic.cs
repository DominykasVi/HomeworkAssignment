using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Scheduler2
{
    public class BussinessLogic
    {
        public UI ui = null;
        List<Meeting> meetings;
        string fileName;
        string user;
        FileManager fileManager;
        public BussinessLogic(UI ui, string user, string filename)
        {
            this.ui = ui;
            this.fileName = filename;
            this.fileManager = new FileManager(filename, ui);
            meetings = fileManager.readMeetings();
            this.user = user;
        }

        public void executeCommand(int command)
        {
            int meetingID;
            string person;
            switch (command)
            {
                case 1:
                    ui.printMeetings(meetings);
                    ui.waitForInput();

                    break;
                case 2:
                    Meeting newMeeting = ui.getMeetingInfo(user);
                    int lastID = -1;
                    Console.WriteLine(meetings != null);
                    if (meetings != null)
                    {
                        lastID = meetings.Last().id;
                    } else
                    {
                        meetings = new List<Meeting>();
                    }
                    newMeeting.id = lastID + 1;
                    meetings.Add(newMeeting);
                    fileManager.saveMeeting(meetings);
                    break;
                case 3:
                    ui.printText("Please enter meeting you wish to delete");
                    ui.printMeetings(meetings);
                    int id = ui.getMeetingID(meetings);
                    foreach(Meeting meeting in meetings)
                    {
                        if (meeting.id == id && meeting.responsiblePerson.Equals(this.user)){
                            meetings.RemoveAll(meet => meet.id == id);
                            ui.printText("Meeting deleted");
                            ui.waitForInput();
                            fileManager.saveMeeting(meetings);
                            break;
                        }
                        else if(meeting.id == id && !meeting.responsiblePerson.Equals(this.user))
                        {
                            ui.printText("You do not have permission to delete this meeting");
                            ui.waitForInput();
                        }
                    }
                    break;
                case 4:
                    ui.printMeetings(meetings);
                    ui.printText("Please enter meeting id you wish to add a person to:");
                    meetingID = ui.getMeetingID(meetings);
                    var meetingToAddPerson = meetings.Find(meet => meet.id == meetingID);
                    person = ui.getPerson();
                    var personsMeetings = from i in meetings
                                         where i.people.Contains(person)
                                         select (i.startDate, i.endDate);
                    personsMeetings = personsMeetings.ToList();
                    foreach(var timeslots in personsMeetings)
                    {
                        if(meetingToAddPerson.startDate < timeslots.endDate && timeslots.startDate < meetingToAddPerson.endDate)
                        {
                            //intersection of timeslots
                            ui.printText("There is an intersection of meetings for this person");
                            ui.waitForInput();
                        }
                    }
                    if(meetingToAddPerson.people.Contains(person))
                    {
                        ui.printText("This person has already been added");
                        ui.waitForInput();
                        break;
                    }else
                    {
                        meetingToAddPerson.people.Add(person); ;
                        fileManager.saveMeeting(meetings);
                        ui.printText("Person Added");
                        ui.waitForInput();
                    }
                    break;
                case 5:
                    ui.printMeetings(meetings);
                    ui.printText("Please enter meeting id you wish to remove a person from:");
                    meetingID = ui.getMeetingID(meetings);
                    var meetingToRemovePerson = meetings.Find(meet => meet.id == meetingID);
                    person = ui.getPerson();
                    if (meetingToRemovePerson.responsiblePerson.Equals(person)){
                        ui.printText("Cannot remove the responsible person");
                        ui.waitForInput();

                        break;
                    }
                    if (meetingToRemovePerson.people.Contains(person))
                    {
                        meetingToRemovePerson.people.Remove(person);
                        fileManager.saveMeeting(meetings);
                    }else
                    {
                        ui.printText("This person is not in the meeting");
                    }
                    ui.waitForInput();
                    break;
                case 6:
                    //Console.WriteLine("Test");
                    ui.printFilters();
                    int filter = ui.getInput(1, 6);
                    List<Meeting> filteredMeeting = Filter(filter, meetings, ui);
                    ui.printMeetings(filteredMeeting);
                    ui.waitForInput();
                    break;

            }

        }

        public static List<Meeting> Filter(int option, List<Meeting> meetingsFilter, UI ui)
        {
            string text;
            switch (option)
            {
                case 1:
                    ui.printText("Enter the description you wish to search");
                    text = ui.getSearchText();

                    var meetingList = from i in meetingsFilter
                                  where i.description.Contains(text)
                                  select i;
                    return emptyCheck(meetingList.ToList());
                    break;
                case 2:
                    ui.printText("Enter the responsible person you wish to search");
                    text = ui.getSearchText();
                    meetingList = from i in meetingsFilter
                                  where i.responsiblePerson.Equals(text)
                                  select i;
                    return emptyCheck(meetingList.ToList());
                    break;
                case 3:
                    ui.printText("Enter the category you wish to search");
                    Category category = ui.GetCategory();
                    meetingList = from i in meetingsFilter
                                  where i.category == category
                                  select i;
                    return emptyCheck(meetingList.ToList());
                    break;
                case 4:
                    ui.printText("Enter the Type you wish to search");
                    Type type = ui.GetType();
                    meetingList = from i in meetingsFilter
                                  where i.type == type
                                  select i;
                    return emptyCheck(meetingList.ToList());
                    break;
                case 5:
                    ui.printText("Enter the from which date you wish to search");
                    DateTime startDate = ui.getDateShort();
                    ui.printText("Enter the to which date you wish to search");

                    DateTime endDate = ui.getDateShort();
                    while(endDate < startDate)
                    {
                        ui.printText("End date cannot be before start date, enter a new end date:");
                        endDate = ui.getDateShort();
                    }
                    meetingList = from i in meetingsFilter
                                  where i.startDate.Date <= endDate.Date && startDate.Date <= i.endDate.Date
                                  select i;
                    return emptyCheck(meetingList.ToList());
                    break;
                case 6:
                    ui.printText("This will filter over how many attendees the meeeting has to have");
                    ui.printText("Enter the number");
                    int number = ui.getInput(0, int.MaxValue);
                    meetingList = from i in meetingsFilter
                                  where i.people.Count > number
                                  select i;
                    return emptyCheck(meetingList.ToList());
                    break;
            }
            return null;
        }

        private static List<Meeting> emptyCheck(List<Meeting> meetingList)
        {
            if (meetingList.Count() == 0)
            {
                return null;
            }
            else
            {
                return meetingList.ToList();
            }
        }
    }
    
    
}
