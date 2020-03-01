using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EffectSpreadType { N = 0, NE = 1, E = 2, SE = 3, S= 4, SW = 5, W= 6, NW= 7,  Horizontal = 8, Vertical = 9, Cardinal = 10, Diagonal = 11, Self = 12, All = 13, Donut = 14 }

[System.Serializable]
public struct EffectSpread
{
    public EffectSpreadType Type;
    public int Radius;
    public int EconomicImpact;
    public int EnvironmentalImpact;
}

[CreateAssetMenu(menuName = "Cards/New Card", fileName = "Card")]
public class CardDefinition : ScriptableObject
{
    public Sprite Sprite;

    public int Cost;
    public List<EffectSpread> effectSpread;

    public GameObject GroundModel;
    public GameObject PlacedModel;
    public List<String> EffectNames;

    private List<CardEffect> _effects;
    public List<CardEffect> Effects
    {
        get
        {
            if (_effects != null)
                return _effects;

            _effects = EffectNames.Select(effectName => Activator.CreateInstance(Type.GetType(effectName)) as CardEffect).ToList();
            return _effects;
        }
    }

    public Card CreateCard()
    {
        var card = new GameObject("Card").AddComponent<Card>();
        card.Init(this, null);
        return card;
    }
}