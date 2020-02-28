public abstract class CardEffect
{
    public abstract void OnPlay(Tile tile);
    public abstract void OnTurnStart();
    public abstract void OnTurnEnd();
    public abstract void OnDestroy();
}