using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleCamera : MonoBehaviour
{
    public float senX;
    public float senY;

    public Transform orintation;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orintation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().fieldOfView = endValue;
    }
}
