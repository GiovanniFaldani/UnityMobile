using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;

public class GameManagerEsame : MonoBehaviour
{
    public static GameManagerEsame Instance { get ; private set; }

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

    public void GenerateLevel()
    {
        // Comincia piazzando 2 bun in due punti random adiacenti, poi genera N ingredienti attorno
        // Enumera tutti gli ingredienti per controllare se il livello è finito con un set quando i due bun sono uniti

        middleIngredientsInScene = FindObjectsByType<MiddleIngredient>(FindObjectsSortMode.None).Cast<Ingredient>().ToHashSet();
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
        // Reset del livello, distruggi e rigenera
    }
}
