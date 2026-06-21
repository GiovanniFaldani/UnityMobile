using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchManager : MonoBehaviour
{
    public TouchManager Instance { get; private set; }

    [SerializeField] float swipeDeltaThreshold = 10f;
    [SerializeField] MyGrid gameGrid;
    [SerializeField] private bool allowTouch = true;
    
    private GridSquare swipeStart;
    private GridSquare swipeEnd;

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
        if (gameGrid == null) Debug.LogError("Assign Game Grid in inspector!");
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (!allowTouch) return;

        if (Touch.activeTouches.Count <= 0) return;

        Touch touch = Touch.activeTouches[0];

        if (touch.phase == TouchPhase.Began)
        {
            // controllo quale oggetto sto cercando di swipare
            Ray ray = Camera.main.ScreenPointToRay(touch.screenPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                swipeStart = gameGrid.GetGridSnap(hit.transform.position);
            }
        }

        // se muovo il dito
        if (touch.phase == TouchPhase.Moved && swipeStart != null)
        {
            // controllo dove si muove il dito
            Vector2 swipeDelta = touch.delta;
            if(
                Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) && 
                Mathf.Abs(swipeDelta.x) > swipeDeltaThreshold
            ){
                // Si muove sulla X in orizzontale, controllo esistenza casella in griglia
                if(
                    swipeDelta.x > 0 &&
                    swipeStart.gridX + 1 < gameGrid.grid.GetLength(0)
                ){
                    swipeEnd = gameGrid.grid[swipeStart.gridX + 1, swipeStart.gridY];
                }
                else if (swipeStart.gridX - 1 >= 0)
                {
                    swipeEnd = gameGrid.grid[swipeStart.gridX - 1, swipeStart.gridY];
                }
            }
            else if(Mathf.Abs(swipeDelta.y) > swipeDeltaThreshold)
            {
                // Si muove sulla Y in verticale, controllo esistenza casella in griglia
                if (
                   swipeDelta.y > 0 &&
                   swipeStart.gridY + 1 < gameGrid.grid.GetLength(1)
               )
                {
                    swipeEnd = gameGrid.grid[swipeStart.gridX, swipeStart.gridY + 1];
                }
                else if (swipeStart.gridY - 1 >= 0)
                {
                    swipeEnd = gameGrid.grid[swipeStart.gridX, swipeStart.gridY - 1];
                }
            }
            else
            {
                swipeEnd = null;
            }

            // esegui spostamento da swipeStart a swipeEnd
            Debug.Log("SwipeStart = [" + swipeStart.gridX + ", " + swipeStart.gridY + "]");
            if (swipeEnd != null)
            {
                Debug.Log("SwipeEnd = [" + swipeEnd.gridX + ", " + swipeEnd.gridY + "]");
            }
        }

    }

    public void SetAllowTouch(bool value)
    {
        allowTouch = value;
    }
}
