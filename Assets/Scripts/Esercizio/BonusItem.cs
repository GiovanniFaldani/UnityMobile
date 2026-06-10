using UnityEngine;

public class BonusItem : MonoBehaviour
{
    public int score = 10;

    private float lifetime = 2f;

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            Destroy(gameObject);
    }
}
