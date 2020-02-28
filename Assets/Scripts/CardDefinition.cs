using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDefinition : ScriptableObject
{
    public int Cost { get; }
    public int EnvironmentalImpact { get; }
    public int EconomicImpact { get; }
    public List<CardEffect> Effects { get; }

    public CardDefinition(int cost, int environmentalImpact, int economicImpact, IEnumerable<CardEffect> effects)
    {
        Cost = cost;
        EnvironmentalImpact = environmentalImpact;
        EconomicImpact = economicImpact;
        Effects = effects as List<CardEffect> ?? effects.ToList();
    }

    public Card CreateCard()
        => new Card(this);
}