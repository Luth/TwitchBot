using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace TwitchTV {
	class Monster {
		public Player player;
		public string name;
		public int hp;
		public int damage;
		public int money;

		public bool isDead() { return hp == 0; }

		public void attack() {
			string properName = char.ToUpper( name[0] ) + name.Substring( 1 );
			Program.sendWhisper( player, properName + " hits you [" + Math.Max( player.getHp() - damage, 0 ) + "/" + player.getMaxHp() + "] for " + damage + "." );
			player.damage( damage );
			if ( player.isDead() ) {
				Program.sendChannel( "@" + player.getUsername() + " was knocked out by " + name + "." );
			}
		}

		public void takeDamage( int amount ) {
			hp = Math.Max( hp - amount, 0 );
			Program.sendWhisper( player, "You hit " + name + " [" + hp + "] for " + amount + "." );
			if ( hp == 0 ) {
				kill();
			} else {
				string sql = "UPDATE monsters SET hp = " + hp + " WHERE username LIKE '" + player.getUsername() + "'";
				if ( Program.getSQLite().exec( sql ) == 0 ) {
					Console.WriteLine( "ERROR: Failed to execute sql: " + sql );
				}
			}
		}

		public void kill() {
			player.incExperience( 1 );
			player.earn(money);
			clear();
			Program.sendChannel( "@" + player.getUsername() + " has defeated " + name + "." );
			Program.sendWhisper( player, "You find $" + money + " on the corpse." );
		}

		public void clear() {
			Monsters.clearFor( player );
		}
	}

	class Monsters {
		static private SQLite _sqlite = Program.getSQLite();
		static private Random _rand = new Random();

		static public Monster createFor( Player player ) {
			Monster mon = new Monster();
			mon.player = player;
			mon.name = "a monster";
			mon.hp = 15;
			mon.damage = 1;
			mon.money = 4;

			for ( int i = 0; i < player.getLevel(); i++ ) {
				mon.hp += _rand.Next( 0, 6 );
				mon.damage += _rand.Next( 0, 2 );
				mon.money += _rand.Next( 0, 5 );
			}

			string sql = "REPLACE INTO monsters ( username, name, hp, damage, money ) "
				+ " VALUES ('" + player.getUsername() + "', '" + mon.name + "', " + mon.hp + ", " + mon.damage + ", " + mon.money + ")";
			_sqlite.exec( sql );

			return mon;
		}

		static public Monster loadFor( Player player ) {
			Monster mon = new Monster();
			mon.player = player;

			string sql = "SELECT * FROM monsters WHERE username LIKE '" + player.getUsername() + "'";
			SQLiteDataReader reader = _sqlite.query( sql );
			if ( !reader.Read() ) {
				return null;
			}

			mon.name = reader.GetString( reader.GetOrdinal( "name" ) );
			mon.hp = reader.GetInt32( reader.GetOrdinal( "hp" ) );
			mon.damage = reader.GetInt32( reader.GetOrdinal( "damage" ) );
			mon.money = reader.GetInt32( reader.GetOrdinal( "money" ) );

			return mon;
		}

		static public void clearFor( Player player ) {
			string sql = "DELETE FROM monsters WHERE username LIKE '" + player.getUsername() + "'";
			_sqlite.exec( sql );
		}
	}
}
