using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class StatusEffect : MonoBehaviour
{
    MONSTER monster;

    public GameObject Burn;
    public GameObject FreezeStone;
    public GameObject Shock;
    public GameObject Stun;

    List<IStatusEffect> activeEffects = new List<IStatusEffect>();
    [HideInInspector] public Renderer renderer;

    private float freezeStack;
    private bool clearNextFrame = false;
    public void Initalize()
    {
        StopAllCoroutines();
        clearNextFrame = true;
     
        renderer.material.SetColor("_EmissionColor", Color.black);
    }

    private void Start()
    {
        monster = GetComponent<MONSTER>();
    }

    public void ApplyBurn()
    {
        activeEffects.RemoveAll(e => e is Burn_Status);

        Burn_Status burn = new Burn_Status();
        burn.Apply(monster, this);
        activeEffects.Add(burn);
    }

    public void ApplyFreeze(float stackAmount)
    {
        var freeze = activeEffects.FirstOrDefault(x => x is Freeze_Status) as Freeze_Status;

        if (freeze != null)
        {
            freeze.AddStack(stackAmount);
            freeze.Apply(monster, this);
        }
        else
        {
            var newFreeze = new Freeze_Status();
            newFreeze.AddStack(stackAmount);
            newFreeze.Apply(monster, this);
            activeEffects.Add(newFreeze);
        }
    }

    public void ApplyShock()
    {
        activeEffects.RemoveAll(e => e is Shock_Status);

        Shock_Status shock = new Shock_Status();
        shock.Apply(monster, this);
        activeEffects.Add(shock);
    }

    public void ApplyStun()
    {
        activeEffects.RemoveAll(e => e is Stun_Status);

        Stun_Status stun = new Stun_Status();
        stun.Apply(monster, this);
        activeEffects.Add(stun);
    }

    public void ApplyKnockback(float distance, float duration)
    {
        if (monster.isDead) return;

        Vector3 direction = (transform.position - Player.instance.transform.position).normalized;
        StartCoroutine(KnockbackCoroutine(direction, distance, duration));
    }

    IEnumerator KnockbackCoroutine(Vector3 dir, float dist, float time)
    {
        float elapsed = 0.0f;
        float speed = dist / time;
        Vector3 start = transform.position;
        Vector3 end = start + dir.normalized * dist;

        while(elapsed < time)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, elapsed / time);
            yield return null;
        }
    }

    public void GetHitEffect()
    {
        StartCoroutine(FlashEmission(0.5f));
    }

    IEnumerator FlashEmission(float fadeTime)
    {
        Color flashColor = Color.white * 4.0f;

        float timer = 0.0f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            Color current = Color.Lerp(flashColor, Color.black, timer / fadeTime);
            renderer.material.SetColor("_EmissionColor", current);
            yield return null;
        }

        renderer.material.SetColor("_EmissionColor", Color.black);
    }

    private void Update()
    {
        if(clearNextFrame)
        {
            foreach (var effect in activeEffects) effect.End(monster, this);

            activeEffects.Clear();
            clearNextFrame = false;
            return;
        }

        for(int i = activeEffects.Count -1; i >= 0; i--)
        {
             activeEffects[i].Tick(monster);
            if (activeEffects[i].IsFinished)
            {
                activeEffects[i].End(monster,this);
                activeEffects.RemoveAt(i);
            }
        }
    }
}
