using System;
using System.Collections.Specialized;
using System.Text;


namespace MetaBuilders.Irc.Messages
{

	/// <summary>
	/// The reply to a <see cref="SourceRequestMessage"/>, 
	/// telling the requestor where to download this client.
	/// </summary>
	[Serializable]
	public class SourceReplyMessage : CtcpReplyMessage
	{

		/// <summary>
		/// Creates a new instance of the <see cref="SourceReplyMessage"/> class.
		/// </summary>
		public SourceReplyMessage()
			: base()
		{
			this.InternalCommand = "SOURCE";
		}

		/// <summary>
		/// Gets or sets the server that hosts the client's distribution.
		/// </summary>
		public virtual String Server
		{
			get
			{
				return this.server;
			}
			set
			{
				this.server = value;
			}
		}
		private String server = "";

		/// <summary>
		/// Gets or sets the folder path to the client's distribution.
		/// </summary>
		public virtual String Folder
		{
			get
			{
				return this.folder;
			}
			set
			{
				this.folder = value;
			}
		}
		private String folder = "";

		/// <summary>
		/// Gets the list of files that must be downloaded.
		/// </summary>
		public virtual StringCollection Files
		{
			get
			{
				return this.files;
			}
		}
		private StringCollection files = new StringCollection();

		/// <summary>
		/// Gets the data payload of the Ctcp request.
		/// </summary>
		protected override String ExtendedData
		{
			get
			{
				StringBuilder result = new StringBuilder();
				result.Append( Server );
				result.Append( ":" );
				result.Append( Folder );
				if ( this.Files.Count > 0 )
				{
					result.Append( ":" );
					result.Append( MessageUtil.CreateList( Files, " " ) );
				}
				return result.ToString();
			}
		}

		/// <summary>
		/// Parses the given string to populate this <see cref="IrcMessage"/>.
		/// </summary>
		public override void Parse( String unparsedMessage )
		{
			base.Parse( unparsedMessage );
			String eData = CtcpUtil.GetExtendedData( unparsedMessage );
			String[] p = eData.Split( ':' );
			if ( p.Length > 0 )
			{
				this.Server = p[ 0 ];
				if ( p.Length > 1 )
				{
					this.Folder = p[ 1 ];
					if ( p.Length == 3 )
					{
						StringCollection fs = MessageUtil.GetParameters( p[ 2 ] );
						foreach ( String f in fs )
						{
							this.Files.Add( f );
						}
					}
				}
			}

		}

		/// <summary>
		/// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
		/// </summary>
		public override void Notify( MetaBuilders.Irc.Messages.MessageConduit conduit )
		{
			conduit.OnSourceReply( new IrcMessageEventArgs<SourceReplyMessage>( this ) );
		}

	}
}
