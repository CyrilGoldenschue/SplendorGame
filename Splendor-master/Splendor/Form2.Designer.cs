namespace Splendor
{
    partial class Form2
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
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 45);
            this.Controls.Add(this.txtAddPlayer);
            this.Controls.Add(this.cmdAddPlayer);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdAddPlayer;
        private System.Windows.Forms.TextBox txtAddPlayer;
    }
}