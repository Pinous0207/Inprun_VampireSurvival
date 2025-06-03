using UnityEngine;
using System.Collections.Generic;
using System;
public class Skill_Mng : MonoBehaviour
{
    private List<SkillBase> activeSkills = new List<SkillBase>();
    private PassiveMng PASSIVE;
    private void Start()
    {
        PASSIVE = GetComponent<PassiveMng>();
    }

    public void RegisterSkill(CardDB db, int level)
    {
        if (db.state == CardState.Active)
        {
            SkillBase existing = activeSkills.Find(x => x.skillid == db.id);
            if (existing != null)
            {
                existing.LevelUp(level);
                return;
            }

            SkillBase skill = CreateSkillFromDB(db);
            skill.Initalize(db, level);
            activeSkills.Add(skill);
        }
        else
        {
            PASSIVE.SetPassiveCard(db, level);
        }
    }

    SkillBase CreateSkillFromDB(CardDB db)
    {
        string scriptName = db.className;

        Type type = Type.GetType(scriptName);
        if(type == null || !type.IsSubclassOf(typeof(SkillBase)))
        {
            Debug.LogError($"[Skill_Mng] 잘못된 스킬 타입: {scriptName}");
            return null;
        }

        SkillBase skill = gameObject.AddComponent(type) as SkillBase;
        return skill;
    }

    public void ApplyStatus(Effect_Status status, StatusEffect effect, float stack = 0.0f)
    {
        switch(status)
        {
            case Effect_Status.None: break;
            case Effect_Status.Burn: effect.ApplyBurn(); break;
            case Effect_Status.Freeze: effect.ApplyFreeze(stack); break;
            case Effect_Status.Shock: effect.ApplyShock(); break;
            case Effect_Status.Stun: effect.ApplyStun(); break;
            case Effect_Status.Knockback: effect.ApplyKnockback(1.5f, 0.2f); break;
        }
    }

    private void Update()
    {
        foreach(var skill in activeSkills)
        {
            skill.Tick();
        }
    }
}

