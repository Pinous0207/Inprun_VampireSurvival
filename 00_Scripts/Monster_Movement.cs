using System.Collections;
using UnityEngine;

public class Monster_Movement : MONSTER
{
    public float speed = 3.0f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void Initalize(Transform player, string monsterID)
    {
        base.Initalize(player, monsterID);

        Rotate(direction(), false);
        if(Boss(monsterID))
        {
            monsterSkill = GetComponentInChildren<MonsterSkill>();
            monsterSkill.monster = this;
        }
        float scale = Boss(monsterID) ? 25.0f : 15.0f;
        speed = Boss(monsterID) ? 5.0f : 3.0f;

        StartCoroutine(SpawnStartCoroutine(new Vector3(scale, scale, scale)));
    }

    IEnumerator SpawnStartCoroutine(Vector3 scaleEnd)
    {
        Vector3 ScaleStart = Vector3.zero;
        Vector3 ScaleEnd = scaleEnd;
        float duration = 0.5f;
        float timer = 0.0f;
        while(timer < duration)
        {
            float t = timer / duration;
            transform.localScale = Vector3.Lerp(ScaleStart, ScaleEnd, t);
            timer += Time.deltaTime;
            yield return null;
        }
        isSpanwed = true;
        animator.SetTrigger("MOVE");
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        if (!isSpanwed) return;
        if (isStunned) return;

        if(Boss(monsterid) && monsterSkill != null)
        {
            skillTimer += Time.fixedDeltaTime;
            if(skillTimer >= skillCooldown && skillCoroutine == null)
            {
                skillCoroutine = StartCoroutine(CastBossSkill());
            }
        }

        if(skillCoroutine == null)
            MoveAndRotate();
    }

    IEnumerator CastBossSkill()
    {
        animator.SetTrigger("MAGIC");

        yield return monsterSkill.CastSkill();

        skillTimer = 0.0f;
        skillCoroutine = null;
    }

    void MoveAndRotate()
    {
        Rotate(direction());
        rb.MovePosition(rb.position + direction() * speed * speedMultiplier * Time.fixedDeltaTime);
    }

    Vector3 direction()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;
        return direction;
    }

    void Rotate(Vector3 direction, bool Lerp = true)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            if (Lerp)
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            else transform.rotation = targetRotation;
        }
    }
}
