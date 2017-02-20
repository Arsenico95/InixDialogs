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

namespace Inixe.InixDialogs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;


	/// <summary>
	/// Class MessageBoxHelper.
	/// </summary>
	/// <remarks>End user helper methods for Message Boxes</remarks>
	public static class MessageBoxHelper 
	{
		/// <summary>
		/// Shows the simple message box.
		/// </summary>
		/// <param name="mediator">The mediator to use.</param>
		/// <param name="message">The message to show.</param>
		/// <remarks>The resulting message box will have a single button. It can be used for End user information purposes.</remarks>
		public static void ShowSimpleMessageBox(IDialogMediator mediator, string message)
		{
			mediator.ThrowIfNull("mediator");
			message.ThrowIfNullOrEmpty("message");

			ShowMessageBoxCore(mediator, message, string.Empty, MessageBoxButtons.Ok, MessageBoxIcon.Information);
		}

		/// <summary>
		/// Shows the simple message box.
		/// </summary>
		/// <param name="mediator">The mediator to use.</param>
		/// <param name="message">The message to show.</param>
		/// <remarks>The resulting message box will have a single button. It can be used for End user information purposes.</remarks>
		public static void ShowSimpleMessageBox(IDialogMediator mediator, string message, string title)
		{
			mediator.ThrowIfNull("mediator");
			message.ThrowIfNullOrEmpty("message");
			title.ThrowIfNullOrEmpty("title");

			ShowMessageBoxCore(mediator, message, title, MessageBoxButtons.Ok, MessageBoxIcon.Information);
		}

		/// <summary>
		/// Shows the simple message box.
		/// </summary>
		/// <param name="mediator">The mediator to use.</param>
		/// <param name="message">The message to show.</param>
		/// <remarks>The resulting message box will have a single button. It can be used for End user information purposes.</remarks>
		public static void ShowMessageBox(IDialogMediator mediator, string message, string title, MessageBoxIcon icon)
		{
			mediator.ThrowIfNull("mediator");
			message.ThrowIfNullOrEmpty("message");
			title.ThrowIfNullOrEmpty("title");

			ShowMessageBoxCore(mediator, message, title, MessageBoxButtons.Ok, icon);
		}

		/// <summary>
		/// Shows the simple message box.
		/// </summary>
		/// <param name="mediator">The mediator to use.</param>
		/// <param name="message">The message to show.</param>
		/// <remarks>The resulting message box will have a single button. It can be used for End user information purposes.</remarks>
		private static void ShowMessageBoxCore(IDialogMediator mediator, string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			MessageBoxDialogSettings settings = new MessageBoxDialogSettings();

			settings.HeaderText = title;
			settings.MessageContent = message;
			settings.Icon = icon;
			settings.Buttons = buttons;

			Action<object, object> emptyAction = (r, s) => { };

			mediator.ShowDialog<object>(emptyAction, emptyAction, null, settings);
		}
	}
}
