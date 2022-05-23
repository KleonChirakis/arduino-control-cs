using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Arduino_Control_CS
{
  
    public class IncomingActionEventArgs : EventArgs
    {
        public string IncomingAction;
        public bool is3digitNumber;
        public bool is3digitNumberCompleted;

    public  IncomingActionEventArgs(string IncomingAction, bool is3digitNumber, bool is3digitNumberCompleted = false)
        {
            this.IncomingAction = IncomingAction;
            this.is3digitNumber = is3digitNumber;
            this.is3digitNumberCompleted = is3digitNumberCompleted;
        }
        
    }



    class Controller
    {
        int IncomingIR;
        AppCommandCode AppCommandCode = 0;
        int lastPressedNum = 0;
        string str_selectedNum = "";
        string IncomingAction;

        System.Timers.Timer numTimer = new System.Timers.Timer(1300);               //this timer is used to catch a number of up to 3 digits

        private SerialControl SerialControl;

        public event EventHandler<IncomingActionEventArgs> IncomingActionEvent;
        private bool _KeepWindowHandle = true;                                        //false if window is minized

        protected virtual void OnIncomingAction(IncomingActionEventArgs e)
        {
            /*if (IncomingAction != null)
                IncomingAction(this, e);*/
            IncomingActionEvent?.Invoke(this, e);
        }


        public Controller(SerialControl SerialControl)
        {
            this.SerialControl = SerialControl;
            InitializeTimers();
        }

        private void InitializeTimers()
        {
            

            numTimer.AutoReset = true;
            numTimer.Enabled = true;
            numTimer.Stop();
            numTimer.Elapsed += OnRunTimeElapsed;
        }





        public void IR_Control(int IncIR)
        {

            //Thread thread = new Thread(async () =>
            //Thread thread = new Thread(() =>
            //{
            IncomingIR = IncIR;
                        switch (IncomingIR)
                        {
                            case 0xFF22DD:
                                AppCommandCode = AppCommandCode.MEDIA_PREVIOUSTRACK;
                                AppCommand(AppCommandCode);
                                IncomingAction = "Previous track";
                                OnIncomingAction(new IncomingActionEventArgs(IncomingAction, false));
                                break;
                            case 0xFF02FD:
                                AppCommandCode = AppCommandCode.MEDIA_NEXTTRACK;
                                AppCommand(AppCommandCode);
                                IncomingAction = "Next track";
                                OnIncomingAction(new IncomingActionEventArgs(IncomingAction, false));
                                break;
                            case 0xFFC23D:
                                AppCommandCode = AppCommandCode.MEDIA_PLAY_PAUSE;
                                AppCommand(AppCommandCode);
                                IncomingAction = "Play/pause";
                                OnIncomingAction(new IncomingActionEventArgs(IncomingAction, false));
                                break;
                            case 0xFFA857:
                                AppCommandCode = AppCommandCode.VOLUME_UP;
                                AppCommand(AppCommandCode);
                                IncomingAction = "Volume up";
                                OnIncomingAction(new IncomingActionEventArgs(IncomingAction, false));
                                break;
                            case 0xFFE01F:
                                AppCommandCode = AppCommandCode.VOLUME_DOWN;
                                AppCommand(AppCommandCode);
                                IncomingAction = "Volume down";
                                OnIncomingAction(new IncomingActionEventArgs(IncomingAction, false));
                                break;
                            case 0xFF6897:
                                numButtonPressed(0);
                                break;
                            case 0xFF30CF:
                                numButtonPressed(1);
                                break;
                            case 0xFF18E7:
                                numButtonPressed(2);
                                break;
                            case 0xFF7A85:
                                numButtonPressed(3);
                                break;
                            case 0xFF10EF:
                                numButtonPressed(4);
                                break;
                            case 0xFF38C7:
                                numButtonPressed(5);
                                break;
                            case 0xFF5AA5:
                                numButtonPressed(6);
                                break;
                            case 0xFF42BD:
                                numButtonPressed(7);
                                break;
                            case 0xFF4AB5:
                                numButtonPressed(8);
                                break;
                            case 0xFF52AD:
                                numButtonPressed(9);
                                break;
                            case unchecked((int)(0xFFFFFFFF)):              // -1 (Send from remote control)
                            if (AppCommandCode > 0)
                                AppCommand(AppCommandCode);
                            else if (AppCommandCode == 0)
                                if (lastPressedNum > -1)
                                    numButtonPressed(lastPressedNum);
                            OnIncomingAction(new IncomingActionEventArgs(IncomingAction, false));
                            break;
                default:
                            IncomingAction = "Unknown command";
                            OnIncomingAction(new IncomingActionEventArgs(IncomingAction, false));
                            lastPressedNum = -1;
                            break;
                            }
               //}); //end thread
            //thread.IsBackground = true;         //avoid threads from keeping the application alive on exit
            //thread.Start();
        }


        private static IntPtr MainWindowHandle = new IntPtr();
        private int WM_APPCOMMAND = 0x319;
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public bool KeepWindowHandle
        {
            get { return this._KeepWindowHandle; }
            set { this._KeepWindowHandle = value; if (value == false) MainWindowHandle = Process.GetCurrentProcess().MainWindowHandle; }    //get MainWindowHandle only when needed (when app is minimized to tray)
        }

        private void AppCommand(AppCommandCode commandCode)
        {
            if (KeepWindowHandle)
                MainWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
            int CommandID = (int)commandCode << 16;
            SendMessageW(MainWindowHandle, WM_APPCOMMAND, MainWindowHandle, (IntPtr)CommandID);
        }

        private void numButtonPressed(int num)
        {
            AppCommandCode = 0;
            lastPressedNum = num;
            str_selectedNum += num.ToString();
            IncomingAction = num + " pressed";
            OnIncomingAction(new IncomingActionEventArgs(IncomingAction, false));
            enableNumTimer();
        }

        void enableNumTimer()
        {
            numTimer.Stop();                //new button pressed, wait for a new period  
            numTimer.Start();

            if (str_selectedNum.Length > 3)                         //3 digits max
                str_selectedNum = str_selectedNum.Substring(3);     //from 3 to the end = digit 3 only

            /*lbl_Number.Invoke(new Action(() =>
            {
                lbl_Number.Text = str_selectedNum;
            }
            */
            OnIncomingAction(new IncomingActionEventArgs(str_selectedNum, true));
        }




        void OnRunTimeElapsed(Object source, ElapsedEventArgs e)
        {
            OnIncomingAction(new IncomingActionEventArgs("number " + str_selectedNum + " selected", true, true));
            str_selectedNum = "";
            numTimer.Stop();
        }

    }
}
