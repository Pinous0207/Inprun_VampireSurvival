using UnityEngine;

public class Treasure_Effect : MonoBehaviour, IItemEffect
{
    public ParticleSystemRenderer particle;

    public Material[] materials;
    int value = 0;
    bool isPickUp = false;

    public void Initalize()
    {
        isPickUp = false;

        int randomValue = Random.Range(0, 3);
        randomValue = 2;
        value = randomValue;
        particle.material = materials[randomValue];
    }
    public void OnPickUp(GameObject owner)
    {
        if (isPickUp) return;
        isPickUp = true;
        Base_Canvas.instance.SelectTreasure(value);
    }
}
