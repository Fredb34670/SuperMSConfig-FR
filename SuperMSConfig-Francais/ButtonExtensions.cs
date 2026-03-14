using System;
using System.Windows.Forms;

public static class ButtonExtensions
{
	public static Button WithClick(this Button button, EventHandler handler)
	{
		button.Click += handler;
		return button;
	}
}
