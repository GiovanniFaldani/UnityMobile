using UnityEngine;

public class Bun : Ingredient
{
    private new void Start()
    {
        base.Start();
    }

    public new void MoveToSquare(GridSquare destination, Vector2 swipeDir)
    {
        base.MoveToSquare(destination, swipeDir);
    }
}
