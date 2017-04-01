using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Windows.Forms;
using System.Drawing;
using TreeLibrary;

[assembly: DebuggerVisualizer(
       typeof(TreeVisualizer.TreeDebuggerVisualizer),
       typeof(VisualizerObjectSource),
       Target = typeof(Node),
       Description = "Node Visualizer")]
namespace TreeVisualizer
{
    public class TreeDebuggerVisualizer : DialogDebuggerVisualizer
    {
        override protected void Show(IDialogVisualizerService windowService,
                          IVisualizerObjectProvider objectProvider)
        {
            //Image image = (Image)objectProvider.GetObject();

            //Form form = new Form();
            //form.Text = string.Format("Width: {0}, Height: {1}",
            //                         image.Width, image.Height);
            //form.ClientSize = new Size(image.Width, image.Height);
            //form.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            //PictureBox pictureBox = new PictureBox();
            //pictureBox.Image = image;
            //pictureBox.Parent = form;
            //pictureBox.Dock = DockStyle.Fill;
            //pictureBox.Width = 300;
            //pictureBox.Height = 200;
            //windowService.ShowDialog(form);

            //System.Diagnostics.Debugger.Break();
             
            Node node = (Node)objectProvider.GetObject();
            
            WindowsVisualizer visual = new WindowsVisualizer();
            visual.SetNodeToView(node);

            windowService.ShowDialog(visual);
        }
    }
}
