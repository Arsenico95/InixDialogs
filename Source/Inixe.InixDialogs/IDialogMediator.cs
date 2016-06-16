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
	using System.Windows.Input;

	/// <summary>
	/// Interface IDialogMediator
	/// </summary>
	public interface IDialogMediator
	{
		/// <summary>
		/// Shows a dialog on the current WPF page
		/// </summary>
		/// <param name="nextAction">If the dialog is not canceled the nextAction will be executed</param>
		/// <param name="otherAction">If the dialog is canceled then this action is executed.</param>
		/// <param name="state">Optional. When an actionCommand is executed the state parameter is handed over to the actionCommand</param>
		/// <param name="settings">The dialog configuration settings <seealso cref="DialogSettingsBase" /></param>
		void ShowDialog<TState>(Action<TState, object> nextAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings);

		/// <summary>
		/// Shows a yes/no dialog on the current WPF page
		/// </summary>
		/// <param name="yesAction">If the yes option is selected this action is Executed.</param>
		/// <param name="noAction">If the yes option is selected this action is Executed.</param>
		/// <param name="otherAction">If the dialog is canceled then this action is executed.</param>
		/// <param name="state">Optional. When an action is executed the state parameter is handed over to the action</param>
		/// <param name="settings">The dialog configuration settings <seealso cref="DialogSettingsBase"/></param>		
		void ShowDialog<TState>(Action<TState, object> yesAction, Action<TState, object> noAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings);
	}
}
