namespace Arduino_Control_CS
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lbl_Number = new System.Windows.Forms.Label();
            this.btn_minimizeToTray = new System.Windows.Forms.Button();
            this.cb_runOnStartUp = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cb_startMinimized = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(110, 43);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(63, 13);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Command...";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 102);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(289, 155);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lbl_Number
            // 
            this.lbl_Number.AutoSize = true;
            this.lbl_Number.Location = new System.Drawing.Point(9, 9);
            this.lbl_Number.Name = "lbl_Number";
            this.lbl_Number.Size = new System.Drawing.Size(0, 13);
            this.lbl_Number.TabIndex = 3;
            // 
            // btn_minimizeToTray
            // 
            this.btn_minimizeToTray.Location = new System.Drawing.Point(208, 64);
            this.btn_minimizeToTray.Name = "btn_minimizeToTray";
            this.btn_minimizeToTray.Size = new System.Drawing.Size(93, 23);
            this.btn_minimizeToTray.TabIndex = 4;
            this.btn_minimizeToTray.Text = "Minimize to tray";
            this.btn_minimizeToTray.UseVisualStyleBackColor = true;
            this.btn_minimizeToTray.Click += new System.EventHandler(this.button1_Click);
            // 
            // cb_runOnStartUp
            // 
            this.cb_runOnStartUp.AutoSize = true;
            this.cb_runOnStartUp.Location = new System.Drawing.Point(57, 263);
            this.cb_runOnStartUp.Name = "cb_runOnStartUp";
            this.cb_runOnStartUp.Size = new System.Drawing.Size(98, 17);
            this.cb_runOnStartUp.TabIndex = 5;
            this.cb_runOnStartUp.Text = "Run on Startup";
            this.cb_runOnStartUp.UseVisualStyleBackColor = true;
            this.cb_runOnStartUp.Click += new System.EventHandler(this.cb_runOnStartUp_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripSplitButton1,
            this.toolStripSplitButton2,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 289);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(313, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(45, 17);
            this.toolStripStatusLabel1.Text = "Status: ";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(133, 20);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton2.DropDownButtonWidth = 0;
            this.toolStripSplitButton2.Image = global::Arduino_Control_CS.Properties.Resources.Refresh_16;
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(21, 20);
            this.toolStripSplitButton2.Text = "Update Ports";
            this.toolStripSplitButton2.Visible = false;
            this.toolStripSplitButton2.ButtonClick += new System.EventHandler(this.toolStripSplitButton2_ButtonClick);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(120, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb_startMinimized
            // 
            this.cb_startMinimized.AutoSize = true;
            this.cb_startMinimized.Location = new System.Drawing.Point(161, 263);
            this.cb_startMinimized.Name = "cb_startMinimized";
            this.cb_startMinimized.Size = new System.Drawing.Size(96, 17);
            this.cb_startMinimized.TabIndex = 8;
            this.cb_startMinimized.Text = "Start minimized";
            this.cb_startMinimized.UseVisualStyleBackColor = true;
            this.cb_startMinimized.Click += new System.EventHandler(this.cb_startMinimized_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 311);
            this.Controls.Add(this.cb_startMinimized);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cb_runOnStartUp);
            this.Controls.Add(this.btn_minimizeToTray);
            this.Controls.Add(this.lbl_Number);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lbl_Number;
        private System.Windows.Forms.Button btn_minimizeToTray;
        private System.Windows.Forms.CheckBox cb_runOnStartUp;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private System.Windows.Forms.CheckBox cb_startMinimized;
    }
}

