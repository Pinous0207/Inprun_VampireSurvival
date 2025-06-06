using System.Collections;
using UnityEngine;

public abstract class MonsterSkill : MonoBehaviour
{
    public MONSTER monster;
    public abstract IEnumerator CastSkill();
}
