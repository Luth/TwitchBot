using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TwitchTV {
	class Weapon {
		public Weapon( string name, string keyword, int cost, int damage ) {
			_name = name;
			_keyword = keyword;
			_cost = cost;
			_damage = damage;
		}

		private string _name;
		private string _keyword;
		private int _cost;
		private int _damage;

		public string getName() { return _name;  }
		public string getKeyword() { return _keyword; }
		public int getCost() { return _cost; }
		public int getDamage() { return _damage; }
	}

	class Weapons {
		private static SQLite _sqlite = Program.getSQLite();
		private static List<Weapon> _weapons = new List<Weapon>();

		static public void init() {
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml( Resources.weapons );

			XmlNodeList xmlnode;
			xmlnode = xmlDoc.GetElementsByTagName( "weapon" );
			for ( int i = 0; i <= xmlnode.Count - 1; i++ ) {
				string name = xmlnode[i]["name"].InnerText;
				string keyword = xmlnode[i]["keyword"].InnerText;
				int cost = Int32.Parse(xmlnode[i]["cost"].InnerText);
				int damage = Int32.Parse(xmlnode[i]["damage"].InnerText);

				_weapons.Add( new Weapon( name, keyword, cost, damage ) );
			}
		}

		static public Weapon getWeapon( string weaponId ) {
			return _weapons.Find( x => x.getKeyword() == weaponId );
		}
	}

}
