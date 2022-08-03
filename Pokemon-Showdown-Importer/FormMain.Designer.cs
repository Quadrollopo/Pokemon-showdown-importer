namespace PokemonShowdownImporter {
	partial class FormMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.importButton = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.copyButton = new System.Windows.Forms.Button();
			this.notify = new System.Windows.Forms.TextBox();
			this.timer1 = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
			this.SuspendLayout();
			// 
			// importButton
			// 
			this.importButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.importButton.Location = new System.Drawing.Point(0, 315);
			this.importButton.Name = "importButton";
			this.importButton.Size = new System.Drawing.Size(525, 23);
			this.importButton.TabIndex = 0;
			this.importButton.Text = "Select save file";
			this.importButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.importButton.UseVisualStyleBackColor = true;
			this.importButton.Click += new System.EventHandler(this.importButton_Click);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new System.Drawing.Size(525, 315);
			this.richTextBox1.TabIndex = 2;
			this.richTextBox1.Text = "";
			// 
			// copyButton
			// 
			this.copyButton.Location = new System.Drawing.Point(425, 12);
			this.copyButton.Name = "copyButton";
			this.copyButton.Size = new System.Drawing.Size(63, 38);
			this.copyButton.TabIndex = 3;
			this.copyButton.Text = "Copy to clipboard";
			this.copyButton.UseVisualStyleBackColor = true;
			this.copyButton.Visible = false;
			this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
			// 
			// notify
			// 
			this.notify.BackColor = System.Drawing.SystemColors.Control;
			this.notify.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.notify.Location = new System.Drawing.Point(425, 56);
			this.notify.Name = "notify";
			this.notify.Size = new System.Drawing.Size(63, 13);
			this.notify.TabIndex = 4;
			this.notify.Text = "Copied!";
			this.notify.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.notify.Visible = false;
			// 
			// timer1
			// 
			this.timer1.AutoReset = false;
			this.timer1.Interval = 2000D;
			this.timer1.SynchronizingObject = this;
			this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(525, 338);
			this.Controls.Add(this.notify);
			this.Controls.Add(this.copyButton);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.importButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormMain";
			this.Text = "Pokemon showdown importer";
			((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private System.Timers.Timer timer1;

		private System.Windows.Forms.TextBox notify;

		private System.Windows.Forms.TextBox textBox1;

		private System.Windows.Forms.Button copyButton;

		private System.Windows.Forms.Button importButton;

		private System.Windows.Forms.RichTextBox richTextBox1;
		
		#endregion
	}
}