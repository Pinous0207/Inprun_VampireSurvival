using UnityEngine;

// ���丮 ���� (Factory Pattern)
public interface IFactory<T> where T : Component
{
    void Build(T entity, string id);
}
