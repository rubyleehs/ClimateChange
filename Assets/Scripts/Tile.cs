using UnityEngine;

public enum Direction { N = 0, NE = 1, E = 2, SE = 3, S = 4, SW = 5, W = 6, NW = 7}

public class Tile : MonoBehaviour
{
    public Tile[] neighbours;
    private Vector2Int indexPosition;

    public void InitBoardTile(Vector2Int indexPosition)
    {
        this.indexPosition = indexPosition;
        neighbours = new Tile[8];
    }

    public void AddNeighbour(Tile tile, Direction direction)
    {
        if (tile == null) return;
        neighbours[(int)direction] = tile;
        tile.neighbours[((int)direction + 4) % 8] = this;
    }
}
