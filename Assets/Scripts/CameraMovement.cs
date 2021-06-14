using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //camera movement speed
    [SerializeField] private float speed;

    private Vector4 worldSize;

    void Start()
    {
        worldSize = GameManager.Get.GetWorldSize();
    }
    
    void Update()
    {
        //get axis input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //calculate move direction
        Vector3 delta = Vector3.forward * v + Vector3.right * h;
        //assign speed and delta time to direction
        delta *= speed * Time.deltaTime;
        //assign move vector
        transform.Translate(delta);
/*
        float x = Mathf.Clamp(transform.position.x, worldSize.x, worldSize.z);
        float z = Mathf.Clamp(transform.position.z, worldSize.y, worldSize.w);

        transform.position = new Vector3(x, 0.0f, z);*/
    }
    
    void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 1f);
        Gizmos.DrawCube(new Vector3(worldSize.x, 0.0f, worldSize.y), 
                new Vector3(worldSize.z - worldSize.x, 20f, worldSize.w - worldSize.y));
    }
}
