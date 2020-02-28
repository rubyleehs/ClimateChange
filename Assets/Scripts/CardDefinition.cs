public class CardDefinition
{
    public int Cost { get; }
    public int EnvironmentalImpact { get; }
    public int EconomicalImpact { get; }
    public CardEffect Effect { get; }

    public CardDefinition(int cost, int environmentalImpact, int economicalImpact, CardEffect effect)
    {
        Cost = cost;
        EnvironmentalImpact = environmentalImpact;
        EconomicalImpact = economicalImpact;
        Effect = effect;
    }

    public Card CreateCard()
        => new Card(this);
}