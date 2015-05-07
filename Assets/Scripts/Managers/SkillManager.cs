using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class SkillManager : Singleton<SkillManager> {
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