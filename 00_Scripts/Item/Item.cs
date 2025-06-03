using UnityEngine;

public class Item : MonoBehaviour
{
    public string ItemId;

    private IFactory<Item> factory;
    private IItemEffect itemEffect;
    public void Initalize(string id)
    {
        ItemId = id;
        if (factory == null)
            factory = new GenericPartFactory<Item>(MANAGER.DB.Item);

        factory.Build(this, id);

        itemEffect = GetComponentInChildren<IItemEffect>();
        itemEffect.Initalize();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemEffect.OnPickUp(other.gameObject);
            MANAGER.POOL.m_pool_Dictionary["Item"].Return(this.gameObject);
        }
    }
}
