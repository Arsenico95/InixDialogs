using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inixe.InixDialogs
{
	public static class MessageBoxHelper 
	{
		public static DialogResult ShowMessageBox(IDialogMediator mediator, string message)
		{
			mediator.ThrowIfNull("mediator");
			message.ThrowIfNullOrEmpty("message");

			return ShowMessageBoxCore(mediator, message, string.Empty, MessageBoxButtons.Ok, MessageBoxIcon.Information);
		}

		public static DialogResult ShowMessageBox(IDialogMediator mediator, string message, string title)
		{
			mediator.ThrowIfNull("mediator");
			message.ThrowIfNullOrEmpty("message");
			title.ThrowIfNullOrEmpty("title");

			return ShowMessageBoxCore(mediator, message, title, MessageBoxButtons.Ok, MessageBoxIcon.Information);
		}

		public static DialogResult ShowMessageBox(IDialogMediator mediator, string message, string title, MessageBoxButtons buttons)
		{
			mediator.ThrowIfNull("mediator");
			message.ThrowIfNullOrEmpty("message");
			title.ThrowIfNullOrEmpty("title");

			return ShowMessageBoxCore(mediator, message, title, buttons, MessageBoxIcon.Information);
		}

		public static DialogResult ShowMessageBox(IDialogMediator mediator, string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			mediator.ThrowIfNull("mediator");
			message.ThrowIfNullOrEmpty("message");
			title.ThrowIfNullOrEmpty("title");

			return ShowMessageBoxCore(mediator, message, title, buttons, icon);
		}

		private static DialogResult ShowMessageBoxCore(IDialogMediator mediator, string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			MessageBoxDialogSettings settings = new MessageBoxDialogSettings();
			throw new NotImplementedException();
		}
	}
}
