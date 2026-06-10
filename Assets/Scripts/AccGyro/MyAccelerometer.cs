using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class MyAccelerometer : MonoBehaviour
{
    [SerializeField] TMP_Text gyroText;

    #region Gyroscope
    //private void OnEnable()
    //{
    //    InputSystem.EnableDevice(Gyroscope.current);
    //}
    #endregion

    private void Update()
    {
        TestAccelerometer();
        //TestGyro();
    }

    void TestAccelerometer()
    {
        Vector3 acc = Accelerometer.current.acceleration.ReadValue();
        Debug.Log(acc);
    }

    //void TestGyro()
    //{
    //    Vector3 rotationRate = Gyroscope.current.angularVelocity.ReadValue();
    //    //Debug.Log(rotationRate);
    //    gyroText.text = rotationRate.ToString();
    //    //AttitudeSensor
    //}
}
