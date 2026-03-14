using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SuperMSConfig;

public class AboutForm : Form
{
	private IContainer components = null;

	private Label lblAbout;

	private LinkLabel linkVersion;

	private Label label2;

	private Label label3;

	private Button assetBack;

	private Label label1;

	public AboutForm()
	{
		InitializeComponent();
		linkVersion.Text = "Version " + Program.GetCurrentVersionTostring() + " / A100";
		SetStyle();
	}

	private void SetStyle()
	{
		BackColor = Color.FromArgb(253, 237, 224);
		assetBack.Text = "\ue72b";
	}

	private void assetBack_Click(object sender, EventArgs e)
	{
		Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuperMSConfig.AboutForm));
		this.lblAbout = new System.Windows.Forms.Label();
		this.linkVersion = new System.Windows.Forms.LinkLabel();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.assetBack = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblAbout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblAbout.AutoEllipsis = true;
		this.lblAbout.BackColor = System.Drawing.Color.Transparent;
		this.lblAbout.Font = new System.Drawing.Font("Segoe UI Variable Small", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAbout.Location = new System.Drawing.Point(14, 88);
		this.lblAbout.Name = "lblAbout";
		this.lblAbout.Size = new System.Drawing.Size(425, 91);
		this.lblAbout.TabIndex = 233;
		this.lblAbout.Text = resources.GetString("lblAbout.Text");
		this.linkVersion.AutoEllipsis = true;
		this.linkVersion.BackColor = System.Drawing.Color.Transparent;
		this.linkVersion.Font = new System.Drawing.Font("Segoe UI Variable Small Semibol", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.linkVersion.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.linkVersion.LinkColor = System.Drawing.Color.Black;
		this.linkVersion.Location = new System.Drawing.Point(12, 59);
		this.linkVersion.Name = "linkVersion";
		this.linkVersion.Size = new System.Drawing.Size(427, 20);
		this.linkVersion.TabIndex = 237;
		this.linkVersion.TabStop = true;
		this.linkVersion.Text = "Version";
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label2.BackColor = System.Drawing.Color.Transparent;
		this.label2.Font = new System.Drawing.Font("Segoe UI Variable Small Semibol", 8.25f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(12, 200);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(387, 15);
		this.label2.TabIndex = 239;
		this.label2.Text = "Microsoft 365 Copilot est une marque déposée de Microsoft Corporation. ";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label3.AutoEllipsis = true;
		this.label3.BackColor = System.Drawing.Color.Transparent;
		this.label3.Font = new System.Drawing.Font("Segoe UI Variable Small Semibol", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.FromArgb(54, 50, 58);
		this.label3.Location = new System.Drawing.Point(14, 225);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(150, 15);
		this.label3.TabIndex = 240;
		this.label3.Text = "Une création Belim (2024)";
		this.assetBack.AutoSize = true;
		this.assetBack.BackColor = System.Drawing.Color.FromArgb(248, 241, 234);
		this.assetBack.Cursor = System.Windows.Forms.Cursors.Hand;
		this.assetBack.FlatAppearance.BorderColor = System.Drawing.Color.Tan;
		this.assetBack.FlatAppearance.BorderSize = 0;
		this.assetBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SeaShell;
		this.assetBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.assetBack.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12f, System.Drawing.FontStyle.Bold);
		this.assetBack.ForeColor = System.Drawing.Color.Tan;
		this.assetBack.Location = new System.Drawing.Point(12, 12);
		this.assetBack.Name = "assetBack";
		this.assetBack.Size = new System.Drawing.Size(40, 36);
		this.assetBack.TabIndex = 296;
		this.assetBack.Text = "...";
		this.assetBack.UseVisualStyleBackColor = false;
		this.assetBack.Click += new System.EventHandler(assetBack_Click);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Transparent;
		this.label1.Font = new System.Drawing.Font("Segoe UI Variable Small Semibol", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.FromArgb(54, 50, 58);
		this.label1.Location = new System.Drawing.Point(200, 26);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(47, 16);
		this.label1.TabIndex = 297;
		this.label1.Text = "À PROPOS";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(447, 249);
		base.ControlBox = false;
		base.Controls.Add(this.label1);
		base.Controls.Add(this.assetBack);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.linkVersion);
		base.Controls.Add(this.lblAbout);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "AboutForm";
		base.Opacity = 0.95;
		base.ShowIcon = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
