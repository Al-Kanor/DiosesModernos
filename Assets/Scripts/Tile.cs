using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class Tile : MonoBehaviour {
        
        public MeshRenderer modelRenderer;

        #region Getters
        public string color {
            get { return _color; }
            set { _color = value; }
        }

        public GameObject unit {
            get { return _unit; }
            set { _unit = value; }
        }

        public int x {
            get { return (int)transform.position.x; }
        }

        public int y {
            get { return (int)transform.position.y; }
        }
        #endregion

        #region Private properties
        string _color = "grey";
        GameObject _unit = null;
        #endregion
    }
}