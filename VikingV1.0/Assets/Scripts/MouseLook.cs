    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 300f;
        public Transform playerBody;
        float xRotation = 0f;
        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState=CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            float Mouse_X = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
            float Mouse_Y = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime;
            xRotation -= Mouse_Y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * Mouse_X);
            //playerBody.LookAt(Input.mousePosition);
        }
    }
