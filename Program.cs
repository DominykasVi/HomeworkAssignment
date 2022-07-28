using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Scheduler2
{
    class Program
    {

        public static int Main()
        {
            var program = new Program();
            UI ui = new UI();
            string user = ui.login();
            string filename = Directory.GetCurrentDirectory() + "\\Meetings.json";
            BussinessLogic bussinessLogic = new BussinessLogic(ui, user, filename);

            while (true)
            {
                ui.printInstructions();
                int command = ui.getInput(0, 6);
                Console.WriteLine("Selection is: " + command.ToString());
                if (command == 0)
                {
                    break;
                }
                bussinessLogic.executeCommand(command);
                

            }
            return 0;
        }

    }
}
