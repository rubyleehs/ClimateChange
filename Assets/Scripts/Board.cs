using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject TilePrefab;
    public GameObject DefaultGroundPrefab;

    public GameObject CliffStraightMidPrefab;
    public GameObject CliffStraightTopPrefab;
    public GameObject CliffCornerInnerMidPrefab;
    public GameObject CliffCornerInnerTopPrefab;
    public GameObject CliffCornerMidPrefab;
    public GameObject CliffCornerTopPrefab;

    public Transform TileParent;

    public Vector2Int MapResolution;
    public Vector3 Origin;
    public Vector2 TileSize;

    public Transform I_projectorRig;
    public static Transform projectorRig;
    
    private Tile[,] _tileMap;
    private int[,] _heightMap;

    private List<(CardEffect cardEffect, Card card, Tile tile)> _cardEffects;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        projectorRig = I_projectorRig;
        _cardEffects = new List<(CardEffect cardEffect, Card card, Tile tile)>();

        CleanUpMap();
        CreateMap(MapResolution, Origin, groundPrefab: DefaultGroundPrefab, TileSize);
    }

    public void AddCardEffect(CardEffect cardEffect, Card card, Tile tile)
        => _cardEffects.Add((cardEffect, card, tile));
    public void RemoveCardEffect(CardEffect cardEffect)
    {
        var entry = _cardEffects.Where(e => e.cardEffect == cardEffect).First();
        _cardEffects.Remove(entry);
    }
    public void RaiseTurnStartEvents()
    {
        foreach (var (cardEffect, card, tile) in _cardEffects)
            cardEffect.OnTurnStart(tile, card.Type);
    }
    public void RaiseTurnEndEvents()
    {
        foreach (var (cardEffect, card, tile) in _cardEffects)
            cardEffect.OnTurnEnd(tile, card.Type);
    }
    public void RaiseDestructionEvents()
    {
        foreach (var (cardEffect, card, tile) in _cardEffects)
            cardEffect.OnDestruction(tile, card.Type);
    }

    public void CreateMap(Vector2Int mapResolution, Vector3 origin, GameObject groundPrefab, Vector2 tileSize)
    {
        int w = mapResolution.x - 1, h = mapResolution.y - 1;

        _heightMap = new int[mapResolution.x, mapResolution.y];
        for (int y = 0; y < _heightMap.GetLength(1); y++)
        {
            for (int x = 0; x < _heightMap.GetLength(0); x++)
            {
                _heightMap[x, y] = (int)(6 * Mathf.PerlinNoise(x / 15f, y / 15f));
            }
        }

        _tileMap = new Tile[mapResolution.x, mapResolution.y];
        for (int y = 0; y < _tileMap.GetLength(1); y++)
        {
            for (int x = 0; x < _tileMap.GetLength(0); x++)
            {
                var curr = _heightMap[x, y];

                var ledge_TL = x > 0 && y > 0 ? (curr - _heightMap[x - 1, y - 1]) : 0;
                var ledge_TC =          y > 0 ? (curr - _heightMap[x,     y - 1]) : 0;
                var ledge_TR = x < w && y > 0 ? (curr - _heightMap[x + 1, y - 1]) : 0;
                var ledge_CL = x > 0          ? (curr - _heightMap[x - 1, y])     : 0;
                var ledge_CR = x < w          ? (curr - _heightMap[x + 1, y])     : 0;
                var ledge_BL = x > 0 && y < h ? (curr - _heightMap[x - 1, y + 1]) : 0;
                var ledge_BC =          y < h ? (curr - _heightMap[x,     y + 1]) : 0;
                var ledge_BR = x < w && y < h ? (curr - _heightMap[x + 1, y + 1]) : 0;

                // Is ledge
                if (ledge_TL < 0 || ledge_TC < 0 || ledge_TR < 0
                    || ledge_CL < 0|| ledge_CR < 0
                    || ledge_BL < 0|| ledge_BC < 0|| ledge_BR < 0)
                {
                    if (ledge_TC < 0)
                    {
                        for (int i = curr; i >= (curr + ledge_TC); i--)
                        {
                            if (i == (curr + ledge_TC))
                                Instantiate(DefaultGroundPrefab, origin + new Vector3(x * tileSize.x, 10 * (i + 1), y * tileSize.y), Quaternion.identity, TileParent);
                            else
                                Instantiate(
                                    i == curr ? CliffStraightTopPrefab : CliffStraightMidPrefab,
                                    origin + new Vector3(x * tileSize.x, 10 * i + 2.5f, y * tileSize.y),
                                    Quaternion.Euler(0, 0, 0), TileParent);
                        }
                    }
                    if (ledge_BC < 0)
                    {
                        for (int i = curr; i >= (curr + ledge_BC); i--)
                        {
                            if (i == (curr + ledge_BC))
                                Instantiate(DefaultGroundPrefab, origin + new Vector3(x * tileSize.x, 10 * (i + 1), y * tileSize.y), Quaternion.identity, TileParent);
                            else
                                Instantiate(
                                    i == curr ? CliffStraightTopPrefab : CliffStraightMidPrefab,
                                    origin + new Vector3((x + 1) * tileSize.x, 10 * i + 2.5f, (y + 1) * tileSize.y),
                                    Quaternion.Euler(0, 180, 0), TileParent);
                        }
                    }
                    if (ledge_CL < 0)
                    {
                        for (int i = curr; i >= (curr + ledge_CL); i--)
                        {
                            if (i == (curr + ledge_CL))
                                Instantiate(DefaultGroundPrefab, origin + new Vector3(x * tileSize.x, 10 * (i + 1), y * tileSize.y), Quaternion.identity, TileParent);
                            else
                                Instantiate(
                                    i == curr ? CliffStraightTopPrefab : CliffStraightMidPrefab,
                                    origin + new Vector3(x * tileSize.x, 10 * i + 2.5f, (y + 1) * tileSize.y),
                                    Quaternion.Euler(0, 90, 0), TileParent);
                        }
                    }
                    if (ledge_CR < 0)
                    {
                        for (int i = curr; i >= (curr + ledge_CR); i--)
                        {
                            if (i == (curr + ledge_CR))
                                Instantiate(DefaultGroundPrefab, origin + new Vector3(x * tileSize.x, 10 * (i + 1), y * tileSize.y), Quaternion.identity, TileParent);
                            else
                                Instantiate(
                                    i == curr ? CliffStraightTopPrefab : CliffStraightMidPrefab,
                                    origin + new Vector3((x + 1) * tileSize.x, 10 * i + 2.5f, y * tileSize.y),
                                    Quaternion.Euler(0, -90, 0), TileParent);
                        }
                    }

                    if (ledge_TL < 0)
                    {
                        for (int i = curr; i >= (curr + ledge_TL); i--)
                        {
                            if (i == (curr + ledge_TL))
                                Instantiate(DefaultGroundPrefab, origin + new Vector3(x * tileSize.x, 10 * (i + 1), y * tileSize.y), Quaternion.identity, TileParent);
                            else
                                Instantiate(
                                    i == curr ? CliffCornerTopPrefab : CliffCornerMidPrefab,
                                    origin + new Vector3(x * tileSize.x, 10 * i + 2.5f, y * tileSize.y),
                                    Quaternion.Euler(0, 0, 0), TileParent);
                        }
                    }
                    if (ledge_TR < 0)
                    {
                        for (int i = curr; i >= (curr + ledge_TR); i--)
                        {
                            if (i == (curr + ledge_TR))
                                Instantiate(DefaultGroundPrefab, origin + new Vector3(x * tileSize.x, 10 * (i + 1), y * tileSize.y), Quaternion.identity, TileParent);
                            else
                                Instantiate(
                                    i == curr ? CliffCornerTopPrefab : CliffCornerMidPrefab,
                                    origin + new Vector3((x + 1) * tileSize.x, 10 * i + 2.5f, y * tileSize.y),
                                    Quaternion.Euler(0, -90, 0), TileParent);
                        }
                    }
                    if (ledge_BL < 0)
                    {
                        for (int i = curr; i >= (curr + ledge_BL); i--)
                        {
                            if (i == (curr + ledge_BL))
                                Instantiate(DefaultGroundPrefab, origin + new Vector3(x * tileSize.x, 10 * (i + 1), y * tileSize.y), Quaternion.identity, TileParent);
                            else
                                Instantiate(
                                    i == curr ? CliffCornerTopPrefab : CliffCornerMidPrefab,
                                    origin + new Vector3(x * tileSize.x, 10 * i + 2.5f, (y + 1) * tileSize.y),
                                    Quaternion.Euler(0, 90, 0), TileParent);
                        }
                    }
                    if (ledge_BR < 0)
                    {
                        for (int i = curr; i >= (curr + ledge_BR); i--)
                        {
                            if (i == (curr + ledge_BR))
                                Instantiate(DefaultGroundPrefab, origin + new Vector3(x * tileSize.x, 10 * (i + 1), y * tileSize.y), Quaternion.identity, TileParent);
                            else
                                Instantiate(
                                    i == curr ? CliffCornerTopPrefab : CliffCornerMidPrefab,
                                    origin + new Vector3((x + 1) * tileSize.x, 10 * i + 2.5f, (y + 1) * tileSize.y),
                                    Quaternion.Euler(0, 180, 0), TileParent);
                        }
                    }
                }
                else
                {
                    _tileMap[x, y] = Instantiate(TilePrefab, origin + new Vector3(x * tileSize.x, 10 * _heightMap[x, y], y * tileSize.y), Quaternion.identity, TileParent).GetComponent<Tile>();
                    _tileMap[x, y].Init(new Vector2Int(x, y), groundPrefab);
                }

                //if (x > 0) _tileMap[x, y].AddNeighbour(_tileMap[x - 1, y], Direction.W);
                //if (y > 0)
                //{
                //    _tileMap[x, y].AddNeighbour(_tileMap[x, y -1], Direction.S);

                //    if(x > 0) _tileMap[x, y].AddNeighbour(_tileMap[x - 1, y - 1], Direction.SW);
                //    if(x < _tileMap.GetLength(0) - 1) _tileMap[x, y].AddNeighbour(_tileMap[x + 1, y - 1], Direction.SE);
                //}
            }
        }
    }

    public void CleanUpMap()
    {
        if (_tileMap == null) return;
        for(int y = 0; y < _tileMap.GetLength(1); y++)
        {
            for (int x = 0; x < _tileMap.GetLength(0); x++)
            {
                if(_tileMap[x,y] != null) Destroy(_tileMap[x, y]);
            }
        }
    }

    public void UpdateAllTextRotation(float textRotationSpeed)
    {
        for (int y = 0; y < _tileMap.GetLength(1); y++)
        {
            for (int x = 0; x < _tileMap.GetLength(0); x++)
            {
                _tileMap[x, y].RotateTextToFaceCamera(textRotationSpeed);
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _tileMap.GetLength(0) || y >= _tileMap.GetLength(1)) return null;
        return _tileMap[x, y];
    }

    public static void HighlightTile(Tile tile)
    {
        if (tile == null) projectorRig.gameObject.SetActive(false);
        else
        {
            projectorRig.gameObject.SetActive(true);
            projectorRig.position = new Vector3(tile.transform.position.x, projectorRig.position.y, tile.transform.position.z);
        }
    }
}
