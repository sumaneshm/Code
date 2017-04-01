namespace TreeDataStructure
{
    partial class frmComplexTree
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
            this.label1.Location = new System.Drawing.Point(98, 393);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Author: Dan Vanderboom";
            // 
            // lblBlog
            // 
            this.lblBlog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBlog.AutoSize = true;
            this.lblBlog.Location = new System.Drawing.Point(98, 413);
            this.lblBlog.Name = "lblBlog";
            this.lblBlog.Size = new System.Drawing.Size(128, 13);
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
            this.lblArticle.Location = new System.Drawing.Point(102, 433);
            this.lblArticle.Name = "lblArticle";
            this.lblArticle.Size = new System.Drawing.Size(118, 13);
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
            this.txtData.Location = new System.Drawing.Point(12, 16);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(307, 345);
            this.txtData.TabIndex = 6;
            // 
            // frmComplexTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 460);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.lblArticle);
            this.Controls.Add(this.lblBlog);
            this.Controls.Add(this.label1);
            this.Name = "frmComplexTree";
            this.Text = "ComplexTree<T>";
            this.Load += new System.EventHandler(this.frmComplexTree_Load);
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

