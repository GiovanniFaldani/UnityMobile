using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class TwoPortionScreen : MonoBehaviour
{
    [SerializeField] GameObject leftObject;
    [SerializeField] GameObject rightObject;

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
        // stato delle due porzioni dello schermo
        bool left = false;
        bool right = false;

        // per ogni tocco
        foreach(Touch touch in Touch.activeTouches)
        {
            // controllo la posizione del tocco
            Vector2 touchPosition = touch.screenPosition;

            // se sono sulla metà sinistra dello schermo, attivo l'oggetto di sinistra
            if (touchPosition.x < Screen.width / 2)
                left = true;
            // se no attivo quello a destra
            else
                right = true;
        }

        // attivo/disattivo gli oggetti in base al tocco
        leftObject.SetActive(left);
        rightObject.SetActive(right);
    }
}
