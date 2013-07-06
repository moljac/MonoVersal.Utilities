//
// System.Collections.Specialized.NameObjectCollectionBase.cs
//
// Author:
//   Gleb Novodran
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Runtime.Serialization;

namespace System.Collections.Specialized
{
	[Serializable]
	public abstract partial class NameObjectCollectionBase : ISerializable, IDeserializationCallback
	{
		// SerializationInfo infoCopy;

		protected NameObjectCollectionBase(SerializationInfo info, StreamingContext context)
		{
			infoCopy = info;
		}

		// ISerializable
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			int count = Count;
			string[] keys = new string[count];
			object[] values = new object[count];
			int i = 0;
			foreach (_Item item in m_ItemsArray)
			{
				keys[i] = item.key;
				values[i] = item.value;
				i++;
			}

#if NET_2_0
	  if (equality_comparer != null) {
		info.AddValue ("KeyComparer", equality_comparer, typeof (IEqualityComparer));
		info.AddValue ("Version", 4, typeof (int));
	  } else {
		info.AddValue ("HashProvider", m_hashprovider, typeof (IHashCodeProvider));
		info.AddValue ("Comparer", m_comparer, typeof (IComparer));
		info.AddValue ("Version", 2, typeof (int));
	  }
#else
			info.AddValue("HashProvider", m_hashprovider, typeof(IHashCodeProvider));
			info.AddValue("Comparer", m_comparer, typeof(IComparer));
#endif
			info.AddValue("ReadOnly", m_readonly);
			info.AddValue("Count", count);
			info.AddValue("Keys", keys, typeof(string[]));
			info.AddValue("Values", values, typeof(object[]));
		}

		// IDeserializationCallback
		public virtual void OnDeserialization(object sender)
		{
			SerializationInfo info = infoCopy;

			// If a subclass overrides the serialization constructor
			// and inplements its own serialization process, infoCopy will
			// be null and we can ignore this callback.
			if (info == null)
				return;

			infoCopy = null;
			m_hashprovider = (IHashCodeProvider)info.GetValue("HashProvider",
							typeof(IHashCodeProvider));
#if NET_2_0
	  if (m_hashprovider == null) {
		equality_comparer = (IEqualityComparer) info.GetValue ("KeyComparer", typeof (IEqualityComparer));
	  } else {
		m_comparer = (IComparer) info.GetValue ("Comparer", typeof (IComparer));
		if (m_comparer == null)
		  throw new SerializationException ("The comparer is null");
	  }
#else
			if (m_hashprovider == null)
				throw new SerializationException("The hash provider is null");

			m_comparer = (IComparer)info.GetValue("Comparer", typeof(IComparer));
			if (m_comparer == null)
				throw new SerializationException("The comparer is null");
#endif
			m_readonly = info.GetBoolean("ReadOnly");
			string[] keys = (string[])info.GetValue("Keys", typeof(string[]));
			if (keys == null)
				throw new SerializationException("keys is null");

			object[] values = (object[])info.GetValue("Values", typeof(object[]));
			if (values == null)
				throw new SerializationException("values is null");

			Init();
			int count = keys.Length;
			for (int i = 0; i < count; i++)
				BaseAdd(keys[i], values[i]);
		}
	}
}