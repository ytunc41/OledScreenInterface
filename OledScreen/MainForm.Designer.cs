namespace OledScreen
{
    partial class MainForm
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
            this.pcbox1 = new System.Windows.Forms.PictureBox();
            this.pcbox2 = new System.Windows.Forms.PictureBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.pcbox3 = new System.Windows.Forms.PictureBox();
            this.pcbox4 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbox128x64 = new System.Windows.Forms.CheckBox();
            this.chbox128x32 = new System.Windows.Forms.CheckBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.lblHeight = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.btnAddImage = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pcbox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbox4)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pcbox1
            // 
            this.pcbox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbox1.Location = new System.Drawing.Point(152, 12);
            this.pcbox1.Name = "pcbox1";
            this.pcbox1.Size = new System.Drawing.Size(512, 384);
            this.pcbox1.TabIndex = 1;
            this.pcbox1.TabStop = false;
            // 
            // pcbox2
            // 
            this.pcbox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbox2.Location = new System.Drawing.Point(683, 12);
            this.pcbox2.Name = "pcbox2";
            this.pcbox2.Size = new System.Drawing.Size(128, 64);
            this.pcbox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pcbox2.TabIndex = 2;
            this.pcbox2.TabStop = false;
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnConnect.Location = new System.Drawing.Point(24, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(95, 29);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // pcbox3
            // 
            this.pcbox3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbox3.Location = new System.Drawing.Point(683, 91);
            this.pcbox3.Name = "pcbox3";
            this.pcbox3.Size = new System.Drawing.Size(128, 64);
            this.pcbox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pcbox3.TabIndex = 4;
            this.pcbox3.TabStop = false;
            // 
            // pcbox4
            // 
            this.pcbox4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbox4.Location = new System.Drawing.Point(683, 170);
            this.pcbox4.Name = "pcbox4";
            this.pcbox4.Size = new System.Drawing.Size(128, 64);
            this.pcbox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pcbox4.TabIndex = 5;
            this.pcbox4.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnApply);
            this.groupBox1.Controls.Add(this.chbox128x64);
            this.groupBox1.Controls.Add(this.chbox128x32);
            this.groupBox1.Controls.Add(this.txtHeight);
            this.groupBox1.Controls.Add(this.txtWidth);
            this.groupBox1.Controls.Add(this.lblHeight);
            this.groupBox1.Controls.Add(this.lblWidth);
            this.groupBox1.Location = new System.Drawing.Point(9, 152);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(134, 160);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image for OLED";
            // 
            // chbox128x64
            // 
            this.chbox128x64.AutoSize = true;
            this.chbox128x64.Location = new System.Drawing.Point(15, 39);
            this.chbox128x64.Name = "chbox128x64";
            this.chbox128x64.Size = new System.Drawing.Size(67, 17);
            this.chbox128x64.TabIndex = 15;
            this.chbox128x64.Text = "128 x 64";
            this.chbox128x64.UseVisualStyleBackColor = true;
            this.chbox128x64.CheckedChanged += new System.EventHandler(this.chbox128x64_CheckedChanged);
            // 
            // chbox128x32
            // 
            this.chbox128x32.AutoSize = true;
            this.chbox128x32.Location = new System.Drawing.Point(15, 19);
            this.chbox128x32.Name = "chbox128x32";
            this.chbox128x32.Size = new System.Drawing.Size(67, 17);
            this.chbox128x32.TabIndex = 15;
            this.chbox128x32.Text = "128 x 32";
            this.chbox128x32.UseVisualStyleBackColor = true;
            this.chbox128x32.CheckedChanged += new System.EventHandler(this.chbox128x32_CheckedChanged);
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(57, 89);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(67, 20);
            this.txtHeight.TabIndex = 14;
            this.txtHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(57, 63);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(67, 20);
            this.txtWidth.TabIndex = 13;
            this.txtWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(12, 93);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(38, 13);
            this.lblHeight.TabIndex = 12;
            this.lblHeight.Text = "Height";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(12, 67);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(35, 13);
            this.lblWidth.TabIndex = 11;
            this.lblWidth.Text = "Width";
            // 
            // btnAddImage
            // 
            this.btnAddImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAddImage.Location = new System.Drawing.Point(24, 107);
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Size = new System.Drawing.Size(95, 29);
            this.btnAddImage.TabIndex = 0;
            this.btnAddImage.Text = "Add Image";
            this.btnAddImage.UseVisualStyleBackColor = true;
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(683, 433);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(151, 23);
            this.progressBar.TabIndex = 7;
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSend.Location = new System.Drawing.Point(10, 350);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(133, 29);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send Image Data";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDisconnect.Location = new System.Drawing.Point(24, 47);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(95, 29);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(7, 443);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(43, 13);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "Status: ";
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnApply.Location = new System.Drawing.Point(15, 119);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(109, 29);
            this.btnApply.TabIndex = 16;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 463);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pcbox4);
            this.Controls.Add(this.pcbox3);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnAddImage);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.pcbox2);
            this.Controls.Add(this.pcbox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OLED Screen Interface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pcbox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbox4)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pcbox1;
        private System.Windows.Forms.PictureBox pcbox2;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.PictureBox pcbox3;
        private System.Windows.Forms.PictureBox pcbox4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Button btnAddImage;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chbox128x64;
        private System.Windows.Forms.CheckBox chbox128x32;
        private System.Windows.Forms.Button btnApply;
    }
}

