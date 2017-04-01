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
    public partial class frmComplexTree : Form
    {
        public frmComplexTree()
        {
            InitializeComponent();
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

        private void frmComplexTree_Load(object sender, EventArgs e)
        {
            Task MakeDinner = new Task("Make Dinner");
            
            Task PrepareIngredients = MakeDinner.Children.Add(new Task("Prepare Ingredients"));
            
            Task ChopOnions = PrepareIngredients.Children.Add(new Task("Chop Onions"));
            Task GrateCheese = PrepareIngredients.Children.Add(new Task("Grate Cheese"));
            Task MeasureMilk = PrepareIngredients.Children.Add(new Task("Measure Milk"));

            Task CookMeal = MakeDinner.Children.Add(new Task("Cook Meal"));

            Task PreheatOven = CookMeal.Children.Add(new Task("Preheat Oven"));
            Task BakeAt350 = CookMeal.Children.Add(new Task("Bake at 350 F"));

            Task Cleanup = MakeDinner.Children.Add(new Task("Clean Up"));

            Task DoDishes = Cleanup.Children.Add(new Task("Do Dishes"));
            Task WipeCounters = Cleanup.Children.Add(new Task("Wipe Down Counters"));

            bool IsAllDone = BakeAt350.Parent.Parent.Complete;
            MakeDinner.Complete = true;
            IsAllDone = MakeDinner.Complete;

            DisplayTaskSubtree(MakeDinner, 0);
        }

        private void DisplayTaskSubtree(Task Task, int Level)
        {
            string indent = string.Empty.PadLeft(Level * 3);
            txtData.Text += indent + Task.Name + "\r\n";

            Level++;
            foreach (Task ChildTask in Task.Children)
            {
                DisplayTaskSubtree(ChildTask, Level);
            }
        }
    }
}