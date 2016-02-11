using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TwitchTV {

	class WhisperBot {
		private IrcClient _irc;
		private SQLite _sqlite;

		public WhisperBot( SQLite sqlite ) {
			_sqlite = sqlite;

			_irc = new IrcClient( "199.9.253.119", 6667, Credentials.login, Credentials.oauth );
			_irc.sendIrcMessage( "CAP REQ :twitch.tv/commands" );

			ThreadStart ts = delegate { this.listnerThread(); };
			Thread thread = new Thread( ts );
			thread.Start();
		}

		public void listnerThread() {
			while ( true ) {
				MessageBundle msg = _irc.parseMessage();
				switch ( msg.type ) {
					case MessageBundle.Type.WHISPER:
						if ( msg.message[0] == '!' ) {
							handleCommand( msg );
						}
						break;
				}
			}
		}

		private void handleCommand( MessageBundle msg ) {
			string[] parts = msg.message.Split( ' ' );
			string cmd = parts[0];
			string[] opts = null;
			if ( parts.Length > 1 ) {
				opts = new string[parts.Length - 1];
				Array.Copy( parts, 1, opts, 0, parts.Length - 1 );
			}
			new PlayerCommand( msg.username ).whisperCommand( cmd, opts );
		}

		public void sendWhisper( string to, string message ) {
			_irc.sendIrcMessage( "PRIVMSG #jtv :/w " + to + " " + message );
		}
	}
}
