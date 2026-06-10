using UnityEngine;
using UnityEngine.AddressableAssets;

public class CharacterListLoader : MonoBehaviour
{
    async void Start()
    {
        // carico una collezione di asset, l'intera categoria tramite label
        // con null gli dico che non mi servono callback, perchè tanto aspetto solo che sia pronto
        var handle = Addressables.LoadAssetsAsync<GameObject>("Characters", null);

        await handle.Task;

        int x = 0;

        foreach(var character in handle.Result)
        {
            Instantiate(character, new Vector3(x*2,0,0), Quaternion.identity);
            x++;
        }

        // rilascio le reference degli asset, ma gli asset non scompaiono dalla scena
        // libera la memoria dagli Addressable caricati, non i GameObject in scena
        Addressables.Release(handle);
    }
}
