using UnityEngine;

public interface IStatusEffect 
{
    void Apply(MONSTER target, StatusEffect effect);
    void Tick(MONSTER target);
    void End(MONSTER target, StatusEffect effect);
    bool IsFinished { get; }
}
