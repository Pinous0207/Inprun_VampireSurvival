using System.Collections;
using UnityEngine;

public class Monster_Effect_Circle : MonoBehaviour
{
    public Transform target;
    private float explosionRange;
    private float damage;

    private float growDuration = 1.5f;
    private float timer;

    private bool exploded = false;

    public Transform circleRenderer;
    public Transform circleLine;

    public void Initialize(Transform player, float range, float dmg)
    {
        target = player;
        explosionRange = range;
        damage = dmg;
        exploded = false;
        timer = 0.0f;
        float lineScale = range * 2f;
        circleLine.localScale = new Vector3(lineScale, lineScale);

        circleRenderer.localScale = Vector3.zero;

        StartCoroutine(GrowAndExplode());
    }

    IEnumerator GrowAndExplode()
    {
        while(timer < growDuration)
        {
            timer += Time.deltaTime;
            float t = timer / growDuration;

            float targetScale = circleLine.localScale.x;
            circleRenderer.localScale = Vector3.Lerp(Vector3.zero, new Vector3(targetScale, targetScale), t);

            yield return null;
        }

        Explode();
    }

    void Explode()
    {
        if (exploded) return;
        exploded = true;

        var lightning = MANAGER.POOL.Pooling_OBJ("Boss_Lightning").Get((value) =>
        {
            value.transform.position = transform.position;
            value.GetComponent<ParticleSystem>().Play();
        });

        float dist = Vector3.Distance(target.position, transform.position);
        if(dist <= explosionRange)
        {
            target.GetComponent<Player>().GetDamage(damage);
        }

        MANAGER.POOL.m_pool_Dictionary["EFFECT_CIRCLE"].Return(this.gameObject);
    }

}
