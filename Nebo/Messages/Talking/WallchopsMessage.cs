using System;
using System.Collections.Specialized;
using System.Text;

namespace MetaBuilders.Irc.Messages
{

	/// <summary>
	/// This message is sent to all channel operators.
	/// </summary>
	[Serializable]
	public class WallchopsMessage : CommandMessage, IChannelTargetedMessage
	{

		/// <summary>
		/// Gets or sets the text of the <see cref="WallchopsMessage"/>.
		/// </summary>
		public virtual String Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException( "value" );
				}
				this.text = value;
			}
		}
		private String text = "";

		/// <summary>
		/// Gets or sets the channel being targeted by the message.
		/// </summary>
		public virtual String Channel
		{
			get
			{
				return this.channel;
			}
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException( "value" );
				}
				this.channel = value;
			}
		}
		private String channel = "";

		/// <summary>
		/// Gets the Irc command associated with this message.
		/// </summary>
		protected override String Command
		{
			get
			{
				return "WALLCHOPS";
			}
		}

		/// <summary>
		/// Overrides <see cref="IrcMessage.AddParametersToFormat"/>.
		/// </summary>
		protected override void AddParametersToFormat( IrcMessageWriter writer )
		{
			base.AddParametersToFormat( writer );
			writer.AddParameter( this.Channel, false );
			writer.AddParameter( this.Text, true );
		}

		/// <summary>
		/// Parses the parameters portion of the message.
		/// </summary>
		protected override void ParseParameters( StringCollection parameters )
		{
			base.ParseParameters( parameters );
			this.Text = String.Empty;
			this.Channel = String.Empty;

			if ( parameters.Count >= 1 )
			{
				this.Channel = parameters[ 0 ];
				if ( parameters.Count >= 2 )
				{
					this.Text = parameters[ 1 ];
				}
			}
		}

		/// <summary>
		/// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
		/// </summary>
		public override void Notify( MetaBuilders.Irc.Messages.MessageConduit conduit )
		{
			conduit.OnWallchops( new IrcMessageEventArgs<WallchopsMessage>( this ) );
		}


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
			;
		}

		#endregion
	}
}
