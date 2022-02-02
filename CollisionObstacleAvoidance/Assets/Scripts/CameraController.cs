using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject followTarget;
    [SerializeField] private Vector3 verticalOffset = new Vector3(0.0f, 10.0f, 0.0f);

    // Update is called once per frame
    void Update()
    {
        transform.position = followTarget.transform.position + verticalOffset;
    }
}
