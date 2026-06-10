using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 10f;
    public int poolIndex;

    private void FixedUpdate()
    {
        // Move forward on local x axis
        transform.position += transform.right * projectileSpeed * Time.fixedDeltaTime;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (ObjectShooter.Instance.usePooling)
    //        ObjectShooter.Instance.ReturnToPool(gameObject);
    //    else
    //        Destroy(gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (ObjectShooter.Instance.usePooling)
        {
            if (ObjectShooter.Instance.useMultiProjectile)
                ObjectShooter.Instance.ReturnToMultiPool(gameObject);
            else
                ObjectShooter.Instance.ReturnToPool(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
