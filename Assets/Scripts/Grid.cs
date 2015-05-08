using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DiosesModernos {
    public class Grid : MonoBehaviour {
        #region Properties
        [Header ("Configuration")]
        [SerializeField]
        [Tooltip ("Number of columns (must be odd)")]
        [Range (1, 12)]
        int _nbColumns = 7;
        [SerializeField]
        [Tooltip ("Number of lines (must be odd)")]
        [Range (1, 12)]
        int _nbLines = 7;
        
        [Header ("Links")]
        [SerializeField]
        GameObject _tilePrefab;

        [SerializeField]
        Material _greyTileMaterial;
        [SerializeField]
        Material _redTileMaterial;
        [SerializeField]
        Material _yellowTileMaterial;
        [SerializeField]
        Material _blueTileMaterial;
        [SerializeField]
        Material _orangeTileMaterial;
        [SerializeField]
        Material _purpleTileMaterial;
        [SerializeField]
        Material _greenTileMaterial;
        [SerializeField]
        Material _blackTileMaterial;
        [SerializeField]
        Material _whiteTileMaterial;
        #endregion

        #region Getters
        public int nbColumns {
            get { return _nbColumns; }
        }

        public int nbLines {
            get { return _nbLines; }
        }

        public Dictionary<Vector2, Tile> tiles {
            get { return _tiles; }
            set { _tiles = value; }
        }
        #endregion

        #region API
        public Tile GetRandomTileByColor (string color, bool mustBeEmpty = false) {
            if (0 == NbTilesByColor (color) || mustBeEmpty && 0 == NbEmptyTiles ("grey")) return null;
            Tile tile = null;
            Vector2 pos = Vector2.zero;
            do {
                pos = new Vector2 (Random.Range ((int)(-nbColumns / 2), (int)(nbColumns / 2) + 1), Random.Range ((int)(-nbLines / 2), (int)(nbLines / 2) + 1));
                tiles.TryGetValue (pos, out tile);
            } while (null == tile || color != tile.color || mustBeEmpty && null != tile.unit);
            return tile;
        }

        public IEnumerator MergeTilesCoroutine (Tile currentTile) {
            do {
                yield return new WaitForSeconds (1);
                MergeTiles (currentTile);
            } while (0 == NbTilesByColor ("grey"));
            GameManager.instance.NewTurn ();
        }

        // Return the number of tiles that have no unit on them. If a color is specified, only check the tiles of this color
        public int NbEmptyTiles (string color = "") {
            int n = 0;
            for (int x = -_nbColumns / 2; x <= _nbColumns / 2; ++x) {
                for (int y = -_nbLines / 2; y <= _nbLines / 2; ++y) {
                    Tile tile;
                    tiles.TryGetValue (new Vector2 (x, y), out tile);
                    if (null != tile && null == tile.unit && ("" == color || tile.color == color)) n++;
                }
            }
            return n;
        }

        public int NbTilesByColor (string color) {
            int n = 0;
            for (int x = -_nbColumns / 2; x <= _nbColumns / 2; ++x) {
                for (int y = -_nbLines / 2; y <= _nbLines / 2; ++y) {
                    Tile tile;
                    tiles.TryGetValue (new Vector2 (x, y), out tile);
                    if (null != tile && tile.color == color) n++;
                }
            }
            return n;
        }

        public int NbTilesByUnit (string unitName) {
            int n = 0;
            for (int x = -_nbColumns / 2; x <= _nbColumns / 2; ++x) {
                for (int y = -_nbLines / 2; y <= _nbLines / 2; ++y) {
                    Tile tile;
                    tiles.TryGetValue (new Vector2 (x, y), out tile);
                    if (null != tile && null != tile.unit && unitName == tile.unit.name) n++;
                }
            }
            return n;
        }

        // Remove all units on the tiles. If a color is specified, only remove units on tiles of the specified color
        public void RemoveUnits (string color = "") {
            for (int x = -_nbColumns / 2; x <= _nbColumns / 2; ++x) {
                for (int y = -_nbLines / 2; y <= _nbLines / 2; ++y) {
                    Tile tile;
                    tiles.TryGetValue (new Vector2 (x, y), out tile);
                    if (null != tile && null != tile.unit && ("" == color || color == tile.color)) {
                        tile.unit.Recycle ();
                        tile.unit = null;
                    }
                }
            }
        }

        public void ResetRandomTile (string color = "grey") {
            if (NbTilesByColor (color) == tiles.Count) return;
            Tile tile = null;
            Vector2 pos = Vector2.zero;
            do {
                pos = new Vector2 (Random.Range ((int)(-nbColumns / 2), (int)(nbColumns / 2) + 1), Random.Range ((int)(-nbLines / 2), (int)(nbLines / 2) + 1));
                tiles.TryGetValue (pos, out tile);
            } while (null == tile || color == tile.color);
            tile.color = color;
            switch (color) {
                case "grey":
                    tile.modelRenderer.material = _greyTileMaterial;
                    break;
                case "red":
                    tile.modelRenderer.material = _redTileMaterial;
                    break;
                case "yellow":
                    tile.modelRenderer.material = _yellowTileMaterial;
                    break;
                case "blue":
                    tile.modelRenderer.material = _blueTileMaterial;
                    break;
                case "orange":
                    tile.modelRenderer.material = _orangeTileMaterial;
                    break;
                case "purple":
                    tile.modelRenderer.material = _purpleTileMaterial;
                    break;
                case "green":
                    tile.modelRenderer.material = _greenTileMaterial;
                    break;
                case "white":
                    tile.modelRenderer.material = _whiteTileMaterial;
                    break;
                case "black":
                    tile.modelRenderer.material = _blackTileMaterial;
                    break;
            }
        }

        public Tile RevealTile (Vector2 pos) {
            Tile tile = null;
            _tiles.TryGetValue (pos, out tile);
            if (null != tile && "grey" == tile.color) {
                Material newMaterial = null;
                string color = null;
                int rnd = Random.Range (0, 3);
                // Hack/
                //rnd = 1;
                // /Hack
                switch (rnd) {
                    case 0:
                        newMaterial = _redTileMaterial;
                        color = "red";
                        tile.color = color;
                        GameManager.instance.Attack (NbAdjacentSameTiles (tile));
                        break;
                    case 1:
                        newMaterial = _yellowTileMaterial;
                        color = "yellow";
                        tile.color = color;
                        GameManager.instance.Energize (NbAdjacentSameTiles (tile));
                        break;
                    case 2:
                        newMaterial = _blueTileMaterial;
                        color = "blue";
                        tile.color = color;
                        GameManager.instance.Heal (NbAdjacentSameTiles (tile));
                        break;
                }
                tile.modelRenderer.material = newMaterial;
                // Skill : mine
                if (null != tile.unit && "Mine" == tile.unit.name) {
                    string log = "Mine revealed ! ";
                    tile.unit.SetActive (true);
                    Cyborg active = GameManager.instance.activeCyborg;
                    if (Cyborg.Passive.MINE_INVULNERABLE != active.passive) {
                        log += active.name + " take 5 damage !";
                        active.health -= 5;
                    }
                    else log += active.name + " is invulnerable to the mines !";
                    GuiManager.instance.Log (log);
                }
            }
            else tile = null;
            return tile;
        }
        #endregion

        #region Unity
        void Start () {
            // Create tiles
            _tiles = new Dictionary<Vector2, Tile> (_nbColumns * _nbLines);
            for (int c = -_nbColumns / 2; c <= _nbColumns / 2; ++c) {
                for (int l = -_nbLines / 2; l <= _nbLines / 2; ++l) {
                    Vector2 pos = new Vector2 (c, l);
                    GameObject tileObject = ObjectPool.Spawn (_tilePrefab, transform, pos);
                    Tile tile = tileObject.GetComponent<Tile> ();
                    //tile.transform.position = pos;
                    tileObject.transform.position = pos;
                    _tiles.Add (pos, tile);
                }
            }
        }
        #endregion

        #region Private properties
        Dictionary<Vector2, Tile> _tiles;
        //bool _isInteractive = true;
        #endregion

        #region Private methods
        void MergeTiles (Tile currentTile, string color = "") {
            if (0 == NbTilesByColor ("grey")) {
                for (int x = -_nbColumns / 2; x <= _nbColumns / 2; ++x) {
                    for (int y = -_nbLines / 2; y <= _nbLines / 2; ++y) {
                        Tile tile = null;
                        tiles.TryGetValue (new Vector2 (x, y), out tile);
                        tile.color = "grey";
                        tile.modelRenderer.material = _greyTileMaterial;
                        if (null != tile.unit) {
                            tile.unit.Recycle ();
                            tile.unit = null;
                        }
                    }
                }
                return;
            }
            if ("" == color) {
                // First tile
                string colorFound = "";
                for (int y = currentTile.y - 1; y <= currentTile.y + 1; ++y) {
                    for (int x = currentTile.x - 1; x <= currentTile.x + 1; ++x) {
                        // If the tile is the current tile or if it's in diagonale → continue
                        if (!(0 == x - currentTile.x ^ 0 == y - currentTile.y)) continue;
                        Tile tile = null;
                        tiles.TryGetValue (new Vector2 (x, y), out tile);
                        if (null == tile || currentTile.color == tile.color) continue;
                        if ("red" == currentTile.color && "yellow" == tile.color || "yellow" == currentTile.color && "red" == tile.color) {
                            if ("" == colorFound) {
                                color = "orange";
                                colorFound = tile.color;
                            }
                            else if (tile.color != colorFound) color = "white";
                        }
                        else if ("red" == currentTile.color && "blue" == tile.color || "blue" == currentTile.color && "red" == tile.color) {
                            if ("" == colorFound) {
                                color = "purple";
                                colorFound = tile.color;
                            }
                            else if (tile.color != colorFound) color = "white";
                        }
                        else if ("yellow" == currentTile.color && "blue" == tile.color || "blue" == currentTile.color && "yellow" == tile.color) {
                            if ("" == colorFound) {
                                color = "green";
                                colorFound = tile.color;
                            }
                            else if (tile.color != colorFound) color = "white";
                        }
                    }
                }
                if ("" == color) return;
            }

            currentTile.color = color;
            Material newColorMaterial;
            switch (color) {
                case "orange": newColorMaterial = _orangeTileMaterial; break;
                case "purple": newColorMaterial = _purpleTileMaterial; break;
                case "green": newColorMaterial = _greenTileMaterial; break;
                default: newColorMaterial = _whiteTileMaterial; break;
            }
            currentTile.modelRenderer.material = newColorMaterial;

            for (int y = currentTile.y - 1; y <= currentTile.y + 1; ++y) {
                for (int x = currentTile.x - 1; x <= currentTile.x + 1; ++x) {
                    // If the tile is the current tile or if it's in diagonale → continue
                    if (!(0 == x - currentTile.x ^ 0 == y - currentTile.y)) continue;
                    Tile tile = null;
                    tiles.TryGetValue (new Vector2 (x, y), out tile);
                    if (null == tile) continue;
                    if ("red" == tile.color || "yellow" == tile.color || "blue" == tile.color) {
                        MergeTiles (tile, color);
                    }
                }
            }
        }

        int NbAdjacentSameTiles (Tile currentTile, ArrayList visited = null) {
            if (null == visited) visited = new ArrayList ();
            int n = 1;
            visited.Add (currentTile);
            for (int y = currentTile.y - 1; y <= currentTile.y + 1; ++y) {
                for (int x = currentTile.x - 1; x <= currentTile.x + 1; ++x) {
                    // If the tile is the current tile or if it's in diagonale → continue
                    if (!(0 == x - currentTile.x ^ 0 == y - currentTile.y)) continue;
                    Tile tile = null;
                    tiles.TryGetValue (new Vector2 (x, y), out tile);
                    if (null == tile || visited.Contains (tile) || currentTile.color != tile.color) continue;
                    n += NbAdjacentSameTiles (tile, visited);
                }
            }
            return n;
        }
        #endregion
    }
}