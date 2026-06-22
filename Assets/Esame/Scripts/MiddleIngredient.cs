using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MiddleIngredient : Ingredient
{
    private new void Start()
    {
        base.Start();
    }

    public new void MoveToSquare(GridSquare destination)
    {
        base.MoveToSquare(destination);
    }
}
