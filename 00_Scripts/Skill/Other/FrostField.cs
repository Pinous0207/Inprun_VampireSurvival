using UnityEngine;

public class FrostField : MonoBehaviour
{
    void Update()
    {
        transform.position = Player.instance.transform.position;
    }
}
