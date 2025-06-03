using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PartData
{
    public string id;
    public GameObject prefab;
}
[CreateAssetMenu(fileName = "Scriptable", menuName = "DB/Part", order = int.MaxValue)]
public class PartDB : ScriptableObject
{
    public List<PartData> parts;

    private Dictionary<string, GameObject> partMap;

    public GameObject Get(string id)
    {
        if(partMap == null)
        {
            partMap = parts.ToDictionary(p => p.id, p => p.prefab);
        }

        return partMap.ContainsKey(id) ? partMap[id] : null;
    }
}
