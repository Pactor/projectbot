using System;
using System.Collections.Specialized;
using System.Globalization;

namespace MetaBuilders.Irc.Messages
{

	/// <summary>
	/// The ErrorMessage received when a user tries to perform a command which requires
	/// channel-operator status.
	/// </summary>
	/// <remarks>
	/// Channel-operator status is set with the OperatorMode of the ChannelModeMessage.
	/// </remarks>
	[Serializable]
	public class ChannelOperatorStatusRequiredMessage : ErrorMessage, IChannelTargetedMessage
	{
		//:irc.dkom.at 482 artificer #NeboBot :You're not channel operator

		/// <summary>
		/// Creates a new instances of the <see cref="ChannelOperatorStatusRequiredMessage"/> class.
		/// </summary>
		public ChannelOperatorStatusRequiredMessage()
			: base()
		{
			this.InternalNumeric = 482;
		}

		/// <summary>
		/// Gets or sets the channel on which the command requires OperatorMode status
		/// </summary>
		public String Channel
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
		private String channel;


		/// <exclude />
		protected override void AddParametersToFormat( IrcMessageWriter writer )
		{
			base.AddParametersToFormat( writer );
			writer.AddParameter( this.Channel );
			writer.AddParameter( "You're not channel operator" );
		}

		/// <exclude />
		protected override void ParseParameters( StringCollection parameters )
		{
			base.ParseParameters( parameters );
			this.Channel = "";
			if ( parameters.Count > 1 )
			{
				this.Channel = parameters[ 1 ];
			}
		}

		/// <summary>
		/// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
		/// </summary>
		public override void Notify( MetaBuilders.Irc.Messages.MessageConduit conduit )
		{
			conduit.OnChannelOperatorStatusRequired( new IrcMessageEventArgs<ChannelOperatorStatusRequiredMessage>( this ) );
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
