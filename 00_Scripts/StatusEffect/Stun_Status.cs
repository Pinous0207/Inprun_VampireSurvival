using UnityEngine;

public class Stun_Status : IStatusEffect
{
    private float duration = 2.0f;
    private float elapsed = 0.0f;
    public bool IsFinished => elapsed >= duration;

    public void Apply(MONSTER target, StatusEffect effect)
    {
        effect.Stun.gameObject.SetActive(true);
        target.SetStunned(true);
    }

    public void End(MONSTER target, StatusEffect effect)
    {
        effect.Stun.gameObject.SetActive(false);
        target.SetStunned(false);
    }

    public void Tick(MONSTER target)
    {
        elapsed += Time.deltaTime;
    }
}
