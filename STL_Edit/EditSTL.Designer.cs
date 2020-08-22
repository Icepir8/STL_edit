namespace STL_Edit
{
    partial class EditSTL
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLookXYZ = new System.Windows.Forms.Button();
            this.btnLookX = new System.Windows.Forms.Button();
            this.btnLookY = new System.Windows.Forms.Button();
            this.btnLookZ = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rotateView1 = new STL_Edit.RotateView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLookXYZ);
            this.panel1.Controls.Add(this.btnLookX);
            this.panel1.Controls.Add(this.btnLookY);
            this.panel1.Controls.Add(this.btnLookZ);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(917, 33);
            this.panel1.TabIndex = 0;
            // 
            // btnLookXYZ
            // 
            this.btnLookXYZ.Location = new System.Drawing.Point(623, 5);
            this.btnLookXYZ.Name = "btnLookXYZ";
            this.btnLookXYZ.Size = new System.Drawing.Size(75, 23);
            this.btnLookXYZ.TabIndex = 5;
            this.btnLookXYZ.Text = "Tri-View";
            this.btnLookXYZ.UseVisualStyleBackColor = true;
            this.btnLookXYZ.Click += new System.EventHandler(this.btnLookXYZ_Click);
            // 
            // btnLookX
            // 
            this.btnLookX.Location = new System.Drawing.Point(524, 5);
            this.btnLookX.Name = "btnLookX";
            this.btnLookX.Size = new System.Drawing.Size(75, 23);
            this.btnLookX.TabIndex = 4;
            this.btnLookX.Text = "Look at X";
            this.btnLookX.UseVisualStyleBackColor = true;
            this.btnLookX.Click += new System.EventHandler(this.btnLookX_Click);
            // 
            // btnLookY
            // 
            this.btnLookY.Location = new System.Drawing.Point(425, 5);
            this.btnLookY.Name = "btnLookY";
            this.btnLookY.Size = new System.Drawing.Size(75, 23);
            this.btnLookY.TabIndex = 3;
            this.btnLookY.Text = "Look at Y";
            this.btnLookY.UseVisualStyleBackColor = true;
            this.btnLookY.Click += new System.EventHandler(this.btnLookY_Click);
            // 
            // btnLookZ
            // 
            this.btnLookZ.Location = new System.Drawing.Point(326, 5);
            this.btnLookZ.Name = "btnLookZ";
            this.btnLookZ.Size = new System.Drawing.Size(75, 23);
            this.btnLookZ.TabIndex = 2;
            this.btnLookZ.Text = "Look at Z";
            this.btnLookZ.UseVisualStyleBackColor = true;
            this.btnLookZ.Click += new System.EventHandler(this.btnLookZ_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureBox4);
            this.panel2.Controls.Add(this.pictureBox3);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(850, 456);
            this.panel2.TabIndex = 1;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Location = new System.Drawing.Point(503, 265);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(100, 50);
            this.pictureBox4.TabIndex = 8;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(198, 247);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(100, 50);
            this.pictureBox3.TabIndex = 7;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(503, 101);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 50);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(198, 101);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.rotateView1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(850, 33);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(67, 456);
            this.panel3.TabIndex = 11;
            // 
            // rotateView1
            // 
            this.rotateView1.Location = new System.Drawing.Point(0, 0);
            this.rotateView1.Name = "rotateView1";
            this.rotateView1.Size = new System.Drawing.Size(64, 64);
            this.rotateView1.TabIndex = 10;
            this.rotateView1.ViewChanged += new System.EventHandler(this.rotateView1_ViewChanged_1);
            // 
            // EditSTL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 489);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "EditSTL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ClientSizeChanged += new System.EventHandler(this.Form1_Resize);
            this.SizeChanged += new System.EventHandler(this.EditSTL_SizeChanged);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLookXYZ;
        private System.Windows.Forms.Button btnLookX;
        private System.Windows.Forms.Button btnLookY;
        private System.Windows.Forms.Button btnLookZ;
        private System.Windows.Forms.Panel panel3;
        private RotateView rotateView1;
    }
}

