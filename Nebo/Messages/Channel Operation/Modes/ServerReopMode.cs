using System;

namespace MetaBuilders.Irc.Messages.Modes {

	/// <summary>
	/// This mode is only available on channels which name begins with the character '!' 
	/// and may only be toggled by the "channel creator". 
	/// </summary>
	public class ServerReopMode : FlagMode {

		/// <summary>
		/// Creates a new instance of the <see cref="ServerReopMode"/> class.
		/// </summary>
		public ServerReopMode() {
		}

		/// <summary>
		/// Creates a new instance of the <see cref="ServerReopMode"/> class with the given <see cref="ModeAction"/>.
		/// </summary>
		public ServerReopMode(ModeAction action) {
			this.Action = action;
		}

		/// <summary>
		/// Gets the irc string representation of the mode being changed or applied.
		/// </summary>
		protected override String Symbol {
			get {
				return "r";
			}
		}
	}
}
