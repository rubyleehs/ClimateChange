using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/New Card", fileName = "CardDefinition")]
public class CardDefinition : ScriptableObject
{
    public int Cost;
    public int EnvironmentalImpact;
    public int EconomicImpact;
    public List<CardEffect> Effects;

    public GameObject tileModel;

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