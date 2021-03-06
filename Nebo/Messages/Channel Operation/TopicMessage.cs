using System;
using System.Collections.Specialized;
using System.Text;

namespace MetaBuilders.Irc.Messages
{
	/// <summary>
	/// The <see cref="TopicMessage"/> is used to change or view the topic of a channel. 
	/// </summary>
	[Serializable]
	public class TopicMessage : CommandMessage, IChannelTargetedMessage
	{

		/// <summary>
		/// Creates a new instance of the <see cref="TopicMessage"/> class.
		/// </summary>
		public TopicMessage() : base()
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="TopicMessage"/> class for the given channel and topic.
		/// </summary>
		/// <param name="channel">The channel to affect.</param>
		/// <param name="topic">The new topic to set.</param>
		public TopicMessage( String channel, String topic )
			: base()
		{
			this.channel = channel;
			this.topic = topic;
		}

		/// <summary>
		/// Gets the Irc command associated with this message.
		/// </summary>
		protected override String Command
		{
			get
			{
				return "TOPIC";
			}
		}

		/// <summary>
		/// Gets or sets the channel affected
		/// </summary>
		public virtual String Channel
		{
			get
			{
				return channel;
			}
			set
			{
				channel = value;
			}
		}

		/// <summary>
		/// Gets or sets the new Topic to apply
		/// </summary>
		/// <remarks>
		/// If Topic is blank, the server will send a <see cref="TopicReplyMessage"/> and probably a <see cref="TopicSetReplyMessage"/>,
		/// telling you what the current topic is, who set it, and when.
		/// </remarks>
		public virtual String Topic
		{
			get
			{
				return topic;
			}
			set
			{
				topic = value;
			}
		}

		/// <summary>
		/// Validates this message against the given server support
		/// </summary>
		public override void Validate( ServerSupport serverSupport )
		{
			base.Validate( serverSupport );
			this.Channel = MessageUtil.EnsureValidChannelName( this.Channel, serverSupport );
		}

		/// <summary>
		/// Overrides <see cref="IrcMessage.AddParametersToFormat"/>.
		/// </summary>
		protected override void AddParametersToFormat( IrcMessageWriter writer )
		{
			base.AddParametersToFormat( writer );
			writer.AddParameter( this.Channel );
			if ( Topic != null && this.Topic.Length != 0 )
			{
				writer.AddParameter( this.Topic );
			}

		}

		/// <summary>
		/// Parse the parameters portion of the message.
		/// </summary>
		protected override void ParseParameters( StringCollection parameters )
		{
			base.ParseParameters( parameters );
			this.Channel = "";
			this.Topic = "";
			if ( parameters.Count >= 1 )
			{
				this.Channel = parameters[ 0 ];
				if ( parameters.Count >= 2 )
				{
					this.Topic = parameters[ 1 ];
				}
			}
		}

		/// <summary>
		/// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
		/// </summary>
		public override void Notify( MetaBuilders.Irc.Messages.MessageConduit conduit )
		{
			conduit.OnTopic( new IrcMessageEventArgs<TopicMessage>( this ) );
		}

		private String channel = "";
		private String topic = "";

		#region IChannelTargetedMessage Members

		bool IChannelTargetedMessage.IsTargetedAtChannel( string channelName )
		{
			return IsTargetedAtChannel( channelName );
		}

		/// <summary>
		/// Determines if the the current message is targeted at the given channel.
		/// </summary>
		protected virtual bool IsTargetedAtChannel( string channelName )
		{
			return MessageUtil.IsIgnoreCaseMatch( this.Channel, channelName );
		}

		#endregion
	}
}
