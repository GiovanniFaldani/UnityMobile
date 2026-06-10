using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[RequireComponent(typeof(BoxCollider))]
public class Paddle : MonoBehaviour
{


    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            SnapToFinger();
            CheckForBonus();
        }
    }

    public void SnapToFinger()
    {
        if (Touch.activeTouches.Count <= 0) return;

        var touch = Touch.activeTouches[0];

        // se non sto toccando skippo tutto
        if (touch.phase != TouchPhase.Moved && touch.phase != TouchPhase.Stationary) return;

        // prendo la posizione del tocco
        Vector2 pos = touch.screenPosition;

        //la trasformo in posizione in world space
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));

        //sposto l'oggetto in quella posizione
        transform.position = new Vector3(position.x, -9, 0);
    }

    public void CheckForBonus()
    {
        if (Touch.activeTouches.Count <= 0) return;

        var touch = Touch.activeTouches[0];

        if (touch.phase != TouchPhase.Began) return;

        Vector2 pos = touch.screenPosition;

        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(pos), out hit, Mathf.Infinity);

        if (hit.collider == null) return;

        GameObject other = hit.collider.gameObject;

        if (other.GetComponent<BonusItem>() != null)
        {
            GameManager.Instance.AddScore(other.GetComponent<BonusItem>().score);
            Destroy(other.gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.AddScore(other.GetComponent<ScoreItem>().score);
        Destroy(other.gameObject);
    }
}
