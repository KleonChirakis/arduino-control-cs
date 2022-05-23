using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino_Control_CS
{
    class Port
    {
        public string Name, Text;
    }

    class SerialControl
    {
        public SerialPort Sp = new SerialPort();

        List<Port> PortsList= new List <Port>();

        public bool connectionSuccess;

        void setupSerial(int BaudRate, int DataBits, System.IO.Ports.Parity Parity, System.IO.Ports.StopBits StopBits, String PortName, int timeOutRx = 100)
        {
            try
            {
                Sp.BaudRate = BaudRate;         //9600
                Sp.DataBits = DataBits;         //8
                Sp.Parity = Parity;             //Parity.None
                Sp.StopBits = StopBits;         //StopBits.One
                Sp.PortName = PortName;         //"COM4"

                Sp.ReadTimeout = timeOutRx;     //100
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void connectToSerial()
        {
                    if (Sp.IsOpen)
                        Sp.Close();            

                    Sp.Open();

                    //TODO: remove these lines
                    //await Task.Delay(2000);
        }

        public List<Port> FindPorts()
        {
            PortsList.Clear();
            string[] tempPorts = SerialPort.GetPortNames();             //BUG-UnFixable: SerialPort will show an unavailable port as available. Manual port list refresh is not possible
            string[] ports = tempPorts.Distinct().ToArray();

            foreach (string port in ports)
            {
                var portItem = new Port()
                {
                    Name = port,
                    Text = port
                };
                PortsList.Add(portItem);
            }
            return PortsList;
        }

        public void ConnectToPort(String portName)            //selects a port and tries to connect to it
        {
            Port Port;

            if (Sp.IsOpen)      //Sp != null || 
                Sp.Close();

            if (portName != null)
            {
                //FindPorts();          Ports have been found in the beggining of the application. If connection is unsuccessful, the ports list will be refreshed 
                Port = PortsList.Find(x => x.Name == portName);
                if (Port != null)               //if port exists
                {
                    setupSerial(9600, 8, Parity.None, StopBits.One, portName);
                    try
                    {
                        connectToSerial();                                              //connect to port
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {                           
                    throw new IOException();                              //port does not exists
                }
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public void CancelConnectionAttempt()
        {
            StaticVariables.tokenSource.Cancel();
        }

        internal void CloseConnection()
        {
            Sp.Close();
        }

        public List<Port> getPortsList() {
            return this.PortsList;
        }
    }
}
