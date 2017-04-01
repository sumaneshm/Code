using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindsorCastleLibrary;

namespace WindsorCastleExecutables
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mainClass =  Program.container.Resolve<MainClass>();
            mainClass.DoSomething();
            label1.Text = mainClass.object1.SomeObject.ToString();
            label2.Text = mainClass.object2.SomeOtherObject.ToString();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
