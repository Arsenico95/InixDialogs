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
	using System.ComponentModel;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	/// <summary>
	/// Class FileInfo.
	/// </summary>
	internal class FileInfoViewModel : INotifyPropertyChanged
	{
		private readonly string _path;
		private readonly Lazy<BitmapSource> _icon;
		private readonly string _extension;

		private long _size;
		private DateTime _creationDate;
		private string _fileName;
						

		/// <summary>
		/// Initializes a new instance of the <see cref="FileInfoViewModel"/> class.
		/// </summary>
		public FileInfoViewModel(string filePath, DateTime creationDate, long size)
		{
			bool validPath = !string.IsNullOrWhiteSpace(filePath);

			_icon = new Lazy<BitmapSource>(this.GetFileIcon);

			_extension = validPath ? System.IO.Path.GetExtension(filePath): string.Empty;
			_fileName = validPath ? System.IO.Path.GetFileNameWithoutExtension(filePath) : string.Empty;
			
			_creationDate = creationDate;
			_size = size;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileInfoViewModel"/> class.
		/// </summary>
		public FileInfoViewModel()
			: this(string.Empty, default(DateTime), 0)
		{
		}

        public event PropertyChangedEventHandler PropertyChanged;

		public DateTime CreationDate
		{
			get
			{
				return _creationDate;
			}
			set
			{
				if (_creationDate == value)
					return;
				_creationDate = value;
				OnPropertyChanged("CreationDate");
			}
		}

		public string FileName
		{
			get
			{
				return _fileName;
			}
			set
			{
				_fileName = value;
			}
		}

		public string Path
		{
			get
			{
				return _path;
			}
		}

		public long Size
		{
			get
			{
				return _size;
			}
			set
			{
				if (_size == value)
					return;
				_size = value;
				OnPropertyChanged("Size");
			}
		}

		public BitmapSource Icon
		{
			get
			{
				return _icon.Value;
			}
		}

		public string Extension
		{
			get
			{
				return _extension;
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = this.PropertyChanged;

			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		private BitmapSource GetFileIcon()
		{
			if (string.IsNullOrWhiteSpace(_extension))
				return null;

			FileIconResolver resolver = FileIconResolver.GetInstance();
			return resolver.GetFileIcon(_fileName);
		}
	}
}
