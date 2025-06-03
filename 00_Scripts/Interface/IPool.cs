using System.Collections.Generic;
using System;
using UnityEngine;

public interface IPool
{
    Transform parentTransform { get; set; }
    Queue<GameObject> pool { get; set; }
    GameObject Get(Action<GameObject> action = null);

    void Return(GameObject obj, Action<GameObject> action = null);
}
