using UnityEngine;

public class Explosion_Effect : MonoBehaviour, IItemEffect
{
    bool isPickUp = false;
    public float radius = 11.0f;
    public float damage = 100.0f;

    public GameObject ExplosionEffect;
    public void OnPickUp(GameObject owner)
    {
        if (isPickUp) return;
        isPickUp = true;
        Vector3 center = transform.position;

        Collider[] hits = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Monster"));
        foreach(var hit in hits)
        {
            if(hit.TryGetComponent(out MONSTER m) && m.isSpanwed)
            {
                m.GetDamage(damage);
            }
        }

        Instantiate(ExplosionEffect, center, Quaternion.Euler(90, 0, 0));
    }

    public void Initalize()
    {
        isPickUp = false;
    }
}
