namespace TreeDataStructure
{
    partial class frmMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSimpleTree = new System.Windows.Forms.Button();
            this.btnComplexTree = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblBlog = new System.Windows.Forms.LinkLabel();
            this.lblArticle = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // btnSimpleTree
            // 
            this.btnSimpleTree.Location = new System.Drawing.Point(20, 16);
            this.btnSimpleTree.Name = "btnSimpleTree";
            this.btnSimpleTree.Size = new System.Drawing.Size(186, 42);
            this.btnSimpleTree.TabIndex = 0;
            this.btnSimpleTree.Text = "SimpleTree<T> Example";
            this.btnSimpleTree.UseVisualStyleBackColor = true;
            this.btnSimpleTree.Click += new System.EventHandler(this.btnSimpleTree_Click);
            // 
            // btnComplexTree
            // 
            this.btnComplexTree.Location = new System.Drawing.Point(20, 68);
            this.btnComplexTree.Name = "btnComplexTree";
            this.btnComplexTree.Size = new System.Drawing.Size(186, 42);
            this.btnComplexTree.TabIndex = 1;
            this.btnComplexTree.Text = "ComplexTree<T> Example";
            this.btnComplexTree.UseVisualStyleBackColor = true;
            this.btnComplexTree.Click += new System.EventHandler(this.btnComplexTree_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Author: Dan Vanderboom";
            // 
            // lblBlog
            // 
            this.lblBlog.AutoSize = true;
            this.lblBlog.Location = new System.Drawing.Point(46, 142);
            this.lblBlog.Name = "lblBlog";
            this.lblBlog.Size = new System.Drawing.Size(128, 13);
            this.lblBlog.TabIndex = 3;
            this.lblBlog.TabStop = true;
            this.lblBlog.Text = "Critical Development Blog";
            this.lblBlog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblBlog_LinkClicked);
            // 
            // lblArticle
            // 
            this.lblArticle.AutoSize = true;
            this.lblArticle.Location = new System.Drawing.Point(50, 162);
            this.lblArticle.Name = "lblArticle";
            this.lblArticle.Size = new System.Drawing.Size(118, 13);
            this.lblArticle.TabIndex = 4;
            this.lblArticle.TabStop = true;
            this.lblArticle.Text = "Original Tree<T> Article";
            this.lblArticle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblArticle_LinkClicked);
            // 
            // frmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(227, 189);
            this.Controls.Add(this.lblArticle);
            this.Controls.Add(this.lblBlog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnComplexTree);
            this.Controls.Add(this.btnSimpleTree);
            this.Name = "frmMenu";
            this.Text = "Tree Data Structures";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSimpleTree;
        private System.Windows.Forms.Button btnComplexTree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lblBlog;
        private System.Windows.Forms.LinkLabel lblArticle;
    }
}

