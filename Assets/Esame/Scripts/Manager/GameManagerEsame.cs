using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagerEsame : MonoBehaviour
{
    public static GameManagerEsame Instance { get ; private set; }

    private Dictionary<GameObject, Vector3> startState = new Dictionary<GameObject, Vector3>();

    private HashSet<Ingredient> middleIngredientsInScene = new HashSet<Ingredient>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        // Comincia piazzando 2 bun in due punti random adiacenti, poi genera N ingredienti attorno
        // Enumera tutti gli ingredienti per controllare se il livello è finito con un set quando i due bun sono uniti

        middleIngredientsInScene = FindObjectsByType<MiddleIngredient>(FindObjectsSortMode.None).Cast<Ingredient>().ToHashSet();

        // salvataggio dello start state per reset
        startState.Clear();
        Ingredient[] ingredientsInScene = FindObjectsByType<Ingredient>(FindObjectsSortMode.None);
        foreach (Ingredient ing in ingredientsInScene)
        {
            Vector3 startPosition = new Vector3(
                ing.gameObject.transform.position.x,
                ing.gameObject.transform.position.y,
                ing.gameObject.transform.position.z
            );
            startState.Add(ing.gameObject, startPosition);
            Debug.Log("Added to startState: " + ing.name + " at position " + startPosition);
        }
    }

    public bool CheckCompleteness(GridSquare destination)
    {
        // controlla se la destinazione dello swipe contiene tutti gli ingredienti del livello
        HashSet<Ingredient> ingredientsOnDestination = destination.ingredientStack.ToHashSet();

        if(middleIngredientsInScene.IsSubsetOf(ingredientsOnDestination))
        {
            Debug.Log("Level complete!");
            return true;
        }
        Debug.Log("Level not complete yet.");
        return false;
    }

    public void ResetGame()
    {
        Debug.Log("Resetting game to start state...");
        // Reset del livello, riporta allo stato di partenza
        foreach (KeyValuePair<GameObject, Vector3> kvp in startState)
        {
            Debug.Log("Resetting ingredient: " + kvp.Key.name + " to position " + kvp.Value);
            GameObject ing = kvp.Key;
            Vector3 startPosition = kvp.Value;

            // Reset transform
            ing.transform.position = startPosition;
            ing.transform.rotation = Quaternion.identity;

            // Reset dello stack su GridSquare
            GridSquare closestSquare = FindFirstObjectByType<MyGrid>().GetGridSnap(startPosition);
            closestSquare.EmptyStack();
            closestSquare.PushToStack(ing.GetComponent<Ingredient>());
        }
    }
}
