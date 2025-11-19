using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float force;
    private GameObject plane;
    private Rigidbody rb;
    private Rigidbody planeRb;

    private float backBound;

    private Powerup powerup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plane = GameObject.Find("Plane");
        planeRb = plane.GetComponent<Rigidbody>();

        rb = GetComponent<Rigidbody>();

        powerup = GameObject.Find("GameManager").GetComponent<Powerup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (plane == null)
        {
            Destroy(gameObject);
        }
        
        Vector3 direction = (plane.transform.position - transform.position).normalized;

        rb.AddForce(direction * force);

        DestroyOutOfRange();
    }

    void DestroyOutOfRange()
    {
        backBound = plane.transform.position.z - 5;

        if (transform.position.z < backBound)
        {
            Destroy(gameObject);
            Debug.Log("Destroyed Obstacle");
        }
    }
}
