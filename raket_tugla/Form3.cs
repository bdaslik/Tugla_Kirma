using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace raket_tugla
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
            FileStream fs = new FileStream("skorlar.txt", FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            string[] veri;
            string satir;
            for (int i = 0; i < 5; i++)
            {
                veri = sw.ReadLine().Split(';');
                if (Convert.ToInt32(veri[1]) < 10) veri[1] = '0' + veri[1];
                if (Convert.ToInt32(veri[2]) < 10) veri[2] = '0' + veri[2];
                if (veri[0].Length>10) satir = veri[0] + "\t" + veri[1] + " : " + veri[2];
                else satir = veri[0] + "\t\t" + veri[1] + " : " + veri[2];

                listBox1.Items.Add(satir);
            }
            sw.Close();
            fs.Close();
        }

        private void Form3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
                this.Close();
        }
    }
}
