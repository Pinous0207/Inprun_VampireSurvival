using System.Collections.Generic;
using UnityEngine;

public delegate void OnExpChanaged(float exp);
public delegate void OnMonsterCountChanged(int value);
public delegate void OnSelectedCard();
public delegate void OnHPChanged(float hp);
public delegate void OnBossTime();
public class Session_Mng : MonoBehaviour
{
    public OnExpChanaged onExpChanged;
    public OnMonsterCountChanged onMonsterCountChanged;
    public OnSelectedCard onSelectedCard;
    public OnHPChanged onHpChanged;
    public OnBossTime onBossTime;

    public List<Orb> Orbs = new List<Orb>();
    public Dictionary<string, SelectCard> SelectedCards = new Dictionary<string, SelectCard>();

    public int CurrentWave;
    public int Level;
    public int monsterCount;

    public float EXP;
    public float GameTime;
    private float BossTime = 10.0f;

    public bool isGameOver = false;

    public float baseMaxHP;
    public float baseDamage;
    private float hp = 50;
    
    [Space(20f)]
    [Header("## Player Data ##")]
    public float magnetRadius;
    public float HP
    {
        get => hp;
        set
        {
            if (value >= MaxHP)
                hp = MaxHP;
            else hp = value;

            onHpChanged?.Invoke(hp);
        }
    }
    public float Damage => baseDamage * (1f + DamagePercent / 100.0f);
    public float MaxHP => baseMaxHP * (1f + HPPercent / 100.0f);


    [Space(20f)]
    [Header("## Player Plus Data ##")]
    public float DamagePercent;
    public float HPPercent;
    public float magnetRadiusPercent;
    public float expPlusPercentage;
    public float CriticalPercentage;
    public float CriticalDamage;

    private void Start()
    {
        baseMaxHP = HP;
        Base_Canvas.instance.HPChanged(HP);
    }

    private void Update()
    {
        GameTime += Time.unscaledDeltaTime;
        if(GameTime >= BossTime)
        {
            Debug.Log("보스가 나타났습니다!");
            BossTime += 10.0f;
            onBossTime?.Invoke();
        }
    }

    public bool HaveCard(CardDB db)
    {
        return SelectedCards.ContainsKey(db.id);
    }

    public void SelectedCard(CardDB db)
    {
        if (HaveCard(db))
        {
            var data = SelectedCards[db.id];
            data.Level++;
        }
        else
        {
            var selected = new SelectCard();
            selected.db = db;
            selected.Level = 1;
            SelectedCards.Add(db.id, selected);
        }
        RegisterSkill(db);
    }

    public void RegisterSkill(CardDB db)
    {
        MANAGER.SKILL.RegisterSkill(db, SelectedCards[db.id].Level);
        onSelectedCard?.Invoke();
    }

    public void AddMonster()
    {
        monsterCount++;
        onMonsterCountChanged?.Invoke(monsterCount);
    }

    public void RemoveMonster()
    {
        monsterCount--;
        onMonsterCountChanged?.Invoke(monsterCount);
    }

    public void GetDamage(float dmg)
    {
        HP -= dmg;
    }

    public void AddExp(float exp)
    {
        float realExp = exp + exp * (expPlusPercentage / 100);
        EXP += exp;
        if(EXP >= GetRequiredExp())
        {
            EXP = 0;
            Level++;
            Base_Canvas.instance.SelectCard();
        }
        onExpChanged?.Invoke(EXP);
    }

    public int GetRequiredExp()
    {
        int level = Level + 1;
        if (level < 20)
            return (level * 10) - 5;
        else if (level == 20)
            return (level * 10) - 5 + 600;
        else if (level < 40)
            return (level * 13) - 6;
        else if (level == 40)
            return (level * 13) - 6 + 2400;
        else return (level * 16) - 8;
    }

    public bool GetCritical()
    {
        float RandomValue = Random.value * 100.0f;
        if (RandomValue <= CriticalPercentage)
            return true;
        else return false;
    }

    public void RefreshHpbyPercent(float oldMaxHP)
    {
        float ratio = HP / oldMaxHP;
        HP = MaxHP * ratio;

        onHpChanged?.Invoke(HP);
    }
}
