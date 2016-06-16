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

namespace Inixe.InixDialogs.Demo.Support
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;

	internal sealed class OperationCommand : ICommand
	{
		private readonly Action _commandAction;
		private readonly Predicate<object> _commandPredicate;
		private bool _lastCanExecute;

		/// <summary>
		/// Initializes a new instance of the <see cref="OperationCommand"/> class.
		/// </summary>
		/// <param name="commandAction"></param>
		public OperationCommand(Action commandAction)
		{
			_commandAction = commandAction;
			_commandPredicate = null;
			_lastCanExecute = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OperationCommand"/> class.
		/// </summary>
		/// <param name="commandAction"></param>
		/// <param name="commandPredicate"></param>
		public OperationCommand(Action commandAction, Predicate<object> commandPredicate)
		{
			_commandAction = commandAction;
			_commandPredicate = commandPredicate;
			_lastCanExecute = false;
		}

		private bool LastCanExecute
		{
			get
			{
				return _lastCanExecute;
			}

			set
			{
				if (_lastCanExecute == value)
					return;

				_lastCanExecute = value;
				OnCanExecuteChanged(value);
			}
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			if (_commandPredicate != null)
				return _commandPredicate(parameter);

			return true;
		}

		public void Execute(object parameter)
		{
			_commandAction();
		}

		private void OnCanExecuteChanged(bool value)
		{
			EventHandler handler = CanExecuteChanged;
			if (handler != null)
				handler(this, new EventArgs());
		}
	}
}
