namespace JumpingPlatformGameWinFormsApp {
	partial class MainForm {
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.worldPanel = new System.Windows.Forms.Panel();
			this.jumpingPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.joeButton = new System.Windows.Forms.Button();
			this.janeButton = new System.Windows.Forms.Button();
			this.jackButton = new System.Windows.Forms.Button();
			this.jillButton = new System.Windows.Forms.Button();
			this.updateTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// worldPanel
			// 
			this.worldPanel.BackColor = System.Drawing.Color.White;
			this.worldPanel.Location = new System.Drawing.Point(12, 41);
			this.worldPanel.Name = "worldPanel";
			this.worldPanel.Size = new System.Drawing.Size(850, 416);
			this.worldPanel.TabIndex = 0;
			// 
			// jumpingPanel
			// 
			this.jumpingPanel.BackColor = System.Drawing.Color.Khaki;
			this.jumpingPanel.Location = new System.Drawing.Point(12, 463);
			this.jumpingPanel.Name = "jumpingPanel";
			this.jumpingPanel.Size = new System.Drawing.Size(850, 96);
			this.jumpingPanel.TabIndex = 1;
			// 
			// joeButton
			// 
			this.joeButton.Location = new System.Drawing.Point(12, 12);
			this.joeButton.Name = "joeButton";
			this.joeButton.Size = new System.Drawing.Size(75, 23);
			this.joeButton.TabIndex = 2;
			this.joeButton.Text = "Joe";
			this.joeButton.UseVisualStyleBackColor = true;
			this.joeButton.Click += new System.EventHandler(this.joeButton_Click);
			// 
			// janeButton
			// 
			this.janeButton.Location = new System.Drawing.Point(93, 12);
			this.janeButton.Name = "janeButton";
			this.janeButton.Size = new System.Drawing.Size(75, 23);
			this.janeButton.TabIndex = 3;
			this.janeButton.Text = "Jane";
			this.janeButton.UseVisualStyleBackColor = true;
			this.janeButton.Click += new System.EventHandler(this.janeButton_Click);
			// 
			// jackButton
			// 
			this.jackButton.Location = new System.Drawing.Point(174, 12);
			this.jackButton.Name = "jackButton";
			this.jackButton.Size = new System.Drawing.Size(75, 23);
			this.jackButton.TabIndex = 4;
			this.jackButton.Text = "Jack";
			this.jackButton.UseVisualStyleBackColor = true;
			this.jackButton.Click += new System.EventHandler(this.jackButton_Click);
			// 
			// jillButton
			// 
			this.jillButton.Location = new System.Drawing.Point(255, 12);
			this.jillButton.Name = "jillButton";
			this.jillButton.Size = new System.Drawing.Size(75, 23);
			this.jillButton.TabIndex = 5;
			this.jillButton.Text = "Jill";
			this.jillButton.UseVisualStyleBackColor = true;
			this.jillButton.Click += new System.EventHandler(this.jillButton_Click);
			// 
			// updateTimer
			// 
			this.updateTimer.Enabled = true;
			this.updateTimer.Interval = 40;
			this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(874, 571);
			this.Controls.Add(this.jillButton);
			this.Controls.Add(this.jackButton);
			this.Controls.Add(this.janeButton);
			this.Controls.Add(this.joeButton);
			this.Controls.Add(this.jumpingPanel);
			this.Controls.Add(this.worldPanel);
			this.Name = "MainForm";
			this.Text = "Jumping Platform Game";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel worldPanel;
		private System.Windows.Forms.FlowLayoutPanel jumpingPanel;
		private System.Windows.Forms.Button joeButton;
		private System.Windows.Forms.Button janeButton;
		private System.Windows.Forms.Button jackButton;
		private System.Windows.Forms.Button jillButton;
		private System.Windows.Forms.Timer updateTimer;
	}
}
