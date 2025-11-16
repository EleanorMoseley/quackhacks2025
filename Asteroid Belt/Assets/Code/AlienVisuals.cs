using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienVisuals : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(target, Vector3.down);
    }
}
