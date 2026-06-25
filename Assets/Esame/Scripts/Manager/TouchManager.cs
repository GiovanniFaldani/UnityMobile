using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

struct SwipeMove
{
    public GridSquare swipeStart;
    public GridSquare swipeEnd;
    public List<Ingredient> movedIngredients;

    public SwipeMove(GridSquare _swipeStart,  GridSquare _swipeEnd, List<Ingredient> _movedIngredients)
    {
        this.swipeStart = _swipeStart;
        this.swipeEnd = _swipeEnd;
        this.movedIngredients = _movedIngredients;
    }
};

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance { get; private set; }

    [SerializeField] float swipeDeltaThreshold = 10f;
    [SerializeField] MyGrid gameGrid;
    [SerializeField] private bool allowTouch = true;
    
    private GridSquare swipeStart;
    private GridSquare swipeEnd;

    // costruttore default all'inizio così la mossa è vuota
    private SwipeMove previousMove = new SwipeMove();

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
            Vector2 swipeDir = new Vector2();
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
                    swipeDir = new Vector2(1, 0);
                }
                else if (swipeStart.gridX - 1 >= 0)
                {
                    swipeEnd = gameGrid.grid[swipeStart.gridX - 1, swipeStart.gridY];
                    swipeDir = new Vector2(-1, 0);
                }
            }
            else if(Mathf.Abs(swipeDelta.y) > swipeDeltaThreshold)
            {
                // Si muove sulla Y in verticale, controllo esistenza casella in griglia
                if (
                   swipeDelta.y > 0 &&
                   swipeStart.gridY + 1 < gameGrid.grid.GetLength(1)
                ){
                    swipeEnd = gameGrid.grid[swipeStart.gridX, swipeStart.gridY + 1];
                    swipeDir = new Vector2(0, 1);
                }
                else if (swipeStart.gridY - 1 >= 0)
                {
                    swipeEnd = gameGrid.grid[swipeStart.gridX, swipeStart.gridY - 1];
                    swipeDir = new Vector2(0, -1);
                }
            }
            else
            {
                swipeEnd = null;
            }

            // esegui spostamento da swipeStart a swipeEnd
            if (swipeEnd != null)
            {
                List<Ingredient> moved = swipeEnd.PushToStackFromSquare(swipeStart, swipeDir);
                if (moved != null)
                    previousMove = new SwipeMove(swipeStart, swipeEnd, moved);
            }
        }

    }

    public void UndoPreviousMove()
    {
        if (previousMove.swipeEnd == null || previousMove.swipeEnd.ingredientStack.Count <= 0) return;

        allowTouch = false;
        // ritorna indietro tutti gli ingredienti mossi nella previous move

        Ingredient ing = previousMove.swipeEnd.ingredientStack.Peek();
        while(previousMove.movedIngredients.Contains(ing))
        {
            ing = previousMove.swipeEnd.ingredientStack.Pop();
            ing.MoveToSquare(
                previousMove.swipeStart,
                new Vector2(
                    previousMove.swipeStart.gridX - previousMove.swipeEnd.gridX,
                    previousMove.swipeStart.gridY - previousMove.swipeEnd.gridY
                )
            );
            previousMove.swipeStart.PushToStack(ing);
            previousMove.swipeStart.occupied = true;

            // controlla il prossimo ingrediente
            ing = previousMove.swipeEnd.ingredientStack.Peek();
        }

        // sovrascrivi memoria della mossa precedente
        previousMove = new SwipeMove();
    }

    public void SetAllowTouch(bool value)
    {
        allowTouch = value;
    }

    public bool GetAllowTouch()
    {
        return allowTouch;
    }
}
