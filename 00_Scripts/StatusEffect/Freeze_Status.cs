using UnityEngine;

public class Freeze_Status : IStatusEffect
{
    public Color originalColor;
    private float freezeStack = 0.0f;
    private float maxFreezeStack = 1.0f;
    private float slowRate = 0.8f; // 최대 감속 60%
    private float slowDuration = 2.5f;
    private float slowTimer;

    private float frozenDuration = 2.0f;
    private float elapsed = 0.0f;

    private bool isFrozen = false;

    private StatusEffect ownerEffect;
    private MONSTER owner;

    public bool IsFinished => isFrozen ? elapsed >= frozenDuration : freezeStack <= 0;

    public void AddStack(float amount)
    {
        freezeStack = Mathf.Clamp01(freezeStack + amount);
    }

    public void Apply(MONSTER target, StatusEffect effect)
    {
        if(owner == null)
        {
            owner = target;
            ownerEffect = effect;
            originalColor = effect.renderer.material.GetColor("_BaseColor");
            effect.renderer.material.SetColor("_BaseColor", new Color(0.5f, 0.8f, 1.0f, 1.0f));
        }
        slowTimer = 0.0f;

        if (!isFrozen && freezeStack >= maxFreezeStack)
        {
            isFrozen = true;
            ownerEffect.FreezeStone.gameObject.SetActive(true);
            Debug.Log(ownerEffect.FreezeStone);
            elapsed = 0.0f;
            target.SetSpeedMultiplier(0.0f);
        }
        else if(!isFrozen)
        {
            float slowMultiplier = 1.0f - Mathf.Lerp(0.0f, slowRate, freezeStack);
            target.SetSpeedMultiplier(slowMultiplier);
        }
    }

    public void End(MONSTER target, StatusEffect effect)
    {
        effect.renderer.material.SetColor("_BaseColor", originalColor);
        ownerEffect.FreezeStone.gameObject.SetActive(false);
        target.SetSpeedMultiplier(1.0f);
    }

    public void Tick(MONSTER target)
    {
        if (isFrozen)
        {
            elapsed += Time.deltaTime;
        }
        else
        {
            slowTimer += Time.deltaTime;

            if(slowTimer >= slowDuration)
            {
                freezeStack = 0.0f;
                target.SetSpeedMultiplier(1.0f);
            }
            else
            {
                float slowMultiplier = 1.0f - Mathf.Lerp(0.0f, slowRate, freezeStack);
                target.SetSpeedMultiplier(slowMultiplier);
            }
        }
    }
}
