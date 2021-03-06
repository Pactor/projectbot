using System;
using System.Collections.Specialized;
using System.Text;
using System.Globalization;

namespace MetaBuilders.Irc.Messages
{

	/// <summary>
	/// A Watch system notification that a watched user's status has changed
	/// </summary>
	[Serializable]
	public abstract class WatchedUserChangedMessage : NumericMessage
	{

		/// <summary>
		/// Gets or sets the watched User who's status has changed.
		/// </summary>
		public User WatchedUser
		{
			get
			{
				if ( watchedUser == null )
				{
					watchedUser = new User();
				}
				return watchedUser;
			}
			set
			{
				this.watchedUser = value;
			}
		}
		private User watchedUser;

		/// <summary>
		/// Gets or sets the time at which the change occured.
		/// </summary>
		public DateTime TimeOfChange
		{
			get
			{
				return changeTime;
			}
			set
			{
				changeTime = value;
			}
		}
		private DateTime changeTime;

		/// <exclude />
		protected abstract String ChangeMessage
		{
			get;
		}


		/// <summary>
		/// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
		/// </summary>
		protected override void AddParametersToFormat( IrcMessageWriter writer )
		{
			base.AddParametersToFormat( writer );
			writer.AddParameter( this.WatchedUser.Nick );
			writer.AddParameter( this.WatchedUser.UserName );
			writer.AddParameter( this.WatchedUser.HostName );
			writer.AddParameter( MessageUtil.ConvertToUnixTime( this.TimeOfChange ).ToString( CultureInfo.InvariantCulture ) );
			writer.AddParameter( this.ChangeMessage );
		}

		/// <summary>
		/// Parses the parameters portion of the message.
		/// </summary>
		protected override void ParseParameters( StringCollection parameters )
		{
			base.ParseParameters( parameters );

			this.WatchedUser = new User();
			this.TimeOfChange = DateTime.MinValue;

			if ( parameters.Count == 6 )
			{
				this.WatchedUser.Nick = parameters[ 1 ];
				this.WatchedUser.UserName = parameters[ 2 ];
				this.WatchedUser.HostName = parameters[ 3 ];
				this.TimeOfChange = MessageUtil.ConvertFromUnixTime( Convert.ToInt32( parameters[ 4 ], CultureInfo.InvariantCulture ) );
			}
		}

	}
}
