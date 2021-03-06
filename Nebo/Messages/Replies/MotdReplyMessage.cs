using System;
using System.Collections.Specialized;
using System.Text;

namespace MetaBuilders.Irc.Messages
{

	/// <summary>
	/// One line of data in a reply to the <see cref="MotdMessage"/> query.
	/// </summary>
	[Serializable]
	public class MotdReplyMessage : NumericMessage
	{

		/// <summary>
		/// Creates a new instance of the <see cref="MotdReplyMessage"/> class.
		/// </summary>
		public MotdReplyMessage()
			: base()
		{
			this.InternalNumeric = 372;
		}

		/// <summary>
		/// Gets or sets the text of the motd line.
		/// </summary>
		public virtual String Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}
		private String text = "";

		/// <summary>
		/// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
		/// </summary>
		protected override void AddParametersToFormat( IrcMessageWriter writer )
		{
			base.AddParametersToFormat( writer );
			writer.AddParameter( "- " + this.Text );
		}

		/// <summary>
		/// Parses the parameters portion of the message.
		/// </summary>
		protected override void ParseParameters( StringCollection parameters )
		{
			base.ParseParameters( parameters );
			String lastOne = parameters[ parameters.Count - 1 ];
			if ( lastOne.StartsWith( "- ", StringComparison.Ordinal ) )
			{
				this.Text = lastOne.Substring( 2 );
			}
		}

		/// <summary>
		/// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
		/// </summary>
		public override void Notify( MetaBuilders.Irc.Messages.MessageConduit conduit )
		{
			conduit.OnMotdReply( new IrcMessageEventArgs<MotdReplyMessage>( this ) );
		}

	}
}
