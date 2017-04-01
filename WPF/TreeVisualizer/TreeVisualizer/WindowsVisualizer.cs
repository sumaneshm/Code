using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeLibrary;

namespace TreeVisualizer
{
    public partial class WindowsVisualizer : Form
    {
        public WindowsVisualizer()
        {
            InitializeComponent();
        }

        public void SetNodeToView(Node nodeVM)
        {
           // this.wpfVisualizer1.theNodeView.DataContext = nodeVM;
            this.wpfVisualizer1.SetNodeToView(nodeVM);
            
        }
    }
}
