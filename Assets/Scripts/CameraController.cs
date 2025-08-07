using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float offsetY;
    [SerializeField] float offsetX;

    Vector3 pos;

    void Update()
    {
        if (player != null)
        {
            pos = player.position;
            pos.y += offsetY;
            pos.x += offsetX;
            pos.z = -10f;

            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
        }        
    }
}
