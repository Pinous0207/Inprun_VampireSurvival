using UnityEngine;

public class GenericPartFactory<T> : IFactory<T> where T : Component
{
    private PartDB db;

    public GenericPartFactory(PartDB database)
    {
        db = database;
    }

    public void Build(T entity, string id)
    {
        for(int i = 0; i < entity.transform.childCount; i++)
        {
            entity.transform.GetChild(i).gameObject.SetActive(false);
        }

        Transform existing = entity.transform.Find(id);
        if(existing != null)
        {
            existing.gameObject.SetActive(true);

            if(typeof(T) == typeof(MONSTER))
                existing.GetComponent<Monster_Renderer_Finder>().Initalize();

            return;
        }

        GameObject prefab = db.Get(id);

        if (prefab == null)
        {
            Debug.LogWarning("공장에 프리팹이 존재하지 않습니다.");
            return;
        }

        GameObject part = GameObject.Instantiate(prefab, entity.transform);

        if (typeof(T) == typeof(MONSTER))
            part.GetComponent<Monster_Renderer_Finder>().Initalize();

        part.name = id;
    }
}
