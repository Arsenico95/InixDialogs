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
	using System.Collections;

	/// <summary>
	/// Class SimpleDialogMediator.
	/// </summary>
	/// <seealso cref="Inixe.InixDialogs.IDialogMediator" />
	/// <seealso cref="Inixe.InixDialogs.IDialogController" />
	/// <remarks>This is the default and most simple form of mediator. This mediator is going to be created and supplied by every control, un less you extend the classes</remarks>
	internal class SimpleDialogMediator : IDialogMediator, IDialogController
	{
		private CallbackSet _callbacks;

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleDialogMediator"/> class.
		/// </summary>
		internal SimpleDialogMediator()
		{
			_callbacks = new CallbackSet();
		}
		
		/// <summary>
		/// Shows a dialog on the current WPF page
		/// </summary>
		/// <typeparam name="TState">The type of the t state.</typeparam>
		/// <param name="nextAction">If the dialog is not canceled the nextAction will be executed</param>
		/// <param name="otherAction">If the dialog is canceled then this action is executed.</param>
		/// <param name="state">Optional. When an actionCommand is executed the state parameter is handed over to the actionCommand</param>
		/// <param name="settings">The dialog configuration settings <seealso cref="DialogSettingsBase" /></param>
		/// <remarks>None</remarks>
		public void ShowDialog<TState>(Action<TState, object> nextAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings)
		{
			nextAction.ThrowIfNull("nextAction");
			otherAction.ThrowIfNull("otherAction");
			state.ThrowIfNull("state");

			// Now let's wrap every callback
			_callbacks.Clear();			
			_callbacks.Add(DialogResult.Ok, (s,r) => nextAction((TState)s, r));
			_callbacks.Add(DialogResult.Cancel, (s, r) => otherAction((TState)s, r));

			OnShow(settings, _callbacks);
		}

		/// <summary>
		/// Shows a yes/no dialog on the current WPF page
		/// </summary>
		/// <typeparam name="TState">The type of the t state.</typeparam>
		/// <param name="yesAction">If the yes option is selected this action is Executed.</param>
		/// <param name="noAction">If the yes option is selected this action is Executed.</param>
		/// <param name="otherAction">If the dialog is canceled then this action is executed.</param>
		/// <param name="state">Optional. When an action is executed the state parameter is handed over to the action</param>
		/// <param name="settings">The dialog configuration settings <seealso cref="DialogSettingsBase" /></param>
		/// <remarks>None</remarks>
		public void ShowDialog<TState>(Action<TState, object> yesAction, Action<TState, object> noAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings)
		{
			yesAction.ThrowIfNull("yesAction");
			noAction.ThrowIfNull("noAction");
			otherAction.ThrowIfNull("otherAction");
			state.ThrowIfNull("state");

			_callbacks.Clear();
			_callbacks.Add(DialogResult.Yes, (s, r) => yesAction((TState)s, r));
			_callbacks.Add(DialogResult.No, (s, r) => noAction((TState)s, r));
			_callbacks.Add(DialogResult.Cancel, (s, r) => otherAction((TState)s, r));

			OnShow(settings, _callbacks);
		}		

		public event EventHandler<ShowEventArgs> Show;


		/// <summary>
		/// Called when the dialog should be shown.
		/// </summary>
		/// <param name="settings">The dialog settings used to create and show the instance.</param>
		/// <remarks>None</remarks>
		protected virtual void OnShow(DialogSettingsBase settings, CallbackSet callbacks)
		{
			settings.ThrowIfNull("settings");
			callbacks.ThrowIfNull("callbacks");

			ShowEventArgs args = new ShowEventArgs(settings, callbacks);
			EventHandler<ShowEventArgs> handler = this.Show;

			if (handler != null)
				handler(this, args);
		}

		public void Execute(DialogResult resultType, object state, object result)
		{
			if (resultType != DialogResult.None)
			{
				_callbacks[resultType](state, result);
			}
		}	
	}
}
