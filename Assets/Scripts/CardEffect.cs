public abstract class CardEffect
{
    public virtual void OnPlay(Tile tile, CardDefinition cardDefinition) { }
    public virtual void OnTurnStart(Tile tile, CardDefinition cardDefinition) { }
    public virtual void OnTurnEnd(Tile tile, CardDefinition cardDefinition) { }
    public virtual void OnDestruction(Tile tile, CardDefinition cardDefinition) { }
}
