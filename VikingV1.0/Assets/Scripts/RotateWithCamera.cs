using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithCamera : MonoBehaviour
{
    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Mouse_X = Input.GetAxis("Mouse X") * 300 * Time.deltaTime;
        float Mouse_Y = Input.GetAxis("Mouse Y") * 300 * Time.deltaTime;
        xRotation -= Mouse_Y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
