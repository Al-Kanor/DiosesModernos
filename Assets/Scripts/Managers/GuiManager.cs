﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DiosesModernos {
    public class GuiManager : Singleton<GuiManager> {
        #region Properties
        [SerializeField]
        [Range (0, 10)]
        float _barsIncreaseSpeed = 1;
        [SerializeField]
        [Tooltip ("Log Text")]
        Text _logText;
        [SerializeField]
        Image _c1PanelImage;
        [SerializeField]
        Image _c2PanelImage;
        [SerializeField]
        Image _c1PassiveIcon;
        [SerializeField]
        Image _c2PassiveIcon;
        [SerializeField]
        Text _c1PassiveCountdown;
        [SerializeField]
        Text _c2PassiveCountdown;
        [SerializeField]
        Image _c1HealthBar;
        [SerializeField]
        Image _c2HealthBar;
        [SerializeField]
        Image _c1HealthFrame;
        [SerializeField]
        Image _c1EnergyBar;
        [SerializeField]
        Image _c2EnergyBar;
        [SerializeField]
        Image _c1EnergyFrame;
        [SerializeField]
        Button[] _c1Buttons;
        #endregion

        #region API
        public void DisableActiveSkills () {
            for (int i = 0; i < _c1Buttons.Length; ++i) {
                _c1Buttons[i].interactable = false;
            }
        }

        public void Log (string text) {
            _logText.text = text;
        }

        public void NewTurn () {
            Color tmpColor = _c1PanelImage.color;
            _c1PanelImage.color = _c2PanelImage.color;
            _c2PanelImage.color = tmpColor;
        }

        public void UpdateActiveSkills () {
            Player p = GameManager.instance.player;
            for (int i = 0; i < _c1Buttons.Length; ++i) {
                _c1Buttons[i].GetComponentInChildren<Text> ().text = p.activeSkills[i].name + " ("+ p.activeSkills[i].cost + ")";
                _c1Buttons[i].interactable = p.energy >= p.activeSkills[i].cost;
            }
        }

        public void UpdateEnergyBar (Cyborg c) {
            StartCoroutine (LerpEnergyBar (c));
        }

        public void UpdateHealthBar (Cyborg c) {
            StartCoroutine (LerpHealthBar (c));
        }

        public void UpdatePassive (Cyborg c) {
            Image passiveIcon = c == GameManager.instance.player ? _c1PassiveIcon : _c2PassiveIcon;
            if (Cyborg.Passive.NO_PASSIVE == c.passive || 0 == c.passiveCountdown) {
                passiveIcon.gameObject.SetActive (false);
            }
            else {
                passiveIcon.gameObject.SetActive (true);
                Text passiveCountdown = c == GameManager.instance.player ? _c1PassiveCountdown : _c2PassiveCountdown;
                passiveCountdown.text = c.passiveCountdown.ToString ();
            }
        }
        #endregion

        #region Private methods
        IEnumerator LerpEnergyBar (Cyborg c) {
            Image energyBar = c == GameManager.instance.player ? _c1EnergyBar : _c2EnergyBar;
            float ratio = _c1EnergyFrame.rectTransform.sizeDelta.x / c.energyMax;
            do {
                float w = c.energy * ratio;
                energyBar.rectTransform.anchoredPosition = Vector2.Lerp (
                    new Vector2 (energyBar.rectTransform.anchoredPosition.x, 0),
                    new Vector2 (w / 2, 0),
                    _barsIncreaseSpeed * Time.fixedDeltaTime
                );
                energyBar.rectTransform.sizeDelta = Vector2.Lerp (
                    new Vector2 (energyBar.rectTransform.sizeDelta.x, energyBar.rectTransform.sizeDelta.y),
                    new Vector2 (w, energyBar.rectTransform.sizeDelta.y),
                    _barsIncreaseSpeed * Time.fixedDeltaTime
                );
                yield return new WaitForFixedUpdate ();
                if (Mathf.Abs (w - energyBar.rectTransform.sizeDelta.x) < 0.1f) break;
            } while (true);
        }

        IEnumerator LerpHealthBar (Cyborg c) {
            Image healthBar = c == GameManager.instance.player ? _c1HealthBar : _c2HealthBar;
            float ratio = _c1HealthFrame.rectTransform.sizeDelta.x / c.healthMax;
            do {
                float w = c.health * ratio;
                healthBar.rectTransform.anchoredPosition = Vector2.Lerp (
                    new Vector2 (healthBar.rectTransform.anchoredPosition.x, 0),
                    new Vector2 (w / 2, 0),
                    _barsIncreaseSpeed * Time.fixedDeltaTime
                );
                healthBar.rectTransform.sizeDelta = Vector2.Lerp (
                    new Vector2 (healthBar.rectTransform.sizeDelta.x, healthBar.rectTransform.sizeDelta.y),
                    new Vector2 (w, healthBar.rectTransform.sizeDelta.y),
                    _barsIncreaseSpeed * Time.fixedDeltaTime
                );
                yield return new WaitForFixedUpdate ();
                if (Mathf.Abs (w - healthBar.rectTransform.sizeDelta.x) < 0.1f) break;
            } while (true);
        }
        #endregion
    }
}
