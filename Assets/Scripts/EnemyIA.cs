using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class EnemyIA : Cyborg {
        #region API
        public void PlayTurn () {
            for (int i = 0; i < activeSkills.Length; ++i) {
                if (energy >= activeSkills[i].cost) {
                    StartCoroutine (UseSkill (i));
                    return;
                }
            }
            Grid grid = GameManager.instance.grid;
            Tile tile = null;
            Vector2 pos = Vector2.zero;
            do {
                pos = new Vector2 (Random.Range ((int)(-grid.nbColumns / 2), (int)(grid.nbColumns / 2) + 1), Random.Range ((int)(-grid.nbLines / 2), (int)(grid.nbLines / 2) + 1));
                grid.tiles.TryGetValue (pos, out tile);
            } while (null == tile || "grey" != tile.color);
            GameManager.instance.EnemyIAClick (pos);
        }
        #endregion
    }
}