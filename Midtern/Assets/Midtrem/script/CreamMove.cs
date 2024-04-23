using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreamMove : MonoBehaviour
{
    public float rotationAngle = 45f;
    private bool isRotating = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            RotatePlayer();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            RotatePlayer();
        }
    }

    void RotatePlayer()
    {
        // ȸ�� �ִϸ��̼� ����
        isRotating = true;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, rotationAngle);

        // ȸ�� �ִϸ��̼�
        StartCoroutine(RotateOverTime(targetRotation, 1f));
    }

    IEnumerator RotateOverTime(Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = transform.rotation;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // ȸ�� �ִϸ��̼� �Ϸ�
        transform.rotation = targetRotation;
        isRotating = false;
    }
}
