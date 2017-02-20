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

namespace Inixe.InixDialogs.Demo.Tests
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Inixe.InixDialogs.Demo.ViewModel;

	[TestClass]
	public class MessageBoxDemoViewModelTests1
	{
		[TestMethod]
		public void MessageBoxDemoViewModelExpected()
		{
			var item = new MessageBoxDemoViewModel1();
			Assert.IsNotNull(item);
		}

		[TestMethod]
		public void ResultExpected()
		{
			var item = new MessageBoxDemoViewModel1();
			bool changed = false;
			item.PropertyChanged += (s, e) => 
			{
				changed = true;
			};

			item.Result = "New State";

			Assert.IsTrue(changed);
		}

		[TestMethod]
		public void ShowMessageBoxCustomExpected1()
		{
			var item = new MessageBoxDemoViewModel1();
			bool mediatorLoaded = false;
			bool resultChanged = false;

			item.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "MessageBox")
					mediatorLoaded = true;

				if (e.PropertyName == "Result")
					resultChanged = true;
			};

			var mock = new MeditatorMock();
			mock.DefaultResult = DialogResult.Ok;
			item.MessageBox = mock;
			item.ShowMessageBoxCommand.Execute(null);

			Assert.IsTrue(mediatorLoaded);
			Assert.IsTrue(resultChanged);
		}

		[TestMethod]
		public void ShowMessageBoxCustomExpected2()
		{
			var item = new MessageBoxDemoViewModel1();
			bool mediatorLoaded = false;
			bool resultChanged = false;

			item.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "MessageBox")
					mediatorLoaded = true;

				if (e.PropertyName == "Result")
					resultChanged = true;
			};

			var mock = new MeditatorMock();
			mock.DefaultResult = DialogResult.Cancel;
			item.MessageBox = mock;
			item.ShowMessageBoxCommand.Execute(null);

			Assert.IsTrue(mediatorLoaded);
			Assert.IsTrue(resultChanged);
		}

		private class MeditatorMock :IDialogMediator
		{
			public DialogResult DefaultResult { get; set; }


			#region IDialogMediator Members

			public void ShowDialog<TState>(Action<TState, object> nextAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings)
			{
				if (DefaultResult == DialogResult.Ok)
					nextAction(state, DefaultResult);

				if (DefaultResult == DialogResult.Cancel)
					otherAction(state, DefaultResult);
			}

			public void ShowDialog<TState>(Action<TState, object> yesAction, Action<TState, object> noAction, Action<TState, object> otherAction, TState state, DialogSettingsBase settings)
			{
				switch (DefaultResult)
				{			
					case DialogResult.Cancel:
						otherAction(state, DefaultResult);
						break;
					case DialogResult.No:
						noAction(state, DefaultResult);
						break;
					case DialogResult.Yes:
						yesAction(state, DefaultResult);
						break;
					default:
						throw new InvalidOperationException();
				}
			}

			#endregion
		}
	}
}
