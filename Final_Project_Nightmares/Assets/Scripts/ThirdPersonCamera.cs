using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public new Camera camera;
    [SerializeField] private float offset = 3f;
    [SerializeField] private float yOffset = 1.5f;
    [SerializeField] private float smoothTime = 0.5f;

    private Vector3 currentVelocity;
    private Vector3 targetPos;
    private Vector3 faceDir;

    void Start()
    {
        
    }

    void Update()
    {   
        // position camera with offset near target
        targetPos = player.transform.position + new Vector3(offset, yOffset, offset);
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetPos, ref currentVelocity, smoothTime);

        // rotate camera to look at target
        faceDir = player.transform.position - camera.transform.position;
        if (faceDir != Vector3.zero)
        {
            camera.transform.rotation = Quaternion.LookRotation(faceDir, Vector3.up);
        }

        // IGNORE TEST CODE
        //Debug.Log($"targetPos = {targetPos}, camera's pos = {camera.transform.position}" + $"FaceDir = {faceDir}");
        //transform.rotation = Vector3.SmoothDamp(transform.rotation, player.t)
        //camera.transform.position = player.transform.position;
    }
}
