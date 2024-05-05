using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    private bool isRotating = false; // ȸ�� ������ ����

    void Update()
    {
        if (!isRotating)
        {
            // �̵� �Է� ����
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // ī�޶��� ������ ���� �̵� ���� ����
            Vector3 moveDirection = GetCameraForward() * vertical + GetCameraRight() * horizontal;

            // �̵�
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    Vector3 GetCameraForward()
    {
        // ī�޶��� forward ������ ��ȯ
        return Quaternion.Euler(45, 45, 0) * Vector3.forward;
    }

    Vector3 GetCameraRight()
    {
        // ī�޶��� right ������ ��ȯ
        return Quaternion.Euler(45, 45, 0) * Vector3.right;
    }

    public void SetIsRotating(bool rotating)
    {
        isRotating = rotating;
    }


}
