public abstract class CardEffect
{
    public virtual void OnPlay(Tile tile, CardDefinition definition) { }
    public virtual void OnTurnStart(CardDefinition definition) { }
    public virtual void OnTurnEnd(CardDefinition definition) { }
    public virtual void OnDestruction(CardDefinition definition) { }
}
