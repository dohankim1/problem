using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreamMove : MonoBehaviour
{
    float pluse = 0;
    float pluse2 = 0;
    Quaternion targetRotation; // 목표 회전 각도

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            pluse += 90;
            targetRotation = Quaternion.Euler(45, 45, pluse);
            StartCoroutine(RotateOverTime(1.0f));
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            pluse2 += 90;
            targetRotation = Quaternion.Euler(45, 45, pluse2);
            StartCoroutine(RotateOverTime(1.0f));
        }
    }

    IEnumerator RotateOverTime(float duration)
    {
        Quaternion startRotation = transform.rotation;
        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // 회전이 완료된 후 목표 회전 각도로 설정
        transform.rotation = targetRotation;
    }
}
