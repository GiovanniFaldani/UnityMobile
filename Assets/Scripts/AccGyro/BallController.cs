using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    Rigidbody rb;
    Vector3 calibration;

    private void OnEnable()
    {
        InputSystem.EnableDevice(Accelerometer.current);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(TimeBeforeUseAccelerometer());
    }

    private void FixedUpdate()
    {
        if (Accelerometer.current == null) return;
        Vector3 acc = Accelerometer.current.acceleration.ReadValue();
        Vector3 correct = acc - calibration;

        // portrait (verticale dritto)
        Vector3 movement = new Vector3(correct.x, 0, correct.y);

        // landscape right
        // Vector3 movement = new Vector3(-correct.y, 0, correct.x);

        // landscape left
        // Vector3 movement = new Vector3(correct.y, 0, -correct.x);

        rb.AddForce(movement * speed);
    }

    IEnumerator TimeBeforeUseAccelerometer()
    {
        yield return new WaitForSeconds(0.1f);
        calibration = Accelerometer.current.acceleration.ReadValue();
    }
}
