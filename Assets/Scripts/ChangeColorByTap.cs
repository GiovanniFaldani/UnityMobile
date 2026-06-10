using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeColorByTap : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }
}
