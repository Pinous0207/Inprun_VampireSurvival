using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

// Instantiate()'생성자' -> 내부적으로 메모리 할당, 컴포넌트 초기화, 씬 트리 갱신 등 비용 큼 
// Destroy() '파괴자' -> Unity는 실제로는 즉시 파괴하지 않고 GC가 처리함 -> GC 메모리 찌꺼기 유발
// 많은 생성/파괴 -> 프레임 드롭, GC Spikes, 성능 저하의 주요 원인
// 3D게임 -> 컴포넌트,메시,물리 충돌 등 더욱 많은 리소스를 사용하므로 부담 더 큼

// '인터페이스'란? - "이렇게 생긴 함수랑 변수는 꼭 있어야 해!"라고 약속해주는 틀


public class Object_Pool : IPool
{
    public Transform parentTransform { get ; set; }

    // Queue -> FIFO ( First In First Out ) -> 선입선출
    // Dequeue (먼저 들어온 오브젝트를 내보낸다)
    // Enqueue (오브젝트를 Queue내부에 집어 넣는다.)

    // Stack -> LIFO ( Last In First Out ) -> 후입선출
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();

    public GameObject Get(Action<GameObject> action = null)
    {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        if(action != null)
        {
            action?.Invoke(obj);
        }
        return obj;
    }

    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj);
        obj.transform.SetParent(parentTransform);
        obj.SetActive(false);
        if(action != null)
        {
            action?.Invoke(obj);
        }
    }
}

public class Pool_Mng : MonoBehaviour
{
    public Dictionary<string, IPool> m_pool_Dictionary = new Dictionary<string, IPool>();

    Transform base_Obj = null;

    private void Start()
    {
        base_Obj = this.transform;
    }

    public IPool Pooling_OBJ(string path)
    {
        if(m_pool_Dictionary.ContainsKey(path) == false)
        {
            Add_Pool(path);
        }
        if (m_pool_Dictionary[path].pool.Count <= 0) Add_Queue(path);
        return m_pool_Dictionary[path];
    }

    private GameObject Add_Pool(string path)
    {
        GameObject obj = new GameObject(path + "##POOL");
        obj.transform.SetParent(base_Obj);
        Object_Pool T_Pool = new Object_Pool();

        m_pool_Dictionary.Add(path, T_Pool);
        T_Pool.parentTransform = obj.transform;
        return obj;
    }

    private void Add_Queue(string path)
    {
        var obj = Instantiate(Resources.Load<GameObject>("POOL/" + path));
        m_pool_Dictionary[path].Return(obj);
    }
}
