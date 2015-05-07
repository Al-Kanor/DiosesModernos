using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class InputManager : Singleton<InputManager> {
        public static Vector2 MouseWorldPosition () {
            Vector2 pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint (pos);
            return pos;
        }
    }
}