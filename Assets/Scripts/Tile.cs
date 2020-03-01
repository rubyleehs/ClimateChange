using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Direction { N = 0, NE = 1, E = 2, SE = 3, S = 4, SW = 5, W = 6, NW = 7 };

public class Tile : MonoBehaviour, IClickable
{
    public Transform ModelParent;

    private Vector2Int _indexPosition;
    public Tile[] _neighbours;
    public Vector2Int Position => new Vector2Int(_indexPosition.x, _indexPosition.y);

    private Transform _groundModel;
    private Transform _placedModel;
    private TextMeshProUGUI _textMesh;

    private Quaternion textStartRot;
    private IEnumerator textRotaterRoutine;

    private int _economicValue;
    public int EconomicValue
    {
        get => _economicValue;
        set
        {
            _economicValue = value;
            _textMesh.text = "";
            if (value != 0) _textMesh.text += value;
        }
    }

    private int _environmentalValue;
    public int EnvironmentalValue
    {
        get => _environmentalValue;
        set
        {
            _environmentalValue = value;
            _textMesh.text = "";
            if (value != 0) _textMesh.text += value;
        }
    }

    public void Init(Vector2Int indexPosition, GameObject groundPrefab)
    {
        _indexPosition = indexPosition;
        _neighbours = new Tile[8];
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textStartRot = _textMesh.transform.rotation;

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
        while (_textMesh.transform.rotation != targetRot)
        {
            _textMesh.transform.rotation = Quaternion.RotateTowards(_textMesh.transform.rotation, targetRot, rotationSpeed * Time.deltaTime * 1.4f);
            yield return new WaitForEndOfFrame();
        }
        textRotaterRoutine = null;
    }

    public void OnClickUp()
    {
        Debug.Log($"Tile at (x: {_indexPosition.x}, y: {_indexPosition.y}) selected.");

        if (Card.IsAnyCardBeingDragged)
        {
            var card = Card.CurrentlyDraggedCard;
            card.PlayOn(this);
        }
    }
    public void OnClickDown() { }
    public void OnClick() { }
}
