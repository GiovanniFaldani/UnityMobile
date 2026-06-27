using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare
{
    public bool occupied = false;
    public Stack<Ingredient> ingredientStack = new Stack<Ingredient>();
    public int gridX;
    public int gridY;
    public Vector3 worldPosition;

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

    public void EmptyStack()
    {
        ingredientStack.Clear();
        occupied = false;
    }

    // Sposta gli ingredienti facendo pop della sorgente e pushandola sulla propria stack
    public List<Ingredient> PushToStackFromSquare(GridSquare toFree, Vector2 swipeDir)
    {
        if (!occupied) return null;

        TouchManager.Instance.SetAllowTouch(false);

        bool bunPresent = false;
        bool bunsPresent = false;

        // controlla condizione vittoria in modo simmetrico tra destinazione e sorgente
        List<Ingredient> myIngredients = new List<Ingredient>(ingredientStack);
        List<Ingredient> sourceIngredients = new List<Ingredient>(toFree.ingredientStack);
        foreach (Ingredient ing1 in sourceIngredients)
        {
            if (ing1 is Bun)
            {
                bunPresent = true;
                foreach (Ingredient ing2 in myIngredients)
                {
                    if (ing2 is Bun)
                    {
                        bunsPresent = true;
                        break;
                    }
                }
            }
        }

        if (bunsPresent)
        {
            // unisci gli ingredienti dei due bun in un solo set
            HashSet<Ingredient> ingredientsOnBuns = new HashSet<Ingredient>(myIngredients);
            ingredientsOnBuns.UnionWith(sourceIngredients);

            if (GameManagerEsame.Instance.CheckCompleteness(ingredientsOnBuns))
            {
                while (toFree.ingredientStack.Count > 0)
                {
                    Ingredient ing = toFree.ingredientStack.Pop();
                    ing.MoveToSquare(this, swipeDir);
                    ingredientStack.Push(ing);
                }
                toFree.occupied = false;

                // Caso vittoria, chiama async il next livello e mostra schermata vittoria
                // Debug.Log("Victory.");
                GameManagerEsame.Instance.WinGame();
                return sourceIngredients;
            }
            else
            {
                TouchManager.Instance.SetAllowTouch(true);
                // Debug.Log("Buns are present but level not complete yet.");
                return null;
            }
        }
        else if (bunPresent)
        {
            TouchManager.Instance.SetAllowTouch(true);
            // Debug.Log("You can't move a bun unless all ingredients are placed on them!");
            return null;
        }
        else
        {
            // Debug.Log($"Moving ingredients from {toFree.gridX},{toFree.gridY} to {gridX},{gridY}");
            while (toFree.ingredientStack.Count > 0)
            {
                Ingredient ing = toFree.ingredientStack.Pop();
                ing.MoveToSquare(this, swipeDir);
                ingredientStack.Push(ing);
            }
            toFree.occupied = false;
            TouchManager.Instance.SetAllowTouch(true);
            return sourceIngredients;
        }
    }
}
