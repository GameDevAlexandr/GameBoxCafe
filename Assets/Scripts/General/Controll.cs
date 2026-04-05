using UnityEngine;
using System;
public class Controll : MonoBehaviour
{
    public Action<Vector2> onLeftMBDown; 
    public Action<Vector2> onRightMBDown;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onLeftMBDown?.Invoke(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(1))
        {
            onRightMBDown?.Invoke(Input.mousePosition);
        }
    }
}
