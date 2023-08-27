namespace chronosguessr
{
    partial class GameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.outputLabel = new System.Windows.Forms.Label();
            this.Map = new System.Windows.Forms.PictureBox();
            this.GuessButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Map)).BeginInit();
            this.SuspendLayout();
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.BackColor = System.Drawing.Color.Transparent;
            this.outputLabel.ForeColor = System.Drawing.Color.Red;
            this.outputLabel.Location = new System.Drawing.Point(12, 9);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(38, 15);
            this.outputLabel.TabIndex = 5;
            this.outputLabel.Text = "label1";
            // 
            // Map
            // 
            this.Map.BackColor = System.Drawing.Color.Transparent;
            this.Map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Map.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Map.ErrorImage = null;
            this.Map.Image = ((System.Drawing.Image)(resources.GetObject("Map.Image")));
            this.Map.ImageLocation = "";
            this.Map.InitialImage = null;
            this.Map.Location = new System.Drawing.Point(588, 194);
            this.Map.Name = "Map";
            this.Map.Size = new System.Drawing.Size(200, 200);
            this.Map.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Map.TabIndex = 6;
            this.Map.TabStop = false;
            // 
            // GuessButton
            // 
            this.GuessButton.Location = new System.Drawing.Point(588, 400);
            this.GuessButton.Name = "GuessButton";
            this.GuessButton.Size = new System.Drawing.Size(75, 23);
            this.GuessButton.TabIndex = 7;
            this.GuessButton.Text = "button1";
            this.GuessButton.UseVisualStyleBackColor = true;
            this.GuessButton.Click += new System.EventHandler(this.GuessButton_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.GuessButton);
            this.Controls.Add(this.Map);
            this.Controls.Add(this.outputLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameForm";
            this.Text = "Chronos Guessr";
            this.Load += new System.EventHandler(this.GameForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Map)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label outputLabel;
        private PictureBox Map;
        private Button GuessButton;
    }
}