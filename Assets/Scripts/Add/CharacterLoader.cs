using UnityEngine;
using UnityEngine.AddressableAssets;    // Ci permette di prenderci Addressables e AssetReference
using UnityEngine.ResourceManagement.AsyncOperations;   // Ci permette di usare AyncOperationHandler<t> e LoadAssetAsync
public class CharacterLoader : MonoBehaviour
{
    // Con async indico che posso aspettare operazioni lunghe senza bloccarei l gioco
    async void Start()
    {
        // Quando l'operazione che stiamo facendo è pronta, voglio come valore di ritorno un GameObject
        // carico un asset in modo asincrono --> prefab
        // mi prendo l'address che abbiamo rinominato nel catalogo
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("Knight");

        // Anche dopo la riga sopra, non è detto che il prefab sia stato caricato
        // perciò attendo che il caricamento sia finito (vado oltre questa riga solo dopo, senza bloccare niente)
        await handle.Task;

        // istanzio in scena il prefab che ho cercato di caricare finora
        Instantiate(handle.Result);
    }
}
