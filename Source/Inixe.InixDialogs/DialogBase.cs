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
	[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	[TemplatePart(Name = "PART_Button1", Type = typeof(Button))]
	[TemplatePart(Name = "PART_Button2", Type = typeof(Button))]
	[TemplatePart(Name = "PART_Button3", Type = typeof(Button))]
	public abstract class DialogBase : Control
    {
		/// <summary>
		/// This Property controls the internal popup behavior
		/// </summary>
		public static readonly DependencyProperty IsOpenProperty;

		/// <summary>
		/// Dialog title property
		/// </summary>
		public static readonly DependencyPropertyKey DialogTitleProperty;

		/// <summary>
		/// This Property controls the internal popup behavior
		/// </summary>
		private static readonly DependencyPropertyKey IsOpenPropertyKey;

		/// <summary>
		/// Mediator property.
		/// </summary>
		/// <remarks>The Mediator acts like a bond between the WPF pages and the control</remarks>
		public static readonly DependencyProperty MediatorProperty;

		private Popup _popUp;
		private Button _button1;
        private Button _button2;
        private Button _button3;

		static DialogBase()
		{
			PropertyMetadata dialogTitleMeta = new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnDialogTitleChanged));
			DialogTitleProperty = DependencyProperty.RegisterReadOnly("DialogTitle", typeof(string), typeof(DialogBase), dialogTitleMeta);

			PropertyMetadata isOpenMeta = new PropertyMetadata(false, new PropertyChangedCallback(OnIsOpenChanged));
			IsOpenPropertyKey = DependencyProperty.RegisterReadOnly("IsOpen", typeof(bool), typeof(DialogBase), isOpenMeta);

			IsOpenProperty = IsOpenPropertyKey.DependencyProperty;

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

		public Popup PopUp
		{
			get
			{
				return _popUp;
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

		internal bool IsOpen
		{
			get
			{
				return (bool)GetValue(IsOpenPropertyKey.DependencyProperty);
			}

			private set
			{
				SetValue(IsOpenPropertyKey, value);
			}
		}

		public string DialogTitle
		{
			get
			{
				return (string)GetValue(DialogTitleProperty.DependencyProperty);
			}

			private set
			{
				SetValue(DialogTitleProperty, value);
			}
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
		
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			//This Will Happen Before Loaded
			RegisterMediator();		
		}

		public sealed override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (this.Template == null)
				return;

			_popUp = (Popup)GetTemplateChild("PART_Popup");

			// If we use CommandBindings we're handing over to inheritors or style overriders the responsability of 
			// implementing the actual command calls in their templates(Or attach'em in the XAML). 
			// I Know that this way there isn't any warranty either, 'cause inheritors can detach the event handlers from the event handler list,
			// But that means that they know what they want and thats a way of getting it. 
			// In the other hand they may forget or may take us out of the way by mistake
			_button1 = (Button)GetTemplateChild("PART_Button1");
			_button2 = (Button)GetTemplateChild("PART_Button2");
			_button3 = (Button)GetTemplateChild("PART_Button3");

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
			IDialogController oldEvents = oldValue as IDialogController;
			IDialogController newEvents = newValue as IDialogController;

			if (oldEvents != null)
				oldEvents.Show -= DialogEvents_Show;

			if (newEvents == null)
			{
				RegisterMediator();
				BindingExpression res = GetBindingExpression(MediatorProperty);

				if (res != null && (res.ParentBinding.Mode == BindingMode.TwoWay || res.ParentBinding.Mode == BindingMode.OneWayToSource))
					res.UpdateSource();
			}
			else
				newEvents.Show += DialogEvents_Show;
		}

		protected virtual IDialogMediator CreateMediator()
		{
			IDialogMediator retval = new SimpleDialogMediator();

			IDialogController dialogEvents = (IDialogController)retval;
			dialogEvents.Show += DialogEvents_Show;

			return retval;
		}

		protected virtual void OnDialogTitleChanged(string oldValue, string newValue)
		{
			// Let inheritors use this
		}

		private static void OnDialogTitleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DialogBase dialogBase = o as DialogBase;
			if (dialogBase != null)
				dialogBase.OnDialogTitleChanged((string)e.OldValue, (string)e.NewValue);
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

			IDialogMediatorController relayerMediator = Mediator as IDialogMediatorController;
			if (relayerMediator != null && relayerMediator.Mediator == null)
				relayerMediator.AddMediator(CreateMediator());
		}

		private void DialogEvents_Show(object sender, ShowEventArgs e)
		{
			this.DialogTitle = e.Settings.HeaderText;
			this.IsOpen = true;

			//TODO: Setup before popup show 
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button sourceButton = (Button)sender;
			
			int id =(int)char.GetNumericValue(sourceButton.Name[sourceButton.Name.Length - 1]);
			
			// TODO: Execute HostActions

			e.Handled = true;
		}
    }
}