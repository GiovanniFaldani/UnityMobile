using System.Collections.Generic;
using UnityEngine;

public class GridSquare
{
    public bool occupied = false;
    public Stack<Ingredient> ingredientStack = new Stack<Ingredient>();
    public int gridX;
    public int gridY;
    public Vector3 worldPosition;

    private float heightStep = 0.1f;

    public GridSquare(Vector3 _worldPosition, int _gridX, int _gridY)
    {
        this.worldPosition = _worldPosition;
        this.gridX = _gridX;
        this.gridY = _gridY;
    }

    public void PushToStack(Ingredient toPush)
    {
        ingredientStack.Push(toPush);
        occupied = true;
    }

    // facendo pop della sua stack e pushandola in quello chiamante
    public void PushToStackFromSquare(GridSquare toFree, Vector2 swipeDir)
    {
        if (!occupied) return;

        TouchManager.Instance.SetAllowTouch(false);

        bool bunsPresent = false;

        List<Ingredient> myIngredients = new List<Ingredient>(ingredientStack);
        foreach (Ingredient ingredient in myIngredients)
        {
            if (ingredient is Bun)
            {
                List<Ingredient> sourceIngredients = new List<Ingredient>(toFree.ingredientStack);
                foreach (Ingredient ing in sourceIngredients)
                {
                    if (ing is Bun)
                    {
                        bunsPresent = true;
                        break;
                    }
                }
            }
        }

        if (bunsPresent)
        {
            if (GameManagerEsame.Instance.CheckCompleteness(this))
            {
                while (toFree.ingredientStack.Count > 0)
                {
                    Ingredient ing = toFree.ingredientStack.Pop();
                    ing.MoveToSquare(this);
                    ingredientStack.Push(ing);
                }
                toFree.occupied = false;

                // Caso vittoria, chiama async il reset del livello
            }
            else
            {
                TouchManager.Instance.SetAllowTouch(true);
                Debug.Log("Bun is present but level not complete yet.");
            }
        }
        else
        {
            Debug.Log($"Moving ingredients from {toFree.gridX},{toFree.gridY} to {gridX},{gridY}");
            while (toFree.ingredientStack.Count > 0)
            {
                Ingredient ing = toFree.ingredientStack.Pop();
                ing.MoveToSquare(this);
                ingredientStack.Push(ing);
            }
            toFree.occupied = false;
        }
    }
}
