using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AcceptAllPoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        GameManager.EconomicPoints += tile.EconomicValue;
        GameManager.EnvironmentalPoints += tile.EnvironmentalValue;
    }
}

public class AcceptEconomicalPoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        GameManager.EconomicPoints += tile.EconomicValue;
    }
}

public class AcceptEnvironmentalPoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        GameManager.EnvironmentalPoints += tile.EnvironmentalValue;
    }
}
