using UnityEngine;
using System.Collections.Generic;
public class Skill01_Lightning : SkillBase
{
    protected override void OnInitalize() { }

    protected override void OnLevelUp() { }

    protected override void Fire()
    {
        for (int i = 0; i < level; i++)
        {
            List<Transform> lists = targetLists(10.0f);
            if (lists.Count <= 0) return;
            Transform targetPoint = lists[Random.Range(0, lists.Count)];
            var lightning = MANAGER.POOL.Pooling_OBJ("Lightning").Get((value) =>
            {
                value.transform.position = targetPoint.position;
                targetPoint.GetComponent<MONSTER>().GetDamage(Damage(targetPoint.GetComponent<StatusEffect>()));
                value.GetComponent<ParticleSystem>().Play();
            });
        }
    }
}
