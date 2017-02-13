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

	internal interface IDialogController
	{
		/// <summary>
		/// Executes the specified identifier.
		/// </summary>
		/// <param name="resultType">The identifier.</param>
		/// <param name="action">The Action that's going to be executed when the Dialog is Closed.</param>
		/// <param name="state">The state that is associated to the dialog poup</param>
		/// <param name="resultValue">The Dialog result.</param>
		/// <remarks>None</remarks>
		void Execute(DialogResult resultType, object state, object resultValue);

		/// <summary>
		/// Occurs when show event is fired.
		/// </summary>
		/// <remarks>This event is the actual event that brings the Popups into the screen.</remarks>
		event EventHandler<ShowEventArgs> Show;
	}
}
