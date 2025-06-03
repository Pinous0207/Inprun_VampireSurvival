using UnityEngine;

public class Skill03_Earthquake : SkillBase
{
    Vector3 particleRange;
    float targetRange;
    protected override void Fire()
    {
        var earthquake = MANAGER.POOL.Pooling_OBJ("Earthquake").Get((value) =>
        {
            value.transform.position = Player.instance.transform.position;
            value.transform.localScale = particleRange;

            var targets = targetLists(targetRange);
            foreach (var hit in targets)
            {
                hit.GetComponent<MONSTER>().GetDamage(Damage(hit.GetComponent<StatusEffect>()));
            }

            StartCoroutine(Util_Coroutine.Delay(1.5f, () =>
            {
                MANAGER.POOL.m_pool_Dictionary["Earthquake"].Return(value);
            }));
        });

    }

    protected override void OnInitalize()
    {
        float range = 1.5f + 0.3f * (level - 1);
        particleRange = new Vector3(range, range, range);
        targetRange = 5.0f + 1.0f * (level - 1);
    }

    protected override void OnLevelUp()
    {
        float range = 1.5f + 0.3f * (level - 1);
        particleRange = new Vector3(range, range, range);
        targetRange = 5.0f + 1.0f * (level - 1);

    }
}
