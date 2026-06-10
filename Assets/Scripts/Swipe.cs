using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class Swipe : MonoBehaviour
{
    public float rotationSpeed = 0.2f;
    public float smoothness = 10f;

    float targetRotation;

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
        if (Touch.activeTouches.Count <= 0) return;

        Touch touch = Touch.activeTouches[0];

        // se muovo il dito
        if (touch.phase == TouchPhase.Moved)
        {
            // controllo di quanto muovo il dito
            float swipeDelta = touch.delta.x;

            // se swipo a sinistra ruoto in senso orario, altrimenti in senso antiorario
            targetRotation -= swipeDelta * rotationSpeed;

            // la rotazione fa riferimento allo swipe e alla velocità
            Quaternion newRotation = Quaternion.Euler(0, targetRotation, 0);

           // ruoto la pagina con una certa smoothness
           transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * smoothness);
        }
    }
}
