using System;

namespace MetaBuilders.Irc.Messages.Modes {

	/// <summary>
	/// This mode sets or unsets a password on a channel.
	/// </summary>
 	public class KeyMode : FlagMode {

		/// <summary>
		/// Creates a new instance of the <see cref="KeyMode"/> class.
		/// </summary>
		public KeyMode() {
		}

		/// <summary>
		/// Creates a new instance of the <see cref="KeyMode"/> class with the given <see cref="ModeAction"/>.
		/// </summary>
		public KeyMode(ModeAction action) {
			this.Action = action;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="KeyMode"/> class 
		/// with the given <see cref="ModeAction"/> and password.
		/// </summary>
		public KeyMode(ModeAction action, String password) {
			this.Action = action;
			this.password = password;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="KeyMode"/> class 
		/// with the given password.
		/// </summary>
		public KeyMode(String password) {
			this.password = password;
		}

		/// <summary>
		/// Gets the irc string representation of the mode being changed or applied.
		/// </summary>
		protected override String Symbol {
			get {
				return "k";
			}
		}

		/// <summary>
		/// Gets or sets the password needed to gain access to a channel.
		/// </summary>
		public virtual String Password {
			get {
				return password;
			}
			set {
				password = value;
			}
		}
		private String password = "";

		/// <summary>
		/// Applies this mode to the ModeArguments property of the given <see cref="ChannelModeMessage" />.
		/// </summary>
		/// <param name="msg">The message which will be modified to include this mode.</param>
		protected override void AddParameter(MetaBuilders.Irc.Messages.ChannelModeMessage msg) {
			if ( this.Password.Length != 0 ) {
				msg.ModeArguments.Add(this.Password);
			}
		}
	}
}
