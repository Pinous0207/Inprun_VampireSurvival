using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Skill06_MultiSlash : SkillBase
{
    Vector3 scale;
    private int count;
    private float range;
    
    protected override void Fire()
    {
        StartCoroutine(SwordDelay());
    }

    IEnumerator SwordDelay()
    {
        bool isRight = true;
        for (int i = 0; i < count; i++)
        {
            var sword = MANAGER.POOL.Pooling_OBJ("Sword").Get((value) =>
            {
                value.transform.position = 
                Player.instance.transform.position 
                + Player.instance.transform.forward
                + new Vector3(0f, 1.5f, 0.0f);

                value.transform.rotation = 
                Quaternion.Euler(0, Player.instance.transform.eulerAngles.y -180.0f, 0);

                float xRotation = isRight ? Random.Range(-75.0f, -90.0f) : Random.Range(75, 90);
                isRight = !isRight;

                value.transform.GetChild(0).
                localRotation = Quaternion.Euler(xRotation, -90.0f, -90.0f);

                value.transform.localScale = scale;

                MANAGER.instance.Run(Util_Coroutine.Delay(1.0f, () =>
                {
                    MANAGER.POOL.m_pool_Dictionary["Sword"].Return(value);
                }));
            });
            var targets = targetLists(range);

            Vector3 origin = Player.instance.transform.position;
            Vector3 forward = Player.instance.transform.forward;
            List<MONSTER> monsterList = new List<MONSTER>();
            foreach(var target in targets)
            {
                if(target.TryGetComponent(out MONSTER m) && m.isSpanwed)
                {
                    Vector3 dirToTarget = (target.transform.position - origin).normalized;
                    float dot = Vector3.Dot(forward, dirToTarget);
                    float threshold = Mathf.Cos(45.0f * 0.5f * Mathf.Deg2Rad);

                    if(dot >= threshold)
                    {
                        monsterList.Add(m);
                    }
                }
            }

            for(int j = 0;j < monsterList.Count; j++)
            {
                if (monsterList[j].isDead == false)
                    monsterList[j].GetDamage(Damage(monsterList[j].GetComponent<StatusEffect>()));
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void OnInitalize()
    {
        count = 2 + 1 * (level - 1);
        range = 5.0f + 1.0f * (level - 1);
        float scaleFloat = 1.0f + 0.5f * (level - 1);
        scale = new Vector3(scaleFloat, scaleFloat, scaleFloat);   
    }

    protected override void OnLevelUp()
    {
        count = 2 + 1 * (level - 1);
        range = 5.0f + 1.0f * (level - 1);
        float scaleFloat = 1.0f + 0.5f * (level - 1);
        scale = new Vector3(scaleFloat, scaleFloat, scaleFloat);
    }
}
