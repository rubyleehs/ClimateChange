using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAllPoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        tile.EconomicValue = 0;
        tile.EnvironmentalValue= 0;
    }
}

public class ClearEconomicalPoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        tile.EconomicValue = 0;
    }
}

public class ClearEnvironmentalPoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        tile.EnvironmentalValue = 0;
    }
}

