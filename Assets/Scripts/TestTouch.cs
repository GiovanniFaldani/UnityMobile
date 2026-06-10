using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

// Setup di EnhancedInput per touch, diverso da Input System legacy e nuovo
// Va attivato e disattivato in base alle circostanze (di solito in un manager)
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TestTouch : MonoBehaviour
{
    bool isPinching = false;
    float lastDistance;
    [SerializeField] float zoomSpeed;
    [SerializeField] float minScale;
    [SerializeField] float maxScale;

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
        //MoveCube();
        //Zoom();

        // ci prendiamo la quantità esatta di tocchi a schermo
        var touches = Touch.activeTouches;

        if (touches.Count == 0)
        {
            isPinching = false;
            return;
        }

        // se tocco con un solo dito, sposto ma non zoommo
        if (touches.Count == 1)
        {
            isPinching = false;
            MoveWithEnhancedInput(touches[0]);
        }

        if (touches.Count >= 2)
        {
            ZoomWithEnhancedInput(touches[0], touches[1]);
        }
    }

    private void MoveWithEnhancedInput(Touch touch)
    {
        // movimento del cubo
        // se non sto toccando skippo tutto
        if (touch.phase != TouchPhase.Moved && touch.phase != TouchPhase.Stationary) return;

        // prendo la posizione del tocco
        Vector2 pos = touch.screenPosition;

        //la trasformo in posizione in world space
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 10f));

        //sposto l'oggetto in quella posizione
        transform.position = position;
    }
    private void ZoomWithEnhancedInput(Touch t0, Touch t1)
    {
        // zoom del cubo
        // posizioni dei due tocchi
        Vector2 pos0 = t0.screenPosition;
        Vector2 pos1 = t1.screenPosition;

        //mi prendo la distanza corrente tra le due dita
        float currentDistance = Vector2.Distance(pos0, pos1);

        //nel momento in cui non stavo zoomando, mi salvo l'ultima distanza, ed essendo che ci sono due tocchi, inizio a zoomare
        if (!isPinching)
        {
            lastDistance = currentDistance;
            isPinching = true;
            return;
        }

        //1 = nessun cambiamento di scala, positivo = le dita si allontanano e sto zoomando, negativo = dita si avvicinano e sto dezoomando
        float changeScaleFactor = 1f + (currentDistance - lastDistance) * zoomSpeed;

        //moltiplico la scala attuale per il cambiamento di scala
        Vector3 newScale = transform.localScale * changeScaleFactor;

        //impedisco alla scala di salire sopra il massimo e scendere sotto il minimo

        newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
        newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
        newScale.z = 1f;

        //setto la nuova scala
        transform.localScale = newScale;

        //aggiorno la distanza tra le dita per il frame successivo
        lastDistance = currentDistance;
    }

    void MoveCube()
    {
        //se sto toccando / ho toccato lo schermo del telefono vado avanti
        if (Touchscreen.current == null) return;

        //il tocco di un singolo dito (il primo)
        var touch = Touchscreen.current.primaryTouch;

        //nel momento in cui tocco o tengo premuto
        if (touch.press.isPressed)
        {
            //mi prendo la posizione a schermo del tocco
            Vector2 pos = touch.position.ReadValue();

            //la trasformo in posizione in world space
            Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 10f));

            //sposto l'oggetto in quella posizione
            transform.position = position;
        }
    }


    void Zoom()
    {
        //mi prendo il tocco
        var touch = Touchscreen.current;

        //se non tocco nulla o tocco con un solo dito, non sto zoomando
        if (touch == null || touch.touches.Count < 2)
        {
            isPinching = false;
            return;
        }

        //mi prendo il tocco del primo e del secondo dito
        var t0 = touch.touches[0];
        var t1 = touch.touches[1];

        //se in qualunque momento non premo con l'uno o con l'altro (touch ghost)
        if (!t0.press.isPressed || !t1.press.isPressed)
        {
            //non sto zoomando
            isPinching = false;
            return;
        }

        //mi prendo la distanza corrente tra le due dita
        float currentDistance = Vector2.Distance(t0.position.ReadValue(), t1.position.ReadValue());

        //nel momento in cui non stavo zoomando, mi salvo l'ultima distanza, ed essendo che ci sono due tocchi, inizio a zoomare
        if (!isPinching)
        {
            lastDistance = currentDistance;
            isPinching = true;
            return;
        }

        //1 = nessun cambiamento di scala, positivo = le dita si allontanano e sto zoomando, negativo = dita si avvicinano e sto dezoomando
        float changeScaleFactor = 1f + (currentDistance - lastDistance) * zoomSpeed;

        //moltiplico la scala attuale per il cambiamento di scala
        Vector3 newScale = transform.localScale * changeScaleFactor;

        //impedisco alla scala di salire sopra il massimo e scendere sotto il minimo

        newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
        newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
        newScale.z = 1f;

        //setto la nuova scala
        transform.localScale = newScale;

        //aggiorno la distanza tra le dita per il frame successivo
        lastDistance = currentDistance;
    }

    /*
		Touchscreen.current =  										il tocco attuale
		Touchscreen.current.primaryTouch =  						il tocco principale (primo dito)
		Touchscreen.current.touches =  								la lista di tutti i tocchi
		Touchscreen.current.touches[0].press.isPressed = 			se il tocco è iniziato
		Touchscreen.current.touches[0].position.ReadValue() = 		posizione del dito 

	*/
}
