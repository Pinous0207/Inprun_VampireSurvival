using UnityEngine;

public class Burn_Status : IStatusEffect
{
    private float totalDamage => MANAGER.SESSION.Damage * 0.5f;
    private float duration = 4.0f;
    private float elapsed = 0.0f;
    private float tickInterval = 1.0f;
    private float tickTimer = 0.0f;
   
    public bool IsFinished => elapsed >= duration;

    public void Apply(MONSTER monster, StatusEffect effect)
    {
        effect.Burn.gameObject.SetActive(true);
    }

    public void End(MONSTER monster, StatusEffect effect)
    {
        effect.Burn.gameObject.SetActive(false);
    }

    public void Tick(MONSTER target)
    {
        elapsed += Time.deltaTime;
        tickTimer += Time.deltaTime;

        if(tickTimer >= tickInterval)
        {
            tickTimer = 0.0f;
            target.GetDamage(totalDamage);
        }
    }
}
