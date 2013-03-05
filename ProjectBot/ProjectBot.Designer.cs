namespace ProjectBot
{
    partial class ProjectBot
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
            this.rtfOutput = new System.Windows.Forms.RichTextBox();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Channel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_Servername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_BotNick = new System.Windows.Forms.TextBox();
            this.btn_Disconnect = new System.Windows.Forms.Button();
            this.txt_TestRead = new System.Windows.Forms.Button();
            this.txt_Interval = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rtfOutput
            // 
            this.rtfOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtfOutput.Dock = System.Windows.Forms.DockStyle.Right;
            this.rtfOutput.HideSelection = false;
            this.rtfOutput.Location = new System.Drawing.Point(356, 0);
            this.rtfOutput.Name = "rtfOutput";
            this.rtfOutput.ReadOnly = true;
            this.rtfOutput.Size = new System.Drawing.Size(323, 292);
            this.rtfOutput.TabIndex = 0;
            this.rtfOutput.Text = "";
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(12, 137);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(114, 29);
            this.btn_Connect.TabIndex = 1;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Servername:";
            // 
            // txt_Channel
            // 
            this.txt_Channel.Location = new System.Drawing.Point(107, 63);
            this.txt_Channel.Name = "txt_Channel";
            this.txt_Channel.Size = new System.Drawing.Size(131, 22);
            this.txt_Channel.TabIndex = 3;
            this.txt_Channel.Text = "#test";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Channel:";
            // 
            // txt_Servername
            // 
            this.txt_Servername.Location = new System.Drawing.Point(107, 35);
            this.txt_Servername.Name = "txt_Servername";
            this.txt_Servername.Size = new System.Drawing.Size(131, 22);
            this.txt_Servername.TabIndex = 5;
            this.txt_Servername.Text = "localhost";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Bot Nick";
            // 
            // txt_BotNick
            // 
            this.txt_BotNick.Location = new System.Drawing.Point(107, 92);
            this.txt_BotNick.Name = "txt_BotNick";
            this.txt_BotNick.Size = new System.Drawing.Size(131, 22);
            this.txt_BotNick.TabIndex = 7;
            this.txt_BotNick.Text = "ProjectBot";
            // 
            // btn_Disconnect
            // 
            this.btn_Disconnect.Location = new System.Drawing.Point(171, 137);
            this.btn_Disconnect.Name = "btn_Disconnect";
            this.btn_Disconnect.Size = new System.Drawing.Size(114, 29);
            this.btn_Disconnect.TabIndex = 8;
            this.btn_Disconnect.Text = "Disconnect";
            this.btn_Disconnect.UseVisualStyleBackColor = true;
            this.btn_Disconnect.Click += new System.EventHandler(this.btn_Disconnect_Click);
            // 
            // txt_TestRead
            // 
            this.txt_TestRead.Location = new System.Drawing.Point(107, 224);
            this.txt_TestRead.Name = "txt_TestRead";
            this.txt_TestRead.Size = new System.Drawing.Size(114, 29);
            this.txt_TestRead.TabIndex = 9;
            this.txt_TestRead.Text = "Test Read";
            this.txt_TestRead.UseVisualStyleBackColor = true;
            this.txt_TestRead.Click += new System.EventHandler(this.txt_TestRead_Click);
            // 
            // txt_Interval
            // 
            this.txt_Interval.Location = new System.Drawing.Point(136, 185);
            this.txt_Interval.Name = "txt_Interval";
            this.txt_Interval.Size = new System.Drawing.Size(55, 22);
            this.txt_Interval.TabIndex = 10;
            this.txt_Interval.Text = "10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Check Interval:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "seconds";
            // 
            // ProjectBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 292);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_Interval);
            this.Controls.Add(this.txt_TestRead);
            this.Controls.Add(this.btn_Disconnect);
            this.Controls.Add(this.txt_BotNick);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_Servername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_Channel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.rtfOutput);
            this.Name = "ProjectBot";
            this.Text = "ProjectBot";
            this.Load += new System.EventHandler(this.ProjectBot_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtfOutput;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_Channel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_Servername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_BotNick;
        private System.Windows.Forms.Button btn_Disconnect;
        private System.Windows.Forms.Button txt_TestRead;
        private System.Windows.Forms.TextBox txt_Interval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

