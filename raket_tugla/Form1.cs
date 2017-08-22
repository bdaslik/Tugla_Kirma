using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace raket_tugla
{
    
    public partial class Form1 : Form
    {
       
        public static oyun o1;
        public Form1()
        {
            InitializeComponent();
            Cursor.Hide();
            o1 = new oyun(this);
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            o1.boyutlandir();
            panel1.Location = new Point(0,this.Size.Height-100);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            oyun.tiklama = true;
            o1.zaman.Start();
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.H) // CTRL + H Tuşuna basıldığında
            {
                skorlari_goster();
            }
        }

        private void skorlari_goster()
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

       
    }

    public class oyun
    {
        public static bool tiklama = false;
        private Form1 form1;
        private Timer timer1;
        public Timer zaman;
        private skor yuksek_skor;
        private sure sayac;
        private raket r1;
        private top t1;
        private duvar d1;
        public oyun(Form1 frm)
        {
            form1 = frm;
            t1 = new top(form1);
            r1 = new raket(form1);
            d1 = new duvar(form1);
            
            sayac = new sure(form1);
            timer_ayarla();
            
        }

        public void boyutlandir() //Tuğlaları Form boyutuna göre ayarlayan fonksiyon
        {
           
            t1.boyutlandir(r1.genislik);
            d1.yeni_boyut();
        } 
    
        public void timer_ayarla()
        {
            timer1 = new Timer();
            timer1.Enabled = true;
            timer1.Interval = 50;
            timer1.Start();
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

            zaman = new Timer();
            zaman.Enabled = true;
            zaman.Interval = 1000;
            this.zaman.Tick += new System.EventHandler(this.zaman_Tick);
        }

        private void zaman_Tick(object sender, EventArgs e)
        {
            if (!tiklama) return;
            
            sayac.say();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (!tiklama)
            {
                t1.baslangic(r1.hareket(),r1.genislik);
            }
            else
            {
                r1.hareket();
                t1.hareket();
            }

            if (t1.ball.Location.X<=15 || t1.ball.Location.X>=form1.Size.Width-(t1.ball.Width+45)) //Sağ,sol Duvara çarpma
            {
                yon_tersle(true);
            }
            if (t1.ball.Location.Y<=25) //Üst Duvara çarpma kontrolü
            {
                yon_tersle(false);
            }
            
            if (t1.ball.Location.Y >= form1.Size.Height - 135 && tiklama) 
            {
                Point r_konum = r1.raket_Konum();

                if (t1.ball.Location.X>=r_konum.X-t1.ball.Width && t1.ball.Location.X<=r_konum.X+r1.genislik) //Rakete çarpma kontrolü
                {
                    yon_tersle(false);
                    t1.hiz_arttir();
                }
                else //Oyun biter..
                {
                    t1.hareket();
                    timer1.Stop();
                    zaman.Stop();
                    Cursor.Show();
                    yuksek_skor = new skor(sayac.dk, sayac.sn,form1);
                    
                }
            }



        }

        public void yon_tersle(bool yatay)
        {
            if (yatay)
            {
                switch (top.yon)
                {
                    case 1:
                        top.yon = 2;
                        break;
                    case 2:
                        top.yon = 1;
                        break;
                    case 3:
                        top.yon = 4;
                        break;
                    case 4:
                        top.yon = 3;
                        break;
                }
            }
            else
            {
                switch (top.yon)
                {
                    case 1:
                        top.yon = 4;
                        break;
                    case 2:
                        top.yon = 3;
                        break;
                    case 3:
                        top.yon = 2;
                        break;
                    case 4:
                        top.yon = 1;
                        break;
                }
            }
        }
        
    }

    public class raket
    {
        private int x;
        public int genislik;
        public int yukseklik;
        private PictureBox raketbox = new PictureBox();
        private Form1 form1;

        public raket(Form1 frm)
        {
            form1 = frm;
            raketbox.Name = "raketbox";
            raketbox.Image = Image.FromFile(Application.StartupPath + "\\raket.jpg");
            raketbox.BackColor = Color.Transparent;
            raketbox.Enabled = true;
            raketbox.Visible = true;
            raketbox.Location = new Point(0, 400);
            raketbox.Width = 100;
            raketbox.Height = 15;
            raketbox.BorderStyle = BorderStyle.Fixed3D;
            
            raketbox.SizeMode = PictureBoxSizeMode.StretchImage;
            form1.Controls.Add(raketbox);
            raketbox.BringToFront();
            
        }
      
        public Point hareket()
        {
            this.genislik = form1.Size.Width / 5;
            this.yukseklik = form1.Size.Height / 35;

            raketbox.Height = yukseklik;
            raketbox.Width = genislik;

            int konum_x = Cursor.Position.X - form1.Location.X;//imleç ile form arasındaki konum farkını kaldırmak

            x = konum_x - (genislik/2);
            if (x < 0) //raketin soldan taşmaması 
                x = 10;
            if (x > form1.Size.Width - (genislik *1.25))//raketin sağdan taşmaması
                x = form1.Size.Width - (genislik + 25);
            
          
            
            
            raketbox.Location = new Point(x,form1.Size.Height-115);

            return raketbox.Location;
        }

        public Point raket_Konum()
        {
            return raketbox.Location;
        }

    }
    
    public class top
    {
        private int hiz=20;
        private int x, y;
        public static int yon=1; //1-sag yukarı 2-sol yukarı 3-sol asagi 4-sag asagi
        Form1 form1;
        public PictureBox ball;
        public top(Form1 form1)
        {
            ball = new PictureBox();
            ball.BackColor = Color.Transparent;
            ball.Image= Image.FromFile(Application.StartupPath + "\\ball.png");
            ball.Width = 20;
            ball.Height = 20;
            ball.SizeMode = PictureBoxSizeMode.StretchImage;
            ball.Location = new Point(41, 469);
            ball.Visible = true;
            ball.Enabled = true;

            form1.Controls.Add(ball);
            ball.BringToFront();

            this.form1 = form1;
        }

        public void baslangic(Point konum,int width)
        {
            konum.Y -= 20;
            konum.X += width/2-10;
            ball.Location = konum;
        }

        public void hareket()
        { 
            switch (yon)
            {
                case 1:
                    x = ball.Location.X+hiz;
                    y = ball.Location.Y-hiz;
                    break;
                case 2:
                    x = ball.Location.X - hiz;
                    y = ball.Location.Y - hiz;
                    break;
                case 3:
                    x = ball.Location.X - hiz;
                    y = ball.Location.Y + hiz;
                    break;
                case 4:
                    x = ball.Location.X + hiz;
                    y = ball.Location.Y + hiz;
                    break;
                default:
                    break;
            }
            
            ball.Location = new Point(x, y);
        }

        public void hiz_arttir()
        {
            hiz = Convert.ToInt32(hiz*1.1);
        }

        public void boyutlandir(int width)
        {
            int x = ball.Location.X + width / 2 - 10;
            ball.Location = new Point(x, ball.Location.Y);
            ball.Height=ball.Width = form1.Width / 28;

        }
    }

    public class duvar
    {
        private PictureBox sag,sol,ust;
        private Form1 form1;
        public duvar(Form1 frm)
        {
            sag = new PictureBox();
            sol = new PictureBox();
            form1 = frm;
            sol.Name = "sol";
            sag.Name = "sag";
            sol.Image=sag.Image = Image.FromFile(Application.StartupPath + "\\duvar.jpg");
            sol.BackColor=sag.BackColor = Color.Transparent;
            sol.Enabled=sag.Enabled = true;
            sol.Visible=sag.Visible = true;
            sol.Location = new Point(0, 0);
            sag.Location = new Point(475, 0);
            sol.Width=sag.Width = 10;
            sol.Height=sag.Height = 512;
            

            sol.SizeMode=sag.SizeMode = PictureBoxSizeMode.StretchImage;
            form1.Controls.Add(sag);
            form1.Controls.Add(sol);
            sag.BringToFront();
            sol.BringToFront();

            ust = new PictureBox();

            ust.Name = "ust";
            ust.Image = Image.FromFile(Application.StartupPath + "\\duvar.jpg");
            ust.BackColor = Color.Transparent;
            ust.Enabled = true;
            ust.Visible = true;
            ust.Location = new Point(10, 0);
            ust.Width = 465;
            ust.Height = 10;

            ust.SizeMode = PictureBoxSizeMode.StretchImage;
            form1.Controls.Add(ust);
            ust.BringToFront();
        }

        public void yeni_boyut()
        {
            sag.Height = sol.Height = form1.Size.Height;
            ust.Width = form1.Width - 35;
            sag.Location = new Point(form1.Size.Width - 25, 0);
        }
    }

    public class sure
    {
        public int dk,sn;
        private Form1 form1;

        public sure(Form1 frm)
        {
            form1 = frm;
            dk = sn = 0;
        }

        public void say()
        {
            string dakika = "", saniye="";

            sn++;
            if (sn==60)
            {
                dk++;
                sn = 0;
            }

            if (dk < 10) dakika = '0' + dk.ToString();
            else dakika = dk.ToString();
            if (sn < 10) saniye = '0' + sn.ToString();
            else saniye = sn.ToString();

            form1.label1.Text = dakika + " : " + saniye;
        }

        public void sifirla()
        {
            dk = sn = 0;
        }
    }

    public class skor
    {
        private string[] isimler=new string[5];
        private int[,] sureler = new int[5, 2];
        private string[] veri;
        string satir = "";
        Form1 form1;
        bool varmi = false;

        public skor(int dk,int sn,Form1 frm)
        {
            form1 = frm;
            oku();
            karsilastir(dk, sn);

        }
        public void oku()
        {
            FileStream fs = new FileStream("skorlar.txt", FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            for (int i = 0; i < 5; i++)
            {
                satir = sw.ReadLine();
                if (satir!=null)
                {
                    veri = satir.Split(';');
                    isimler[i] = veri[0];
                    sureler[i, 0] = Convert.ToInt32(veri[1]);
                    sureler[i, 1] = Convert.ToInt32(veri[2]);
                }
                
            }
            sw.Close();
            fs.Close();
        }

        public void karsilastir(int dk,int sn)
        {
            
            for (int i = 0; i < 5; i++)
            {
                if (sureler[i, 0] < dk || (sureler[i, 0] == dk && sureler[i, 1] < sn))
                {
                    Form2 form2 = new Form2(i,dk,sn,isimler,sureler,form1);
                    form2.Show();
                    varmi = true;
                    break;
                }
               
            }
            if (!varmi)
            {
                form1.Close();
            }
        }

      
    }

}
