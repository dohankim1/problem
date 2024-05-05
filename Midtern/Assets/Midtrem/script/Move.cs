using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    private bool isRotating = false; // 회전 중인지 여부

    void Update()
    {
        if (!isRotating)
        {
            // 이동 입력 감지
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // 카메라의 각도에 따라 이동 방향 설정
            Vector3 moveDirection = GetCameraForward() * vertical + GetCameraRight() * horizontal;

            // 이동
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    Vector3 GetCameraForward()
    {
        // 카메라의 forward 방향을 반환
        return Quaternion.Euler(45, 45, 0) * Vector3.forward;
    }

    Vector3 GetCameraRight()
    {
        // 카메라의 right 방향을 반환
        return Quaternion.Euler(45, 45, 0) * Vector3.right;
    }

    public void SetIsRotating(bool rotating)
    {
        isRotating = rotating;
    }


}
