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
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Class Utils.
	/// </summary>
	/// <remarks>This class is meant to hold all house keeping methods</remarks>
	internal static class Utils
	{
		/// <summary>
		/// Throws a <see cref="System.ArgumentNullException"/> if the extened object instance is <c>null</c>.
		/// </summary>
		/// <param name="obj">The object that will be extended.</param>
		/// <param name="argumentName">Name of the argument that we're going to use if any exception is thrown.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="obj"/> is <c>null</c></exception>
		/// <remarks>Tests the <paramref name="obj"/> for <c>null</c>value</remarks>
		public static void ThrowIfNull(this object obj, string argumentName)
		{
			ThrowIfNullCore(obj, argumentName);
		}

		/// <summary>
		/// Throws a <see cref="System.ArgumentNullException"/> if the extened object instance is <c>null</c>.
		/// </summary>
		/// <param name="obj">The object that will be extended.</param>
		/// <param name="argumentName">Name of the argument that we're going to use if any exception is thrown.</param>
		/// <param name="message">The message that's going to be used as part of the exception thrown if any.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="obj"/> is <c>null</c></exception>
		/// <remarks>Tests the <paramref name="obj"/> for <c>null</c>value</remarks>
		public static void ThrowIfNull(this object obj, string argumentName, string message)
		{
			ThrowIfNullCore(obj, argumentName, message);
		}

		public static void ThrowIfNullOrEmpty(this string str, string argumentName)
		{
			ThrowIfNullOrEmptyCore(str, argumentName);
		}

		public static void ThrowIfNullOrEmpty(this string str, string argumentName, string message)
		{
			ThrowIfNullOrEmptyCore(str, argumentName, message);
		}

		public static IDictionary ToSimpleDictionary<TKey,TValue>(this IDictionary<TKey, TValue> dic)
		{
			dic.ThrowIfNull("dic");
			return new Dictionary<TKey, TValue>(dic);
		}

		private static void ThrowIfNullOrEmptyCore(string str, string argumentName)
		{
			ThrowIfNullOrEmptyCore(str, argumentName);
		}

		private static void ThrowIfNullCore(object obj, string argumentName)
		{
			ThrowIfNullCore(obj, argumentName, string.Empty);
		}

		private static void ThrowIfNullCore(object obj, string argumentName, string message)
		{
			if (obj is ValueType)
				return;

			if (obj != null)
				return;

			if (!string.IsNullOrWhiteSpace(argumentName))
			{
				if (!string.IsNullOrWhiteSpace(message))
					throw new ArgumentException(message, argumentName);

				throw new ArgumentException(string.Format("{0} cannot be null", argumentName), argumentName);
			}
			else
				throw new ArgumentException();
		}

		private static void ThrowIfNullOrEmptyCore(string str, string argumentName, string message)
		{
			if (!string.IsNullOrEmpty(str))
				return;

			if (!string.IsNullOrEmpty(argumentName))
			{
				if (!string.IsNullOrEmpty(message))
					throw new ArgumentException(message, argumentName);
				else
					throw new ArgumentException(string.Format("Invalid string was supplied in {0}", argumentName), argumentName);
			}

			throw new ArgumentException();
		}
	}
}
