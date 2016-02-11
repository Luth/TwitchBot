using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TwitchTV {

	class MainBot {
		private IrcClient _irc;
		private SQLite _sqlite;

		public MainBot(SQLite sqlite) {
			_sqlite = sqlite;

			_irc = new IrcClient( "irc.twitch.tv", 6667, Credentials.login, Credentials.oauth );
			_irc.joinRoom( Credentials.login );
			_irc.sendIrcMessage( "CAP REQ :twitch.tv/membership" );
			_irc.sendIrcMessage( "CAP REQ :twitch.tv/commands" );
			_irc.sendIrcMessage( "CAP REQ :twitch.tv/tags" );

			ThreadStart ts = delegate { this.ListnerThread(); };
			Thread thread = new Thread( ts );
			thread.Start();
		}

		public void ListnerThread() {
			while ( true ) {
				MessageBundle msg = _irc.parseMessage();
				switch ( msg.type ) {
					case MessageBundle.Type.UNKNOWN:
						Console.WriteLine( "Unknown: " + msg.message );
						break;
					case MessageBundle.Type.JOIN:
						Console.WriteLine( "Joined: " + msg.message );
						break;
					case MessageBundle.Type.TEXT:
						if ( msg.message[0] == '!' ) {
							handleCommand( msg );
						}
						break;
				}
			}
		}

		private void handleCommand( MessageBundle msg ) {
			string[] parts = msg.message.Split(' ');
			string cmd = parts[0];
			string[] opts = null;
			if(parts.Length > 1) {
				opts = new string[parts.Length-1];
				Array.Copy(parts, 1, opts, 0, parts.Length-1);
			}
			cmd = cmd.ToLower();
			new PlayerCommand( msg.username ).chatCommand( cmd, opts );
		}

		public void sendChannelMessage( string message ) {
			_irc.sendChatMessage( message );
		}
	}
}
