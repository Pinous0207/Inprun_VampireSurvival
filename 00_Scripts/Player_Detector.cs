using UnityEngine;

public class Player_Detector : MonoBehaviour
{
    public LayerMask orbLayer;

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, Magnet(), orbLayer);

        foreach(var hit in hits)
        {
            Orb orb = hit.GetComponent<Orb>();
            if(orb != null && orb.isIdle)
            {
                orb.StartFollow(transform);
            }
        }
    }

    private float Magnet()
    {
        float baseMagnet = MANAGER.SESSION.magnetRadius;
        float magnet = baseMagnet + baseMagnet * (MANAGER.SESSION.magnetRadiusPercent / 100);
        return magnet;
    }
}
