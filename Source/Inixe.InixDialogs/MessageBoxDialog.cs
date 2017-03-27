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



using System.Collections.Generic;
namespace Inixe.InixDialogs
{
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Interop;
	using System.Runtime.InteropServices;
	using System;


	/// <summary>
	/// Class MessageBoxDialog.
	/// </summary>
	/// <seealso cref="Inixe.InixDialogs.DialogBase" />
	[TemplatePart(Name = "PART_MessageContents", Type = typeof(ContentPresenter))]
	public class MessageBoxDialog : DialogBase
	{
		public static readonly DependencyPropertyKey MessageContentProperty;
		public static readonly DependencyPropertyKey IconProperty;
		public static readonly DependencyProperty MessageContentTemplateProperty;
		
		
		private ContentPresenter _messageContent;
		private IDictionary<string, DialogResult> _resultMappings;

		/// <summary>
		/// Initializes static members of the <see cref="MessageBoxDialog"/> class.
		/// </summary>
		static MessageBoxDialog()
		{
			PropertyChangedCallback messageContentTemplateCallback = new PropertyChangedCallback(OnMessageContentTemplateChanged);
			PropertyMetadata messageContentTemplateMeta = new PropertyMetadata(messageContentTemplateCallback);
			MessageContentTemplateProperty = DependencyProperty.Register("MessageContentTemplate", typeof(DataTemplate), typeof(MessageBoxDialog), messageContentTemplateMeta);

			PropertyChangedCallback iconCallback = new PropertyChangedCallback(OnMessageBoxIconChanged);
			PropertyMetadata iconMeta = new PropertyMetadata(null, iconCallback);
			IconProperty = DependencyProperty.RegisterReadOnly("Icon", typeof(ImageSource), typeof(MessageBoxDialog), iconMeta);

			PropertyChangedCallback messageContentCallback = new PropertyChangedCallback(OnMessageContentChanged);
			PropertyMetadata messageContentMetaData = new PropertyMetadata(null, messageContentCallback);
			MessageContentProperty = DependencyProperty.RegisterReadOnly("MessageContent", typeof(object), typeof(MessageBoxDialog), messageContentMetaData);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MessageBoxDialog" /> class.
		/// </summary>
		public MessageBoxDialog()
		{
			DefaultStyleKey = typeof(MessageBoxDialog);
			_resultMappings = new Dictionary<string, DialogResult>();
		}

		public DataTemplate MessageContentTemplate
		{
			// IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
			get
			{
				return (DataTemplate)GetValue(MessageContentTemplateProperty);
			}
			set
			{
				SetValue(MessageContentTemplateProperty, value);
			}
		}
		/// <summary>
		/// Gets the icon image source.
		/// </summary>
		/// <value>The icon.</value>
		/// <remarks>When using the custom icon mode this Icon that's going to be displayed</remarks>
		public ImageSource Icon
		{
			get
			{
				return (ImageSource)GetValue(IconProperty.DependencyProperty);
			}

			private set
			{
				SetValue(IconProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>The content.</value>
		public object MessageContent
		{			
			get
			{
				return (object)GetValue(MessageContentProperty.DependencyProperty);
			}

			private set
			{
				SetValue(MessageContentProperty, value);
			}
		}

		/// <summary>
		/// Gets the button and <see cref="DialogResult" />result mapping.
		/// </summary>
		/// <value>The button result mapping.</value>
		/// <remarks>Button result mapping is responsable of mapping the result values to a button when it's clicked.</remarks>
		protected override IDictionary<string, DialogResult> ButtonDialogResultMapping
		{
			get
			{
				return this._resultMappings;
			}
		}

		/// <summary>
		/// Called when the cotrol should apply it's template.
		/// </summary>
		/// <remarks>None</remarks>
		public sealed override void OnApplyTemplate()
		{
			_messageContent = (ContentPresenter)GetTemplateChild("PART_MessageContents");
			base.OnApplyTemplate();
		}

		/// <summary>
		/// Setups the dialog.
		/// </summary>
		/// <param name="settings">The settings.</param>
		/// <remarks>Tells the inheritors to setup their properties in order to display the dialog.</remarks>
		protected sealed override void SetupDialog(DialogSettingsBase settings)
		{
			MessageBoxDialogSettings msgBoxSettings = settings as MessageBoxDialogSettings;

			MessageContent = msgBoxSettings.MessageContent;

			if (msgBoxSettings.Icon != MessageBoxIcon.Custom)
				Icon = FetchMessageBoxIcon((int)msgBoxSettings.Icon);

			_resultMappings.Clear();
			switch (msgBoxSettings.Buttons)
			{
				case MessageBoxButtons.Ok:
					Button1.Content = Properties.Resources.OkButtonText;
					Button2.Visibility = System.Windows.Visibility.Collapsed;
					Button3.Visibility = System.Windows.Visibility.Collapsed;

					_resultMappings.Add(Button1.Name, DialogResult.Ok);
					break;
				case MessageBoxButtons.OkCancel:
					Button1.Content = Properties.Resources.OkButtonText;
					Button2.Visibility = System.Windows.Visibility.Collapsed;
					Button3.Content = Properties.Resources.CancelButtonText;
					Button3.Visibility = System.Windows.Visibility.Visible;

					_resultMappings.Add(Button1.Name, DialogResult.Ok);
					_resultMappings.Add(Button3.Name, DialogResult.Cancel);
					break;
				case MessageBoxButtons.RetryCancel:
					Button1.Content = Properties.Resources.RetryButtonText;
					Button2.Visibility = System.Windows.Visibility.Collapsed;
					Button3.Content = Properties.Resources.CancelButtonText;
					Button3.Visibility = System.Windows.Visibility.Visible;

					_resultMappings.Add(Button1.Name, DialogResult.Ok);
					_resultMappings.Add(Button3.Name, DialogResult.Cancel);
					break;
				case MessageBoxButtons.YesNo:
					Button1.Visibility = System.Windows.Visibility.Visible;
					Button1.Content = Properties.Resources.YesButtonText;

					Button2.Visibility = System.Windows.Visibility.Visible;
					Button2.Content = Properties.Resources.NoButtonText;

					Button3.Visibility = System.Windows.Visibility.Collapsed;

					_resultMappings.Add(Button1.Name, DialogResult.Yes);
					_resultMappings.Add(Button2.Name, DialogResult.No);
					break;
				case MessageBoxButtons.YesNoCancel:
					Button1.Visibility = System.Windows.Visibility.Visible;
					Button1.Content = Properties.Resources.YesButtonText;

					Button2.Visibility = System.Windows.Visibility.Visible;
					Button2.Content = Properties.Resources.NoButtonText;

					Button3.Content = Properties.Resources.CancelButtonText;
					Button3.Visibility = System.Windows.Visibility.Visible;

					_resultMappings.Add(Button1.Name, DialogResult.Yes);
					_resultMappings.Add(Button2.Name, DialogResult.No);
					_resultMappings.Add(Button3.Name, DialogResult.Cancel);
					break;
			}  
		}

		/// <summary>
		/// Gets the dialog result once the dialog is closing.
		/// </summary>
		/// <param name="identifier">The identifier.</param>
		/// <returns>System.Object.</returns>
		/// <remarks>None</remarks>
		protected sealed override object GetDialogResult(DialogResult identifier)
		{
			return identifier;
		}

		/// <summary>
		/// Called when message content template has changed.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		/// <remarks>None</remarks>
		protected virtual void OnMessageContentTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
		{
			// TODO: Add your property changed side-effects. Descendants can override as well.
		}

		/// <summary>
		/// Called when message box icon has changed.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		/// <remarks>None</remarks>
		protected virtual void OnMessageBoxIconChanged(ImageSource oldValue, ImageSource newValue)
		{
			// TODO: Add your property changed side-effects. Descendants can override as well.
		}
		
		/// <summary>
		/// Called when content has changed.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		protected virtual void OnMessageContentChanged(object oldValue, object newValue)
		{
			// TODO: Add your property changed side-effects. Descendants can override as well.
		}

		[DllImport(@"user32.dll", CharSet = CharSet.Ansi)]
		private static extern IntPtr LoadIcon(IntPtr instance, int resource);

		[DllImport(@"user32.dll", CharSet = CharSet.Ansi)]
		private static extern IntPtr DestroyIcon(IntPtr icon);


		private static BitmapSource FetchMessageBoxIcon(int id)
		{		
			//Let's use the small size first. Later we'll get de system's config and use the right one instead
			Int32Rect rect = new Int32Rect(0, 0, (int)SystemParameters.IconWidth, (int)SystemParameters.IconHeight);

			IntPtr hIcon = LoadIcon(IntPtr.Zero, id);
			BitmapSource src = Imaging.CreateBitmapSourceFromHIcon(hIcon, rect, BitmapSizeOptions.FromEmptyOptions());

			DestroyIcon(hIcon);

			return src;
		}

		/// <summary>
		/// Handles the <see cref="E:MessageBoxIconChanged" /> event.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		/// <remarks>None</remarks>
		private static void OnMessageBoxIconChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			MessageBoxDialog messageBoxDialog = o as MessageBoxDialog;
			if (messageBoxDialog != null)
				messageBoxDialog.OnMessageBoxIconChanged((ImageSource)e.OldValue, (ImageSource)e.NewValue);
		}

		/// <summary>
		/// Handles the <see cref="E:ContentChanged" /> event.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnMessageContentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			MessageBoxDialog messageBoxDialog = o as MessageBoxDialog;
			if (messageBoxDialog != null)
				messageBoxDialog.OnMessageContentChanged((object)e.OldValue, (object)e.NewValue);
		}

		/// <summary>
		/// Handles the <see cref="E:MessageContentTemplateChanged" /> event.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		/// <remarks>None</remarks>
		private static void OnMessageContentTemplateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			MessageBoxDialog messageBoxDialog = o as MessageBoxDialog;
			if (messageBoxDialog != null)
				messageBoxDialog.OnMessageContentTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}
	}
}
