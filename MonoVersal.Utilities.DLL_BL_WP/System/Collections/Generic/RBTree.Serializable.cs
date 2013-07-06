//
// System.Collections.Generic.RBTree
//
// Authors:
//   Raja R Harinath <rharinath@novell.com>
//

//
// Copyright (C) 2007, Novell, Inc.
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

#define ONE_MEMBER_CACHE


using System;
using System.Collections;

namespace System.Collections.Generic
{
	[Serializable]
	internal class RBTree
	{
	}

	public struct NodeEnumerator 
	{

	}
}

#if TEST
namespace Mono.ValidationTest {
	using System.Collections.Generic;

	internal class TreeSet<T> : IEnumerable<T>, IEnumerable
	{
		public class Node : RBTree.Node {
			public T value;

			public Node (T v)
			{
				value = v;
			}

			public override void SwapValue (RBTree.Node other)
			{
				Node o = (Node) other;
				T v = value;
				value = o.value;
				o.value = v;
			}

			public override void Dump (string indent)
			{
				Console.WriteLine ("{0}{1} {2}({3})", indent, value, IsBlack ? "*" : "", Size);
				if (left != null)
					left.Dump (indent + "  /");
				if (right != null)
					right.Dump (indent + "  \\");
			}
		}

		public class NodeHelper : RBTree.INodeHelper<T> {
			IComparer<T> cmp;

			public int Compare (T value, RBTree.Node node)
			{
				return cmp.Compare (value, ((Node) node).value);
			}

			public RBTree.Node CreateNode (T value)
			{
				return new Node (value);
			}

			private NodeHelper (IComparer<T> cmp)
			{
				this.cmp = cmp;
			}
			static NodeHelper Default = new NodeHelper (Comparer<T>.Default);
			public static NodeHelper GetHelper (IComparer<T> cmp)
			{
				if (cmp == null || cmp == Comparer<T>.Default)
					return Default;
				return new NodeHelper (cmp);
			}
		}

		public struct Enumerator : IDisposable, IEnumerator, IEnumerator<T> {
			RBTree.NodeEnumerator host;

			internal Enumerator (TreeSet<T> tree)
			{
				host = new RBTree.NodeEnumerator (tree.tree);
			}

			void IEnumerator.Reset ()
			{
				host.Reset ();
			}

			public T Current {
				get { return ((Node) host.Current).value; }
			}

			object IEnumerator.Current {
				get { return Current; }
			}

			public bool MoveNext ()
			{
				return host.MoveNext ();
			}

			public void Dispose ()
			{
				host.Dispose ();
			}
		}

		RBTree tree;

		public TreeSet () : this (null)
		{
		}

		public TreeSet (IComparer<T> cmp)
		{
			tree = new RBTree (NodeHelper.GetHelper (cmp));
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		public Enumerator GetEnumerator ()
		{
			return new Enumerator (this);
		}

		// returns true if the value was inserted, false if the value already existed in the tree
		public bool Insert (T value)
		{
			RBTree.Node n = new Node (value);
			return tree.Intern (value, n) == n;
		}

		// returns true if the value was removed, false if the value didn't exist in the tree
		public bool Remove (T value)
		{
			return tree.Remove (value) != null;
		}

		public bool Contains (T value)
		{
			return tree.Lookup (value) != null;
		}

		public T this [int index] {
			get { return ((Node) tree [index]).value; }
		}

		public int Count {
			get { return (int) tree.Count; }
		}

		public void VerifyInvariants ()
		{
			tree.VerifyInvariants ();
		}

		public void Dump ()
		{
			tree.Dump ();
		}
	}
	
	class Test {
		static void Main (string [] args)
		{
			Random r = new Random ();
			Dictionary<int, int> d = new Dictionary<int, int> ();
			TreeSet<int> t = new TreeSet<int> ();
			int iters = args.Length == 0 ? 100000 : Int32.Parse (args [0]);
			int watermark = 1;

			for (int i = 0; i < iters; ++i) {
				if (i >= watermark) {
					watermark += 1 + watermark/4;
					t.VerifyInvariants ();
				}

				int n = r.Next ();
				if (d.ContainsKey (n))
					continue;
				d [n] = n;

				try {
					if (t.Contains (n))
						throw new Exception ("tree says it has a number it shouldn't");
					if (!t.Insert (n))
						throw new Exception ("tree says it has a number it shouldn't");
				} catch {
					Console.Error.WriteLine ("Exception while inserting {0} in iteration {1}", n, i);
					throw;
				}
			}
			t.VerifyInvariants ();
			if (d.Count != t.Count)
				throw new Exception ("tree count is wrong?");

			Console.WriteLine ("Tree has {0} elements", t.Count);

			foreach (int n in d.Keys)
				if (!t.Contains (n))
					throw new Exception ("tree says it doesn't have a number it should");

			Dictionary<int, int> d1 = new Dictionary<int, int> (d);

			int prev = -1;
			foreach (int n in t) {
				if (n < prev)
					throw new Exception ("iteration out of order");
				if (!d1.Remove (n))
					throw new Exception ("tree has a number it shouldn't");
				prev = n;
			}

			if (d1.Count != 0)
				throw new Exception ("tree has numbers it shouldn't");

			for (int i = 0; i < iters; ++i) {
				int n = r.Next ();
				if (!d.ContainsKey (n)) {
					if (t.Contains (n))
						throw new Exception ("tree says it doesn't have a number it should");
				} else if (!t.Contains (n)) {
					throw new Exception ("tree says it has a number it shouldn't");
				}
			}

			int count = t.Count;
			foreach (int n in d.Keys) {
				if (count <= watermark) {
					watermark -= watermark/4;
					t.VerifyInvariants ();
				}
				try {
					if (!t.Remove (n))
						throw new Exception ("tree says it doesn't have a number it should");
					--count;
					if (t.Count != count)
						throw new Exception ("Remove didn't remove exactly one element");
				} catch {
					Console.Error.WriteLine ("While trying to remove {0} from tree of size {1}", n, t.Count);
					t.Dump ();
					t.VerifyInvariants ();
					throw;
				}
				if (t.Contains (n))
					throw new Exception ("tree says it has a number it shouldn't");
			}
			t.VerifyInvariants ();

			if (t.Count != 0)
				throw new Exception ("tree claims to have elements");
		}
	}
}
#endif

