using UnityEngine;

public class PoolProjectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 10f;

    private void FixedUpdate()
    {
        // Move forward on x axis
        transform.position += transform.right * projectileSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ObjectShooter.Instance.usePooling)
            ObjectShooter.Instance.ReturnToPool(gameObject);
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ObjectShooter.Instance.usePooling)
            ObjectShooter.Instance.ReturnToPool(gameObject);
        else
            Destroy(gameObject);
    }
}
