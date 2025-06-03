using UnityEngine;

public class Skill04_FrostField : SkillBase
{
    Vector3 particleRange;
    float targetRange;
    private GameObject FrostFieldParticle;
    protected override void Fire()
    {
        var targets = targetLists(targetRange);
        foreach (var hit in targets)
        {
            hit.GetComponent<MONSTER>().GetDamage(Damage(hit.GetComponent<StatusEffect>()));
        }
    }

    protected override void OnInitalize()
    {
        float range = 1.2f + 0.3f* (level - 1);
        particleRange = new Vector3(range, range, range);
        targetRange = 5.0f + 1.0f * (level - 1);

        FrostFieldParticle = MANAGER.POOL.Pooling_OBJ("FrostField").Get((value) =>
        {
            value.transform.localScale = particleRange;
        });
    }

    protected override void OnLevelUp()
    {
        float range = 1.2f + 0.3f * (level - 1);
        particleRange = new Vector3(range, range, range);
        
        if(FrostFieldParticle != null)
            FrostFieldParticle.transform.localScale = particleRange;
        
        targetRange = 5.0f + 1.0f * (level - 1);
    }
}
