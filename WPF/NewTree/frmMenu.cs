// This collection of non-binary tree data structures created by Dan Vanderboom.
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace TreeDataStructure
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        private void lblBlog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://dvanderboom.wordpress.com"));
        }

        private void lblArticle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/"));
        }

        private void btnSimpleTree_Click(object sender, EventArgs e)
        {
            frmSimpleTree simple = new frmSimpleTree();
            simple.Show();
        }

        private void btnComplexTree_Click(object sender, EventArgs e)
        {
            frmComplexTree complex = new frmComplexTree();
            complex.Show();
        }
    }
}