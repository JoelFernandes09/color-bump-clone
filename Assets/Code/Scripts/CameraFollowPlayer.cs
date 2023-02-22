using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float moveSpeed = 4f;

    private void Start() {

    }
    private void LateUpdate()
    {
        if(PlayerInfo.isGamePaused) return;
        transform.position += new Vector3(0f, 0f, 1f) * moveSpeed * Time.deltaTime;
    }

}
