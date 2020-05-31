using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using System.Text.Json;

namespace Q_pop
{
    public partial class Form1 : Form
    {

        Queue queue = new Queue(Properties.Settings.Default.DefaultSkill);
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            queue.checkAceyus();
            PopupNotifier popup = new PopupNotifier();
            popup.TitleText = Properties.Settings.Default.DefaultSkill;
            popup.TitleColor = System.Drawing.Color.Blue;
            popup.TitleFont = SystemFonts.IconTitleFont;
            popup.ContentText = queue.availability;
            popup.Popup();// show

            textBoxStatus.AppendText(System.DateTime.Now.ToString() + "\r\n");            
            textBoxStatus.AppendText(queue.availability + "\r\n\r\n");            
            textBoxStatus.AppendText(System.DateTime.Now.ToString() + "\r\n");            
            textBoxStatus.AppendText(queue.callsWaiting + "\r\n\r\n");
            













        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}