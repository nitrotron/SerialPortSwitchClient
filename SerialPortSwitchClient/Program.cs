using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections;
using SerialPortSwitchClient.cs.SerialSwitchService;


namespace SerialPortSwitchClient
{
    class Program
    {
        public void printStatus(Dictionary<string, decimal> dict)
        {
            foreach (var item in dict)
            {
                Console.WriteLine(item.Key.ToString() + " = " + item.Value.ToString());
            }
        }
        static void Main(string[] args)
        {
            
            ArduinoSelfHostClient Client = new ArduinoSelfHostClient();
            Program prog = new Program();

            Dictionary<string, decimal> status = Client.GetStatus();

            for (int i = 0; i < 15; i++)
            {
                Client.SendCommand(ArduinoCommandsCommandTypes.SetTempAlarmLow, "1,50");
                Client.UpdateStatus();
                System.Threading.Thread.Sleep(15000);
                status = Client.GetStatus();
                prog.printStatus(status);

                Console.WriteLine("THermo 0 = " + status["Thermometer0"]);
            }
            Console.WriteLine("Hit Enter to exit");
            Console.ReadLine();
            
        }
    }
}
