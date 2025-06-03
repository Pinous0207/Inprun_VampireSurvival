using UnityEngine;

public interface IItemEffect
{
    void Initalize();
    void OnPickUp(GameObject owner);   
}
