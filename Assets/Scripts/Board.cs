using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject TilePrefab;
    public GameObject DefaultGroundPrefab;

    public Transform TileParent;

    public Vector2Int MapResolution;
    public Vector3 Origin;
    public Vector2 TileSize;

    public static Tile[,] map;

    private void Awake()
    {
        Init();
    }

    public void Init()
    { 
        CleanUpMap();
        CreateMap(MapResolution, Origin, groundPrefab: DefaultGroundPrefab, TileSize);
    }

    public void CreateMap(Vector2Int mapResolution, Vector3 origin, GameObject groundPrefab, Vector2 tileSize)
    {
        map = new Tile[mapResolution.x, mapResolution.y];
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                map[x, y] = Instantiate(TilePrefab, origin + new Vector3(x * tileSize.x, 0, y * tileSize.y), Quaternion.identity, TileParent).GetComponent<Tile>();
                map[x, y].Init(new Vector2Int(x, y), groundPrefab);

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

    public static void UpdateAllTextRotation(float textRotationSpeed)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                map[x, y].RotateTextToFaceCamera(textRotationSpeed);
            }
        }
    }

    public static Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1)) return null;
        return map[x, y];
    }
}
