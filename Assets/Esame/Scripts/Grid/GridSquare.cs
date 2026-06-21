using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare
{
    public bool occupied = false;
    public Stack<GameObject> squareStack = new Stack<GameObject>();
    public int gridX;
    public int gridY;
    public Vector3 worldPosition;

    public GridSquare(Vector3 _worldPosition, int _gridX, int _gridY)
    {
        this.worldPosition = _worldPosition;
        this.gridX = _gridX;
        this.gridY = _gridY;
    }

    public void PushToStack(GameObject toPush)
    {
        squareStack.Push(toPush);
    }

    public GameObject PopFromStack()
    {
        return squareStack.Pop();
    }

    // prende uno square in input che viene liberato
    // facendo pop della sua stack e pushandola in quello chiamante
    public void PushToStackFromSquare(GridSquare toFree)
    {
        while (toFree.squareStack.Count > 0)
        {
            squareStack.Push(toFree.squareStack.Pop());
        }
        toFree.occupied = false;
    }
}
