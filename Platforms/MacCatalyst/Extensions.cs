#nullable disable
using System;
using Microsoft.Maui.Controls.Internals;
using ObjCRuntime;
using UIKit;

namespace CustomButtonLayout
{
	public static class Extensions
	{
		internal static bool IsHorizontal(this Button.ButtonContentLayout layout) =>
			layout.Position == Button.ButtonContentLayout.ImagePosition.Left ||
			layout.Position == Button.ButtonContentLayout.ImagePosition.Right;
	}
}