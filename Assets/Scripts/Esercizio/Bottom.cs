using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Bottom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
