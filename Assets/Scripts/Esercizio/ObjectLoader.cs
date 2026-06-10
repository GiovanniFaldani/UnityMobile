using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectLoader : MonoBehaviour
{
    public static ObjectLoader Instance { get; private set; }

    [HideInInspector] public bool ready = false;
    [HideInInspector] public List<GameObject> scoreItems;
    [HideInInspector] public GameObject bonusItem;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    async void Start()
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>("ScoreItems", null);

        var handle2 = Addressables.LoadAssetAsync<GameObject>("Bonus");

        await handle.Task;
        await handle2.Task;

        scoreItems = new List<GameObject>(handle.Result);
        bonusItem = handle2.Result;

        ready = true;

        // GameObjects have been loaded, release handle
        handle.Release();
        handle2.Release();
    }


}
