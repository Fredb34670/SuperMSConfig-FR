using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SuperMSConfig;

public class PluginsForm : Form
{
	private IContainer components = null;

	public PluginsForm()
	{
		InitializeComponent();
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
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(414, 507);
		base.Name = "PluginsForm";
		base.ShowIcon = false;
		base.ResumeLayout(false);
	}
}
