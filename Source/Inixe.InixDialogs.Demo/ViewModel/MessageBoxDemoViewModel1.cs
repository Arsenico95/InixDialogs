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

namespace Inixe.InixDialogs.Demo.ViewModel
{
	using Inixe.InixDialogs.Demo.Support;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using System.Windows;

	/// <summary>
	/// Class MessageBoxDemoViewModel1.
	/// </summary>
	/// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
	/// <remarks>None</remarks>
	public class MessageBoxDemoViewModel1 : INotifyPropertyChanged
	{
		private ICommand _showMessageBoxCommand;
		private string _result;
		private IDialogMediator _messageBox;


		/// <summary>
		/// Initializes a new instance of the <see cref="MessageBoxDemoViewModel1"/> class.
		/// </summary>
		public MessageBoxDemoViewModel1()
		{
			_showMessageBoxCommand = new OperationCommand(OnShowMessageBox);
		}

		/// <summary>
		/// Occurs when a property has changed.
		/// </summary>
		/// <remarks>None</remarks>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Gets or sets the result.
		/// </summary>
		/// <value>The result.</value>
		/// <remarks>None</remarks>
		public string Result
		{
			get
			{
				return _result;
			}

			set
			{
				if (_result != value)
				{
					_result = value;
					OnPropertyChanged("Result");
				}
			}
		}

		/// <summary>
		/// Gets the show message box command.
		/// </summary>
		/// <value>The show message box command.</value>
		/// <remarks>None</remarks>
		public ICommand ShowMessageBoxCommand
		{
			get
			{
				return _showMessageBoxCommand;
			}
		}

		/// <summary>
		/// Gets or sets the message box mediator.
		/// </summary>
		/// <value>The message box mediator.</value>
		/// <remarks>Message box property can be null, when attached to a view with a compatible control
		/// The Control is going to lazy inject an <see cref=" IDialogMediator"/> instance that knows the existence of
		/// the corresponding control it's bound to. If the Mediator property is not <c>null</c>, then the supplied instance will
		/// Act as the actual caller for the methods the mediator implements. Check example 2 in order to see what happends.</remarks>
		public IDialogMediator MessageBox
		{
			get
			{
				return _messageBox;
			}

			set
			{
				if (_messageBox != value)
				{
					_messageBox = value;
					OnPropertyChanged("MessageBox");
				}
			}
		}

		/// <summary>
		/// Triggers the PropertyChanged event.
		/// </summary>
		/// <remarks>None</remarks>
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = this.PropertyChanged;

			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}


		/// <summary>
		/// Called when the show Commad is signaled, opens a Message Box.
		/// </summary>
		/// <remarks>None</remarks>
		private void OnShowMessageBox()
		{
			var dialogSettings = new MessageBoxDialogSettings();
			dialogSettings.HeaderText = "Demo";
			dialogSettings.Buttons = MessageBoxButtons.OkCancel;
			dialogSettings.Icon = MessageBoxIcon.Asterix;
			dialogSettings.MessageContent = "Are you Sure?";

			_messageBox.ShowDialog<object>((p, s) => Result = "You wanted this!!", (p, s) => Result = "Ok. So then you back off", this, dialogSettings);
		}
	}
}
