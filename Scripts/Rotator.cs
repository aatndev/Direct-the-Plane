using UnityEngine;

public class rotator : MonoBehaviour
{
    private float speed = 500f;
    
    public GameObject propellor;
    public GameObject wheel1;
    public GameObject wheel2;
    public GameObject wheel3;
    
    // Update is called once per frame
    void Update()
    {
        propellor.transform.Rotate(Vector3.left * Time.deltaTime * speed);
        wheel1.transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        wheel2.transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        wheel3.transform.Rotate(Vector3.forward * Time.deltaTime * speed);
    }
}
