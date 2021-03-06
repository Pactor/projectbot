using System;
using System.Collections.Specialized;
using System.Text;


namespace MetaBuilders.Irc.Messages
{

	/// <summary>
	/// The reply to a <see cref="ClientInfoRequestMessage"/>, giving the human-readable response to the request.
	/// </summary>
	[Serializable]
	public class ClientInfoReplyMessage : CtcpReplyMessage
	{

		/// <summary>
		/// Creates a new instances of the <see cref="ClientInfoReplyMessage"/> class.
		/// </summary>
		public ClientInfoReplyMessage()
			: base()
		{
			this.InternalCommand = "CLIENTINFO";
		}

		private String response = "";
		/// <summary>
		/// Gets or sets the response to the request's query.
		/// </summary>
		/// <remarks>
		/// This is only intended to be read by humans.
		/// It should be as complete and specific as the incoming request.
		/// </remarks>
		public virtual String Response
		{
			get
			{
				return this.response;
			}
			set
			{
				this.response = value;
			}
		}


		/// <summary>
		/// Gets the data payload of the Ctcp request.
		/// </summary>
		protected override String ExtendedData
		{
			get
			{
				return this.response;
			}
		}

		/// <summary>
		/// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
		/// </summary>
		public override void Notify( MetaBuilders.Irc.Messages.MessageConduit conduit )
		{
			conduit.OnClientInfoReply( new IrcMessageEventArgs<ClientInfoReplyMessage>( this ) );
		}

		/// <summary>
		/// Parses the given string to populate this <see cref="IrcMessage"/>.
		/// </summary>
		public override void Parse( String unparsedMessage )
		{
			base.Parse( unparsedMessage );
			this.Response = CtcpUtil.GetExtendedData( unparsedMessage );
		}
	}

}
