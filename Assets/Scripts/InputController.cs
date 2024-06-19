using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SwipeManager))]
public class InputController : MonoBehaviour
{
    private Transform _camera;

    public float slideAmount;

    void Start()
    {
        SwipeManager swipeManager = GetComponent<SwipeManager>();
        swipeManager.onSwipe += HandleSwipe;
        swipeManager.onLongPress += HandleLongPress;
        _camera = transform;
        slideAmount = GenerateGrid.instance.tilesXrow;
        _camera.Translate(Vector3.up * slideAmount);
        _camera.Translate(Vector3.right * slideAmount);
        GetComponent<Camera>().fieldOfView = Mathf.Round((slideAmount-5)/5f)*10+50;
    }

    void HandleSwipe(SwipeAction swipeAction)
    {
        //Debug.LogFormat("HandleSwipe: {0}", swipeAction);
        if (swipeAction.direction == SwipeDirection.Up || swipeAction.direction == SwipeDirection.UpRight)
        {
            // move down
            if (_camera != null && _camera.transform.position.z - slideAmount > 0)
                _camera.Translate(Vector3.down * slideAmount);
        }
        else if (swipeAction.direction == SwipeDirection.Right || swipeAction.direction == SwipeDirection.DownRight)
        {
            // move left
            if (_camera != null && _camera.transform.position.x - slideAmount > slideAmount)
                _camera.Translate(Vector3.left * slideAmount);
        }
        else if (swipeAction.direction == SwipeDirection.Down || swipeAction.direction == SwipeDirection.DownLeft)
        {
            // move up
            if (_camera != null && _camera.transform.position.z < slideAmount*4)
                _camera.Translate(Vector3.up * slideAmount);
        }
        else if (swipeAction.direction == SwipeDirection.Left || swipeAction.direction == SwipeDirection.UpLeft)
        {
            // move right
            if (_camera != null && _camera.transform.position.x + slideAmount < slideAmount*4)
                _camera.Translate(Vector3.right * slideAmount);
        }
    }

    void HandleLongPress(SwipeAction swipeAction)
    {
        //Debug.LogFormat("HandleLongPress: {0}", swipeAction);
    }
}