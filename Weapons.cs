using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TwitchTV {
	class Weapon {
		public Weapon( string name, string keyword, string stat, int damage ) {
			_name = name;
			_keyword = keyword;
			_stat = stat;
			_damage = damage;
		}

		private string _name;
		private string _keyword;
		private string _stat;
		private int _damage;

		public string getName() { return _name;  }
		public string getKeyword() { return _keyword; }
		public string getStat() { return _stat; }
		public int getDamage() { return _damage; }
	}

	class Weapons {
		private static SQLite _sqlite = Program.getSQLite();
		private static List<Weapon> _weapons = new List<Weapon>();

		static public void init() {
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml( Resources.WeaponAttr );

			XmlNodeList xmlnode;
			xmlnode = xmlDoc.GetElementsByTagName( "weapon" );
			for ( int i = 0; i <= xmlnode.Count - 1; i++ ) {
				string name = xmlnode[i]["name"].InnerText;
				string keyword = xmlnode[i]["keyword"].InnerText;
                /* changed cost to stat
                 * Allows for additional damage through stat scaling.
                 * Removed Cost as it could vary by vendor or method of obtainment (blacksmith vs shopkeep), so should be stored seperatly from weapon attributes.*/
                string stat = xmlnode[i]["stat"].InnerText; 
				int damage = Int32.Parse(xmlnode[i]["basedamage"].InnerText); //changed to basedamage to be distinct from total damage

				_weapons.Add( new Weapon( name, keyword, stat, damage ) );
			}
		}

		static public Weapon getWeapon( string weaponId ) {
			return _weapons.Find( x => x.getKeyword() == weaponId );
		}
	}

}
