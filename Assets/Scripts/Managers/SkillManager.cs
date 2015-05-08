using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class SkillManager : Singleton<SkillManager> {
        #region Properties
        [SerializeField]
        GameObject _minePrefab;
        #endregion

        #region Getters
        public GameObject minePrefab {
            get { return _minePrefab; }
        }
        #endregion

        #region API
        public void LoadSkills () {
            for (int i = 0; i < 2; ++i) {
                Cyborg c = 0 == i ? GameManager.instance.activeCyborg : GameManager.instance.nonActiveCyborg;
                c.skills = new ArrayList ();
                switch (c.id) {
                    case "bryven":
                        LoadBryvenSkills (c);
                        break;
                    case "zakk":
                        LoadZakkSkills (c);
                        break;
                }
                c.LoadActiveSkills ();
            }
            GuiManager.instance.UpdateActiveSkills ();
        }
        #endregion

        #region Private methods
        void LoadBryvenSkills (Cyborg c) {
            Skill skill;

            skill = new Skill ();
            skill.id = "forestFire";
            skill.name = "Feu de Forêt";
            skill.cost = 2;
            skill.description = "Détruit tout ce qui se trouve sur les cases vertes";
            c.skills.Add (skill);

            skill = new Skill ();
            skill.id = "armure";
            skill.name = "Armure";
            skill.cost = 2;
            skill.description = "Augmente les PV max de 1. Rend 1 PV";
            c.skills.Add (skill);

            skill = new Skill ();
            skill.id = "mine";
            skill.name = "Mine";
            skill.cost = 1;
            skill.description = "Place une mine sur une case grise aléatoire. La mine inflige 5 blessures à celui qui la révèle";
            c.skills.Add (skill);

            skill = new Skill ();
            skill.id = "bombMaster";
            skill.name = "Maître Bombardier";
            skill.cost = 3;
            skill.description = "Vous êtes insensible aux mines pendant 5 tours";
            c.skills.Add (skill);

            skill = new Skill ();
            skill.id = "reactivation";
            skill.name = "Réactivation";
            skill.cost = 4;
            skill.description = "Inflige 1 blessure par mine dévoilée";
            c.skills.Add (skill);
        }

        void LoadZakkSkills (Cyborg c) {
            Skill skill;

            skill = new Skill ();
            skill.id = "direct";
            skill.name = "Coup direct";
            skill.cost = 1;
            skill.description = "Inflige 1 blessure";
            c.skills.Add (skill);

            skill = new Skill ();
            skill.id = "earthquake";
            skill.name = "Tremblement de terre";
            skill.cost = 2;
            skill.description = "Détruit une case aléatoire";
            c.skills.Add (skill);

            skill = new Skill ();
            skill.id = "eruption";
            skill.name = "Éruption";
            skill.cost = 3;
            skill.description = "Inflige 1 blessure par cases rouges révélées";
            c.skills.Add (skill);
        }
        #endregion
    }
}