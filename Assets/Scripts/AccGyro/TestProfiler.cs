using UnityEngine;

public class TestProfiler : MonoBehaviour
{
    [SerializeField] GameObject testGO;
    int counter = 0;

    private void Update()
    {
        if (counter < 200)
        {
            Instantiate(testGO);
            counter++;
        }

    }
}
