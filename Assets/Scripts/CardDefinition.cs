using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/New Card", fileName = "Card")]
public class CardDefinition : ScriptableObject
{
    public Sprite Sprite;

    public int Cost;
    public int EnvironmentalImpact;
    public int EconomicImpact;

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
}