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
	using System.Text;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows;
	using System.Windows.Controls.Primitives;
	using System.Windows.Data;

	/// <summary>
	/// Class DialogBase.
	/// </summary>    
	[TemplatePart(Name = "Popup_PART", Type = typeof(Popup))]
	[TemplatePart(Name = "Button1_PART", Type = typeof(Button))]
	[TemplatePart(Name = "Button2_PART", Type = typeof(Button))]
	[TemplatePart(Name = "Button3_PART", Type = typeof(Button))]
	public abstract class DialogBase : Popup
    {
		private Popup _popUp;
		private Button _button1;
        private Button _button2;
        private Button _button3;

		internal static readonly DependencyPropertyKey IsOpenProperty;
				
		/// <summary>
		/// Mediator property.
		/// </summary>
		/// <remarks>The Mediator acts like a bond between the WPF pages and the control</remarks>
		public static readonly DependencyProperty MediatorProperty;

		static DialogBase()
		{
			PropertyMetadata isOpenMeta = new PropertyMetadata(false, new PropertyChangedCallback(OnIsOpenChanged));
			IsOpenProperty = DependencyProperty.RegisterReadOnly("IsOpen", typeof(bool), typeof(DialogBase), isOpenMeta);

			var nullMediator = new NullDialogMediator();
			PropertyMetadata mediatorMetadata = new PropertyMetadata(nullMediator, new PropertyChangedCallback(OnMediatorChanged));
			MediatorProperty = DependencyProperty.Register("Mediator", typeof(IDialogMediator), typeof(DialogBase), mediatorMetadata);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DialogBase"/> class.
		/// </summary>
		public DialogBase()
		{
			_popUp = null;
			_button1 = null;
			_button2 = null;
			_button3 = null;
		}

		protected Button Button1
		{
			get
			{
				return _button1;
			}
		}

		protected Button Button2
		{
			get
			{
				return _button2;
			}
		}

		protected Button Button3
		{
			get
			{
				return _button3;
			}
		}

		public Popup PopUp
		{
			get
			{
				return _popUp;
			}
		}
		internal bool IsOpen
		{		
			get
			{
				return (bool)GetValue(IsOpenProperty.DependencyProperty);
			}			
		}

		public IDialogMediator Mediator
		{		
			get
			{
				return (IDialogMediator)GetValue(MediatorProperty);
			}
			set
			{
				SetValue(MediatorProperty, value);
			}
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			//This Will Happen Before Loaded
			RegisterMediator();		
		}

		public sealed override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_popUp = (Popup) GetTemplateChild("Popup_PART");
			_button1 = (Button)GetTemplateChild("Button1_PART");
			_button2 = (Button)GetTemplateChild("Button2_PART");
			_button3 = (Button)GetTemplateChild("Button3_PART");

			_button1.Click += Button_Click;
			_button2.Click += Button_Click;
			_button3.Click += Button_Click;
		}

		protected virtual void OnIsOpenChanged(bool oldValue, bool newValue)
		{
			// Let inheritors use this
		}

		protected virtual void OnMediatorChanged(IDialogMediator oldValue, IDialogMediator newValue)
		{
			IShowDialogEvents oldEvents = oldValue as IShowDialogEvents;
			IShowDialogEvents newEvents = newValue as IShowDialogEvents;

			if (oldEvents != null)
				oldEvents.Show -= DialogEvents_Show;

			if (newEvents != null)
				newEvents.Show += DialogEvents_Show;
			else			
			{
				RegisterMediator();
				var res = GetBindingExpression(MediatorProperty);
				res.UpdateSource();
			}
		}

		protected virtual IDialogMediator CreateMediator()
		{
			IDialogMediator retval = new SimpleDialogMediator();

			IShowDialogEvents dialogEvents = (IShowDialogEvents)retval;
			dialogEvents.Show += DialogEvents_Show;

			return retval;
		}

		private static void OnMediatorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DialogBase dialogBase = o as DialogBase;
			if (dialogBase != null)
				dialogBase.OnMediatorChanged((IDialogMediator)e.OldValue, (IDialogMediator)e.NewValue);
		}

		private static void OnIsOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DialogBase dialogBase = o as DialogBase;
			if (dialogBase != null)
				dialogBase.OnIsOpenChanged((bool)e.OldValue, (bool)e.NewValue);
		}

		private void RegisterMediator()
		{
			if (Mediator == null)
				Mediator = CreateMediator();

			IRelayMediator relayerMediator = Mediator as IRelayMediator;
			if (relayerMediator != null && relayerMediator.Relayer == null)
				relayerMediator.AddRelayer(CreateMediator());
		}

		private void DialogEvents_Show(object sender, ShowEventArgs e)
		{
			SetValue(IsOpenProperty.DependencyProperty, true);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button sourceButton = (Button)sender;

			// TODO: Give the Button an ID

			e.Handled = true;
		}
    }
}