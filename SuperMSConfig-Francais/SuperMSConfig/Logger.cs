using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SuperMSConfig;

public class Logger
{
	private readonly MainForm mainForm;

	public Logger(MainForm mainForm)
	{
		this.mainForm = mainForm ?? throw new ArgumentNullException("mainForm");
	}

	public void Log(string message, Color color, string details = "")
	{
		RichTextBox logBox = mainForm.Controls.Find("rtbLog", searchAllChildren: true).FirstOrDefault() as RichTextBox;
		if (logBox == null)
		{
			return;
		}
		if (logBox.InvokeRequired)
		{
			logBox.Invoke((Action)delegate
			{
				AppendLog(logBox, message, color, details);
			});
		}
		else
		{
			AppendLog(logBox, message, color, details);
		}
	}

	private void AppendLog(RichTextBox logBox, string message, Color color, string details)
	{
		logBox.SelectionColor = color;
		logBox.AppendText($"{DateTime.Now}: {message}\n");
		if (!string.IsNullOrEmpty(details))
		{
			logBox.SelectionColor = Color.Gray;
			logBox.AppendText("\tDetails: " + details + "\n");
		}
		logBox.SelectionStart = logBox.Text.Length;
		logBox.ScrollToCaret();
	}
}
