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
        List<URL> URLs = new List<URL>();

        public Form1()
        {
            InitializeComponent();
        }

        private void CargarComboBox()
        {            
            //Limpiar el combobox            
            comboBoxUrls.DataSource = null;
            comboBoxUrls.Refresh();

            //Como nuestra clase tiene varias propiedades, le debemos indicar
            //al combobox cual de las propiedades tiene que mostrar
            comboBoxUrls.DisplayMember = "url";
            //La fuente de datos del combobox sera la lista de URLs
            comboBoxUrls.DataSource = URLs;
            comboBoxUrls.Refresh();
            
        }
        private void Guardar(string fileName)
        {            
            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);            
            StreamWriter writer = new StreamWriter(stream);
            //Hay que recorrer la lista para ir guardando cada elemento de la lista            

            foreach (var u in URLs)
            {                                
                writer.WriteLine(u.url);
                writer.WriteLine(u.veces);
                writer.WriteLine(u.fecha);
            }            
            writer.Close();
        }
        private void Leer(string fileName)
        {

            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);

            while (reader.Peek() > -1)
            {
                //lo leido del archivo lo cargamos a una url temporal
                URL urltemp = new URL();
                urltemp.url = reader.ReadLine();
                urltemp.veces = Convert.ToInt32(reader.ReadLine());
                urltemp.fecha = Convert.ToDateTime(reader.ReadLine());

                //la url temporal la guardamos en la lista
                URLs.Add(urltemp);

            }

            reader.Close();
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

            //Solucion usando listas

            //Buscar si la URL ya esta en la lista para actualizar las veces y la fecha
            int posicion = URLs.FindIndex(u => u.url == uri);

            //si no está en la lista insertarla en la lista
            if (posicion == -1)
            {
                URL url = new URL();
                url.url = uri;
                url.veces = 1;
                url.fecha = DateTime.Now;

                URLs.Add(url);
            }
            //si ya está actualizar las veces y la fecha
            else
            {
                URLs[posicion].veces++;
                URLs[posicion].fecha = DateTime.Now;
            }
                        
            Guardar("Historial.txt");
            CargarComboBox();
           
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
              
        private void Form1_Load(object sender, EventArgs e)
        {
            Leer("Historial.txt");
            CargarComboBox();
        }

        private void porFechaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            URLs = URLs.OrderByDescending(u => u.fecha).ToList();
            CargarComboBox();
        }

        private void porVisitasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            URLs = URLs.OrderByDescending(u => u.veces).ToList();
            CargarComboBox();
        }
    }
}
