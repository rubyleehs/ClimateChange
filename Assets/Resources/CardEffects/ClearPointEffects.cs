using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAllPoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        GameManager.EconomicalPoints = 0;
        GameManager.EnvironmentalPoints = 0;
    }
}

public class ClearEconomicalPoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        GameManager.EconomicalPoints = 0;
    }
}

public class ClearEnvironmentalPoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        GameManager.EnvironmentalPoints = 0;
    }
}

