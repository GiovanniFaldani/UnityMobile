using UnityEngine;
using UnityEngine.AddressableAssets;    // ci prendiamo l'AssetReference

public class CharacterSpawner : MonoBehaviour
{
    // AssetReference è un ogetto che contiene le info necessarie a identificare un Addressable
    // contiene: Address, GUID e Metadata ed è un collegamento sicuro verso l'Addressable
    public AssetReference character;

    async void Start()
    {
        // evita l'import e compila più veloce
        var handle = character.LoadAssetAsync<GameObject>();

        await handle.Task;

        Instantiate(handle.Result);
    }
}
