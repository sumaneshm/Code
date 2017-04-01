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
    public partial class frmSimpleTree : Form
    {
        public frmSimpleTree()
        {
            InitializeComponent();
        }

        void frmSimpleTree_Load(object sender, EventArgs e)
        {
            MakeDinner();
            TestTraversals();
        }

        void TestTraversals()
        {
            var root = new SimpleTreeNode<string>("a");

            var b = root.Children.Add("b");
            var c = root.Children.Add("c");

            var d = b.Children.Add("d");
            var e = b.Children.Add("e");

            var f = c.Children.Add("f");
            var g = c.Children.Add("g");

            txtData.Text += "\r\n\r\nTraversals\r\n\r\n";
            DisplayTree(root, 0);

            txtData.Text += "\r\nDepth-First, Top-Down: ";
            foreach (SimpleTreeNode<string> node in root.GetEnumerable(TreeTraversalType.DepthFirst, TreeTraversalDirection.TopDown))
            {
                txtData.Text += node.Value;
            }

            txtData.Text += "\r\nDepth-First, Bottom-Up: ";
            foreach (SimpleTreeNode<string> node in root.GetEnumerable(TreeTraversalType.DepthFirst, TreeTraversalDirection.BottomUp))
            {
                txtData.Text += node.Value;
            }

            txtData.Text += "\r\nBreadth-First, Top-Down: ";
            foreach (SimpleTreeNode<string> node in root.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown))
            {
                txtData.Text += node.Value;
            }

            txtData.Text += "\r\nBreadth-First, Bottom-Up: ";
            foreach (SimpleTreeNode<string> node in root.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.BottomUp))
            {
                txtData.Text += node.Value;
            }
        }

        void MakeDinner()
        {
            // declare a new tree
            SimpleTree<string> MakeDinnerTree = new SimpleTree<string>();

            // the tree will also be our root node
            SimpleTreeNode<string> root = MakeDinnerTree;
            root.Value = "make dinner";

            SimpleTreeNode<string> prepare = root.Children.Add("prepare ingredients");
            prepare.Children.Add("chop onions");
            prepare.Children.Add("grate cheese");
            prepare.Children.Add("measure milk");

            root.Children.Add("bake at 350 degrees F");

            SimpleTreeNode<string> cleanup = root.Children.Add("clean up");
            cleanup.Children.Add("do dishes");
            cleanup.Children.Add("wipe down counters");

            txtData.Text += "-- cleanup subtree --\r\n\r\n";

            // display only the "clean up" subtree, passing in a SimpleTreeNode
            DisplayTree(cleanup, 0);

            txtData.Text += "\r\n-- entire tree --\r\n\r\n";

            // display the whole subtree, passing in a SimpleTree
            DisplayTree(MakeDinnerTree, 0);
        }

        void DisplayTree(SimpleTreeNode<string> Subtree, int Level)
        {
            string indent = string.Empty.PadLeft(Level * 3);
            txtData.Text += indent + Subtree.Value + "\r\n";

            Level++;
            foreach (SimpleTreeNode<string> node in Subtree.Children)
            {
                DisplayTree(node, Level);
            }
        }

        #region links

        private void lblBlog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://dvanderboom.wordpress.com"));
        }

        private void lblArticle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/"));
        }

        #endregion
    }
}