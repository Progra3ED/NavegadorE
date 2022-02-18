using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NavegadorE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Guardar(string fileName, string texto)
        {            
            FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);            
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(texto);            
            writer.Close();
        }


        private void buttonIr_Click(object sender, EventArgs e)
        {
            string uri = "";

            if (comboBoxUrls.SelectedItem != null)
                uri = comboBoxUrls.SelectedItem.ToString();
            if (comboBoxUrls.Text != null)
                uri = comboBoxUrls.Text;

            if (!uri.Contains("."))
                uri = "https://www.google.com/search?q=" + uri;
            if (!uri.Contains("https://"))
                uri = "https://" + uri;

            webBrowser1.Navigate(new Uri(uri));

            int yaEsta = 0;

            for (int i = 0; i < comboBoxUrls.Items.Count; i++)
            {
                if (uri == comboBoxUrls.Items[i].ToString())
                    yaEsta++;
            }

            if (yaEsta == 0)
            {
                comboBoxUrls.Items.Add(uri);
                Guardar("Historial.txt", uri);
            }

        }

        private void adelanteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void atrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoHome();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void Leer (string fileName)
        {
            
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            
            while (reader.Peek() > -1)
            {
                //richTextBox1.AppendText(reader.ReadLine());
                comboBoxUrls.Items.Add(reader.ReadLine());
            }

            reader.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Leer("Historial.txt");

        }
    }
}
