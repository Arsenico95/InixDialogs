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
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.IO;
	using System.Windows;

	/// <summary>
	/// Class FileDialog.
	/// </summary>
	public class FileDialog : DialogBase
	{
		public static readonly DependencyProperty InitialPathProperty;
		
		// Fields...
		private ObservableCollection<FileInfo> _files;
		private ICollectionView _filesView;

		static FileDialog()
		{
			InitialPathProperty = DependencyProperty.Register("InitialPath", typeof(string), typeof(FileDialog), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnInitialPathChanged)));
		}

		protected FileDialog()
		{
			_files = new ObservableCollection<FileInfo>();
		}

		// Methods...
		private static void OnInitialPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			FileDialog fileDialog = o as FileDialog;
			if (fileDialog != null)
				fileDialog.OnInitialPathChanged((string)e.OldValue, (string)e.NewValue);
		}

		protected virtual void OnInitialPathChanged(string oldValue, string newValue)
		{			
			var files = Directory.GetFiles(newValue);

		}

		public ICollectionView FilesView
		{
			get
			{
				return _filesView;
			}
		}

		public string InitialPath
		{			
			get
			{
				return (string)GetValue(InitialPathProperty);
			}
			set
			{
				SetValue(InitialPathProperty, value);
			}
		}
		public ObservableCollection<FileInfo> Files
		{
			get { return _files; }
		}

		protected override void SetupDialog(DialogSettingsBase settings)
		{
			throw new System.NotImplementedException();
		}

		protected override object GetDialogResult(DialogResult identifier)
		{
			throw new System.NotImplementedException();
		}
	}
}
