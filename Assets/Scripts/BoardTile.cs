using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { N = 0, NE = 1, E = 2, SE = 3, S = 4, SW = 5, W = 6, NW = 7}

[RequireComponent(typeof(SpriteRenderer))]
public class BoardTile : MonoBehaviour
{
    public BoardTile[] neighbours;
    private Vector2Int indexPosition;

    public void InitBoardTile(Vector2Int indexPosition)
    {
        this.indexPosition = indexPosition;
        neighbours = new BoardTile[8];
    }

    public void AddNeighbour(BoardTile tile, Direction direction)
    {
        if (tile == null) return;
        neighbours[(int)direction] = tile;
        tile.neighbours[((int)direction + 4) % 8] = this;
    }

}
