using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class Player : Cyborg {
        #region Getters
        public bool clickEnabled {
            get { return _clickEnabled; }
            set { _clickEnabled = value; }
        }
        #endregion

        #region Unity
        void Update () {
            if (Input.GetMouseButtonDown (0) && _clickEnabled) {
                Vector2 clampPos = InputManager.MouseWorldPosition ();
                clampPos.x = Mathf.RoundToInt (clampPos.x);
                clampPos.y = Mathf.RoundToInt (clampPos.y);
                GameManager.instance.PlayerClick (clampPos);
            }
        }
        #endregion

        #region Private properties
        bool _clickEnabled = true;
        #endregion
    }
}