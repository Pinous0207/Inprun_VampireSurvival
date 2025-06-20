using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnRadius = 10.0f;
    public GameObject monsterPrefab;
    public Transform player;
    private float spawnInterval;

    public float timer;

    private void Start()
    {
        spawnInterval = MANAGER.DB.levelDesign.MonsterSpawnRate;
        MANAGER.SESSION.onBossTime += SpawnBossMonster;
    }

    private void OnDestroy()
    {
        MANAGER.SESSION.onBossTime -= SpawnBossMonster;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnInterval)
        {
            timer = 0f;
            SpawnMonsterAtEdge();
        }
    }

    void SpawnBossMonster()
    {
        SpawnMonsterAtEdge("Skeleton_BOSS");
    }

    void SpawnMonsterAtEdge(string id = "")
    {
        Vector3 spawnPos = GetRandomPointOnCircleEdge(player.position, spawnRadius);

        var monster = MANAGER.POOL.Pooling_OBJ("Monster").Get((value) =>
        {
            value.transform.position = spawnPos;
            value.GetComponent<MONSTER>().Initalize(player, string.IsNullOrEmpty(id) ? "Skeleton_01" : id);
        });
    }

    Vector3 GetRandomPointOnCircleEdge(Vector3 center, float radius)
    {
        float angle = Random.Range(0.0f, Mathf.PI * 2f);
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        return new Vector3(center.x + x, center.y, center.z + z);
    }
}
