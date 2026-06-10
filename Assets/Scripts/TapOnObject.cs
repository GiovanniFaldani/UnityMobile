using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TapOnObject : MonoBehaviour
{
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
        // se non tappo da nessuna parte, skippo
        if (Touch.activeTouches.Count <= 0) return;

        // mi prendo il primo tocco 
        Touch touch = Touch.activeTouches[0];

        // me lo prendo quando inizia
        if (touch.phase != TouchPhase.Began) return;

        // faccio partire il ray da quel punto
        Ray ray = Camera.main.ScreenPointToRay(touch.screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            hit.transform.gameObject.GetComponent<MeshRenderer>().material.color =
                new Color(Random.value, Random.value, Random.value);
        }
    }
}
