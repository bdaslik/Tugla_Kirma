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
    public partial class Form2 : Form
    {
        public string isim="isimsiz";
        Form1 form1;

        public int sira, dk, sn;
        string[] isimler;
        int[,] sureler;
        public Form2(int sira,int dk,int sn,string[] isimler,int[,] sureler,Form1 frm)
        {
            InitializeComponent();
            form1 = frm;
            this.sira = sira;
            this.dk = dk;
            this.sn = sn;
            this.isimler = isimler;
            this.sureler = sureler;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isim = textBox1.Text;

            kayit();
            Cursor.Hide();
            this.Close();
        }

        public void kayit()
        {
            File.Delete("skorlar.txt");
            FileStream fs = new FileStream("skorlar.txt", FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            for (int j = 4; j > sira; j--)
            {
                isimler[j] = isimler[j - 1];
                sureler[j, 0] = sureler[j - 1, 0];
                sureler[j, 1] = sureler[j - 1, 1];
            }
            isimler[sira] = isim;
            sureler[sira, 0] = dk;
            sureler[sira, 1] = sn;

            for (int i = 0; i < 5; i++)
            {
                sw.WriteLine(isimler[i] + ';' + sureler[i, 0].ToString() + ';' + sureler[i, 1].ToString());
            }

            sw.Close();

            MessageBox.Show("Skorunuz Başarıyla Kaydedildi..");
            form1.Close();
        }
    }
}
