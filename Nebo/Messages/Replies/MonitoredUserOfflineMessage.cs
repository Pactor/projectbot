using System;
using System.Collections.Specialized;
using System.Text;
using System.Globalization;

namespace MetaBuilders.Irc.Messages
{

	/// <summary>
	/// A Monitor system notification that a monitored user is online
	/// </summary>
	[Serializable]
	public class MonitoredUserOfflineMessage : MonitoredNicksListMessage
	{

		/// <summary>
		/// Creates a new instance of the <see cref="MonitoredUserOfflineMessage"/>.
		/// </summary>
		public MonitoredUserOfflineMessage()
			: base()
		{
			this.InternalNumeric = 731;
		}

		/// <summary>
		/// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
		/// </summary>
		public override void Notify( MetaBuilders.Irc.Messages.MessageConduit conduit )
		{
			conduit.OnMonitoredUserOffline( new IrcMessageEventArgs<MonitoredUserOfflineMessage>( this ) );
		}

	}
}
