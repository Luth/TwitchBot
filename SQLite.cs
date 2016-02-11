using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace TwitchTV {
	class SQLite {
		private SQLiteConnection _sqldb;

		public void setup() {
			string sql;

			if ( !File.Exists( "mud.sqlite" ) )
				SQLiteConnection.CreateFile( "mud.sqlite" );

			_sqldb = new SQLiteConnection( "Data Source=mud.sqlite;Version=3;" );
			_sqldb.Open();

			//TODO: remove this line
			sql = "DROP TABLE IF EXISTS users";
			exec( sql );

			sql = "CREATE TABLE IF NOT EXISTS users (username UNIQUE, experience, level, hp, maxHp, money, zone, weapon, diedAt )";
			exec( sql );

			sql = "CREATE TABLE IF NOT EXISTS monsters ( username UNIQUE, name, hp, damage, money )";
			exec( sql );
		}

		public int exec( string sql ) {
			SQLiteCommand command = new SQLiteCommand( sql, _sqldb );
			try {
				return command.ExecuteNonQuery();
			} catch ( SQLiteException ex ) {
				Console.WriteLine( "SQL Exception: " + ex.ToString() );
			}
			return 0;
		}

		public SQLiteDataReader query( string sql ) {
			SQLiteCommand command = new SQLiteCommand( sql, _sqldb );
			SQLiteDataReader reader = command.ExecuteReader();
			return reader;
		}

		public TKey queryScalar<TKey>( string sql ) {
			SQLiteCommand command = new SQLiteCommand( sql, _sqldb );
			
			if ( typeof( TKey ) != typeof( int ) ) {
				return (TKey)command.ExecuteScalar();
			}

			object scalar = command.ExecuteScalar();
			if ( scalar == null ) {
				return (TKey)(object)scalar;
			}
			return (TKey)(object)Convert.ToInt32(scalar);
		}
	}
}
