using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesign", menuName = "DB/LevelDesign")]
public class LevelDesign : ScriptableObject
{
    [Header("-----���� ���� ������----")]
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

    [Header("----���� ���� ���� �ֱ�----")]
    public float BossSpawnRate = 600.0f;

    [Header("----�÷��̾� ����ġ ȹ�淮----")]
    public float PlayerExpExponent = 1.5f;

    [Header("----���� ����ġ ��----")]
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
