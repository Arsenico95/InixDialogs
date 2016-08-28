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
	using System.Linq;
	using System.Windows.Input;
	using System.Collections.Generic;	

	internal class SimpleDialogMediator : IDialogMediator, IDialogController
	{
		public void ShowDialog<TState>(Action<TState, object> nextAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings)
		{
			OnShow(settings);
		}

		public void ShowDialog<TState>(Action<TState, object> yesAction, Action<TState, object> noAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings)
		{
			OnShow(settings);
		}		

		public event EventHandler<ShowEventArgs> Show;

		protected virtual void OnShow(DialogSettingsBase settings)
		{
			settings.ThrowIfNull("settings");

			ShowEventArgs args = new ShowEventArgs(settings);
			EventHandler<ShowEventArgs> handler = this.Show;

			if (handler != null)
				handler(this, args);
		}

		public void Execute(int id)
		{
			throw new NotImplementedException();
		}
	}
}
