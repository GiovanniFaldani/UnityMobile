using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManagerEsame : MonoBehaviour
{
    public static GameManagerEsame Instance { get ; private set; }

    private Dictionary<GameObject, Vector3> startState = new Dictionary<GameObject, Vector3>();

    private HashSet<Ingredient> middleIngredientsInScene = new HashSet<Ingredient>();

    [SerializeField] private GameObject bunPrefab;
    [SerializeField] private List<GameObject> middleIngredientsPrefabs = new List<GameObject>();

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
        // cancella tutto ciò che c'era in precedenza
        Ingredient[] ingredientsInScene = FindObjectsByType<Ingredient>(FindObjectsSortMode.None);
        foreach (Ingredient ing in ingredientsInScene) Destroy(ing.gameObject);
        ingredientsInScene = new Ingredient[] { };
        middleIngredientsInScene.Clear();
        startState.Clear();

        // Comincia piazzando 2 bun in due punti random adiacenti, poi genera N ingredienti attorno
        MyGrid gameGrid = FindAnyObjectByType<MyGrid>();
        int xSize = gameGrid.grid.GetLength(0);
        int ySize = gameGrid.grid.GetLength(1);
        HashSet<Vector2> availablePositions = new HashSet<Vector2>();
        HashSet<Vector2> takenpositions = new HashSet<Vector2>();
        for(int x = 0; x < xSize; x++)
        {
           for(int y = 0; y < ySize; y++)
            {
                availablePositions.Add(new Vector2(x, y));
                gameGrid.grid[x, y].EmptyStack();
            }
        }

        // Determina un punto di partenza casuale
        Vector2 startPoint = new Vector2(
            Random.Range(0, xSize),
            Random.Range(0, ySize)
        );

        GameObject bun1 = Instantiate(bunPrefab, gameGrid.grid[(int)startPoint.x, (int)startPoint.y].worldPosition, Quaternion.identity);
        startState.Add(bun1, gameGrid.grid[(int)startPoint.x, (int)startPoint.y].worldPosition);
        availablePositions.Remove(startPoint);
        takenpositions.Add(startPoint);

        // Aggiunge un altro panino in una casella adiacente, uso list comprehension se no sta roba prende 20 righe
        var adjacentFreePoints = from vec in availablePositions where Vector2.Distance(startPoint, vec) <= 1 && !takenpositions.Contains(vec) select vec;
        Vector2 secondStartPoint = adjacentFreePoints.ToList()[Random.Range(0, adjacentFreePoints.Count())];

        GameObject bun2 = Instantiate(bunPrefab, gameGrid.grid[(int)secondStartPoint.x, (int)secondStartPoint.y].worldPosition, Quaternion.identity);
        startState.Add(bun2, gameGrid.grid[(int)secondStartPoint.x, (int)secondStartPoint.y].worldPosition);
        availablePositions.Remove(secondStartPoint);
        takenpositions.Add(secondStartPoint);

        // spawna un numero casuale di altri ingredienti
        int ingredientNumber = Random.Range(4, 15);

        for(int i = 0; i < ingredientNumber; i++)
        {
            int randomIndex = Random.Range(0, takenpositions.Count);
            Vector2 refPoint = takenpositions.ElementAt(randomIndex);
            adjacentFreePoints = from vec in availablePositions where Vector2.Distance(refPoint, vec) <= 1 && !takenpositions.Contains(vec) select vec;

            if(adjacentFreePoints.Count() <= 0)
            {
                i--;
                continue;
            }

            GameObject middleIngredientPrefab = middleIngredientsPrefabs[Random.Range(0, middleIngredientsPrefabs.Count)];
            Vector2 ingredientPoint = adjacentFreePoints.ToList()[Random.Range(0, adjacentFreePoints.Count())];

            GameObject ingredient = Instantiate(middleIngredientPrefab, gameGrid.grid[(int)ingredientPoint.x, (int)ingredientPoint.y].worldPosition, Quaternion.identity);
            startState.Add(ingredient, gameGrid.grid[(int)ingredientPoint.x, (int)ingredientPoint.y].worldPosition);
            middleIngredientsInScene.Add(ingredient.GetComponent<MiddleIngredient>());
            availablePositions.Remove(ingredientPoint);
            takenpositions.Add(ingredientPoint);
        }

        // Enumera tutti gli ingredienti per, in futuro, controllare se il livello è finito con un set quando i due bun sono uniti
        Debug.Log($"Middle Ingredients In Scene Size: {middleIngredientsInScene.Count}");
    }

    public bool CheckCompleteness(HashSet<Ingredient> ingredientsOnBuns)
    {
        // controlla se il set in input contiene tutti gli ingredienti del livello
        if(middleIngredientsInScene.IsSubsetOf(ingredientsOnBuns)) return true;

        return false;
    }

    public void ResetGame()
    {
        // Debug.Log("Resetting game to start state...");
        // Reset del livello, riporta allo stato di partenza
        Debug.Log($"Start State size: {startState.Count}");
        foreach (KeyValuePair<GameObject, Vector3> kvp in startState)
        {
            // Debug.Log("Resetting ingredient: " + kvp.Key.name + " to position " + kvp.Value);
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
