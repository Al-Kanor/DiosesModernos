using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DiosesModernos {
    public class ScreenShake : Singleton<ScreenShake> {
        public Transform LocalCamera;
        public AnimationCurve ShakeIntensityCurve;
        public Vector2 ShakeVect;
        public float Duration;

        void Start () {
            if (Camera.main != null && Camera.main.transform != null)
                LocalCamera = Camera.main.transform;
        }

        public void ShakePredef () {
            ShakeInstant (ShakeVect, Duration);
        }

        public void ShakeInstant (Vector2 intensity, float duration = 1) {
            if (LocalCamera == null)
                if (Camera.main != null && Camera.main.transform != null)
                    LocalCamera = Camera.main.transform;

            if (LocalCamera != null) {
                StopAllCoroutines ();
                StartCoroutine (Shake (intensity, duration));
            }
            else
                Debug.LogError ("No Camera to shake!");
        }

        public IEnumerator Shake (Vector2 intensity, float duration = 1) {
            float elapsed = 0.0f;

            Vector3 originalCamPos = LocalCamera.transform.localPosition;

            while (elapsed < duration) {
                elapsed += Time.deltaTime;

                float percentComplete = elapsed / duration;
                float damper = ShakeIntensityCurve.Evaluate (percentComplete);

                float x = Random.value * 2.0f - 1.0f;
                float y = Random.value * 2.0f - 1.0f;

                x *= intensity.x * damper;
                y *= intensity.y * damper;

                LocalCamera.transform.localPosition = new Vector3 (originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

                yield return null;
            }

            LocalCamera.transform.localPosition = originalCamPos;
        }
    }
}