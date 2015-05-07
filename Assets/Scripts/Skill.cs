using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class Skill {
        #region Getters
        public int cost {
            get { return _cost; }
            set { _cost = value; }
        }

        public string description {
            get { return _description; }
            set { _description = value; }
        }

        public string id {
            get { return _id; }
            set { _id = value; }
        }

        public string name {
            get { return _name; }
            set { _name = value; }
        }
        #endregion

        #region API
        public void Execute () {
            Cyborg active = GameManager.instance.activeCyborg;
            Cyborg inactive = GameManager.instance.nonActiveCyborg;
            Grid grid = GameManager.instance.grid;
            switch (_id) {
                case "direct" :
                    inactive.health -= 2;
                    break;
                case "earthquake":
                    grid.ResetRandomTile ("black");
                    break;
                case "eruption":
                    inactive.health -= grid.NbTilesByColor ("red");
                    break;
            }
            active.energy -= _cost;
        }
        #endregion

        #region Private properties
        string _id;
        string _name;
        int _cost;
        string _description;
        #endregion
    }
}