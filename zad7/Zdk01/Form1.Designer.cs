namespace Zdk01
{
    partial class Form1
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

        private void CreateTextBoxes()
        {
            int initialX = 10;
            int initialY = 10;

            for (int i = 1; i <= 9; i++)
            {
                var x = i - 1;

                for (int j = 1; j <= 9; j++)
                {
                    System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox();
                    txt.Size = new System.Drawing.Size(30, 20);

                    var location = gb.Location;

                    var y = j - 1;

                    txt.Location = new System.Drawing.Point(j * initialY + y * 25, i * initialX + x * 25);
                    gb.Controls.Add(txt);
                }
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gb = new System.Windows.Forms.GroupBox();
            this.btnIspuni = new System.Windows.Forms.Button();
            this.btnProvjeriIspravnost = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblRezultat = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gb
            // 
            this.gb.Location = new System.Drawing.Point(21, 12);
            this.gb.Name = "gb";
            this.gb.Size = new System.Drawing.Size(360, 319);
            this.gb.TabIndex = 8;
            this.gb.TabStop = false;
            // 
            // btnIspuni
            // 
            this.btnIspuni.Location = new System.Drawing.Point(387, 23);
            this.btnIspuni.Name = "btnIspuni";
            this.btnIspuni.Size = new System.Drawing.Size(177, 61);
            this.btnIspuni.TabIndex = 9;
            this.btnIspuni.Text = "Ispuni polja";
            this.btnIspuni.UseVisualStyleBackColor = true;
            this.btnIspuni.Click += new System.EventHandler(this.btnIspuni_Click);
            // 
            // btnProvjeriIspravnost
            // 
            this.btnProvjeriIspravnost.Location = new System.Drawing.Point(387, 90);
            this.btnProvjeriIspravnost.Name = "btnProvjeriIspravnost";
            this.btnProvjeriIspravnost.Size = new System.Drawing.Size(177, 68);
            this.btnProvjeriIspravnost.TabIndex = 10;
            this.btnProvjeriIspravnost.Text = "Provjeri ispravnost";
            this.btnProvjeriIspravnost.UseVisualStyleBackColor = true;
            this.btnProvjeriIspravnost.Click += new System.EventHandler(this.btnProvjeriIspravnost_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(388, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 26);
            this.label1.TabIndex = 11;
            this.label1.Text = "Ispravno (da/ne):";
            // 
            // lblRezultat
            // 
            this.lblRezultat.AutoSize = true;
            this.lblRezultat.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRezultat.Location = new System.Drawing.Point(533, 246);
            this.lblRezultat.Name = "lblRezultat";
            this.lblRezultat.Size = new System.Drawing.Size(30, 26);
            this.lblRezultat.TabIndex = 12;
            this.lblRezultat.Text = "...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 343);
            this.Controls.Add(this.lblRezultat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnProvjeriIspravnost);
            this.Controls.Add(this.btnIspuni);
            this.Controls.Add(this.gb);
            this.Name = "Form1";
            this.Text = "Sudoku";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox gb;
        private System.Windows.Forms.Button btnIspuni;
        private System.Windows.Forms.Button btnProvjeriIspravnost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRezultat;
    }
}

