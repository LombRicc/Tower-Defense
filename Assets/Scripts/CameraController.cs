using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 5.0f;
    public float minZoom;
    public float maxZoom;

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;

        Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position = cursorWorldPos;

        float step = 1000f * Time.deltaTime;

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(position.x, transform.position.y, position.z), step);
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(position.x, transform.position.y, position.z), step);
        }

        transform.position.Set(Mathf.Lerp(transform.position.x, position.x, Time.deltaTime), transform.position.y, Mathf.Lerp(transform.position.z, position.z, Time.deltaTime)); 
    }
}
