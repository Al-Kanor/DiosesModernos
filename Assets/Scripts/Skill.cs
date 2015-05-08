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
            SkillManager sm = SkillManager.instance;
            switch (_id) {
                case "armure":
                    active.healthMax++;
                    active.health++;
                    break;
                case "bombMaster":
                    active.passive = Cyborg.Passive.MINE_INVULNERABLE;
                    active.passiveCountdown = 5;
                    GuiManager.instance.UpdatePassive (active);
                    break;
                case "direct" :
                    GuiManager.instance.Log (active.name + " launchs Direct !");
                    inactive.health -= 2;
                    break;
                case "earthquake":
                    GuiManager.instance.Log (active.name + " launchs Earthquake !");
                    grid.ResetRandomTile ("black");
                    break;
                case "eruption":
                    GuiManager.instance.Log (active.name + " launchs Eruption !");
                    inactive.health -= grid.NbTilesByColor ("red");
                    break;
                case "forestFire":
                    grid.RemoveUnits ("yellow");
                    break;
                case "mine":
                    Tile tile = grid.GetRandomTileByColor ("grey", true);
                    GuiManager.instance.Log (active.name + " places a mine under a random grey tile !");
                    //Debug.Log ("Mine on " + tile.x + ", " + tile.y);
                    Debug.Log (tile.gameObject.name);
                    tile.unit = ObjectPool.Spawn (sm.minePrefab, tile.transform, Vector2.zero);
                    tile.unit.name = "Mine";
                    tile.unit.SetActive (false);
                    break;
                case "reactivation":
                    inactive.health -= grid.NbTilesByUnit ("Mine");
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