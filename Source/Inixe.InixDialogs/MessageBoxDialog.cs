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
		
		
		private ContentPresenter _messageContent;

		/// <summary>
		/// Initializes static members of the <see cref="MessageBoxDialog"/> class.
		/// </summary>
		static MessageBoxDialog()
		{
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
			this.DefaultStyleKey = typeof(MessageBoxDialog);
		}

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
			{
				Icon = FetchMessageBoxIcon((int)msgBoxSettings.Icon);
			}
		}

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
	}
}
