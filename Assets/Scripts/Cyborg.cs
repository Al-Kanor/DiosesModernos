using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class Cyborg : MonoBehaviour {
        #region Public enums
        public enum Passive {
            NO_PASSIVE,
            BURN,
            ICED,
            INVULNERABLE,
            MINE_INVULNERABLE,
            PARALIZED,
            POISONED
        };
        #endregion

        #region Getters
        public Skill[] activeSkills {
            get { return _activeSkills; }
        }

        public int energy {
            get { return _energy; }
            set {
                _energy = Mathf.Clamp (value, 0, _energyMax);
                GuiManager.instance.UpdateEnergyBar (this);
            }
        }

        public int energyMax {
            get { return _energyMax; }
            set {
                _energyMax = Mathf.Max (1, value);
                GuiManager.instance.UpdateEnergyBar (this);
            }
        }

        public int health {
            get { return _health; }
            set {
                _health = Mathf.Clamp (value, 0, _healthMax);
                if (0 == _health) Application.LoadLevel (Application.loadedLevel);
                GuiManager.instance.UpdateHealthBar (this);
            }
        }

        public int healthMax {
            get { return _healthMax; }
            set {
                _healthMax = Mathf.Max (1, value);
                GuiManager.instance.UpdateHealthBar (this);
            }
        }

        public string id {
            get { return _id; }
            set { _id = value; }
        }

        public Passive passive {
            get { return _passive; }
            set { _passive = value; }
        }

        public int passiveCountdown {
            get { return _passiveCountdown; }
            set { _passiveCountdown = value; }
        }

        public ArrayList skills {
            get { return _skills; }
            set { _skills = value; }
        }
        #endregion

        #region API
        public void LoadActiveSkills () {
            _activeSkills = new Skill[3];
            do {
                for (int i = 0; i < _activeSkills.Length; ++i) {
                    _activeSkills[i] = (Skill)_skills[Random.Range (0, _skills.Count)];
                }
            } while (_activeSkills[0] == _activeSkills[1] || _activeSkills[0] == _activeSkills[2] || _activeSkills[1] == _activeSkills[2]);
        }

        public IEnumerator UseSkill (int index) {
            _activeSkills[index].Execute ();
            do {
                _activeSkills[index] = (Skill)_skills[Random.Range (0, _skills.Count)];
            } while (_activeSkills[0] == _activeSkills[1] || _activeSkills[0] == _activeSkills[2] || _activeSkills[1] == _activeSkills[2]);
            GuiManager.instance.UpdateActiveSkills ();
            yield return new WaitForSeconds (1);
            GameManager.instance.NewTurn ();
        }
        #endregion

        #region Unity

        #endregion

        #region Private properties
        string _id;
        int _health = 10;
        int _healthMax = 10;
        int _energy = 0;
        int _energyMax = 10;
        protected ArrayList _skills;
        Skill[] _activeSkills;
        Passive _passive = Passive.NO_PASSIVE;
        int _passiveCountdown;
        #endregion

        #region Private methods
        
        #endregion
    }
}