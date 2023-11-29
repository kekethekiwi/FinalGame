using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject player;
    private float offset = 5f;
    private float smoothTime = 0.3f;
    private Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref currentVelocity, smoothTime) * offset;
        //transform.rotation = Vector3.SmoothDamp(transform.rotation, player.t)
    }
}
