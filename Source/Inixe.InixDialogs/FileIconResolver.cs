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
	using System.Text;
	using System.Threading.Tasks;
	using System.Runtime.InteropServices;
	using System.Windows.Media.Imaging;
	using System.Windows.Interop;
	using System.Windows;

	internal sealed class FileIconResolver 
	{
		private const int MaxPath = 256;

		private const ulong FileAttributeNormal = 0x80;

		private static FileIconResolver _instance;
		private Dictionary<string, BitmapSource> _iconCache;

		private FileIconResolver()
		{  
			_iconCache = new Dictionary<string,BitmapSource>();
		}

		public static FileIconResolver GetInstance()
		{
			if (_instance == null)
				_instance = new FileIconResolver();

			return _instance;
		}

		public BitmapSource GetFileIcon(string path)
		{
			path.ThrowIfNullOrEmpty("path");

			string extension = System.IO.Path.GetExtension(path);
			BitmapSource retval;

			if (!_iconCache.TryGetValue(extension, out retval))
			{
				retval = FetchIcon(extension);
				if (retval != null)
					_iconCache.Add(extension, retval);
			}

			return retval;
		}


		private static BitmapSource FetchIcon(string extension)
		{
			extension.ThrowIfNullOrEmpty("extension");

			SHFILEINFO fileInfo = new SHFILEINFO();
			ShGFIFlags flags = ShGFIFlags.UseFileAttributes | ShGFIFlags.Icon | ShGFIFlags.SmallIcon;
			IntPtr res = SHGetFileInfo(extension, FileAttributeNormal, ref fileInfo, Marshal.SizeOf(fileInfo), (ulong)flags);

			if (res != IntPtr.Zero)
				return null;

			//Let's use the small size first. Later we'll get de system's config and use the right one instead
			Int32Rect rect = new Int32Rect(0, 0, 16, 16);

			BitmapSource src = Imaging.CreateBitmapSourceFromHIcon(fileInfo.hIcon, rect, BitmapSizeOptions.FromEmptyOptions());
			DestroyIcon(fileInfo.hIcon);

			return src;
		}

		[DllImport(@"Shell32.dll", CharSet = CharSet.Ansi)]
		private static extern IntPtr SHGetFileInfo(string filePath, ulong fileAttributes, ref SHFILEINFO fileInfo, int fileInfoLen, ulong flags);

		[DllImport(@"user32.dll", CharSet = CharSet.Ansi)]
		private static extern IntPtr DestroyIcon(IntPtr icon);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		private struct SHFILEINFO 
		{
			public IntPtr hIcon;
			public int   iIcon;
			public uint dwAttributes;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst=MaxPath)]
			public char[] szDisplayName;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst=80)]
			public char[] szTypeName;
		}

		[Flags]
		private enum ShGFIFlags
		{
			UseFileAttributes = 0x000000010,
			Icon = 0x000000100,
			SmallIcon = 0x000000001,
			ShellIconSize = 0x000000004,
			LargeIcon = 0x000000000,
			LinkOverLay = 0x000008000,
			OpenIcon = 0x000000002,
			OverlayIndex = 0x000000040,
			IDList = 0x000000008,
			Selected = 0x000010000,
			SystemIconIndex = 0x000004000,
			TypeName = 0x000000400,
			IconLocation = 0x000001000,
			ExtensionType = 0x000002000,
			DisplayName = 0x000000200,
			Attributes = 0x000000800,
			AddonOverlays = 0x000000020,
			AttributeSpecified = 0x000020000
			
		}
	}
}
