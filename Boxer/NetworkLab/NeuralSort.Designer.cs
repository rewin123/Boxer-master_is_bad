namespace NetworkLab
{
    partial class NeuralSort
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
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.увеличитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.аглютинацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.нетАггToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.следующееИзображениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(721, 657);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.увеличитьToolStripMenuItem,
            this.аглютинацияToolStripMenuItem,
            this.нетАггToolStripMenuItem,
            this.следующееИзображениеToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(218, 92);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // увеличитьToolStripMenuItem
            // 
            this.увеличитьToolStripMenuItem.Name = "увеличитьToolStripMenuItem";
            this.увеличитьToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.увеличитьToolStripMenuItem.Text = "Увеличить";
            // 
            // аглютинацияToolStripMenuItem
            // 
            this.аглютинацияToolStripMenuItem.Name = "аглютинацияToolStripMenuItem";
            this.аглютинацияToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.аглютинацияToolStripMenuItem.Text = "Аглютинация";
            // 
            // нетАггToolStripMenuItem
            // 
            this.нетАггToolStripMenuItem.Name = "нетАггToolStripMenuItem";
            this.нетАггToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.нетАггToolStripMenuItem.Text = "Нет агг";
            // 
            // следующееИзображениеToolStripMenuItem
            // 
            this.следующееИзображениеToolStripMenuItem.Name = "следующееИзображениеToolStripMenuItem";
            this.следующееИзображениеToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.следующееИзображениеToolStripMenuItem.Text = "Следующее изображение";
            // 
            // NeuralSort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 682);
            this.Controls.Add(this.pictureBox1);
            this.Name = "NeuralSort";
            this.Text = "NeuralSort";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem увеличитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem аглютинацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem нетАггToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem следующееИзображениеToolStripMenuItem;
    }
}