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
	/// <remarks>This is the base class for every Dialog on the library. The class defines the basic structure of a modal dialog box.
	/// <para>Dialog Boxes come in many flavours and the only sole thing they share is the fact that the modal dialg is going to be closed at some point in time.
	/// So the basic items that all modal dialogs share are the closing buttons, and the title bar, if we think of the classic Windows Message Box and Dialogs.</para></remarks>
	[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	[TemplatePart(Name = "PART_Button1", Type = typeof(Button))]
	[TemplatePart(Name = "PART_Button2", Type = typeof(Button))]
	[TemplatePart(Name = "PART_Button3", Type = typeof(Button))]
	public abstract class DialogBase : Control
    {		
		/// <summary>
		/// This Property controls the internal popup behavior
		/// </summary>
		/// <remarks>None</remarks>
		public static readonly DependencyProperty IsOpenProperty;

		/// <summary>
		/// Dialog title property
		/// </summary>
		/// <remarks>None</remarks>
		public static readonly DependencyPropertyKey DialogTitleProperty;

		/// <summary>
		/// This Property controls the internal popup behavior
		/// </summary>
		/// <remarks>None</remarks>
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

		private Point _startDragPoint;
		private bool _isDragging;

		/// <summary>
		/// Initializes static members of the <see cref="DialogBase"/> class.
		/// </summary>
		/// <remarks>None</remarks>
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
		/// <remarks>None</remarks>
		public DialogBase()
		{
			_popUp = null;
			_button1 = null;
			_button2 = null;
			_button3 = null;
		}

		/// <summary>
		/// Gets the pop up instance.
		/// </summary>
		/// <value>The pop up.</value>
		/// <remarks>None</remarks>
		public Popup PopUp
		{
			get
			{
				return _popUp;
			}
		}

		/// <summary>
		/// Gets or sets the mediator used by the instance.
		/// </summary>
		/// <value>The mediator.</value>
		/// <remarks>None</remarks>
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

		/// <summary>
		/// Gets a value indicating whether this instance is open.
		/// </summary>
		/// <value><c>true</c> if this instance is open; otherwise, <c>false</c>.</value>
		/// <remarks>None</remarks>
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

		/// <summary>
		/// Gets the dialog title used for this instance.
		/// </summary>
		/// <value>The dialog title.</value>
		/// <remarks>None</remarks>
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

		/// <summary>
		/// Gets the button and <see cref="DialogResult"/>result mapping.
		/// </summary>
		/// <value>The button result mapping.</value>
		/// <remarks>Button result mapping is responsable of mapping the result values to a button when it's clicked.</remarks>
		protected abstract IDictionary<string, DialogResult> ButtonDialogResultMapping { get; }

		/// <summary>
		/// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized" /> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized" /> is set to true internally.
		/// </summary>
		/// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" /> that contains the event data.</param>
		/// <remarks>None</remarks>
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			//This Will Happen Before Loaded
			RegisterMediator();		
		}

		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
		/// </summary>
		/// <remarks>None</remarks>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (Template == null)
				return;

			_popUp = (Popup)GetTemplateChild("PART_Popup");
			_popUp.Child.MouseDown += PopupChild_MouseDown;
			_popUp.Child.MouseMove += PopupChild_MouseMove;
			_popUp.Child.MouseUp += PopupChild_MouseUp;

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

		/// <summary>
		/// Called when the dialog settings have changed.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		/// <remarks>None</remarks>
		protected virtual void OnSettingsChanged(DialogSettingsBase oldValue, DialogSettingsBase newValue)
		{
			// Let inheritors use this
		}

		/// <summary>
		/// Called when the is open property has changed.
		/// </summary>
		/// <param name="oldValue">if set to <c>true</c> The dialog was opened and therefore now is closing.</param>
		/// <param name="newValue">if set to <c>true</c> The dialog was closed and therefore now is showing.</param>
		/// <remarks>None</remarks>
		protected virtual void OnIsOpenChanged(bool oldValue, bool newValue)
		{
			// Let inheritors use this
		}

		/// <summary>
		/// Called when the dialog mediator has changed.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		/// <remarks>When overriden this method is responsible of attaching the mediator to the instance.</remarks>
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

		/// <summary>
		/// Creates the mediator the current instance is going to be attached to.
		/// </summary>
		/// <returns>A IDialogMediator.</returns>
		/// <remarks>Every Dialog can override this method to have a custom mediator implementation.</remarks>
		protected virtual IDialogMediator CreateMediator()
		{
			IDialogMediator retval = new SimpleDialogMediator();

			IDialogController dialogEvents = (IDialogController)retval;
			dialogEvents.Show += DialogEvents_Show;

			return retval;
		}

		/// <summary>
		/// Setups the dialog.
		/// </summary>
		/// <param name="settings">The settings.</param>
		/// <remarks>Tells the inheritors to setup their properties in order to display the dialog.</remarks>
		protected abstract void SetupDialog(DialogSettingsBase settings);

		/// <summary>
		/// Gets the dialog result once the dialog is closing.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		/// <returns>System.Object.</returns>
		/// <remarks>None</remarks>
		protected abstract object GetDialogResult(DialogResult identifier);

		/// <summary>
		/// Called when the dialog title has changed.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		/// <remarks>None</remarks>
		protected virtual void OnDialogTitleChanged(string oldValue, string newValue)
		{
			// Let inheritors use this
		}

		/// <summary>
		/// Handles the <see cref="E:SettingsChanged" /> event.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		/// <remarks>None</remarks>
		private static void OnSettingsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DialogBase dialogBase = o as DialogBase;
			if (dialogBase != null)
				dialogBase.OnSettingsChanged((DialogSettingsBase)e.OldValue, (DialogSettingsBase)e.NewValue);
		}

		/// <summary>
		/// Handles the <see cref="E:DialogTitleChanged" /> event.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		/// <remarks>None</remarks>
		private static void OnDialogTitleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DialogBase dialogBase = o as DialogBase;
			if (dialogBase != null)
				dialogBase.OnDialogTitleChanged((string)e.OldValue, (string)e.NewValue);
		}

		/// <summary>
		/// Handles the <see cref="E:MediatorChanged" /> event.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		/// <remarks>None</remarks>
		private static void OnMediatorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DialogBase dialogBase = o as DialogBase;
			if (dialogBase != null)
				dialogBase.OnMediatorChanged((IDialogMediator)e.OldValue, (IDialogMediator)e.NewValue);
		}

		/// <summary>
		/// Handles the <see cref="E:IsOpenChanged" /> event.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		/// <remarks>None</remarks>
		private static void OnIsOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DialogBase dialogBase = o as DialogBase;
			if (dialogBase != null)
				dialogBase.OnIsOpenChanged((bool)e.OldValue, (bool)e.NewValue);
		}

		/// <summary>
		/// Registers the mediator.
		/// </summary>
		/// <remarks>If no Mediator is supplied through the <see cref="Mediator"/>, a default mediator is created for the instance. 
		/// Otherwise the instance mediator registers it self into the current mediator.</remarks>
		private void RegisterMediator()
		{
			if (Mediator == null)
				Mediator = CreateMediator();

			IDialogMediatorController relayerMediator = Mediator as IDialogMediatorController;
			if (relayerMediator != null && relayerMediator.Mediator == null)
				relayerMediator.AddMediator(CreateMediator());
		}

		/// <summary>
		/// Handles the Show event of the DialogEvents control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="ShowEventArgs"/> instance containing the event data.</param>
		/// <remarks>None</remarks>
		private void DialogEvents_Show(object sender, ShowEventArgs e)
		{
			DialogTitle = e.Settings.HeaderText;

			SetupDialog(e.Settings);

			IsOpen = true;
		}

		/// <summary>
		/// Handles the Click event of the Button control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		/// <remarks>Here's where the actual dialog result is resolved. In other words this is the first stage of the end.
		/// Then a controller is executed and then we'll be called back into the mediator instance.</remarks>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button sourceButton = (Button)sender;

			DialogResult resultType;

			try
			{
				resultType = ButtonDialogResultMapping[sourceButton.Name];
			}
			catch (KeyNotFoundException)
			{

				resultType = DialogResult.None;
			}

			IDialogController controller = (IDialogController)Mediator;

			object res = GetDialogResult(resultType);

			controller.Execute(resultType, null, res);

			e.Handled = true;
			IsOpen = false;
		}

		/// <summary>
		/// Handles the MouseUp event of the PopupChild control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
		/// <remarks>This method forms part of a chain of events that handle the Dialog moving on the screen.</remarks>
		private void PopupChild_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left && _isDragging)
			{
				_popUp.Child.ReleaseMouseCapture();

				_isDragging = false;
				e.Handled = true;
			}
		}

		/// <summary>
		/// Handles the MouseMove event of the PopupChild control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
		/// <remarks>This method forms part of a chain of events that handle the Dialog moving on the screen.</remarks>
		private void PopupChild_MouseMove(object sender, MouseEventArgs e)
		{
			if (_isDragging)
			{
				var pos = e.GetPosition(null);

				_popUp.HorizontalOffset += (pos.X - _startDragPoint.X);
				_popUp.VerticalOffset += (pos.Y - _startDragPoint.Y);
				e.Handled = true;
			}
		}

		/// <summary>
		/// Handles the MouseDown event of the PopupChild control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
		/// <remarks>This method forms part of a chain of events that handle the Dialog moving on the screen.</remarks>
		private void PopupChild_MouseDown(object sender, MouseButtonEventArgs e)
		{
			
			if(e.ChangedButton== MouseButton.Left)
			{
				_startDragPoint = e.GetPosition(null);

				_popUp.Child.CaptureMouse();
				
				_isDragging = true;
				e.Handled = true;
			}
		}
    }
}