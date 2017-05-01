using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleMusicPlayer02
{
    public partial class Form1 : Form
    {
        SimplePlayer sp = new SimplePlayer();
        int poczatek, minuta, trackTracking;
        string pathToFile;
        double liczbaSekundCalegoUtworu;
        bool czyRozpoczacOdNowa = false;
        public Form1()
        {
            InitializeComponent();
        }

        //open
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd1 = new OpenFileDialog();
            if (ofd1.ShowDialog() == DialogResult.OK)
            {
                pathToFile = ofd1.FileName;
                sp.open(pathToFile);
                textBox1.Text = ofd1.SafeFileName;
                czyRozpoczacOdNowa = true;
            }
        }

        //stop
        private void button2_Click(object sender, EventArgs e)
        {
            sp.stop();
            timer1.Stop();
            textBox1.Text = "";
        }

        //play
        private void button3_Click(object sender, EventArgs e)
        {
            if (czyRozpoczacOdNowa)
            {
                sp.stop();
                sp.open(pathToFile);
            }
            sp.play();
            double liczbaMinut = sp.getSongLength() / 60000;
            double liczbaSekund = (sp.getSongLength() / 1000) % 60;
            label3.Text = (liczbaMinut).ToString() + ":" + (liczbaSekund).ToString();
            SongTimer();
            sp.setVolume(trackBar2.Value * 100);
        }

        //pause
        private void button4_Click(object sender, EventArgs e)
        {
            sp.pause();
            if (sp.isPaused)
            {
                timer1.Stop();
            }
            else
            {
                timer1.Start();
                minuta = 0;
                poczatek = Convert.ToInt32(sp.getCurrentMilisecond() / 1000);
                trackBar1.Value = trackTracking;
            sprawdzCzas:
                if (poczatek > 59)
                {
                    minuta += 1;
                    poczatek -= 60;
                    goto sprawdzCzas;
                }
            }

        }

        private void SongTimer()
        {
            poczatek = 0;
            minuta = 0;
            trackTracking = 0;
            label1.Text = "00:00";
            timer1.Enabled = true;
            liczbaSekundCalegoUtworu = sp.getSongLength() / 1000;
            trackBar1.Maximum = Convert.ToInt32(liczbaSekundCalegoUtworu);
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            sp.setVolume(trackBar2.Value * 100);
        }

        private void trackBar1_MouseCaptureChanged(object sender, EventArgs e)
        {
            trackTracking = trackBar1.Value;
            sp.SetPosition(trackTracking * 1000);
            minuta = 0;
            poczatek = Convert.ToInt32(sp.getCurrentMilisecond() / 1000);
        sprawdzCzas2:
            if (poczatek > 59)
            {
                minuta += 1;
                poczatek -= 60;
                goto sprawdzCzas2;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            /*if (trackBar1.Value > trackTracking + 1)
            {
            }*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            poczatek += 1;
            trackTracking += 1;
            if (poczatek == 60)
            {
                minuta += 1;
                poczatek = 0;
            }

            if (minuta < 1)
            {
                if (poczatek < 10)
                {
                    label1.Text = "00:0" + poczatek.ToString();
                }
                else
                {
                    label1.Text = "00:" + poczatek.ToString();
                }
            }
            else if (minuta < 10 && minuta > 1)
            {
                if (poczatek < 10)
                {
                    label1.Text = "0" + minuta + ":0" + poczatek.ToString();
                }
                else
                {
                    label1.Text = "0" + minuta + ":" + poczatek.ToString();
                }
            }
            else if (minuta >= 10)
            {
                if (poczatek < 10)
                {
                    label1.Text = minuta + ":0" + poczatek.ToString();
                }
                else
                {
                    label1.Text = minuta + ":" + poczatek.ToString();
                }
            }
            trackBar1.Value = trackTracking;
        }
    }
}
