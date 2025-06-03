using UnityEngine;

public class Health_Effect : MonoBehaviour,IItemEffect
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

        float hpCount = MANAGER.SESSION.MaxHP * 0.3f;
        MANAGER.SESSION.HP += hpCount;

        var damageFont = MANAGER.POOL.Pooling_OBJ("DamageTMP").Get((value) =>
        {
            value.GetComponent<DamageTMP>().Initalize(
                Base_Canvas.instance.HOLDERLAYER,
                transform.position,
                ((int)hpCount).ToString(), Color.green,
                false);
        });
    }
}
