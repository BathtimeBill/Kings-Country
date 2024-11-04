using System;
using UnityEngine;

public class LineupCamera : MonoBehaviour
{
    public float speed = 1f;
    private void OnCameraMove(Vector2 obj)
    {
        transform.Translate(Vector2.left * obj.x * Time.deltaTime * speed, Space.World);
    }
    
    private void OnEnable()
    {
        InputManager.OnCameraMove += OnCameraMove;
    }
    
    private void OnDisable()
    {
        InputManager.OnCameraMove -= OnCameraMove;
    }
}
