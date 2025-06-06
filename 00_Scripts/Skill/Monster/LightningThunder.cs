using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class LightningThunder : MonsterSkill
{
    public int skillCount = 5;
    public float radiusAroundPlayer = 5.0f;
    public float delayCircles = 0.3f;

    public float explosionRange = 2.0f;
    public float damage = 10;
    public override IEnumerator CastSkill()
    {
        for(int i = 0; i < skillCount; i++)
        {
            Vector3 randomPos = monster.target.position + Utils_World.GetRandomCircleOffset(radiusAroundPlayer);
            var circle = MANAGER.POOL.Pooling_OBJ("EFFECT_CIRCLE").Get((value) =>
            {
                value.transform.position = randomPos;
            });
            var skill = circle.GetComponent<Monster_Effect_Circle>();
            skill.Initialize(monster.target, explosionRange, damage);

            yield return new WaitForSeconds(delayCircles);
        }

        yield return new WaitForSeconds(1.5f);
    }
}
