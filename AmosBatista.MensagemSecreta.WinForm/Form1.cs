using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AmosBatista.MensagemSecreta.App;

namespace AmosBatista.MensagemSecreta.WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var twitterAPI = new TwitterAPI();
            var listaUsers = new List<string>();
            listaUsers.Add("SamuelSBatista");
            listaUsers.Add("alkaidw");
            twitterAPI.SendTwitterDirectMessage(listaUsers, "Sam e Lucas, esta é a segunda mensagema. Amós.");
        }
    }
}
