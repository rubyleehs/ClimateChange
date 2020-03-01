using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SetPoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        for(int i = 0; i < definition.effectSpread.Count; i++)
        {
            EffectsSpreader.ApplyEffectSpread(tile, definition.effectSpread[i], SetPoint);
        }
    }

    public static void SetPoint(Tile tile, int economicalPoint, int environmentalPoint)
    {
        if (tile == null) return;
        tile.EconomicValue = economicalPoint;
        tile.EnvironmentalValue = environmentalPoint;
    }  
}

public class ChangePoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        for (int i = 0; i < definition.effectSpread.Count; i++)
        {
            EffectsSpreader.ApplyEffectSpread(tile, definition.effectSpread[i], ChangePoint);
        }
    }

    public static void ChangePoint(Tile tile, int economicalPoint, int environmentalPoint)
    {
        if (tile == null) return;
        tile.EconomicValue += economicalPoint;
        tile.EnvironmentalValue += environmentalPoint;
    }
}

public class MultiplyPoints : CardEffect
{
    public override void OnPlay(Tile tile, CardDefinition definition)
    {
        base.OnPlay(tile, definition);
        for (int i = 0; i < definition.effectSpread.Count; i++)
        {
            EffectsSpreader.ApplyEffectSpread(tile, definition.effectSpread[i], MultiplyPoint);
        }
    }

    public static void MultiplyPoint(Tile tile, int economicalPoint, int environmentalPoint)
    {
        if (tile == null) return;
        tile.EconomicValue *= economicalPoint;
        tile.EnvironmentalValue *= environmentalPoint;
    }
}

public class EffectsSpreader
{
    public static void ApplyEffectSpread(Tile tile, EffectSpread spread, Action<Tile, int,int> pointApplierFunc)
    { 
        //if spread in one direction only
        if ((int)spread.Type < 8) ApplyEffectSpreadHelper(tile._neighbours[(int)spread.Type], spread, pointApplierFunc, (Direction)spread.Type, spread.Radius - 1);
        else
        {
            if (spread.Type == EffectSpreadType.Horizontal || spread.Type == EffectSpreadType.Cardinal)
            {
                ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.E], spread, pointApplierFunc, Direction.E, spread.Radius - 1);
                ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.W], spread, pointApplierFunc, Direction.W, spread.Radius - 1);
            }

            if (spread.Type == EffectSpreadType.Vertical || spread.Type == EffectSpreadType.Cardinal)
            {
                ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.N], spread, pointApplierFunc, Direction.N, spread.Radius - 1);
                ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.S], spread, pointApplierFunc, Direction.S, spread.Radius - 1);
            }

            switch (spread.Type)
            {
                case EffectSpreadType.Self:
                    pointApplierFunc(tile, spread.EconomicImpact, spread.EnvironmentalImpact);
                    break;
                case EffectSpreadType.Diagonal:
                    ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.NE], spread, pointApplierFunc, Direction.NE, spread.Radius - 1);
                    ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.NW], spread, pointApplierFunc, Direction.NW, spread.Radius - 1);
                    ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.SE], spread, pointApplierFunc, Direction.SE, spread.Radius - 1);
                    ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.SW], spread, pointApplierFunc, Direction.SW, spread.Radius - 1);
                    break;
                case EffectSpreadType.All | EffectSpreadType.Donut:
                    for (int dy = -spread.Radius; dy <= spread.Radius; dy++)
                    {
                        for (int dx = -spread.Radius; dx <= spread.Radius; dy++)
                        {
                            if (spread.Type == EffectSpreadType.Donut && dx == 0 && dy == 0) continue;
                            pointApplierFunc(GameManager.Instance.Board.GetTile(tile.Position.x + dx, tile.Position.y + dy), spread.EconomicImpact, spread.EnvironmentalImpact);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public static void ApplyEffectSpreadHelper(Tile root, Direction direction, Action<Tile> applyFunc, int maxIteration)
    {
        ApplyEffectSpreadHelper(root, (Tile a) => a._neighbours[(int)direction], applyFunc, maxIteration);
    }

    public static void ApplyEffectSpreadHelper(Tile root, Func<Tile, Tile> spreadFunc, Action<Tile> applyFunc, int maxIteration)
    {
        Tile travel = root;
        for (int i = 0; i < maxIteration; i++)
        {
            if (travel = null) return;

            applyFunc(travel);
            travel = spreadFunc(travel);
        }
    }

    public static void ApplyEffectSpreadHelper(Tile tile, EffectSpread spread, Action<Tile, int, int> pointApplierFunc, Direction? direction = null, int distance = 0)
    {
        ApplyEffectSpreadHelper(tile, direction.Value, (Tile b) => pointApplierFunc(b, spread.EconomicImpact, spread.EnvironmentalImpact), distance);
    }
}




