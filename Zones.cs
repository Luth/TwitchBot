using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace TwitchTV {
	class Zone {
		public Zone( string name, string keyword, bool canGo, List<string> commands ) {
			_name = name;
			_keyword = keyword;
			_canGo = canGo;
			_commands = commands;
		}
		private string _name;
		private string _keyword;
		private bool _canGo;
		private List<string> _commands;

		public string getName() { return _name; }
		public string getKeyword() { return _keyword; }
		public bool canGo() { return _canGo; }
		public List<string> getCommands() { return _commands; }
	}

	class Zones {
		private static SQLite _sqlite = Program.getSQLite();
		private static List<Zone> _zones = new List<Zone>();

		static public void init() {
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml( Resources.zones );

			XmlNodeList xmlnode;
			xmlnode = xmlDoc.GetElementsByTagName( "zone" );
			for ( int i = 0; i <= xmlnode.Count - 1; i++ ) {
				string name = xmlnode[i]["name"].InnerText;
				string keyword = xmlnode[i]["keyword"].InnerText;
				bool canGo = Convert.ToBoolean( xmlnode[i]["cango"].InnerText );
				XmlNode cmd = xmlnode[i]["command"];

				List<string> commands = new List<string>();
				if ( cmd != null ) {
					do {
						commands.Add( cmd.InnerText );
					} while ( (cmd = cmd.NextSibling) != null );
				}

				_zones.Add( new Zone( name, keyword, canGo, commands ) );
			}
		}

		static public bool isValidZoneId( string zoneId ) {
			return _zones.Find( x => x.getKeyword() == zoneId ) != null;
		}

		static public Zone getZone( string zoneId ) {
			return _zones.Find( x => x.getKeyword() == zoneId );
		}

		static public List<string> getZoneKeywords() {
			List<string> names = new List<string>();
			foreach ( Zone zone in _zones ) {
				if ( zone.canGo() ) {
					names.Add( zone.getKeyword() );
				}
			}
			return names;
		}

		static public void sendZoneList( Player player ) {
			List<string> keywords = getZoneKeywords();
			string message = "From here you can !go to the ";
			for ( int i = 0; i < keywords.Count; i++ ) {
				message += keywords[i];
				if ( i < keywords.Count - 1 ) {
					message += ", ";
				}
				if ( i == keywords.Count - 2 ) {
					message += "or ";
				}
			}
			message += ". (eg: !go forest)";
			Program.sendWhisper( player, message );
		}

		static public bool tryCommand( Player player, string zoneId, string command, string[] options = null ) {
			Zone zone = getZone( zoneId );
			if ( zone == null ) {
				return false;
			}

			switch ( zoneId ) {
				case "tavern":
					switch ( command ) {
						case "drink":
							cmd_tavern_drink( player );
							return true;
						case "info":
							cmd_tavern_info( player );
							return true;
						default:
							return false;
					}

				case "blacksmith":
					switch ( command ) {
						default:
							return false;
					}

				case "forest":
					switch ( command ) {
						case "adventure":
							cmd_forest_adventure( player );
							return true;
						default:
							return false;
					}

				case "forestfight":
					switch(command) {
						case "attack":
						case "a":
						case "run":
						case "r":
							Program.sendWhisper( player, "Fight commands go in the WHISPER window." );
							return true;
						default:
							return false;
					}

				default:
					return false;
			}
		}

		static public bool tryWhisperCommand( Player player, string zoneId, string command, string[] options = null ) {
			Zone zone = getZone( zoneId );
			if ( zone == null ) {
				return false;
			}

			switch ( zoneId ) {
				case "forestfight":
					switch ( command ) {
						case "attack":
						case "a":
							cmd_forest_attack( player );
							return true;
						case "run":
						case "r":
							cmd_forest_run( player );
							return true;
						default:
							return false;
					}

				default:
					return false;
			}
		}


		// ----- Tavern Commands --------------------

		static private void cmd_tavern_drink( Player player ) {
			if ( !player.pay( 5 ) ) {
				Program.sendChannel( "Hehe, @" + player.getUsername() + " can't afford $5 for an ale." );
				return;
			}

			var hp = player.heal( 5 );
			Program.sendChannel( "@" + player.getUsername() + " drinks his $5 ale. [+ " + hp + " hp]" );
		}

		static private void cmd_tavern_info( Player player ) {
			Program.sendWhisper( player, "\"Need a little help, eh?\" says the barman." );
			sendZoneList( player );
			Program.sendWhisper( player, "\"You can also ask for !help or check your !status at any time." );
		}


		// ----- Blacksmith Commands --------------------


		// ----- Forest Commands --------------------

		static private void cmd_forest_adventure( Player player ) {
			// Move player to fight room
			player.changeZone( "forestfight" );

			// crete a new fight instance
			Monster mon = Monsters.createFor( player );

			// Whisper fight status
			Program.sendWhisper( player, "You see " + mon.name + " [" + mon.hp + "]! You have " + player.getHp() + "/" + player.getMaxHp() + " hp." );
			Program.sendWhisper( player, "You can !attack (!a) or !run (!r) -- Please WHISPER fight commands, do not spam the main channel." );
		}

		static private void cmd_forest_attack( Player player ) {
			Monster mon = Monsters.loadFor( player );
			if ( mon == null ) {
				return;
			}

			mon.takeDamage( player.getDamage() );
			if ( mon.isDead() ) {
				// Move player to forest
				player.changeZone( "forest" );
			} else {
				mon.attack();
			}
		}

		static private void cmd_forest_run( Player player ) {
			Monster mon = Monsters.loadFor( player );
			if ( mon == null ) {
				Console.WriteLine( "ERROR: running from a fight where no monster exists" );
				// Move player to forest
				player.changeZone( "forest" );
				return;
			}

			// failure?
			if ( new Random().Next( 0, 3 ) == 0 ) {
				Program.sendWhisper( player, "You failed to run away!" );
				mon.attack();
				return;
			}

			// Move player to forest
			player.changeZone( "forest" );
			Program.sendWhisper( player, "You successfully run away." );
			mon.clear();
		}
	}
}
