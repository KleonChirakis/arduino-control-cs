using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Arduino_Control_CS
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.NotifyIcon appNotification = new NotifyIcon();
        bool notificationMinimized = false;
        System.Timers.Timer infoTimer = new System.Timers.Timer(500);               //this timer is used to show that a number was pressed (by changing the color of a label)
        SerialControl SerialControl= new SerialControl();
        Controller Controller;


        Task pendingTask = null; // pending session
        CancellationTokenSource cts = null; // CTS for pending session

        void initializeApp()
        {
            //-----------------------------------Test------------------------------------

            //Properties.Settings.Default.DefaultPort = "";

            //-----------------------------------Test------------------------------------

            infoTimer.AutoReset = true;
            infoTimer.Enabled = true;
            infoTimer.Elapsed += OnInfoTimeElapsed;

            appNotification.MouseDoubleClick += appNotification_MouseDoubleClick;
            this.appNotification.Icon = Properties.Resources.arduino_logo;

            this.Text = StaticVariables.AppName;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            initializeApp();                                //initialize settings and GUI
            getMinimizedStartupState();                     //check if app is started minimized to tray
            getStartupState();                            //set the check state of the checkbox
            FindPortsDelegate(true);                        //find all the available serial ports

            Controller = new Controller(SerialControl);
            
            string portName = Properties.Settings.Default.DefaultPort;
            Controller.IncomingActionEvent += IncomingAction;
            Select_ConnectToPort_StartControllerAsync(portName, new CancellationTokenSource().Token); //cts.Token);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (getMinimizedStartupState())
            {
                this.WindowState = FormWindowState.Minimized;       //to hide the form when initializing from the user
                minimizeToTray();
            }

        }

        void IncomingAction(object sender, IncomingActionEventArgs e)
        {
            UpdateLabel(e.IncomingAction, e.is3digitNumber, e.is3digitNumberCompleted);
        }

        void UpdateLabel(String IncomingAction, bool is3digitNumber, bool is3digitNumberCompleted = false)
        {
            if (is3digitNumber && is3digitNumberCompleted)
            {
                lbl_Number.Invoke(new Action(() =>
                {
                    lbl_Number.Text = "";
                }));

                textBox1.Invoke(new Action(() =>
                {
                    textBox1.Text += DateTime.Now.ToString("h:mm:ss tt") + ": " + IncomingAction + Environment.NewLine;
                }));
            }
            else if (is3digitNumber)
            {
                lbl_Number.Invoke(new Action(() =>
                {
                    lbl_Number.Text = IncomingAction;
                }));
            }
            else
            {
                Label1.Invoke(new Action(() =>
                {
                    Label1.Text = IncomingAction.ToString();
                    Label1.ForeColor = Color.FromArgb(192, 64, 0);
                    infoTimer.Start();
                }));

                textBox1.Invoke(new Action(() =>
                {
                    textBox1.Text += DateTime.Now.ToString("h:mm:ss tt") + ": " + IncomingAction + Environment.NewLine;
                }));
            }                   
        }

        void OnInfoTimeElapsed(Object source, ElapsedEventArgs e)
        {
            Label1.ForeColor = SystemColors.ControlText;
            infoTimer.Stop();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        #region Notification
        private void appNotification_MouseDoubleClick(object sender, EventArgs e)
        {
            notificationMinimized = false;
            appNotification.Visible = false;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            minimizeToTray();
        }

        private void minimizeToTray()
        {
            appNotification.BalloonTipTitle = StaticVariables.AppName; 
            appNotification.BalloonTipText = StaticVariables.AppName + "is running in the background";
            //appNotification.Text = StaticVariables.AppName + " -" + (((SerialControl.connectionSuccess) ? " " : " Not ") + "Connected");

            if (!notificationMinimized)
            {
                notificationMinimized = true;
                appNotification.Visible = true;
                appNotification.ShowBalloonTip(500);
                Controller.KeepWindowHandle = false;                    //informs the Controller if the window is minized, so as to stop keeping track of the WindowHandle property
                this.Hide();                                            //the Controller keeps track of the WindowHandle property in order to have the newest property value for use when windows is minized
            }
            else
            {
                Controller.KeepWindowHandle = true;
            }
        }
        #endregion

        #region Startup CheckBox
        private void cb_runOnStartUp_Click(object sender, EventArgs e)
        {
            GeneralFunctions.SetStartup(cb_runOnStartUp.Checked);
        }

        private void getStartupState()
        {
            cb_runOnStartUp.Checked = GeneralFunctions.checkStartupState();
        }
        #endregion

        #region Serial And Controller
        private void FindPorts(bool shouldListsBeEqual)
        {
            //SerialControl.FindPorts() returns List<Port>. For each of these items, a new ToolStripMenuItem is created
            List<Port> oldPortsList = new List<Port>(SerialControl.getPortsList());
            List<Port> newPortsList = new List<Port>(SerialControl.FindPorts());
            if (!GeneralFunctions.UnorderedEqual(oldPortsList, newPortsList) || shouldListsBeEqual)        //at firstCheck (FormLoad()) the two lists are going to be equal, thus no items are going to be added at toolStripSplitButton1.DropDownItems (lists should be equal and should be shown)
            {
                toolStripSplitButton1.DropDownItems.Clear();
                toolStripSplitButton1.DropDownItems.AddRange(newPortsList.Select(e => new ToolStripMenuItem(e.Text, (Image)null, (EventHandler)portItem_Click, e.Name) { }).ToArray());
            }
        }

        private void FindPortsDelegate(bool shouldListsBeEqual)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => {  FindPorts(shouldListsBeEqual); }));
                return;
            }
            else
            {
                FindPorts(shouldListsBeEqual);
            }
        }

        private void portItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(string.Concat("You have Clicked '", sender.ToString(), "' Menu"), "Menu Items Event", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Select_ConnectToPort_StartControllerAsync(sender.ToString(), new CancellationTokenSource().Token); //cts.Token);
            Properties.Settings.Default.DefaultPort = sender.ToString();
            Properties.Settings.Default.Save();
        }

        private async void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            cancelOperation();
        }

        private async void cancelOperation()
        {
            if (cts != null)
            {
                // cancel the previous operation and wait for its termination
                cts.Cancel();

                try { await this.pendingTask; cts = null; } catch { }
            }

            toolStripStatusLabel2.Text = "Operation halted";
        }
        
        // runs on the UI thread
        public async Task Select_ConnectToPort_StartControllerAsync(String portName, CancellationToken token)
        {
            toolStripStatusLabel2Update("Attempting to connect...", null);
            toolStripSplitButton1Update(portName, null);

            var previousCts = this.cts;
            var newCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            //var newCts = new CancellationTokenSource();
            this.cts = newCts;

            if (previousCts != null)
            {
                // cancel the previous session and wait for its termination
                previousCts.Cancel();

                try { await this.pendingTask; } catch { } 
            }

            newCts.Token.ThrowIfCancellationRequested();
            this.pendingTask = Select_ConnectToPort_StartControllerHelper(portName, newCts.Token);
            await this.pendingTask;
        }
         
        // the actual task logic
         async Task Select_ConnectToPort_StartControllerHelper(String portName, CancellationToken token)
        {
            if (!String.IsNullOrEmpty(portName))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        SerialControl.connectionSuccess = false;
                        do {
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                SerialControl.ConnectToPort(portName);
                                SerialControl.connectionSuccess = true;
                            }
                            catch (OperationCanceledException)
                            {
                                throw;
                            }
                            catch (Exception ex)
                            {
                                if (ex is IOException)
                                    FindPortsDelegate(false);        // port not found, refresh ports list too check if it is connected    

                                toolStripStatusLabel2Update("Connection failed. Retrying...", null);
                                //Task.Delay(1000);
                                Thread.Sleep(250);
                            }
                            if (notificationMinimized)
                                appNotification.Text = StaticVariables.AppName + " -" + (((SerialControl.connectionSuccess) ? " " : " Not ") + "Connected");
                        } while (!SerialControl.connectionSuccess);

                        ToolStripItem[] menuItems = toolStripSplitButton1.DropDownItems.Cast<ToolStripItem>().ToArray();
                        foreach (ToolStripItem item in menuItems)                                                       // uncheck all port items
                        {
                            toolStripSplitButtonDropDownItemsDelegate(item, false);
                        }

                        if (!String.IsNullOrEmpty(portName))                                                            // check the connected port
                        menuItems = toolStripSplitButton1.DropDownItems.Find(portName, false);
                        toolStripSplitButtonDropDownItemsDelegate(menuItems[0], true);

                        toolStripSplitButton1Update(null, Color.Black);
                        toolStripStatusLabel2Update("Connected successfully " + SerialControl.Sp.PortName, null);

                        while (true)
                        {
                            try
                            {
                                int IncomingIR = Int32.Parse(SerialControl.Sp.ReadLine().Replace("\r", ""), System.Globalization.NumberStyles.HexNumber);       //Read time out @ 100ms
                                Controller.IR_Control(IncomingIR);
                            }
                            catch (TimeoutException)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    toolStripStatusLabel2.Text = "Operation halted";
                                    token.ThrowIfCancellationRequested();
                                }
                            }
                            catch
                            {
                                toolStripStatusLabel2.Text = "Operation halted";
                                Select_ConnectToPort_StartControllerAsync(portName, new CancellationTokenSource().Token);       //try reconnection with new connection (restarts the operation task)
                                break;           //exit while, then the tasks stops    
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //close current serial port
                        SerialControl.Sp.Close();  
                    }
                    catch (IOException)
                    {
                        toolStripStatusLabel2Update("Port not found. Disconnected?", null);         //port not found, it may have been disconnected, refesh ports list to check if device exists
                        FindPortsDelegate(false);
                    }
                    catch (NullReferenceException)
                    {
                        toolStripSplitButton1Update("N/A", null);
                        toolStripStatusLabel2Update("Port not selected", null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, StaticVariables.AppName);
                    }
                }, token);
            }
            else
            {
                toolStripStatusLabel2Update("Port not selected", null);
                toolStripSplitButton1.Text = "Ports";
            }
        }

        protected virtual void OnClosed(CancelEventArgs e)
        {
            cancelOperation();
            SerialControl.CloseConnection();
        }
        #endregion

        #region GUI Thread Handling
        private void toolStripSplitButton1Update(String Text, Color? Color)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string, Color?>(toolStripSplitButton1Update), new object[] { Text, Color });
                return;
            }

            if (Color != null)
                toolStripSplitButton1.ForeColor = (Color)Color;
            if (!String.IsNullOrEmpty(Text))
                toolStripSplitButton1.Text = Text;
        }

        private void toolStripStatusLabel2Update (String Text, Color? Color)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string, Color?>(toolStripStatusLabel2Update), new object[] { Text, Color });
                return;
            }
            if (Color != null)
                toolStripStatusLabel2.ForeColor = (Color)Color;
            if (!String.IsNullOrEmpty(Text))
                toolStripStatusLabel2.Text = Text;
        }

        private void toolStripSplitButtonDropDownItemsDelegate(ToolStripItem item, bool Checked)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<ToolStripItem, bool>(toolStripSplitButtonDropDownItemsDelegate), new object[] { item, Checked });
                return;
            }
            ((ToolStripMenuItem)item).Checked = Checked;
        }
        #endregion

        #region MinimizedStartup CheckBox
        private void cb_startMinimized_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartMinimized = cb_startMinimized.Checked;
            Properties.Settings.Default.Save();
        }

        private bool getMinimizedStartupState()
        {
            bool StartMinimized = Properties.Settings.Default.StartMinimized;
            cb_startMinimized.Checked = StartMinimized;
            return StartMinimized;
        }
        #endregion


        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            FindPortsDelegate(false);
        }

       

    }
}


//TODO: Everytime a port is connected/disconnected, the port list must be refreshed - cannot be done, add refresh device list button - cannot be done, SerialPort class not working right (disconnected ports keep showing up)
//TODO: Add description to the COM, eg. COM4 - Arduino/Genuino