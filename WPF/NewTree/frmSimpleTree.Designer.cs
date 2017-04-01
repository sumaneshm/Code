namespace TreeDataStructure
{
    partial class frmSimpleTree
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblBlog = new System.Windows.Forms.LinkLabel();
            this.lblArticle = new System.Windows.Forms.LinkLabel();
            this.txtData = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 743);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Author: Dan Vanderboom";
            // 
            // lblBlog
            // 
            this.lblBlog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBlog.AutoSize = true;
            this.lblBlog.Location = new System.Drawing.Point(149, 768);
            this.lblBlog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBlog.Name = "lblBlog";
            this.lblBlog.Size = new System.Drawing.Size(169, 17);
            this.lblBlog.TabIndex = 3;
            this.lblBlog.TabStop = true;
            this.lblBlog.Text = "Critical Development Blog";
            this.lblBlog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblBlog_LinkClicked);
            // 
            // lblArticle
            // 
            this.lblArticle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblArticle.AutoSize = true;
            this.lblArticle.Location = new System.Drawing.Point(155, 792);
            this.lblArticle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblArticle.Name = "lblArticle";
            this.lblArticle.Size = new System.Drawing.Size(159, 17);
            this.lblArticle.TabIndex = 4;
            this.lblArticle.TabStop = true;
            this.lblArticle.Text = "Original Tree<T> Article";
            this.lblArticle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblArticle_LinkClicked);
            // 
            // txtData
            // 
            this.txtData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtData.Location = new System.Drawing.Point(21, 20);
            this.txtData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtData.Size = new System.Drawing.Size(657, 702);
            this.txtData.TabIndex = 5;
            // 
            // frmSimpleTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 826);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.lblArticle);
            this.Controls.Add(this.lblBlog);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmSimpleTree";
            this.Text = "SimpleTree<T>";
            this.Load += new System.EventHandler(this.frmSimpleTree_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lblBlog;
        private System.Windows.Forms.LinkLabel lblArticle;
        private System.Windows.Forms.TextBox txtData;
    }
}

