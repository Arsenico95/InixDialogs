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

namespace Inixe.InixDialogs.Tests
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Inixe.InixDialogs;

	[TestClass]
	public class DefaultMediatorTests
	{
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void DefaultDialogMediatorExpected()
		{
			DefaultDialogMediator mediator = new DefaultDialogMediator();
		}

		[TestMethod]
		public void DefaultDialogMediatorWithRelayerExpected()
		{			
			var mock = new DialogMediatorMock(this.TestContext);
			PrivateObject po = new PrivateObject(typeof(DefaultDialogMediator), mock);
		}
		
		private class DialogMediatorMock : IDialogMediator
		{
			private readonly TestContext context;
			internal DialogMediatorMock(TestContext context)
			{
				this.context = context;
			}
			#region IDialogMediator Members

			public void ShowDialog<TState>(Action<TState, object> nextAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings)
			{
				this.context.WriteLine("Showing Two Buttons");
			}

			public void ShowDialog<TState>(Action<TState, object> yesAction, Action<TState, object> noAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings)
			{
				this.context.WriteLine("Showing Three Buttons");
			}

			#endregion
		}

	}
}
