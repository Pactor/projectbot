using System;
using System.Collections.Specialized;
using System.Text;
using System.Globalization;

namespace MetaBuilders.Irc.Messages
{
	/// <summary>
	/// The UserNotificationMessage is used at the beginning of connection to specify the username, hostname and realname of a new user.
	/// </summary>
	[Serializable]
	public class UserNotificationMessage : CommandMessage
	{

		/// <summary>
		/// Gets the Irc command associated with this message.
		/// </summary>
		protected override String Command
		{
			get
			{
				return "USER";
			}
		}

		/// <summary>
		/// Gets or sets the UserName of client.
		/// </summary>
		public virtual String UserName
		{
			get
			{
				return this.userName;
			}
			set
			{
				this.userName = value;
			}
		}
		private String userName = "";

		/// <summary>
		/// Gets or sets if the client is initialized with a user mode of invisible.
		/// </summary>
		public virtual Boolean InitialInvisibility
		{
			get
			{
				return initialInvisibility;
			}
			set
			{
				initialInvisibility = value;
			}
		}
		private Boolean initialInvisibility = true;

		/// <summary>
		/// Gets or sets if the client is initialized with a user mode of receiving wallops.
		/// </summary>
		public virtual Boolean InitialWallops
		{
			get
			{
				return initialWallops;
			}
			set
			{
				initialWallops = value;
			}
		}
		private Boolean initialWallops = false;

		/// <summary>
		/// Gets or sets the real name of the client.
		/// </summary>
		public virtual String RealName
		{
			get
			{
				return this.realName;
			}
			set
			{
				this.realName = value;
			}
		}
		private String realName = "";

		/// <exclude />
		public override bool CanParse( string unparsedMessage )
		{
			if ( !base.CanParse( unparsedMessage ) ) {
				return false;
			}			
			StringCollection p = MessageUtil.GetParameters( unparsedMessage );
			Int32 tempInt;
			if ( p.Count != 4 || !Int32.TryParse( p[ 1 ], out tempInt ) || p[ 2 ] != "*" )
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Overrides <see cref="IrcMessage.AddParametersToFormat"/>.
		/// </summary>
		protected override void AddParametersToFormat( IrcMessageWriter writer )
		{
			base.AddParametersToFormat( writer );
			writer.AddParameter( this.UserName );
			Int32 modeBitMask = 0;
			if ( this.InitialInvisibility )
				modeBitMask += 8;
			if ( this.InitialWallops )
				modeBitMask += 4;
			writer.AddParameter( modeBitMask.ToString( CultureInfo.InvariantCulture ) );
			writer.AddParameter( "*" );
			writer.AddParameter( this.RealName );
		}

		/// <summary>
		/// Parses the parameters portion of the message.
		/// </summary>
		protected override void ParseParameters( StringCollection parameters )
		{
			base.ParseParameters( parameters );
			if ( parameters.Count >= 4 )
			{
				this.UserName = parameters[ 0 ];
				this.RealName = parameters[ 3 ];
				Int32 modeBitMask = Convert.ToInt32( parameters[ 1 ], CultureInfo.InvariantCulture );
				this.InitialInvisibility = ( ( modeBitMask & 8 ) == 8 );
				this.InitialWallops = ( ( modeBitMask & 4 ) == 4 );
			}
			else
			{
				this.UserName = "";
				this.RealName = "";
				this.InitialInvisibility = true;
				this.InitialWallops = false;
			}
		}

		/// <summary>
		/// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
		/// </summary>
		public override void Notify( MetaBuilders.Irc.Messages.MessageConduit conduit )
		{
			conduit.OnUserNotification( new IrcMessageEventArgs<UserNotificationMessage>( this ) );
		}

	}
}
