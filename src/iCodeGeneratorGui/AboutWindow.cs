using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace iCodeGenerator.iCodeGeneratorGui
{
	/// <summary>
	/// Summary description for AboutWindow.
	/// </summary>
	public class AboutWindow : Form
	{
		private Label label2;
		private Label label3;
		private LinkLabel uiCodeGeneratorLink;
        private System.Windows.Forms.Label uiCodeGeneratorLabel;
        private Label label1;
        private Label label4;
        private LinkLabel linkLabel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public AboutWindow()
		{
			
			InitializeComponent();
			uiCodeGeneratorLabel.Text = uiCodeGeneratorLabel.Text + MainWindow.Version;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutWindow));
            this.uiCodeGeneratorLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.uiCodeGeneratorLink = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // uiCodeGeneratorLabel
            // 
            this.uiCodeGeneratorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiCodeGeneratorLabel.Location = new System.Drawing.Point(16, 8);
            this.uiCodeGeneratorLabel.Name = "uiCodeGeneratorLabel";
            this.uiCodeGeneratorLabel.Size = new System.Drawing.Size(248, 23);
            this.uiCodeGeneratorLabel.TabIndex = 0;
            this.uiCodeGeneratorLabel.Text = "iCodeGenerator";
            this.uiCodeGeneratorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Author: Victor Y Dominguez";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(164, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "2005 - 2006";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiCodeGeneratorLink
            // 
            this.uiCodeGeneratorLink.Location = new System.Drawing.Point(32, 54);
            this.uiCodeGeneratorLink.Name = "uiCodeGeneratorLink";
            this.uiCodeGeneratorLink.Size = new System.Drawing.Size(232, 23);
            this.uiCodeGeneratorLink.TabIndex = 3;
            this.uiCodeGeneratorLink.TabStop = true;
            this.uiCodeGeneratorLink.Text = "http://www.icodegenerator.net";
            this.uiCodeGeneratorLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiCodeGeneratorLink.VisitedLinkColor = System.Drawing.Color.Red;
            this.uiCodeGeneratorLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.uiCodeGeneratorLink_LinkClicked);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Author: Marc Kohaupt (LOCOM)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(180, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "2010";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(32, 100);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(232, 23);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://github.com/garfield/iCodeGenerator";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Red;
            // 
            // AboutWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(292, 136);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uiCodeGeneratorLink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.uiCodeGeneratorLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AboutWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iCodeGenerator";
            this.Click += new System.EventHandler(this.AboutWindow_Click);
            this.ResumeLayout(false);

		}
		#endregion

		private void uiCodeGeneratorLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				VisitLink();
			}
			catch
			{
				MessageBox.Show("Unable to open link that was clicked.");
			}

		}

		private void VisitLink()
		{
			// Change the color of the link text by setting LinkVisited 
			// to true.
			uiCodeGeneratorLink.LinkVisited = true;
			//Call the Process.Start method to open the default browser 
			//with a URL:
			Process.Start(uiCodeGeneratorLink.Text);

		}

		private void AboutWindow_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}
	}
}
