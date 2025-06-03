using UnityEngine;

public class Shock_Status : IStatusEffect
{
    private float duration = 3.0f;
    private float elapsed = 0.0f;
    private float damageAmp = 0.2f;
    public bool IsFinished => elapsed >= duration;

    public void Apply(MONSTER target, StatusEffect effect)
    {
        target.SetShockAmp(damageAmp);
        effect.Shock.gameObject.SetActive(true);
    }

    public void End(MONSTER target, StatusEffect effect)
    {
        target.SetShockAmp(0f);
        effect.Shock.gameObject.SetActive(false);
    }


    public void Tick(MONSTER target)
    {
        elapsed += Time.deltaTime;
    }
}
