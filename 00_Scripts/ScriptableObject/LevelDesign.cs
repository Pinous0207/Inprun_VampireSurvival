using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesign", menuName = "DB/LevelDesign")]
public class LevelDesign : ScriptableObject
{
    [Header("-----몬스터 레벨 디자인----")]
    [TextArea(0,3)]
    public string ExplaneMonsterLevelDesi;

    [Range(0, 20)]
    public float baseHP, bossHpMultiplier = 10.0f;
    [Range(0, 1.0f)]
    public float growhRate = 0.25f;
    [Range(0, 5.0f)]
    public float exponent = 2.0f;
    [Range(0, 10.0f)]
    public float MonsterSpawnRate;

    [Header("----보스 몬스터 생성 주기----")]
    public float BossSpawnRate = 600.0f;

    [Header("----플레이어 경험치 획득량----")]
    public float PlayerExpExponent = 1.5f;

    [Header("----몬스터 경험치 량----")]
    [Range(0, 5)]
    public float MinEXP;
    [Range(5, 15)]
    public float MaxEXP;

    public float GetHp(float gameTime, bool isBoss)
    {
        float minutes = gameTime / 60.0f;
        float finalHp = baseHP * (1f + growhRate * Mathf.Pow(minutes, exponent));
        return isBoss ? finalHp * bossHpMultiplier : finalHp;
    }
}
