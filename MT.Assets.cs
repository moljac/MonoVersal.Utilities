using System;
using System.IO;

using MonoTouch;
using MonoTouch.Foundation;
using MonoTouch.AssetsLibrary;

namespace StreamHelper
{
	/// <summary>
	/// A readonly stream to access an Asset resource
	/// see http://msdn.microsoft.com/en-us/library/system.io.stream.aspx
	/// </summary>
	public class AssetLibraryReadStream : Stream
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Intranel.Mobile.Touch.AssetLibraryReadStream"/> class.
		/// </summary>
		public AssetLibraryReadStream(ALAsset asset, ALAssetsLibrary assetLibrary)
		{
			AssetRep = asset.DefaultRepresentation;
			Lib = assetLibrary;
			m_Position = 0;
		}

		/// <summary>
		/// The internal resource to be streamed
		/// </summary>
		protected ALAssetRepresentation AssetRep {set; get;}
		/// <summary>
		/// have to retain a refernce to the lib so that the assets stay valid
		/// </summary>
		protected ALAssetsLibrary Lib{set;get;}

		#region implemented abstract members of System.IO.Stream
		/// <summary>
		/// Nothing to flush
		/// </summary>
		public override void Flush()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Read into the specified buffer from offset, the count number of bytes.
		/// see http://msdn.microsoft.com/en-us/library/system.io.stream.read.aspx
		/// </summary>
		public override int Read(byte[] buffer, int offset, int count)
		{
			if(buffer == null) throw new ArgumentNullException("buffer");
			if(offset < 0) throw new ArgumentOutOfRangeException("offset", offset, "offset is negative");
			if(count < 0) throw new ArgumentOutOfRangeException("count", count, "count is negative");
			if(AssetRep == null) throw new ObjectDisposedException("AssetLibraryReadStream", "Can not call methods on the stream after it is closed");

			NSError error;
			int bytesRead;
			// Using a fixed pointer to stop buffer being moved by the GC during the copy
			unsafe
			{
				fixed (byte* pointer = buffer)
				{
					bytesRead = (int)AssetRep.GetBytes((IntPtr)(pointer + offset), (long)Position, (uint)count, out error);
				}
			}
			if (error != null)
			{
				throw new IOException(string.Format
				(
					"Error reading bytes from: {1}{0}offset:{2}{0}count:{3}{0}message:{4}",
					Environment.NewLine,
					AssetRep.Url.AbsoluteString,
					offset,
					count,
					error.LocalizedDescription
				));
			}

			Position += bytesRead;

			return bytesRead;
		}

		/// <summary>
		/// Seek the stream to the specified offset from the specified origin.
		/// If an exception is thrown due to trying to seek outside the range of the source then the position is unchanged.
		/// see http://msdn.microsoft.com/en-us/library/system.io.stream.seek.aspx
		/// </summary>
		public override long Seek(long offset, SeekOrigin origin)
		{
			if(AssetRep == null) throw new ObjectDisposedException("AssetLibraryReadStream", "Can not call methods on the stream after it is closed");
			switch (origin)
			{
			case SeekOrigin.Begin:
				Position = offset;
				break;
			case SeekOrigin.Current:
				Position += offset;
				break;
			case SeekOrigin.End:
				Position = AssetRep.Size - offset;
				break;
			}

			// Don't check that this is a valid position because MS say that seek can go out of bounds...
			// "Seeking to any location beyond the length of the stream is supported."
			return Position;
		}
		/// <summary>
		/// Can't set the length - readonly
		/// </summary>
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Can't write - Readonly
		/// </summary>
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets a value indicating whether this instance can read - which it can provided it is not closed.
		/// </summary>
		public override bool CanRead { get {return AssetRep != null; } }

		/// <summary>
		/// Gets a value indicating whether this instance can seek - which it can provided it is not closed.
		/// </summary>
		public override bool CanSeek { get {return AssetRep != null; } }

		/// <summary>
		/// Gets a value indicating whether this instance can write - which it definately can't;
		/// </summary>
		public override bool CanWrite {	get{return false;}}

		/// <summary>
		/// Gets the length of the stream
		/// </summary>
		public override long Length
		{
			get
			{
				if(AssetRep == null) throw new ObjectDisposedException("AssetLibraryReadStream", "Can not access the stream after it is closed");

				return AssetRep.Size;
			}
		}

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public override long Position
		{
			get
			{
				if(AssetRep == null) throw new ObjectDisposedException("AssetLibraryReadStream", "Can not access the stream after it is closed");

				return m_Position;
			}
			set
			{
				if(AssetRep == null) throw new ObjectDisposedException("AssetLibraryReadStream", "Can not access the stream after it is closed");

				m_Position = value;
			}
		}
		protected long m_Position;

		/// <summary>
		/// Close this stream and dispose the underlying source. If dispose is not explicitly called then the underlying source will stay allocated until the GC gets this object
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				AssetRep.Dispose();
				AssetRep = null;
				Lib.Dispose();
				Lib = null;
			}
		}
		#endregion
	}
}
