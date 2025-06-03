using UnityEngine;

// ∆—≈‰∏Æ ∆–≈œ (Factory Pattern)
public interface IFactory<T> where T : Component
{
    void Build(T entity, string id);
}
