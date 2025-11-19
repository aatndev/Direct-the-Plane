using UnityEditor;
using UnityEngine;

public class followPlane : MonoBehaviour
{
    private GameObject plane;
    private Vector3 offset;
    private Vector3 rotationOffset;
    private GameManager gameManager;

    private Vector3 menuCamPos;
    private Quaternion menuCamRot;

    void Start()
    {
        offset = new Vector3(0f, 2.7f, -4.8f);
        rotationOffset = new Vector3(21f, 0f, 0f);

        menuCamPos = new Vector3(-5.56f, 3.3f, 1.84f);
        menuCamRot = Quaternion.Euler(15, 135, 9);

        plane = GameObject.Find("Plane");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    void LateUpdate()
    {
        if (!gameManager.isMenu && !gameManager.gameOver)
        {
            transform.position = plane.transform.TransformPoint(offset);
            transform.rotation = plane.transform.rotation * Quaternion.Euler(rotationOffset);
        }
        if (gameManager.isMenu)
        {
            transform.position = menuCamPos;
            transform.rotation = menuCamRot;
        }
        
    }
}
