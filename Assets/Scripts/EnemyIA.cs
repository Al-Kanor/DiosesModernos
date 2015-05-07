using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class EnemyIA : Cyborg {
        #region API
        public void PlayTurn () {
            Grid grid = GameManager.instance.grid;
            Tile tile = null;
            Vector2 pos = Vector2.zero;
            do {
                pos = new Vector2 (Random.Range ((int)(-grid.nbColumns / 2), (int)(grid.nbColumns / 2) + 1), Random.Range ((int)(-grid.nbLines / 2), (int)(grid.nbLines / 2) + 1));
                grid.tiles.TryGetValue (pos, out tile);
            } while (null == tile || "grey" != tile.color);
            GameManager.instance.EnemyIAClick (pos);

            /*for (int x = -grid.nbColumns / 2; x <= grid.nbColumns / 2; ++x) {
                for (int y = -grid.nbLines / 2; y <= grid.nbLines / 2; ++y) {
                    Vector2 pos = new Vector2 (x, y);
                    grid.tiles.TryGetValue (pos, out tile);
                    if (null != tile && "grey" == tile.color) {
                        GameManager.instance.EnemyIAClick (pos);
                        return;
                    }
                }
            }*/
        }
        #endregion
    }
}