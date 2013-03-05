using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilders.Irc.Messages;

namespace MetaBuilders.Irc.Contacts
{
	internal class ContactsMonitorTracker : ContactsTracker
	{
		public ContactsMonitorTracker( ContactList contacts )
			: base( contacts )
		{
		}

		public override void Initialize()
		{
			this.Contacts.Client.Messages.MonitoredUserOffline += new EventHandler<MetaBuilders.Irc.Messages.IrcMessageEventArgs<MetaBuilders.Irc.Messages.MonitoredUserOfflineMessage>>( client_MonitoredUserOffline );
			this.Contacts.Client.Messages.MonitoredUserOnline += new EventHandler<MetaBuilders.Irc.Messages.IrcMessageEventArgs<MetaBuilders.Irc.Messages.MonitoredUserOnlineMessage>>( client_MonitoredUserOnline );
			base.Initialize();
		}

		protected override void AddNicks( System.Collections.Specialized.StringCollection nicks )
		{
			MonitorAddUsersMessage add = new MonitorAddUsersMessage();
			foreach ( String nick in nicks )
			{
				add.Nicks.Add( nick );
			}
			this.Contacts.Client.Send( add );
		}
		
		protected override void AddNick( String nick )
		{
			MonitorAddUsersMessage add = new MonitorAddUsersMessage();
			add.Nicks.Add( nick );
			this.Contacts.Client.Send( add );
		}

		protected override void RemoveNick( String nick )
		{
			MonitorRemoveUsersMessage remove = new MonitorRemoveUsersMessage();
			remove.Nicks.Add( nick );
			this.Contacts.Client.Send( remove );
		}

		#region Reply Handlers

		void client_MonitoredUserOnline( object sender, MetaBuilders.Irc.Messages.IrcMessageEventArgs<MetaBuilders.Irc.Messages.MonitoredUserOnlineMessage> e )
		{
			foreach ( User onlineUser in e.Message.Users )
			{
				User knownUser = this.Contacts.Users.Find( onlineUser.Nick );
				if ( knownUser != null )
				{
					knownUser.MergeWith( onlineUser );
					if ( knownUser.OnlineStatus == UserOnlineStatus.Offline )
					{
						knownUser.OnlineStatus = UserOnlineStatus.Online;
					}
				}
			}
		}

		void client_MonitoredUserOffline( object sender, MetaBuilders.Irc.Messages.IrcMessageEventArgs<MetaBuilders.Irc.Messages.MonitoredUserOfflineMessage> e )
		{
			foreach ( String offlineNick in e.Message.Nicks )
			{
				User knownUser = this.Contacts.Users.Find( offlineNick );
				if ( knownUser != null )
				{
					knownUser.OnlineStatus = UserOnlineStatus.Offline;
				}
			}
		}

		#endregion

	}
}
