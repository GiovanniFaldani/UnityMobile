using UnityEngine;

public class GameManagerEsame : MonoBehaviour
{
    public GameManagerEsame Instance { get ; private set; }

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
        // Enumaera tutti gli ingredienti per controllare se il livello è finito con un set quando i due bun sono uniti
    }
}
