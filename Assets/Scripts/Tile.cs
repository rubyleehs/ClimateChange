using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Direction { N = 0, NE = 1, E = 2, SE = 3, S = 4, SW = 5, W = 6, NW = 7 };

public class Tile : MonoBehaviour
{
    public Transform ModelParent;

    private Vector2Int _indexPosition;
    private Tile[] _neighbours;

    private Transform _groundModel;
    private TextMeshProUGUI _textMesh;

    private int _landValue;
    public int LandValue
    {
        get => _landValue;
        set
        {
            _landValue = value;
            _textMesh.text = "";
            if (value != 0) _textMesh.text += value;
        }
    }

    public void Init(Vector2Int indexPosition, GameObject groundPrefab)
    {
        _indexPosition = indexPosition;
        _neighbours = new Tile[8];
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();

        UpdateGroundModel(groundPrefab);
    }

    public void AddNeighbour(Tile tile, Direction direction)
    {
        if (tile == null) return;
        _neighbours[(int)direction] = tile;
        tile._neighbours[((int)direction + 4) % 8] = this;
    }

    private void UpdateGroundModel(GameObject ground)
    {
        if (ground == null)
            throw new ArgumentException("A valid ground model has to be passed in.");

        if (_groundModel != null) Destroy(_groundModel.gameObject);
        _groundModel = Instantiate(ground, ModelParent.position, Quaternion.identity, ModelParent).transform;
    }
}
