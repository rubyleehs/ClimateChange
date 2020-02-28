using UnityEngine;

public enum Direction { N = 0, NE = 1, E = 2, SE = 3, S = 4, SW = 5, W = 6, NW = 7}

public class Tile : MonoBehaviour
{
    public Tile[] neighbours;
    public Transform modelsParent;

    private Vector2Int indexPosition;
    private Transform groundModel;

    public void InitBoardTile(Vector2Int indexPosition, CardDefinition cardDefinition = null)
    {
        this.indexPosition = indexPosition;
        neighbours = new Tile[8];

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
        if (card == null || card.tileModel == null) groundModel = Instantiate(Board.defaultCardDefination.tileModel, modelsParent.position, Quaternion.identity, modelsParent).transform;
        else groundModel = Instantiate(card.tileModel, modelsParent.position, Quaternion.identity, modelsParent).transform;
    }
}
