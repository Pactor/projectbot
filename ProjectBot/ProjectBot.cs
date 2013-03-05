using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ProjectBot.Managers;
using MetaBuilders.Irc.Messages;
using MetaBuilders.Irc.Network;
using MetaBuilders.Irc;
using System.Xml;
using ProjectBot.Events;

namespace ProjectBot
{
    public partial class ProjectBot : Form
    {
        static ProjectBot This { get; set; }
        private Client client;
        string Host { get; set; }
        string Channel { get; set; }
        string BotNick { get; set; }
        private MonitorFeed FeedMonitor { get; set; }
        private System.Windows.Forms.Timer CheckFeeds { get; set; }

        public ProjectBot()
        {
            This = this;
            InitializeComponent();
            CheckFeeds = new Timer();
            CheckFeeds.Tick += new EventHandler(CheckFeeds_Tick);
            XMLManager.AddFeed("projectircbot");
        }
        private void CheckFeeds_Tick(object sender, EventArgs e)
        {
            int newInterval = 0;
            int.TryParse(txt_Interval.Text+"000", out newInterval);
            WriteLine("Got new timer interval of " + newInterval.ToString());
            if (newInterval == 0) { newInterval = 10000; } // 10 second default
            CheckFeeds.Interval = newInterval;

            foreach (string feed in XMLManager.GetAllFeedNames())
            {
                FeedMonitor = new MonitorFeed(feed);
                FeedMonitor.changeDetected += new MonitorFeed.ChangeDetected(FeedMonitor_changeDetected);
            }

        }
        private void FeedMonitor_changeDetected(object sender, ChangeArgs e)
        {
            ProjectBot.WriteLine(e.Project.Description);
        }
        #region WriteLine
        public static void WriteLine(string msg)
        {
            if (This.rtfOutput.InvokeRequired)
            {
                This.Invoke((MethodInvoker)
                    delegate
                    {
                        This.rtfOutput.AppendText(msg + "\n");
                    });
            }
            else
            {
                This.rtfOutput.AppendText(msg + "\n");
            }
        }
        public static void WriteLine(string msg, Color color)
        {
            if (This.rtfOutput.InvokeRequired)
            {
                This.Invoke((MethodInvoker)
                    delegate
                    {
                        This.rtfOutput.SelectionColor = color;
                        This.rtfOutput.AppendText(msg + "\n");
                    });
            }
            else
            {
                This.rtfOutput.SelectionColor = color;
                This.rtfOutput.AppendText(msg + "\n");
            }
        }
        #endregion
        private void ProjectBot_Load(object sender, EventArgs e)
        {
            // Make sure our config dir is there
            if (!Directory.Exists(Globals.AppDir)) { Directory.CreateDirectory(Globals.AppDir); }
            // Initilize our config file.
            XMLManager.Load();
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            if (txt_Servername.Text == string.Empty) { return; }
            if (txt_Channel.Text == string.Empty) { return; }
            if (txt_BotNick.Text == string.Empty) { return; }
            Host = txt_Servername.Text;
            Channel = txt_Channel.Text;
            BotNick = txt_BotNick.Text;
            client = new Client(Host, txt_BotNick.Text, "A ProjectBot");

            Ident.Service.User = this.client.User;
            // Once I'm welcomed, I can start joining channels
            client.Messages.Welcome += new EventHandler<IrcMessageEventArgs<WelcomeMessage>>(welcomed);

            // People are chatting, pay attention so I can be a lame echobot :)
            client.Messages.Chat += new EventHandler<IrcMessageEventArgs<TextMessage>>(chatting);

            client.Messages.TimeRequest += new EventHandler<IrcMessageEventArgs<TimeRequestMessage>>(timeRequested);

            client.DataReceived += new EventHandler<ConnectionDataEventArgs>(dataGot);
            client.DataSent += new EventHandler<ConnectionDataEventArgs>(dataSent);

            client.Connection.Disconnected += new EventHandler<ConnectionDataEventArgs>(logDisconnected);

            client.EnableAutoIdent = false;

            // Since I'm a Windows.Forms application, i pass in this form to the Connect method so it can sync with me.
            try
            {
                client.Connection.Connect(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void logDisconnected(Object sender, ConnectionDataEventArgs e)
        {
            String data = "*** Disconnected: " + e.Data;
            WriteLine(data, Color.Red);
        }

        private void dataGot(Object sender, ConnectionDataEventArgs e)
        {
            String data = "<-" + e.Data;
            WriteLine(data);
        }

        private void dataSent(Object sender, ConnectionDataEventArgs e)
        {
         //   this.textBox1.Text += "->" + e.Data + System.Environment.NewLine;
           // this.textBox1.Select(this.textBox1.Text.Length - 1, 1);
           // this.textBox1.ScrollToCaret();
        }

        private void chatting(Object sender, IrcMessageEventArgs<TextMessage> e)
        {
            //if (e.Message.Text.StartsWith(BotNick))
            //{
            string[] args = e.Message.Text.Split(' ');
            if (args.Length > 1)
            {
                if (args[0].ToLower() == BotNick.ToLower())
                {
                    string reply = string.Empty;
                    // I am being talked to, what am i being asked ?
                    if (args.Length >= 2)
                    {
                        switch (args[1].ToLower())
                        {
                            case "addfeed":
                                break;
                                case "removefeed":
                                break;
                            case "help":
                                reply = "help here";
                                break;
                            default:
                                reply = "No known command.";
                                break;
                        }
                        if (reply != string.Empty) { client.SendChat(reply, Channel); }
                    }
                }
            }
            //}
        }

        private void timeRequested(Object sender, IrcMessageEventArgs<TimeRequestMessage> e)
        {
            MetaBuilders.Irc.Messages.TimeReplyMessage reply = new MetaBuilders.Irc.Messages.TimeReplyMessage();
            reply.CurrentTime = DateTime.Now.ToLongTimeString();
            reply.Target = e.Message.Sender.Nick;
            client.Send(reply);
        }

        private void welcomed(Object sender, IrcMessageEventArgs<WelcomeMessage> e)
        {
            client.SendJoin(Channel);
        }

        private void SayIt_Click(object sender, System.EventArgs e)
        {
            // change the first channel to text to send
            client.SendChat(Channel, Channel);
            //this.ChatEntry.Text = "";
        }

        private void SendIt_Click(object sender, System.EventArgs e)
        {
            MetaBuilders.Irc.Messages.GenericMessage msg = new MetaBuilders.Irc.Messages.GenericMessage();
            //if (msg.CanParse(this.ChatEntry.Text))
            //{
             //   msg.Parse(this.ChatEntry.Text);
              //  client.Send(msg);
            //}
            //else
            //{
            //    MessageBox.Show("Cannot Parse Your Command.");
            //}
        }

        private void btn_Disconnect_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                client.SendQuit("I'm Outa Here");
            }
        }

        private void txt_TestRead_Click(object sender, EventArgs e)
        {
            WriteLine("Reading ...");
            CheckFeeds.Start();
        }
    }
}
