using System.Collections;
using UnityEngine;

public abstract class Ingredient : MonoBehaviour, ISwipeable
{
    protected void Start()
    {
        Debug.Log("Ingredient Start");
        MyGrid grid = FindFirstObjectByType<MyGrid>();
        GridSquare closestSquare = grid.GetGridSnap(transform.position);
        closestSquare.PushToStack(this);
    }

    public void MoveToSquare(GridSquare destination, Vector2 swipeDir)
    {
        // calcolo la posizione del prossimo elemento nello stack
        Vector3 target = new Vector3(
            destination.worldPosition.x,
            0.05f + destination.ingredientStack.Count * 0.1f,
            destination.worldPosition.z
        );

        Vector3 midpoint = (transform.position + target) / 2 + Vector3.up * 1f;

        StartCoroutine(SwipeAnimation(transform.position, midpoint, target, swipeDir));
    }

    // TODO trovare modo elegante di ruotare in direzione giusta
    public IEnumerator SwipeAnimation(Vector3 beginning, Vector3 apex, Vector3 end, Vector2 swipeDir)
    {
        // muovo l'ingrediente 

        float current = 0;
        float lerpValue = 0;

        while (current < 0.5f)
        {
            lerpValue = Mathf.InverseLerp(0, 0.5f, current);

            transform.position = Vector3.Lerp(beginning, apex, lerpValue);
            transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                Quaternion.Euler(transform.rotation.x + (90 * swipeDir.y), 0, transform.rotation.z + (90 * swipeDir.x)), lerpValue
            );

            current += Time.deltaTime;

            yield return null;
        }

        current = 0;
        lerpValue = 0;

        while (current < 0.5f)
        {
            lerpValue = Mathf.InverseLerp(0, 0.5f, current);

            transform.position = Vector3.Lerp(apex, end, lerpValue);
            transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                Quaternion.Euler(transform.rotation.x + (180 * swipeDir.y), 0, transform.rotation.z + (180 * swipeDir.x)), lerpValue
            );

            current += Time.deltaTime;

            yield return null;
        }
        TouchManager.Instance.SetAllowTouch(true);
    }
}
