using UnityEngine;

public class Monster_Renderer_Finder : MonoBehaviour
{
    public Renderer renderer;

    public void Initalize()
    {
        Animator animator = GetComponent<Animator>();
        transform.parent.GetComponent<MONSTER>().animator = animator;
        StatusEffect effect = transform.parent.GetComponent<StatusEffect>();
        effect.renderer = renderer;
    }
}
