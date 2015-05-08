using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DiosesModernos {
    public class GameManager : Singleton<GameManager> {
        #region Properties
        [SerializeField]
        Grid _grid;
        [SerializeField]
        Player _player;
        [SerializeField]
        EnemyIA _enemyIA;
        #endregion

        #region Getters
        public Cyborg activeCyborg {
            get { return _activeCyborg; }
        }

        public Grid grid {
            get { return _grid; }
        }

        public bool isPlayerTurn {
            get { return _isPlayerTurn; }
        }

        public Cyborg nonActiveCyborg {
            get { return _nonActiveCyborg; }
        }

        public Player player {
            get { return _player; }
        }
        #endregion

        #region API
        // The current player attacks the other
        public void Attack (int damage) {
            _nonActiveCyborg.health -= damage;
            GuiManager.instance.Log ("c" + (_isPlayerTurn ? "2" : "1") + " (" + _nonActiveCyborg.health + " health)");
        }

        public void EnemyIAClick (Vector2 pos) {
            Tile tile = grid.RevealTile (pos);
            StartCoroutine (grid.MergeTilesCoroutine (tile));
        }

        public void Energize (int energy) {
            _activeCyborg.energy += energy;
            GuiManager.instance.Log ("c" + (_isPlayerTurn ? "1" : "2") + " (" + _activeCyborg.energy + " energy)");
        }

        public void Heal (int heal) {
            _activeCyborg.health += heal;
            GuiManager.instance.Log ("c" + (_isPlayerTurn ? "1" : "2") + " (" + _activeCyborg.health + " health)");
        }

        public void NewTurn () {
            _isPlayerTurn = !_isPlayerTurn;
            if (_isPlayerTurn) {
                _activeCyborg = _player;
                _nonActiveCyborg = _enemyIA;
                _player.clickEnabled = true;
                GuiManager.instance.UpdateActiveSkills ();
                _skillUsed = false;
            }
            else {
                GuiManager.instance.DisableActiveSkills ();
                _activeCyborg = _enemyIA;
                _nonActiveCyborg = _player;
                _enemyIA.PlayTurn ();
            }
            if (1 == _activeCyborg.passiveCountdown) _activeCyborg.passive = Cyborg.Passive.NO_PASSIVE;
            _activeCyborg.passiveCountdown = Mathf.Max (0, _activeCyborg.passiveCountdown - 1);
            GuiManager.instance.UpdatePassive (_activeCyborg);
            GuiManager.instance.NewTurn ();
        }

        public void PlayerClick (Vector2 pos) {
            Tile tile = grid.RevealTile (pos);
            if (null != tile) {
                _player.clickEnabled = false;
                StartCoroutine (grid.MergeTilesCoroutine (tile));
            }
        }

        public void UseSkill (int index) {
            if (!_skillUsed) {
                _skillUsed = true;
                GuiManager.instance.DisableActiveSkills ();
                _player.clickEnabled = false;
                StartCoroutine (player.UseSkill (index));
            }
        }
        #endregion

        #region Unity
        void Start () {
            _player.id = "bryven";
            _enemyIA.id = "zakk";
            _isPlayerTurn = true;
            if (_isPlayerTurn) {
                _activeCyborg = _player;
                _nonActiveCyborg = _enemyIA;
            }
            else {
                _activeCyborg = _enemyIA;
                _nonActiveCyborg = _player;
            }
            SkillManager.instance.LoadSkills ();
        }
        #endregion

        #region Private properties
        Cyborg _activeCyborg;
        Cyborg _nonActiveCyborg;
        bool _isPlayerTurn;
        bool _skillUsed = false;
        #endregion

        #region Private methods
        #endregion
    }
}