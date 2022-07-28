using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Scheduler2
{
    public class FileManager
    {
        private string fileName;
        private UI ui;
        public FileManager(string fileName, UI ui)
        {
            this.fileName = fileName;
            if (!File.Exists(fileName))
            {
                try
                {
                    File.Create(fileName).Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

            this.ui = ui;
        }
        public bool saveMeeting(List<Meeting> meetings)
        {
            string jsonString = JsonSerializer.Serialize(meetings);
            try
            {
                File.WriteAllText(this.fileName, jsonString);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }
            return true;
        }
        public List<Meeting> readMeetings()
        {

            string jsonString = File.ReadAllText(fileName);
            List<Meeting> meetings = null;
            if(jsonString != null)
            {
                try
                {
                    meetings = JsonSerializer.Deserialize<List<Meeting>>(jsonString)!;
                }
                catch(Exception ex)
                {
                    ui.printText("Cannot decode the file\nError:\n" + ex.ToString());
                }
            };
            
            return meetings;
        }
    }
}
