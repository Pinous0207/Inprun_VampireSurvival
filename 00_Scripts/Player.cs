using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public static Player instance = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    [SerializeField] private Volume volume;
    private Vignette vignette;
    [SerializeField] private Color vignetteColor;

    public float detectionRadius;
    public LayerMask monsterLayer;
    public List<Transform> targets = new List<Transform>();
    public Renderer[] renderer;
    bool isHit = false;
    public Transform target 
    { 
        get { return GetNearestMonster(); }
    }

    private void Start()
    {
        volume.profile.TryGet(out vignette);
    }


    public Vector3 Direction()
    {
        Vector3 dirToMonster = (target.position - transform.position).normalized;
        return dirToMonster;
    }

    public List<Transform> GetCollidersHitMonsters(float radius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, monsterLayer);
        List<Transform> targetLists = new List<Transform>();
        foreach (Collider col in hits)
        {
            if (col.GetComponent<MONSTER>().isSpanwed)
            {
                targetLists.Add(col.transform);
            }
        }
        return targetLists;
    }

    public Transform GetNearestMonster()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayer);
        
        Transform nearest = null;
        float minDist = Mathf.Infinity;
        targets = new List<Transform>();
        foreach (Collider col in hits)
        {
            if (col.GetComponent<MONSTER>().isSpanwed)
            {
                targets.Add(col.transform);
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = col.transform;
                }
            }
        }
 
        return nearest;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            if(isHit == false)
               GetDamage(10);
        }
    }

    public void GetDamage(float dmg)
    {
        isHit = true;
        StartCoroutine(FlashEmission(0.5f));
        StartCoroutine(CameraShake(Camera.main.transform, 0.1f, 0.05f));

        var damageFont = MANAGER.POOL.Pooling_OBJ("DamageTMP").Get((value) =>
        {
            value.GetComponent<DamageTMP>().Initalize(
                Base_Canvas.instance.HOLDERLAYER,
                transform.position,
                ((int)dmg).ToString(),Color.red);
        });

        MANAGER.SESSION.GetDamage(dmg);
    }

    IEnumerator FlashEmission(float fadeTime)
    {
        Color flashColor = Color.white * 4.0f;

        float timer = 0.0f;
        while(timer < fadeTime)
        {
            timer += Time.deltaTime;
            Color current = Color.Lerp(flashColor, Color.black, timer / fadeTime);
            for (int i = 0; i < renderer.Length; i++)
                renderer[i].material.SetColor("_EmissionColor", current);
            yield return null;
        }

        for (int i = 0; i < renderer.Length; i++)
            renderer[i].material.SetColor("_EmissionColor", Color.black);

        isHit = false;
    }

    IEnumerator VignettePulse()
    {
        float t = 0.0f;
        float duration = 0.15f;
        float maxIntensity = 0.4f;

        vignette.color.value = vignetteColor;

        while (t < duration)
        {
            t += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(0.0f, maxIntensity, t / duration);
            yield return null;
        }

        t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(maxIntensity, 0.0f, t / duration);
            yield return null;
        }

        vignette.intensity.value = 0.0f;
    }

    IEnumerator CameraShake(Transform camTransform, float duration, float strength)
    {
        Vector3 originalPos = camTransform.localPosition;
        float timer = 0.0f;

        while(timer < duration)
        {
            timer += Time.deltaTime;

            float offsetX = Random.Range(-1.0f, 1.0f) * strength;
            float offsetY = Random.Range(-1.0f, 1.0f) * strength;

            camTransform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0.0f);

            yield return null;
        }

        camTransform.localPosition = originalPos;
    }
}
