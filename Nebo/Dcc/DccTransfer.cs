using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.IO;
using MetaBuilders.Irc.Messages;

namespace MetaBuilders.Irc.Dcc
{

	/// <summary>
	/// Handles the networks level communication protocols for sending and receiving files over dcc.
	/// </summary>
	
	public class DccTransfer
	{

		#region Properties
		/// <summary>
		/// Gets or sets a stream to the file being transfered.
		/// </summary>
		public FileStream File
		{
			get
			{
				return file;
			}
			set
			{
				file = value;
			}
		}

		/// <summary>
		/// Gets or sets the startposition in the file to transfer the information.
		/// </summary>
		public Int64 StartPosition
		{
			get
			{
				return startPosition;
			}
			set
			{
				startPosition = value;
			}
		}

		/// <summary>
		/// Gets or sets the socket the file transfer will use.
		/// </summary>
		public Socket TransferSocket
		{
			get
			{
				return transferSocket;
			}
			set
			{
				transferSocket = value;
			}
		}

		/// <summary>
		/// Gets or sets the size of the buffer for transfer of the file.
		/// </summary>
		public Int32 BufferSize
		{
			get
			{
				return bufferSize;
			}
			set
			{
				if ( value > 8192 )
				{
					throw new ArgumentException( NeboResources.BufferSizeIsLimited, "value" );
				}
				bufferSize = value;
			}
		}


		/// <summary>
		/// Gets or sets if the transfer uses the "turbo" extension in increase transfer speed.
		/// </summary>
		public Boolean TurboMode
		{
			get
			{
				return turboMode;
			}
			set
			{
				turboMode = value;
			}
		}

		/// <summary>
		/// Gets or sets if the transfer uses SSL to secure the transfer.
		/// </summary>
		public Boolean Secure
		{
			get
			{
				return secure;
			}
			set
			{
				secure = value;
			}
		}

		/// <summary>
		/// Gets or sets if the transfer uses the "send ahead" extension to increase transfer speed.
		/// </summary>
		public Boolean SendAhead
		{
			get
			{
				return sendAhead;
			}
			set
			{
				sendAhead = value;
			}
		}

		/// <summary>
		/// Gets the number of bytes transfered so far.
		/// </summary>
		public Int64 BytesTransferred
		{
			get
			{
				return bytesTransferred;
			}
		}

		/// <summary>
		/// Gets or sets the size of the file being transfered.
		/// </summary>
		public Int64 FileSize
		{
			get
			{
				return fileSize;
			}
			set
			{
				fileSize = value;
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// The TransferInterruption event occurs when the file has not completely transfered, but the connection has been stopped.
		/// </summary>
		public event EventHandler TransferInterruption;
		/// <summary>
		/// Raises the <see cref="TransferInterruption"/> event.
		/// </summary>
		protected void OnTransferInterruption( EventArgs e )
		{
			if ( TransferInterruption != null )
			{
				this.TransferInterruption( this, e );
			}
		}

		/// <summary>
		/// The TransferComplete event occurs when the file has been completely transfered.
		/// </summary>
		public event EventHandler TransferComplete;
		/// <summary>
		/// Raises the <see cref="TransferComplete"/> event.
		/// </summary>
		protected void OnTransferComplete( EventArgs e )
		{
			if ( TransferComplete != null )
			{
				this.TransferComplete( this, e );
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Sends the file over the current socket.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
		internal void Send()
		{
			if ( !this.File.CanRead )
			{
				throw new InvalidOperationException( NeboResources.CannotReadFromFile );
			}

			this.bytesTransferred = 0;

			this.buffer = new Byte[ this.BufferSize ];
			Byte[] acknowledgment = new Byte[ 4 ];
			Int32 bytesSent;


			while ( ( bytesSent = this.File.Read( buffer, 0, buffer.Length ) ) != 0 )
			{
				try
				{
					this.transferSocket.Send( buffer, bytesSent, SocketFlags.None );
					this.bytesTransferred += bytesSent;
					if ( !this.TurboMode && !this.SendAhead )
					{
						this.transferSocket.Receive( acknowledgment );
					}
				}
				catch
				{
					this.OnTransferInterruption( EventArgs.Empty );
				}
			}

			if ( !this.TurboMode )
			{
				while ( !this.AllAcknowledgmentsReceived( acknowledgment ) )
				{
					this.transferSocket.Receive( acknowledgment );
				}
			}
			this.OnTransferComplete( EventArgs.Empty );
		}

		/// <summary>
		/// Receives the file over the current socket.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
		internal void Receive()
		{
			this.bytesTransferred = 0;

			this.buffer = new Byte[ this.BufferSize ];
			Int32 bytesReceived;

			while ( !this.IsTransferComplete )
			{
				bytesReceived = this.transferSocket.Receive( buffer );
				if ( bytesReceived == 0 )
				{
					this.OnTransferInterruption( EventArgs.Empty );
					return;
				}
				this.bytesTransferred += bytesReceived;
				if ( this.File.CanWrite )
				{
					this.File.Write( buffer, 0, bytesReceived );
				}
				this.SendAcknowledgement();
			}
			this.File.Flush();
			this.OnTransferComplete( EventArgs.Empty );
		}

		#endregion

		#region Helpers
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
		private void SendAcknowledgement()
		{
			if ( !this.TurboMode )
			{
				//Convert BytesTransfered to a 4 byte array containing the number
				Byte[] bytesAck = DccBytesReceivedFormat();

				// Send it over the socket.
				this.transferSocket.Send( bytesAck );
			}
		}

		private Boolean AllAcknowledgmentsReceived( Byte[] lastAck )
		{
			Int64 acknowledgedBytes = DccBytesToLong( lastAck );
			return acknowledgedBytes >= this.BytesTransferred;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
		private Boolean IsTransferComplete
		{
			get
			{
				if ( fileSize == -1 )
				{
					return false;
				}
				return startPosition + bytesTransferred >= fileSize;
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
		private byte[] DccBytesReceivedFormat()
		{
			byte[] size = new byte[4];
			byte[] longBytes = BitConverter.GetBytes( NetworkUnsignedLong( this.BytesTransferred ) );
			Array.Copy( longBytes, 0, size, 0, 4 );
			return size;
		}

		private static long DccBytesToLong( byte[] received )
		{
			return IPAddress.NetworkToHostOrder( BitConverter.ToInt32( received, 0 ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
		private static long NetworkUnsignedLong( long hostOrderLong )
		{
			long networkLong = IPAddress.HostToNetworkOrder( hostOrderLong );
			return ( networkLong >> 32 ) & 0x00000000ffffffff;
		}

		#endregion


		#region Private Members

		private Byte[] buffer;

		private FileStream file;
		private Int64 startPosition = 0;
		private Socket transferSocket;
		private Int32 bufferSize = 4096;
		private Boolean turboMode = false;
		private Boolean secure = false;
		private Boolean sendAhead = true;
		private Int64 bytesTransferred = 0;
		private Int64 fileSize = -1;

		#endregion

	}
}
