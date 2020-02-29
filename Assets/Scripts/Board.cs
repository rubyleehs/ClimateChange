using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject tileGO;
    public CardDefinition I_defaultCardDefination;

    public Transform tileParent;

    public Vector2Int mapResolution;
    public Vector2 tileSize;
    public Vector3 origin;

    public static CardDefinition defaultCardDefination;

    private Tile[,] map;

    public Camera Camera;

    public CardDefinition CardDefinition;

    private void Awake()
    {
        InitNewBoard();
    }

    public void InitNewBoard()
    {
        CleanUpMap();
        CreateMap(mapResolution, defaultTileGO, tileSize, origin);

        Camera.transform.SetPositionAndRotation(new Vector3(-10, 88f, -10), Quaternion.Euler(45, 45, 0));

        var card = new GameObject("Card");
        var component = card.AddComponent<Card>();
        component.Init(CardDefinition);
        card.transform.position = new Vector3(5, 20, 5);
        card.transform.localScale = new Vector3(25, 25, 25);
    }

    public void CreateMap(Vector2Int mapResolution, GameObject tileGO, CardDefinition defaultCardDefination, Vector2 tileSize , Vector3 origin)
    {
        /*
        this.mapResolution = mapResolution;
        this.tileSize = tileSize;
        this.origin = origin;
        */

        Board.defaultCardDefination = defaultCardDefination;

        map = new Tile[mapResolution.x, mapResolution.y];
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                map[x,y] = Instantiate(tileGO, origin + new Vector3(x * tileSize.x, 0 , y * tileSize.y), Quaternion.identity, tileParent).GetComponent<Tile>();
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
