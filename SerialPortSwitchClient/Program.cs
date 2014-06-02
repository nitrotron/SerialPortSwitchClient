using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections;
//using SerialPortSwitchClient.cs.SerialSwitchService;

[ServiceContract]
public interface IArduinoSelfHost //: IArduinoSerial
{
    [OperationContract]
    string GetRawStatus();
    [OperationContract]
    Dictionary<string, decimal> GetStatus();
    [OperationContract]
    void SendCommand(int ArduinoCommands, string text);
    //[OperationContract]
    //Dictionary<string, decimal> SendCommandWithResponse(ArduinoCommands.CommandTypes cmd, string text);
    [OperationContract]
    void UpdateStatus();
}

    public class ArduinoSelfHostClient : ClientBase<IArduinoSelfHost>, IArduinoSelfHost
    {
        public ArduinoSelfHostClient(Binding binding, EndpointAddress address)
            : base(binding, address)
        {
        }

        public string GetRawStatus()
        {
            return Channel.GetRawStatus();
        }
        public Dictionary<string, decimal> GetStatus()
        {
            return Channel.GetStatus();
        }
        public void UpdateStatus()
        {
            Channel.UpdateStatus();
        }

        public void SendCommand(int ArduinoCommands, string text)
        {
            Channel.SendCommand(ArduinoCommands, text);
        }
    }


    class Program
    {
        public void printStatus(Dictionary<string, decimal> dict)
        {
            Console.WriteLine(dict.Count());
            foreach (var item in dict)
            {
                Console.WriteLine(item.Key.ToString() + " = " + item.Value.ToString());
            }
        }
        static void Main(string[] args)
        {
            var binding = new BasicHttpBinding();
            //var address = new EndpointAddress("http://localhost:8080/SerialSwitch");
            var address = new EndpointAddress("http://192.168.0.16:8080/SerialSwitch");
            //var client = new HelloClient(binding, address);
            
            ArduinoSelfHostClient Client = new ArduinoSelfHostClient(binding,address);
            Program prog = new Program();

            Dictionary<string, decimal> status = Client.GetStatus();

            for (int i = 0; i < 2; i++)
            {
                //Client.SendCommand(ArduinoCommandsCommandTypes.SetTempAlarmLow, "1,50");
                //Client.UpdateStatus();
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("Getting Data");
                status = Client.GetStatus();
                prog.printStatus(status);
                Console.WriteLine("DoneGetting data");
                //Console.WriteLine("THermo 0 = " + status["Thermometer0"]);
            }

            Client.SendCommand(1, "");
            Console.WriteLine("Hit Enter to exit");
            Console.ReadLine();
            
        }
    }

