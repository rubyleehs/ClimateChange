﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Direction { N = 0, NE = 1, E = 2, SE = 3, S = 4, SW = 5, W = 6, NW = 7};
public static class DirectionHelper
{
    public static Vector2Int ConvertToVector2Int(Direction direction)
    {
        Vector2Int delta = Vector2Int.zero;
        int dir = (int)direction;
        if (dir > 0 && dir < 4) delta += Vector2Int.right;
        else if (dir > 4) delta += Vector2Int.left;

        if (dir > 2 && dir < 6) delta += Vector2Int.down;
        else if (dir < 2 || dir > 6) delta += Vector2Int.up;

        return delta;
    }
}

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
    public TextMeshProUGUI _economyChangeTextMesh;
    public TextMeshProUGUI _environmentChangeTextMesh;
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
            var change = value - _environmentalValue;
            _environmentalValue = value;

            if (change != 0)
            {
                _economyChangeTextMesh.text = $"{change:+#;-#;0}";

                var transform = _economyChangeTextMesh.GetComponent<RectTransform>();
                StartCoroutine(Animation.Tween(3f,
                    (t) => { transform.localPosition = Vector3.Lerp(new Vector3(0, -20, 0), new Vector3(0, -5, 0), t); },
                    Animation.EaseOutCubic));

                var gradient = GetGradientFromTone(GetColorFromPopupValue(change));
                StartCoroutine(Animation.Tween(3f,
                    (t) => { _economyChangeTextMesh.color = Color32.Lerp(gradient.opaque, gradient.transparent, t); },
                    Animation.EaseOutCubic));
            }

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
            var change = value - _environmentalValue;
            _environmentalValue = value;

            if (change != 0)
            {
                _environmentChangeTextMesh.text = $"{change:+#;-#;0}";

                var transform = _environmentChangeTextMesh.GetComponent<RectTransform>();
                StartCoroutine(Animation.Tween(3f,
                    (t) => { transform.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 15, 0), t); },
                    Animation.EaseOutCubic));

                var gradient = GetGradientFromTone(GetColorFromPopupValue(change));
                StartCoroutine(Animation.Tween(3f,
                    (t) => { _environmentChangeTextMesh.color = Color32.Lerp(gradient.opaque, gradient.transparent, t); },
                    Animation.EaseOutCubic));
            }

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

    private Color32 GetColorFromPopupValue(int value)
        => value >= 0 ? new Color32(0, 212, 53, 255) : new Color32(212, 0, 10, 255);
    private (Color32 opaque, Color32 transparent) GetGradientFromTone(Color32 tone)
        => (opaque: new Color32(tone.r, tone.g, tone.b, 255), transparent: new Color32(tone.r, tone.g, tone.b, 0));

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
        GameManager.Instance.Board.HighlightTile(this);
    }

    void IMouseOverable.OnMouseExit()
    {
        GameManager.Instance.Board.HighlightTile(null);   
    }
}
