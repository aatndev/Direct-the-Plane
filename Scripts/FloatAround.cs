using System.Collections;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private Rigidbody obstacleRb;
    private float force;
    private float torque;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        obstacleRb = GetComponent<Rigidbody>();
        force = Random.Range(30, 50);
        torque = Random.Range(15, 30);

        //Float Around
        obstacleRb.AddForce(Vector3.up * force);
        obstacleRb.AddTorque(torque, torque, torque);
    }
}
