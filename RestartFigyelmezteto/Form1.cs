using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace RestartFigyelmezteto
{
    

    public partial class Form1 : Form
    {

        System.Timers.Timer t;
        int m = 9, s = 59 ;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 f1 = new Form1();
            this.Hide();
            f1.ShowDialog();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "A vírusirtó frissítése miatt újra kell a számítógépet indítani.\nKérlek mentsd el amin dolgozol.\nAz OK gomb megnyomásával azonnal újraindul a számítógép\nÜdv, Horváth Péter\n";
            t = new System.Timers.Timer();
            t.Interval = 500;
            t.Elapsed += OnTimeEvent;
            t.Start();
        }

        private void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s -=  1;
                if (s == 0 )
                {
                    s = 60;
                    if (m == 0)
                    {
                        t.Stop();
                        if (MessageBox.Show("Ha nem mentettél, akkor minden adatot el fog veszni!", "Így jártál", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                        {
                            FormBorderStyle = FormBorderStyle.None;
                            label3.Visible = false;
                        }
                    }
                    else
                    m -= 1;
                }
                if (m == 0 && s == 0)
                {
                    t.Stop();
                    MessageBox.Show("Ha nem mentettél, akkor minden adatot el fog veszni!","Így jártál",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                label3.Text = string.Format("{0}:{1}",m.ToString().PadLeft(1,'0'),s.ToString().PadLeft(1, '0'));
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            Log();

            Process pr = new Process();
            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = @"C:\windows\system32\cmd.exe";
            process.Arguments = @"/c" + "shutdown -r -t 1";
            pr.StartInfo = process;
            pr.Start();
        }

        private void Log() 
        {
            if (File.Exists(@"\\fs\mindenki\IT\RESTART_LOG.txt"))
            {
                StreamWriter sw = File.AppendText(@"\\fs\mindenki\IT\RESTART_LOG.txt");
                sw.WriteLine(Environment.MachineName + "\t" + DateTime.Now);
                sw.Flush();
                sw.Close();

            }
            else
            {
                using (File.Create(@"\\fs\mindenki\IT\RESTART_LOG.txt"))
                {
                    Console.WriteLine("Elkészült");
                }
                StreamWriter sw = File.AppendText(@"\\fs\mindenki\IT\RESTART_LOG.txt");
                sw.WriteLine(Environment.MachineName + "\t" + DateTime.Now);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
