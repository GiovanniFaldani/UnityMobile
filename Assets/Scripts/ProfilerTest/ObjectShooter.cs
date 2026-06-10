using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ObjectShooter : MonoBehaviour
{
    public static ObjectShooter Instance { get; private set; }

    [SerializeField] float fireRate = 2f;
    [SerializeField] GameObject projectileObject;
    [SerializeField] public bool useMultiProjectile;
    [SerializeField] GameObject[] multiProjectileObjects;
    [SerializeField] Transform projectileStart;

    [SerializeField] public bool usePooling;
    [SerializeField] int poolSize = 20;
    [SerializeField] Transform poolStart;

    [SerializeField] TMP_Text toggleText;

    private float fireTimer;
    private float fireWait = 0f;

    // Pool queue for ready objects
    Queue<GameObject> PoolQueue;

    // Pool queues for different projectiles
    List<Queue<GameObject>> PoolQueues = new List<Queue<GameObject>>();

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

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
        if (usePooling)
        {
            if (useMultiProjectile)
                InitializeMultiPool();
            else
                InitializePool();
        }

        fireTimer = 1 / fireRate;
    }

    private void Update()
    {
        if (useMultiProjectile)
            ShootMultiProjectiles();
        else
            ShootSingleProjectile();
    }

    private void ShootSingleProjectile()
    {
        fireWait += Time.deltaTime;

        if (fireWait < fireTimer) return;

        fireWait = 0f;

        // se non tappo da nessuna parte, skippo
        if (Touch.activeTouches.Count <= 0) return;

        // mi prendo il primo tocco 
        Touch touch = Touch.activeTouches[0];

        // me lo prendo quando inizia e quando rimane
        if (touch.phase != TouchPhase.Began && touch.phase != TouchPhase.Stationary && touch.phase != TouchPhase.Moved) 
            return;

        if (usePooling)
        {
            GameObject item = GetFromPool();
            item.transform.position = projectileStart.position;
            item.transform.rotation = projectileStart.rotation;
            item.SetActive(true);
        }
        else
        {
            GameObject item = InstantiateNewItem();
        }
    }

    private void ShootMultiProjectiles()
    {
        fireWait += Time.deltaTime;

        if (fireWait < fireTimer) return;

        fireWait = 0f;

        // se non tappo da nessuna parte, skippo
        if (Touch.activeTouches.Count <= 0) return;

        // Controllo quante dita stanno toccando
        int touchNumber = 0;

        foreach(Touch touch in Touch.activeTouches)
        {
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                touchNumber++;
        }

        touchNumber = Mathf.Clamp(touchNumber, 0, multiProjectileObjects.Length);
        if (touchNumber == 0) return;

        //Debug.Log("Detetcted " + touchNumber + " touches!");

        if (usePooling)
        {
            GameObject item = GetFromMultiPool(touchNumber - 1);
            item.transform.position = projectileStart.position;
            item.transform.rotation = projectileStart.rotation;
            item.SetActive(true);
        }
        else
        {
            GameObject item = InstantiateNewMultiItem(touchNumber - 1);
        }

    }

    private void InitializePool()
    {
        PoolQueue = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject poolItem = Instantiate(projectileObject, this.transform);
            poolItem.transform.position = poolStart.position;
            poolItem.transform.rotation = poolStart.rotation;
            poolItem.SetActive(false);
            PoolQueue.Enqueue(poolItem);
        }
    }

    private void InitializeMultiPool()
    {
        for (int i = 0; i < multiProjectileObjects.Length; i++)
        {
            Queue<GameObject> Pool = new Queue<GameObject>();
            for (int j = 0; j < poolSize; j++)
            {
                GameObject poolItem = Instantiate(multiProjectileObjects[i], this.transform);
                poolItem.transform.position = poolStart.position;
                poolItem.transform.rotation = poolStart.rotation;
                poolItem.SetActive(false);
                Pool.Enqueue(poolItem);
            }
            PoolQueues.Add(Pool);
        }
    }

    private GameObject GetFromPool()
    {
        if(PoolQueue.Count > 0)
        {
            GameObject item = PoolQueue.Dequeue();
            return item;
        }
        return InstantiateNewItem();
    }

    private GameObject GetFromMultiPool(int i)
    {
        Queue<GameObject> Pool = PoolQueues[i];
        if (Pool.Count > 0)
        {
            GameObject item = Pool.Dequeue();
            return item;
        }
        return InstantiateNewMultiItem(i);
    }

    private GameObject InstantiateNewItem()
    {
        GameObject item = Instantiate(projectileObject, this.transform);
        item.transform.position = projectileStart.position;
        item.transform.rotation = projectileStart.rotation;
        return item;
    }

    private GameObject InstantiateNewMultiItem(int i)
    {
        GameObject item = Instantiate(multiProjectileObjects[i], this.transform);
        item.transform.position = projectileStart.position;
        item.transform.rotation = projectileStart.rotation;
        return item;
    }

    public void ReturnToPool(GameObject item)
    {
        item.transform.position = poolStart.position;
        item.transform.rotation = poolStart.rotation;
        item.SetActive(false);
        PoolQueue.Enqueue(item);
    }

    public void ReturnToMultiPool(GameObject item)
    {
        Queue<GameObject> Pool = PoolQueues[item.GetComponent<Projectile>().poolIndex];
        item.transform.position = poolStart.position;
        item.transform.rotation = poolStart.rotation;
        item.SetActive(false);
        Pool.Enqueue(item);
    }

    public void TogglePooling()
    {
        usePooling = !usePooling;
        if (usePooling)
        {
            toggleText.text = "Disable Pooling";
        }
        else
        {
            toggleText.text = "Use Pooling";
        }
    }
}
