using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchTV {
	class PlayerCommand {
		private Player _player;

		public PlayerCommand( string username ) {
			_player = new Player( username );
		}

		public PlayerCommand( Player player ) {
			_player = player;
		}

		public void chatCommand( string command, string[] opts = null ) {
			if ( command[0] == '!' ) {
				command = command.Substring( 1 );
			}

			Zone zone;

			// Global Commands
			switch ( command ) {
				case "letsplay":
					if ( _player.isPlaying() ) {
						Program.sendChannel( "You are already playing, @" + _player.getUsername() + ". Try !help" );
					} else {
						_player.startNewGame();
						Program.sendChannel( "@" + _player.getUsername() + " enters the world." );
						new PlayerCommand( _player ).chatCommand( "!help" );
					}
					return;

				case "help":
					if ( _player.isPlaying() ) {
						zone = Zones.getZone( _player.getZoneId() );
						List<string> commands = zone.getCommands();
						string message = "You are in the " + zone.getName() + ". Your commands are: ";
						for ( int i = 0; i < commands.Count; i++ ) {
							message += "!" + commands[i];
							if ( i < commands.Count - 1 ) {
								message += ", ";
							}
						}
						Program.sendWhisper( _player, message );
						Zones.sendZoneList( _player );
					}
					return;

				case "status":
					if ( _player.isPlaying() ) {
						zone = Zones.getZone( _player.getZoneId() );
						Program.sendWhisper( _player, "Experience: " + _player.getExperience() + "/" + (_player.getExpForLevel()) + " Level: " + _player.getLevel() );
						Program.sendWhisper( _player, "HP: " + _player.getHp() + "/" + _player.getMaxHp() + " Money: $" + _player.getMoney() + " Zone: " + zone.getName() );
						if ( _player.isDead() ) {
							Program.sendWhisper( _player, "You have been knocked unconscious. You must rest until tomorrow." );
						}
					}
					return;

				case "go":
					string newZoneId = opts[0];
					if ( !Zones.isValidZoneId( newZoneId ) ) {
						Program.sendWhisper( _player, "And just where do you think you're going? " + newZoneId + "? Never heard of it." );
						return;
					}
					string curZoneId = _player.getZoneId();
					zone = Zones.getZone( curZoneId );
					if ( newZoneId == curZoneId ) {
						Program.sendChannel( "@" + _player.getUsername() + " tries to leave the " + zone.getName() + ", but realizes he is already there." );
						return;
					}

					// Can I leave here?
					if ( !zone.canGo() ) {
						Program.sendWhisper( _player, "You cannot leave this area right now." );
						return;
					}

					// Can I go to there?
					zone = Zones.getZone( newZoneId );
					if ( !zone.canGo() ) {
						return;
					}

					_player.changeZone( newZoneId );
					Program.sendChannel( "@" + _player.getUsername() + " travels to the " + zone.getName() );
					return;
			}

			// Zone commands
			if ( Zones.tryCommand( _player, _player.getZoneId(), command, opts ) )
				return;

			// other commands???
			Console.WriteLine( "Unknown command: " + _player.getUsername() + " " + command );
		}

		public void whisperCommand( string command, string[] opts = null ) {
			if ( command[0] == '!' ) {
				command = command.Substring( 1 );
			}

			// Global Commands

			// Zone commands
			if ( Zones.tryWhisperCommand( _player, _player.getZoneId(), command, opts ) )
				return;

			// other commands???
			Console.WriteLine( "Unknown whisper command: " + _player.getUsername() + " " + command );
		}

	}
}
