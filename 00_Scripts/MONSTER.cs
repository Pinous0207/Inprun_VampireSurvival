using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MONSTER : MonoBehaviour
{
    public float HP;
    public float MaxHP;

    public Transform target;

    public string monsterid;

    public bool isSpanwed = false;
    public bool isDead = false;
    public bool isStunned = false;

    private IFactory<MONSTER> factory;
    protected float speedMultiplier = 1.0f;
    protected float shockAmp = 0.0f;
    protected float skillCooldown = 5.0f;
    protected float skillTimer;
    protected Coroutine skillCoroutine;
    public MonsterSkill monsterSkill;
    public Animator animator;
    StatusEffect effect;

    public virtual void Initalize(Transform player, string monsterID)
    {
        if(effect == null)
        {
            effect = GetComponent<StatusEffect>();
        }
        MANAGER.SESSION.AddMonster();
        isSpanwed = false;
        monsterid = monsterID;
        HP = MANAGER.DB.levelDesign.GetHp(MANAGER.SESSION.GameTime, Boss(monsterid));
        MaxHP = HP;

        isDead = false;

        factory = new GenericPartFactory<MONSTER>(MANAGER.DB.Monster);
        target = player;
        factory.Build(this, monsterID);
    }

    protected bool Boss(string monsterID)
    {
        return monsterID.Split("_")[1] == "BOSS";
    }

    public void SetStunned(bool isStun)
    {
        isStunned = isStun;
        animator.speed = isStun ? 0.0f : 1.0f;
    }

    public void SetSpeedMultiplier(float value)
    {
        animator.speed = value;
        speedMultiplier = value;
    }

    public void SetShockAmp(float value)
    {
        shockAmp = value;
    }

    public void GetDamage(float dmg)
    {
        bool critical = MANAGER.SESSION.GetCritical();
        float criticalDmg = critical ? dmg + dmg * (MANAGER.SESSION.CriticalDamage / 100) : dmg;
        float realDmg = criticalDmg * (1 + shockAmp);
        HP -= realDmg;
        effect.GetHitEffect();
        var damageFont = MANAGER.POOL.Pooling_OBJ("DamageTMP").Get((value) =>
        {
            value.GetComponent<DamageTMP>().Initalize(
                Base_Canvas.instance.HOLDERLAYER,
                transform.position,
                ((int)realDmg).ToString(), Color.white,
                critical);
        });

        if (HP <= 0)
        {
            isDead = true;
            effect.Initalize();
            MANAGER.SESSION.RemoveMonster();

            var deadEffect = MANAGER.POOL.Pooling_OBJ("DeadEffect").Get((value) =>
            {
                value.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            });

            MANAGER.instance.Run(Util_Coroutine.Delay(0.5f,
                () => MANAGER.POOL.m_pool_Dictionary["DeadEffect"].Return(deadEffect)));

            MANAGER.POOL.m_pool_Dictionary["Monster"].Return(this.gameObject);

            int exp = Mathf.FloorToInt(Random.Range(MANAGER.DB.levelDesign.MinEXP, MANAGER.DB.levelDesign.MaxEXP + 1));
            DropEXP(transform.position, 
                Boss(monsterid) ? exp * 10 : exp);
        }
    }

    private void DropEXP(Vector3 deathPosition, float exp = 1.0f)
    {
        float[] units = { 9, 2, 1};

        foreach(float unit in units)
        {
            while(exp >= unit)
            {
                exp -= unit;

                OrbMake(deathPosition, unit);
            }
        }

        if(exp > 0.01f)
        {
            OrbMake(deathPosition, exp);
        }
    }

    private void OrbMake(Vector3 deathPosition, float exp)
    {
        Vector3 spawnPos = deathPosition + Utils_World.GetRandomCircleOffset(1.5f);
        spawnPos.y += 0.5f;
        var orb = MANAGER.POOL.Pooling_OBJ("Orb").Get((value) =>
        {
            value.transform.position = transform.position;
            value.GetComponent<Orb>().Initalize(exp, spawnPos);
        });
    }
}
