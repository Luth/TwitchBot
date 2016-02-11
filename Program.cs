using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;


namespace TwitchTV {

	class Program {

		static SQLite _sqlite;
		static MainBot _mainBot;
		static WhisperBot _whisperBot;

		static void Main( string[] args ) {
			_sqlite = new SQLite();
			_sqlite.setup();

			Zones.init();
			Weapons.init();

			_mainBot = new MainBot( _sqlite );
			_whisperBot = new WhisperBot( _sqlite );

			while ( true ) {
				;
			}
		}

		static public SQLite getSQLite() { return _sqlite; }

		static public void sendChannel( string message ) {
			_mainBot.sendChannelMessage( message );
		}

		static public void sendWhisper( Player player, string message ) {
			_whisperBot.sendWhisper( player.getUsername(), message );
		}
	}
}
