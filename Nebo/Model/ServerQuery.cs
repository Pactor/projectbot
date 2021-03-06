﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using MetaBuilders.Irc.Network;
using System.Collections.Specialized;

namespace MetaBuilders.Irc
{

	/// <summary>
	/// Represents a status window for communication between the user and the server
	/// </summary>
	public class ServerQuery : INotifyPropertyChanged
	{

		#region CTor

		/// <summary>
		/// Creates a new instance of the <see cref="Query"/> class on the given client with the given User.
		/// </summary>
		public ServerQuery( Client client )
		{
			this.client = client;
			this.journal.CollectionChanged += new NotifyCollectionChangedEventHandler( journal_CollectionChanged );
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the journal of messages on the query
		/// </summary>
		public virtual Journal Journal
		{
			get
			{
				return journal;
			}
		}
		private Journal journal = new Journal();

		/// <summary>
		/// Gets the client which the status is on.
		/// </summary>
		public virtual Client Client
		{
			get
			{
				return client;
			}
		}
		private Client client;

		#endregion

		#region Event Handlers

		void journal_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			this.OnPropertyChanged( new PropertyChangedEventArgs( "Journal" ) );
		}

		#endregion

		#region INotifyPropertyChanged Members

		/// <summary>
		/// Raised when a property on the instance has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged( PropertyChangedEventArgs e )
		{
			if ( this.PropertyChanged != null )
			{
				this.PropertyChanged( this, e );
			}
		}

		private void NotifyPropertyChanged( String propertyName )
		{
			this.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
		}

		#endregion
	}
}
