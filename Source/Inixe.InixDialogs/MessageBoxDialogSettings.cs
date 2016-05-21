/*
							MIT License

Copyright (c) 2016 Ingemar Parra H.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System.Windows.Media;
namespace Inixe.InixDialogs
{
	/// <summary>
	/// Enum MessageBoxButtons
	/// </summary>
	public enum MessageBoxButtons
	{
		Ok,
		OkCancel,
		RetryCancel,
		YesNo,
		YesNoCancel
	}

	/// <summary>
	/// Enum MessageBoxIcon
	/// </summary>
	public enum MessageBoxIcon
	{
		/// <summary>
		/// An asterix icon will be displayed
		/// </summary>
		Asterix,

		/// <summary>
		/// An error icon will be displayed
		/// </summary>
		Error,
		
		Exclamation,
		Hand,
		Information,
		None,
		Question,
		Stop,
		Warning,
		Custom
	}

	public enum DialogResult
	{
		Cancel,
		Ok,
		No,
		Yes
	}

	/// <summary>
	/// Class MessageBoxDialogSettings. This class cannot be inherited.
	/// </summary>
	public sealed class MessageBoxDialogSettings : DialogSettingsBase
	{
		public ImageSource IconSource { get; set; }
		public bool WrapText { get; set; }
		public MessageBoxIcon Icon { get; set; }
		public string MessageText { get; set; }		
	}
}
