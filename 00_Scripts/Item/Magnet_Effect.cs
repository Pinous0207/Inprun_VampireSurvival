using UnityEngine;
using System.Collections.Generic;

public class Magnet_Effect : MonoBehaviour , IItemEffect
{
    bool isPickUp = false;
    public void Initalize()
    {
        isPickUp = false;
    }

    public void OnPickUp(GameObject owner)
    {
        if (isPickUp) return;
        isPickUp = true;
        List<Orb> orbs = MANAGER.SESSION.Orbs;
        for (int i= 0;  i < orbs.Count; i++)
        {
            orbs[i].StartFollow(Player.instance.transform);
        }
    }
   
}
