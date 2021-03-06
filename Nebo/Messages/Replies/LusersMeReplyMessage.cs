using System;
using System.Collections.Specialized;
using System.Text;
using System.Globalization;

namespace MetaBuilders.Irc.Messages
{

	/// <summary>
	/// One of the responses to the <see cref="LusersMessage"/> query.
	/// </summary>
	[Serializable]
	public class LusersMeReplyMessage : NumericMessage
	{

		/// <summary>
		/// Creates a new instance of the <see cref="LusersMeReplyMessage"/> class.
		/// </summary>
		public LusersMeReplyMessage()
			: base()
		{
			this.InternalNumeric = 255;
		}

		/// <summary>
		/// Gets or sets the number of clients connected to the server.
		/// </summary>
		public virtual Int32 ClientCount
		{
			get
			{
				return clientCount;
			}
			set
			{
				clientCount = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of servers linked to the current server.
		/// </summary>
		public virtual Int32 ServerCount
		{
			get
			{
				return serverCount;
			}
			set
			{
				serverCount = value;
			}
		}

		private Int32 clientCount = -1;
		private Int32 serverCount = -1;
		private String iHave = "I have ";
		private String clientsAnd = " clients and ";
		private String servers = " servers";

		/// <summary>
		/// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
		/// </summary>
		protected override void AddParametersToFormat( IrcMessageWriter writer )
		{
			base.AddParametersToFormat( writer );
			writer.AddParameter( this.iHave + this.ClientCount + this.clientsAnd + this.serverCount + this.servers );
		}

		/// <summary>
		/// Parses the parameters portion of the message.
		/// </summary>
		protected override void ParseParameters( StringCollection parameters )
		{
			base.ParseParameters( parameters );
			String payload = parameters[ 1 ];
			this.ClientCount = Convert.ToInt32( MessageUtil.StringBetweenStrings( payload, this.iHave, this.clientsAnd ), CultureInfo.InvariantCulture );
			this.ServerCount = Convert.ToInt32( MessageUtil.StringBetweenStrings( payload, this.clientsAnd, this.servers ), CultureInfo.InvariantCulture );

		}

		/// <summary>
		/// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
		/// </summary>
		public override void Notify( MetaBuilders.Irc.Messages.MessageConduit conduit )
		{
			conduit.OnLusersMeReply( new IrcMessageEventArgs<LusersMeReplyMessage>( this ) );
		}

	}
}
