namespace Splendor
{
    partial class FormAddPlayer
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
            this.cmdAddPlayer = new System.Windows.Forms.Button();
            this.txtAddPlayer = new System.Windows.Forms.TextBox();
            this.lstPlayer = new System.Windows.Forms.ListBox();
            this.cmdDeletePlayer = new System.Windows.Forms.Button();
            this.cmdCloseAddPlayer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdAddPlayer
            // 
            this.cmdAddPlayer.Location = new System.Drawing.Point(205, 12);
            this.cmdAddPlayer.Name = "cmdAddPlayer";
            this.cmdAddPlayer.Size = new System.Drawing.Size(75, 23);
            this.cmdAddPlayer.TabIndex = 0;
            this.cmdAddPlayer.Text = "Ajouter";
            this.cmdAddPlayer.UseVisualStyleBackColor = true;
            this.cmdAddPlayer.Click += new System.EventHandler(this.cmdAddPlayer_Click);
            // 
            // txtAddPlayer
            // 
            this.txtAddPlayer.Location = new System.Drawing.Point(12, 12);
            this.txtAddPlayer.Name = "txtAddPlayer";
            this.txtAddPlayer.Size = new System.Drawing.Size(187, 20);
            this.txtAddPlayer.TabIndex = 1;
            this.txtAddPlayer.Enter += new System.EventHandler(this.txtAddPlayer_Enter);
            // 
            // lstPlayer
            // 
            this.lstPlayer.FormattingEnabled = true;
            this.lstPlayer.Location = new System.Drawing.Point(12, 38);
            this.lstPlayer.Name = "lstPlayer";
            this.lstPlayer.Size = new System.Drawing.Size(268, 134);
            this.lstPlayer.TabIndex = 2;
            this.lstPlayer.SelectedIndexChanged += new System.EventHandler(this.lstPlayer_SelectedIndexChanged);
            // 
            // cmdDeletePlayer
            // 
            this.cmdDeletePlayer.Enabled = false;
            this.cmdDeletePlayer.Location = new System.Drawing.Point(12, 178);
            this.cmdDeletePlayer.Name = "cmdDeletePlayer";
            this.cmdDeletePlayer.Size = new System.Drawing.Size(268, 23);
            this.cmdDeletePlayer.TabIndex = 3;
            this.cmdDeletePlayer.Text = "Retirer";
            this.cmdDeletePlayer.UseVisualStyleBackColor = true;
            this.cmdDeletePlayer.Click += new System.EventHandler(this.cmdDeletePlayer_Click);
            // 
            // cmdCloseAddPlayer
            // 
            this.cmdCloseAddPlayer.Location = new System.Drawing.Point(12, 207);
            this.cmdCloseAddPlayer.Name = "cmdCloseAddPlayer";
            this.cmdCloseAddPlayer.Size = new System.Drawing.Size(268, 23);
            this.cmdCloseAddPlayer.TabIndex = 4;
            this.cmdCloseAddPlayer.Text = "Sauvegarder";
            this.cmdCloseAddPlayer.UseVisualStyleBackColor = true;
            this.cmdCloseAddPlayer.Click += new System.EventHandler(this.cmdCloseAddPlayer_Click);
            // 
            // FormAddPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 235);
            this.Controls.Add(this.cmdCloseAddPlayer);
            this.Controls.Add(this.cmdDeletePlayer);
            this.Controls.Add(this.lstPlayer);
            this.Controls.Add(this.txtAddPlayer);
            this.Controls.Add(this.cmdAddPlayer);
            this.Name = "FormAddPlayer";
            this.Text = "Ajout de joueur";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdAddPlayer;
        private System.Windows.Forms.TextBox txtAddPlayer;
        private System.Windows.Forms.ListBox lstPlayer;
        private System.Windows.Forms.Button cmdDeletePlayer;
        private System.Windows.Forms.Button cmdCloseAddPlayer;
    }
}