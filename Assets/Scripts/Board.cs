﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject defaultTileGO;
    public Vector2Int mapResolution;
    public Vector2 tileSize;
    public Vector3 origin;

    private Tile[,] map;

    private void Awake()
    {
        InitNewBoard();
    }

    public void InitNewBoard()
    {
        CleanUpMap();
        CreateMap(mapResolution, defaultTileGO, tileSize, origin);
    }

    public void CreateMap(Vector2Int mapResolution, GameObject tileGO, Vector2 tileSize , Vector3 origin)
    {
        /*
        this.mapResolution = mapResolution;
        this.tileSize = tileSize;
        this.origin = origin;
        */

        map = new Tile[mapResolution.x, mapResolution.y];
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                map[x,y] = Instantiate(tileGO, origin + new Vector3(x * tileSize.x, 0 , y * tileSize.y), Quaternion.identity, this.transform).GetComponent<Tile>();
                map[x, y].InitBoardTile(new Vector2Int(x, y));

                if (x > 0) map[x, y].AddNeighbour(map[x - 1, y], Direction.W);
                if (y > 0)
                {
                    map[x, y].AddNeighbour(map[x, y -1], Direction.S);

                    if(x > 0) map[x, y].AddNeighbour(map[x - 1, y - 1], Direction.SW);
                    if(x < map.GetLength(0) - 1) map[x, y].AddNeighbour(map[x + 1, y - 1], Direction.SE);
                }
            }
        }
    }

    public void CleanUpMap()
    {
        if (map == null) return;
        for(int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if(map[x,y] != null) Destroy(map[x, y]);
            }
        }
    }

    public Vector2Int GetMapResolution()
    {
        return mapResolution;
    }
}
