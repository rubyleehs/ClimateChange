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
        tile.EconomicalValue = economicalPoint;
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
        tile.EconomicalValue += economicalPoint;
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
        tile.EconomicalValue *= economicalPoint;
        tile.EnvironmentalValue *= environmentalPoint;
    }
}

public class EffectsSpreader
{
    public static void ApplyEffectSpread(Tile tile, EffectSpread spread, Action<Tile, int,int> pointApplierFunc)
    { 
        //if spread in one direction only
        if ((int)spread.type < 8) ApplyEffectSpreadHelper(tile._neighbours[(int)spread.type], spread, pointApplierFunc, (Direction)spread.type, spread.radius - 1);
        else
        {
            if (spread.type == EffectSpreadType.Horizontal || spread.type == EffectSpreadType.Cardinal)
            {
                ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.E], spread, pointApplierFunc, Direction.E, spread.radius - 1);
                ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.W], spread, pointApplierFunc, Direction.W, spread.radius - 1);
            }

            if (spread.type == EffectSpreadType.Vertical || spread.type == EffectSpreadType.Cardinal)
            {
                ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.N], spread, pointApplierFunc, Direction.N, spread.radius - 1);
                ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.S], spread, pointApplierFunc, Direction.S, spread.radius - 1);
            }

            switch (spread.type)
            {
                case EffectSpreadType.Self:
                    pointApplierFunc(tile, spread.economicalImpact, spread.environmentalImpact);
                    break;
                case EffectSpreadType.Diagonal:
                    ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.NE], spread, pointApplierFunc, Direction.NE, spread.radius - 1);
                    ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.NW], spread, pointApplierFunc, Direction.NW, spread.radius - 1);
                    ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.SE], spread, pointApplierFunc, Direction.SE, spread.radius - 1);
                    ApplyEffectSpreadHelper(tile._neighbours[(int)Direction.SW], spread, pointApplierFunc, Direction.SW, spread.radius - 1);
                    break;
                case EffectSpreadType.All | EffectSpreadType.Donut:
                    for (int dy = -spread.radius; dy <= spread.radius; dy++)
                    {
                        for (int dx = -spread.radius; dx <= spread.radius; dy++)
                        {
                            if (spread.type == EffectSpreadType.Donut && dx == 0 && dy == 0) continue;
                            pointApplierFunc(Board.GetTile(tile._indexPosition.x + dx, tile._indexPosition.y + dy), spread.economicalImpact, spread.environmentalImpact);
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
        ApplyEffectSpreadHelper(tile, direction.Value, (Tile b) => pointApplierFunc(b, spread.economicalImpact, spread.environmentalImpact), distance);
    }
}




