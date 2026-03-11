namespace merzzy
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pbx_mainImage = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbx_mainImage).BeginInit();
            SuspendLayout();
            // 
            // pbx_mainImage
            // 
            pbx_mainImage.BackColor = Color.Transparent;
            pbx_mainImage.Dock = DockStyle.Fill;
            pbx_mainImage.Location = new Point(0, 0);
            pbx_mainImage.Name = "pbx_mainImage";
            pbx_mainImage.Size = new Size(400, 400);
            pbx_mainImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbx_mainImage.TabIndex = 0;
            pbx_mainImage.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkMagenta;
            ClientSize = new Size(400, 400);
            Controls.Add(pbx_mainImage);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form1";
            ShowInTaskbar = false;
            Text = "Form1";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)pbx_mainImage).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbx_mainImage;
    }
}
