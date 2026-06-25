using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwipeable
{
    public void MoveToSquare(GridSquare destination, Vector2 swipeDir);

}
