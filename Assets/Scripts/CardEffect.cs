using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public virtual void OnPlay(Tile tile) { }
    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }
    public virtual void OnDestruction() { }
}