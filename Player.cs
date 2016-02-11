using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace TwitchTV {
	class Player {
		private static SQLite _sqlite = Program.getSQLite();
		private static Random _rand = new Random();

		private string _username;
		private bool _isPlaying;
		private int _experience;
		private int _level;
		private int _hp;
		private int _maxHp;
		private int _money;
		private string _zone;
		private string _weapon;
		private int _diedAt;

		public Player( string username ) {
			_username = username;

			string sql = "SELECT * FROM users WHERE username LIKE '" + username + "'";
			SQLiteDataReader reader = _sqlite.query( sql );
			if ( !reader.Read() ) {
				return;
			}

			_isPlaying = true;
			_experience = reader.GetInt32( reader.GetOrdinal( "experience" ) );
			_level = reader.GetInt32( reader.GetOrdinal( "level" ) );
			_hp = reader.GetInt32( reader.GetOrdinal( "hp" ) );
			_maxHp = reader.GetInt32( reader.GetOrdinal( "maxHp" ) );
			_money = reader.GetInt32( reader.GetOrdinal( "money" ) );
			_zone = reader.GetString( reader.GetOrdinal( "zone" ) );
			_weapon = reader.GetString( reader.GetOrdinal( "weapon" ) );
			_diedAt = reader.GetInt32( reader.GetOrdinal( "diedAt" ) );
		}

		// ----- Getters --------------------

		public bool isPlaying() { return _isPlaying; }
		public bool isDead() { return _diedAt == DateTime.Now.DayOfYear; }
		public string getUsername() { return _username; }
		public int getExperience() { return _experience; }
		public int getExpForLevel() { return _level * 10; }
		public int getLevel() { return _level; }
		public int getHp() { return _hp; }
		public int getMaxHp() { return _maxHp; }
		public int getMoney() { return _money; }
		public string getZoneId() { return _zone; }
		public string getWeaponId() { return _weapon; }

		public int getDamage() {
			Weapon weapon = Weapons.getWeapon( _weapon );
			return weapon.getDamage();
		}


		// ----- Private Functions --------------------

		private void levelUp() {
			_level++;
			_maxHp += _rand.Next( 3, 6 );
			_hp = _maxHp;

			string sql = "UPDATE users SET level = " + _level + ", hp = " + _hp + ", maxHp = " + _maxHp + " WHERE username LIKE '" + _username + "'";
			if ( _sqlite.exec( sql ) == 0 ) {
				Console.WriteLine( "ERROR in sql: " + sql );
				return;
			}
		}

		// ----- Public Functions --------------------

		public void startNewGame() {
			string sql = "INSERT INTO users ( username, experience, level, hp, maxHp, money, zone, weapon, diedAt ) "
				+ " VALUES ( '" + _username + "' , 0 , 1 , 25 , 25 , 100 , 'tavern' , 'none', 0 )";
			if ( _sqlite.exec( sql ) == 0 ) {
				Console.WriteLine( "ERROR in sql: " + sql );
				return;
			}

			_isPlaying = true;
			_experience = 0;
			_level = 1;
			_hp = 25;
			_maxHp = 25;
			_money = 100;
			_zone = "tavern";
			_weapon = "none";
			_diedAt = 0;
		}

		public void changeZone( string zoneId ) {
			if ( !Zones.isValidZoneId( zoneId ) ) {
				return;
			}

			string sql = "UPDATE users SET zone = '" + zoneId + "' WHERE username LIKE '" + _username + "'";
			if ( _sqlite.exec( sql ) == 0 ) {
				Console.WriteLine( "ERROR in sql: " + sql );
				return;
			}

			_zone = zoneId;
		}

		public bool pay( int amount ) {
			string sql = "UPDATE users SET money = money - " + amount + " WHERE money >= " + amount + " AND username LIKE '" + _username + "'";
			if ( _sqlite.exec( sql ) == 0 ) {
				Console.WriteLine( "ERROR in sql: " + sql );
				return false;
			}
			_money -= amount;
			return true;
		}

		public void earn( int amount ) {
			string sql = "UPDATE users SET money = money + " + amount + " WHERE username LIKE '" + _username + "'";
			if ( _sqlite.exec( sql ) == 0 ) {
				Console.WriteLine( "ERROR in sql: " + sql );
				return;
			}
			_money += amount;
		}

		public int heal( int amount ) {
			if ( _hp == _maxHp ) {
				return 0;
			}

			int newHp = Math.Min( _hp + amount, _maxHp );
			string sql = "UPDATE users SET hp = " + newHp + " WHERE username LIKE '" + _username + "'";
			if ( _sqlite.exec( sql ) == 0 ) {
				Console.WriteLine( "ERROR in sql: " + sql );
				return 0;
			}

			int diff = newHp - _hp;
			_hp = newHp;

			return diff;
		}

		public void damage( int amount ) {
			int newHp = Math.Max( _hp - amount, 0 );

			if ( newHp == 0 ) {
				this.die();
				return;
			}

			string sql = "UPDATE users SET hp = " + newHp + " WHERE username LIKE '" + _username + "'";
			if ( _sqlite.exec( sql ) == 0 ) {
				Console.WriteLine( "ERROR in sql: " + sql );
				return;
			}

			_hp = newHp;
		}

		public void die() {
			Program.sendWhisper( this, "You were knocked unconscious. You'll be out the rest of the day. Better luck tomorrow." );
			string sql = "UPDATE users SET diedAt = " + DateTime.Now.DayOfYear + " WHERE username LIKE '" + _username + "'";
			if( _sqlite.exec(sql) == 0) {
				Console.WriteLine( "ERROR in sql: " + sql );
				return;
			}

			_diedAt = DateTime.Now.DayOfYear;
		}

		public void incExperience(int amount) {
			_experience += amount;
			while ( _experience >= getExpForLevel() ) {
				_experience -= getExpForLevel();
				levelUp();
			}

			string sql = "UPDATE users SET experience = " + _experience + " WHERE username LIKE '" + _username + "'";
			if ( _sqlite.exec( sql ) == 0 ) {
				Console.WriteLine( "ERROR in sql: " + sql );
				return;
			}
		}
	}
}
