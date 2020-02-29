using UnityEngine;
using TMPro;

public enum Direction { N = 0, NE = 1, E = 2, SE = 3, S = 4, SW = 5, W = 6, NW = 7}

public class Tile : MonoBehaviour
{
    public Tile[] neighbours;
    public Transform modelsParent;

    private Vector2Int indexPosition;
    private Transform groundModel;
    private TextMeshProUGUI textMesh;

    private int landValue;

    public void InitBoardTile(Vector2Int indexPosition, CardDefinition cardDefinition = null)
    {
        this.indexPosition = indexPosition;
        neighbours = new Tile[8];
        textMesh = this.GetComponentInChildren<TextMeshProUGUI>();

        PlaceCard(cardDefinition);
    }

    public void AddNeighbour(Tile tile, Direction direction)
    {
        if (tile == null) return;
        neighbours[(int)direction] = tile;
        tile.neighbours[((int)direction + 4) % 8] = this;
    }

    public void PlaceCard(CardDefinition card)
    {
        UpdateGroundModel(card);
    }

    public void UpdateGroundModel(CardDefinition card)
    {
        if (groundModel != null) Destroy(groundModel.gameObject);
        if (card == null || card.groundModel == null) groundModel = Instantiate(Board.defaultCardDefination.groundModel, modelsParent.position, Quaternion.identity, modelsParent).transform;
        else groundModel = Instantiate(card.groundModel, modelsParent.position, Quaternion.identity, modelsParent).transform;
    }

    public void SetLandValue(int value)
    {
        landValue = value;
        textMesh.text = "";
        if (value != 0)  textMesh.text += value;
    }

    public int GetLandValue()
    {
        return landValue;
    }
}
