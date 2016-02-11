using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace TwitchTV {
	class MessageBundle {
		public enum Type {
			UNKNOWN,
			EXCEPTION,
			JOIN,
			TEXT,
			WHISPER
		};

		public bool isMod = false;
		public string username = "";
		public string display = "";
		public string message = "";
		public Type type = Type.UNKNOWN;
	}

	class IrcClient {
		private TcpClient _tcpClient;
		private StreamReader _inputStream;
		private StreamWriter _outputStream;
		private string _username;
		private string _channel;

		public IrcClient( string ip, int port, string username, string password ) {
			_username = username;
			_tcpClient = new TcpClient( ip, port );
			_inputStream = new StreamReader( _tcpClient.GetStream() );
			_outputStream = new StreamWriter( _tcpClient.GetStream() );

			_outputStream.WriteLine( "PASS " + password );
			_outputStream.WriteLine( "NICK " + username );
			_outputStream.WriteLine( "USER " + username + " 8 * :" + username );
			_outputStream.Flush();
		}

		public void joinRoom( string channel ) {
			_channel = channel;
			_outputStream.WriteLine( "JOIN #" + channel );
			_outputStream.Flush();
		}

		public void sendIrcMessage( string message ) {
			_outputStream.WriteLine( message );
			_outputStream.Flush();
		}

		public void sendChatMessage( string message ) {
			sendIrcMessage( ":" + _username + "!" + _username + "@" + _username + ".tmi.twitch.tv PRIVMSG #" + _channel + " :" + message );
		}

		public MessageBundle parseMessage() {
			string message = _inputStream.ReadLine();
			/*
			 * ":tmi.twitch.tv 001 luthbot :Welcome, GLHF!"
			 * ":tmi.twitch.tv 002 luthbot :Your host is tmi.twitch.tv"
			 * ":luthbot!luthbot@luthbot.tmi.twitch.tv JOIN #luthbot"
			 * ":luthbot.tmi.twitch.tv 353 luthbot = #luthbot :luthbot"
			 * ":luthbot.tmi.twitch.tv 366 luthbot #luthbot :End of /NAMES list"
			 * ":tmi.twitch.tv CAP * ACK :twitch.tv/membership"
			 * ":luth_ac!luth_ac@luth_ac.tmi.twitch.tv JOIN #luthbot"
			 * ":jtv MODE #luthbot +o luthbot"
			 * "PING :tmi.twitch.tv"
			 * @color=;display-name=Luth_AC;emotes=;mod=1;room-id=115341176;subscriber=0;turbo=0;user-id=94328712;user-type=mod :luth_ac!luth_ac@luth_ac.tmi.twitch.tv PRIVMSG #luthbot :testing
			 * @color=;display-name=Luth_AC;emotes=;mod=1;room-id=115341176;subscriber=0;turbo=0;user-id=94328712;user-type=mod :luth_ac!luth_ac@luth_ac.tmi.twitch.tv PRIVMSG #luthbot :☺ACTION action☺
			 * @color=;display-name=Luth_AC;emotes=51838:0-6;mod=1;room-id=115341176;subscriber=0;turbo=0;user-id=94328712;user-type=mod :luth_ac!luth_ac@luth_ac.tmi.twitch.tv PRIVMSG #luthbot :ArgieB8
			 * :luth_ac!luth_ac@luth_ac.tmi.twitch.tv WHISPER luthbot :testing whisper
			*/

			MessageBundle msg = new MessageBundle();
			msg.message = message;

			try {
				// JOIN
				if ( message.Contains( " JOIN #" ) ) {
					msg.type = MessageBundle.Type.JOIN;

					string[] parts = message.Split( ' ' );
					/*
					 * ":luthbot!luthbot@luthbot.tmi.twitch.tv
					 * JOIN
					 * #luthbot"
					 */
					msg.message = parts[2];
				}
				// TEXT
				else if ( message.Contains( " PRIVMSG #" ) ) {
					//TODO: filter ACTION
					msg.type = MessageBundle.Type.TEXT;

					string[] parts = message.Split( ';' );
					/*
					 * @color=
					 * display-name=Luth_AC
					 * emotes=
					 * mod=1
					 * room-id=115341176
					 * subscriber=0
					 * turbo=0
					 * user-id=94328712
					 * user-type=mod :luth_ac!luth_ac@luth_ac.tmi.twitch.tv PRIVMSG #luthbot :testing
					 */
					for ( int i = 0; i < parts.Length; i++ ) {
						string[] kv = parts[i].Split( '=' );
						switch ( kv[0] ) {
							case "mod":
								int val;
								if ( Int32.TryParse( kv[1], out val ) )
									msg.isMod = val != 0;
								break;

							case "display-name":
								msg.display = kv[1];
								break;
						}
					}

					parts = message.Split( ':' );
					/*
					 * @color=;display-name=Luth_AC;emotes=;mod=1;room-id=115341176;subscriber=0;turbo=0;user-id=94328712;user-type=mod 
					 * luth_ac!luth_ac@luth_ac.tmi.twitch.tv PRIVMSG #luthbot 
					 * testing
					 */
					msg.message = parts[2];

					parts = parts[1].Split( '!' );
					/*
					 * luth_ac
					 * luth_ac@luth_ac.tmi.twitch.tv PRIVMSG #luthbot 
					 */
					msg.username = parts[0];
				}
				// WHISPER
				else if ( message.Contains( " WHISPER " + _username + " " ) ) {
					msg.type = MessageBundle.Type.WHISPER;
					
					string[] parts = message.Split( ':' );
					/*
					 * <blank>
					 * luth_ac!luth_ac@luth_ac.tmi.twitch.tv WHISPER luthbot
					 * testing whisper
					 */
					msg.message = parts[2];

					parts = parts[1].Split( '!' );
					msg.username = parts[0];
				}
				// UNKNOWN
				else {
					msg.message = message;
				}
			} catch ( Exception ex ) {
				Console.WriteLine( "EXCEPTION: " + ex.ToString() );
				msg = new MessageBundle();
				msg.type = MessageBundle.Type.EXCEPTION;
				msg.message = message;
			}

			return msg;
		}
	}
}
