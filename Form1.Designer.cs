namespace LiftSimulator
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
            this.components = new System.ComponentModel.Container();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.fireButton = new System.Windows.Forms.Button();
            this.newPassengerButton0 = new LiftSimulator.NewPassengerButton();
            this.newPassengerButton1 = new LiftSimulator.NewPassengerButton();
            this.newPassengerButton2 = new LiftSimulator.NewPassengerButton();
            this.newPassengerButton3 = new LiftSimulator.NewPassengerButton();
            this.SuspendLayout();
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 10;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // fireButton
            // 
            this.fireButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fireButton.ForeColor = System.Drawing.Color.OrangeRed;
            this.fireButton.Location = new System.Drawing.Point(46, 42);
            this.fireButton.Name = "fireButton";
            this.fireButton.Size = new System.Drawing.Size(75, 23);
            this.fireButton.TabIndex = 13;
            this.fireButton.Text = "FIRE";
            this.fireButton.UseVisualStyleBackColor = true;
            this.fireButton.Click += new System.EventHandler(this.fireButton_Click);
            // 
            // newPassengerButton0
            // 
            this.newPassengerButton0.FloorIndex = 0;
            this.newPassengerButton0.Location = new System.Drawing.Point(46, 578);
            this.newPassengerButton0.Name = "newPassengerButton0";
            this.newPassengerButton0.Size = new System.Drawing.Size(75, 40);
            this.newPassengerButton0.TabIndex = 12;
            this.newPassengerButton0.Text = "New passenger";
            this.newPassengerButton0.UseVisualStyleBackColor = true;
            // 
            // newPassengerButton1
            // 
            this.newPassengerButton1.FloorIndex = 1;
            this.newPassengerButton1.Location = new System.Drawing.Point(46, 468);
            this.newPassengerButton1.Name = "newPassengerButton1";
            this.newPassengerButton1.Size = new System.Drawing.Size(75, 40);
            this.newPassengerButton1.TabIndex = 11;
            this.newPassengerButton1.Text = "New passenger";
            this.newPassengerButton1.UseVisualStyleBackColor = true;
            // 
            // newPassengerButton2
            // 
            this.newPassengerButton2.FloorIndex = 2;
            this.newPassengerButton2.Location = new System.Drawing.Point(46, 358);
            this.newPassengerButton2.Name = "newPassengerButton2";
            this.newPassengerButton2.Size = new System.Drawing.Size(75, 40);
            this.newPassengerButton2.TabIndex = 10;
            this.newPassengerButton2.Text = "New passenger";
            this.newPassengerButton2.UseVisualStyleBackColor = true;
            // 
            // newPassengerButton3
            // 
            this.newPassengerButton3.FloorIndex = 3;
            this.newPassengerButton3.Location = new System.Drawing.Point(46, 248);
            this.newPassengerButton3.Name = "newPassengerButton3";
            this.newPassengerButton3.Size = new System.Drawing.Size(75, 40);
            this.newPassengerButton3.TabIndex = 9;
            this.newPassengerButton3.Text = "New passenger";
            this.newPassengerButton3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 662);
            this.Controls.Add(this.fireButton);
            this.Controls.Add(this.newPassengerButton0);
            this.Controls.Add(this.newPassengerButton1);
            this.Controls.Add(this.newPassengerButton2);
            this.Controls.Add(this.newPassengerButton3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Text = "Elevator Simulator - MIT";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private NewPassengerButton newPassengerButton3;
        private NewPassengerButton newPassengerButton2;
        private NewPassengerButton newPassengerButton1;
        private NewPassengerButton newPassengerButton0;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Button fireButton;
    }
}

