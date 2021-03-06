using System;
using System.Collections.Specialized;
using System.Text;


namespace MetaBuilders.Irc.Messages
{

	/// <summary>
	/// This is a message sent from a server to a client upon connection 
	/// to tell the client what irc features the server supports.
	/// </summary>
	[Serializable]
	public class SupportMessage : NumericMessage
	{

		/// <summary>
		/// Creates a new instance of the <see cref="SupportMessage"/> class.
		/// </summary>
		public SupportMessage()
			: base()
		{
			this.InternalNumeric = 005;
		}

		/// <summary>
		/// Gets the list of items supported by the server.
		/// </summary>
		public virtual NameValueCollection SupportedItems
		{
			get
			{
				return supportedItems;
			}
		}
		private NameValueCollection supportedItems = new NameValueCollection();

		/// <summary>
		/// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
		/// </summary>
		protected override void AddParametersToFormat( IrcMessageWriter writer )
		{
			base.AddParametersToFormat( writer );

			StringCollection paramsToString = new StringCollection();
			foreach ( String name in this.SupportedItems.Keys )
			{
				String value = this.SupportedItems[ name ];
				if ( value.Length != 0 )
				{
					paramsToString.Add( name + "=" + this.SupportedItems[ name ] );
				}
				else
				{
					paramsToString.Add( name );
				}
			}
			writer.AddList( paramsToString, " ", false );
			writer.AddParameter( areSupported );
		}

		private String areSupported = "are supported by this server";

		/// <summary>
		/// Parses the parameters portion of the message.
		/// </summary>
		protected override void ParseParameters( StringCollection parameters )
		{
			base.ParseParameters( parameters );
			for ( Int32 i = 1; i < parameters.Count - 1; i++ )
			{
				String nameValue = parameters[ i ];
				String name;
				String value;
				Int32 indexOfEquals = nameValue.IndexOf( "=", StringComparison.Ordinal );
				if ( indexOfEquals > 0 )
				{
					name = nameValue.Substring( 0, indexOfEquals );
					value = nameValue.Substring( indexOfEquals + 1 );
				}
				else
				{
					name = nameValue;
					value = "";
				}
				this.SupportedItems[ name ] = value;
			}
		}

		/// <summary>
		/// Determines if the message can be parsed by this type.
		/// </summary>
		public override Boolean CanParse( String unparsedMessage )
		{
			if ( unparsedMessage == null )
			{
				return false;
			}
			return ( base.CanParse( unparsedMessage ) && unparsedMessage.IndexOf( this.areSupported, StringComparison.Ordinal ) > 0 );
		}

		/// <summary>
		/// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
		/// </summary>
		public override void Notify( MetaBuilders.Irc.Messages.MessageConduit conduit )
		{
			conduit.OnSupport( new IrcMessageEventArgs<SupportMessage>( this ) );
		}

	}
}
