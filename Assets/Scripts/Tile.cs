using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Direction { N = 0, NE = 1, E = 2, SE = 3, S = 4, SW = 5, W = 6, NW = 7 };

public class Tile : MonoBehaviour, IClickable, IMouseOverable
{
    public Transform ModelParent;

    private Vector2Int _indexPosition;
    public Tile[] _neighbours;
    public Vector2Int Position => new Vector2Int(_indexPosition.x, _indexPosition.y);

    private Transform _groundModel;
    private Transform _placedModel;
    public TextMeshProUGUI _economyTextMesh;
    public TextMeshProUGUI _environmentTextMesh;
    private Transform _textRotator;
    
    private Quaternion textStartRot;
    private IEnumerator textRotaterRoutine;

    public bool isEnabled = true;

    private int _economicValue;
    public int EconomicValue
    {
        get => _economicValue;
        set
        {
            _economicValue = value;
            _economyTextMesh.text = "";
            if (value != 0 && isEnabled) _economyTextMesh.text += value;
        }
    }

    private int _environmentalValue;
    public int EnvironmentalValue
    {
        get => _environmentalValue;
        set
        {
            _environmentalValue = value;
            _environmentTextMesh.text = "";
            if (value != 0 && isEnabled) _environmentTextMesh.text += value;
        }
    }

    public void Init(Vector2Int indexPosition, GameObject groundPrefab)
    {
        _indexPosition = indexPosition;
        _neighbours = new Tile[8];
        _textRotator = _economyTextMesh.transform.parent;
        textStartRot = _textRotator.transform.rotation;
        
        UpdateGroundModel(groundPrefab);
    }

    public void AddNeighbour(Tile tile, Direction direction)
    {
        if (tile == null) return;
        _neighbours[(int)direction] = tile;
        tile._neighbours[((int)direction + 4) % 8] = this;
    }

    public void UpdateGroundModel(GameObject model)
    {
        if (model == null) return;

        if (_groundModel != null) Destroy(_groundModel.gameObject);
        _groundModel = Instantiate(model, ModelParent.position, Quaternion.identity, ModelParent).transform;
    }

    public void UpdatePlacedModel(GameObject model)
    {
        if (model == null) return;

        if (_placedModel != null) Destroy(_placedModel.gameObject);
        _placedModel = Instantiate(model, ModelParent.position, Quaternion.identity, ModelParent).transform;
    }

    public void RotateTextToFaceCamera(float rotationSpeed)
    {
        if (textRotaterRoutine != null)
            StopCoroutine(textRotaterRoutine);

        textRotaterRoutine = RotateTextToFaceCameraRoutine(rotationSpeed);
        StartCoroutine(textRotaterRoutine);
    }

    private IEnumerator RotateTextToFaceCameraRoutine(float rotationSpeed)
    {
        Quaternion targetRot = textStartRot * Quaternion.Euler(Vector3.forward * -45 * (int)MainCameraControl.camFaceDirection);
        while (_textRotator.transform.rotation != targetRot)
        {
            _textRotator.transform.rotation = Quaternion.RotateTowards(_textRotator.transform.rotation, targetRot, rotationSpeed * Time.deltaTime * 1.4f);
            yield return new WaitForEndOfFrame();
        }
        textRotaterRoutine = null;
    }

    public void OnClickUp()
    {
        if (!isEnabled) return;
        Debug.Log($"Tile at (x: {_indexPosition.x}, y: {_indexPosition.y}) selected.");

        if (Card.IsAnyCardBeingDragged)
        {
            var card = Card.CurrentlyDraggedCard;
            card.PlayOn(this);
        }
    }
    public void OnClickDown() { }
    public void OnClick() { }

    void IMouseOverable.OnMouseEnter()
    {
        
    }

    void IMouseOverable.OnMouseOver()
    {
        if (!isEnabled) return;
        Board.HighlightTile(this);
    }

    void IMouseOverable.OnMouseExit()
    {
        Board.HighlightTile(null);   
    }
}
