using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

// Instantiate()'������' -> ���������� �޸� �Ҵ�, ������Ʈ �ʱ�ȭ, �� Ʈ�� ���� �� ��� ŭ 
// Destroy() '�ı���' -> Unity�� �����δ� ��� �ı����� �ʰ� GC�� ó���� -> GC �޸� ��� ����
// ���� ����/�ı� -> ������ ���, GC Spikes, ���� ������ �ֿ� ����
// 3D���� -> ������Ʈ,�޽�,���� �浹 �� ���� ���� ���ҽ��� ����ϹǷ� �δ� �� ŭ

// '�������̽�'��? - "�̷��� ���� �Լ��� ������ �� �־�� ��!"��� ������ִ� Ʋ


public class Object_Pool : IPool
{
    public Transform parentTransform { get ; set; }

    // Queue -> FIFO ( First In First Out ) -> ���Լ���
    // Dequeue (���� ���� ������Ʈ�� ��������)
    // Enqueue (������Ʈ�� Queue���ο� ���� �ִ´�.)

    // Stack -> LIFO ( Last In First Out ) -> ���Լ���
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
