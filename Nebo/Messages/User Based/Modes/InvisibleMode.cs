using System;

namespace MetaBuilders.Irc.Messages.Modes {

	/// <summary>
	/// This mode signifies that the user is invisible to other users.
	/// </summary>
	public class InvisibleMode : UserMode {

		/// <summary>
		/// Creates a new instance of the <see cref="InvisibleMode"/> class.
		/// </summary>
		public InvisibleMode() {
		}

		/// <summary>
		/// Creates a new instance of the <see cref="InvisibleMode"/> class with the given <see cref="ModeAction"/>.
		/// </summary>
		public InvisibleMode(ModeAction action) {
			this.Action = action;
		}

		/// <summary>
		/// Gets the irc string representation of the mode being changed or applied.
		/// </summary>
		protected override String Symbol {
			get {
				return "i";
			}
		}

	}
}
