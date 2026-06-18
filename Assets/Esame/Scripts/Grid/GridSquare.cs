using UnityEngine;

public class GridSquare
{
    public bool occupied = false;
    public int gridX;
    public int gridY;
    public Vector3 worldPosition;

    public GridSquare(Vector3 _worldPosition, int _gridX, int _gridY)
    {
        this.worldPosition = _worldPosition;
        this.gridX = _gridX;
        this.gridY = _gridY;
    }
}
