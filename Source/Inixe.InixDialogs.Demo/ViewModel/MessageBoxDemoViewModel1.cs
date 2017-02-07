
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

	internal class MessageBoxDemoViewModel1 : INotifyPropertyChanged
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
			// System.Windows.SystemColors.
		}

		public event PropertyChangedEventHandler PropertyChanged;

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

		public ICommand ShowMessageBoxCommand
		{
			get
			{
				return _showMessageBoxCommand;
			}
		}

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
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = this.PropertyChanged;

			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}


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
