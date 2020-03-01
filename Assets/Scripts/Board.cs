using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject TilePrefab;
    public GameObject DefaultGroundPrefab;

    public Transform TileParent;

    public Vector2Int MapResolution;
    public Vector3 Origin;
    public Vector2 TileSize;

    public Transform projectorRig;

    private Tile[,] _map;

    private List<(CardEffect cardEffect, Card card, Tile tile)> _cardEffects;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
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
        _map = new Tile[mapResolution.x, mapResolution.y];
        for (int y = 0; y < _map.GetLength(1); y++)
        {
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                _map[x, y] = Instantiate(TilePrefab, origin + new Vector3(x * tileSize.x, 0, y * tileSize.y), Quaternion.identity, TileParent).GetComponent<Tile>();
                _map[x, y].Init(new Vector2Int(x, y), groundPrefab);

                if (x > 0) _map[x, y].AddNeighbour(_map[x - 1, y], Direction.W);
                if (y > 0)
                {
                    _map[x, y].AddNeighbour(_map[x, y -1], Direction.S);

                    if(x > 0) _map[x, y].AddNeighbour(_map[x - 1, y - 1], Direction.SW);
                    if(x < _map.GetLength(0) - 1) _map[x, y].AddNeighbour(_map[x + 1, y - 1], Direction.SE);
                }
            }
        }
    }

    public void CleanUpMap()
    {
        if (_map == null) return;
        for(int y = 0; y < _map.GetLength(1); y++)
        {
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                if(_map[x,y] != null) Destroy(_map[x, y]);
            }
        }
    }

    public void UpdateAllTextRotation(float textRotationSpeed)
    {
        for (int y = 0; y < _map.GetLength(1); y++)
        {
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                _map[x, y].RotateTextToFaceCamera(textRotationSpeed);
            }
        }
    }

    public Tile GetTile(Vector2Int index)
    {
        return GetTile(index.x, index.y);
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _map.GetLength(0) || y >= _map.GetLength(1)) return null;
        return _map[x, y];
    }

    public void HighlightTile(Tile tile)
    {
        if (tile == null) projectorRig.gameObject.SetActive(false);
        else
        {
            projectorRig.gameObject.SetActive(true);
            projectorRig.position = new Vector3(tile.transform.position.x, projectorRig.position.y, tile.transform.position.z);
        }
    }
}
