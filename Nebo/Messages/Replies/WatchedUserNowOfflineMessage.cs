using System;
using System.Collections.Specialized;
using System.Text;

namespace MetaBuilders.Irc.Messages
{

	/// <summary>
	/// A Watch system notification that a user is now offline.
	/// </summary>
	[Serializable]
	public class WatchedUserNowOfflineMessage : WatchedUserOfflineMessage
	{

		/// <summary>
		/// Creates a new instance of the <see cref="WatchedUserNowOfflineMessage"/>.
		/// </summary>
		public WatchedUserNowOfflineMessage()
			: base()
		{
			this.InternalNumeric = 600;
		}

		/// <exclude />
		protected override string ChangeMessage
		{
			get
			{
				return "logged offline";
			}
		}

	}
}
