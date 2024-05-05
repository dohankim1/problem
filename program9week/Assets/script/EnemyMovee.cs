using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovee : MonoBehaviour
{
    float speed = 5;
    bool isgo = true;
    bool backgo = false;
    public Camera mainCamera; // 카메라 참조
    public Transform player;
    bool playerInSightr = false;
    bool isRotating = false;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerInSight())
        {
            ChasePlayer();
            playerInSightr = true;
            isRotating = true;
            //isgo= false;
            //backgo= false;
        }
        else
        {
            // 플레이어가 시야에 없을 때
            if (playerInSightr)
            {
                // 이전에 플레이어를 감지했었다면
                playerInSightr = false;
                // 카메라를 왼쪽으로 회전
                RotateCameraLeft();
            }

        }
        if (!isRotating && !IsPlayerInSight())
        {
            StartCoroutine(MoveToZPosition(-15.5f));
        }

        bool playerInSight = IsPlayerInSight();

        // 플레이어가 시야 내에 있으면 감지된 것으로 처리
        if (playerInSight)
        {
            Debug.Log("플레이어 감지!");
            // 추가 작업 수행
        }

        if (isgo == true)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (transform.position.x < -17)
            {
                isgo = false;
                backgo = true;
            }
        }
        if (backgo == true)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (transform.position.x > 19)
            {
                isgo = true;
                backgo = false;
            }
        }

    }
    bool IsPlayerInSight()
    {
        Vector3 cameraPosition = mainCamera.transform.position; // 카메라 위치
        Vector3 playerPosition = player.position; // 플레이어 위치

        // 카메라에서 플레이어까지의 방향 벡터
        Vector3 directionToPlayer = (playerPosition - cameraPosition).normalized;

        // 카메라의 시야 각도
        float fieldOfView = mainCamera.fieldOfView;

        // 플레이어를 향하는 벡터가 카메라의 시야 각도 내에 있는지 확인
        if (Vector3.Angle(mainCamera.transform.forward, directionToPlayer) < fieldOfView / 2)
        {
            // 카메라 시야 안에 플레이어가 있는지 레이캐스트로 확인
            RaycastHit hit;
            if (Physics.Raycast(cameraPosition, directionToPlayer, out hit))
            {
                // 레이캐스트가 플레이어에 충돌하면
                if (hit.collider.CompareTag("Player"))
                {
                    return true; // 플레이어가 시야 내에 있다고 판단
                }
            }
        }

        return false; // 플레이어가 시야 내에 없다고 판단
    }
    void ChasePlayer()
    {
        // 플레이어의 위치로 이동
        transform.LookAt(player);
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // 플레이어 쪽으로 이동
    }

    void RotateCameraLeft()
    {
        // 왼쪽으로 3초 동안 회전 (90도)
        StartCoroutine(RotateCamera(3f, -90f));
    }

    IEnumerator RotateCamera(float duration, float angle)
    {
        // 회전 시작 시간
        float startTime = Time.time;

        // 회전을 시작하기 전의 카메라의 현재 회전 각도
        Quaternion startRotation = mainCamera.transform.rotation;

        // 회전을 시작하기 전의 카메라의 현재 회전 각도를 기준으로 목표 회전 각도 계산
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, angle, 0);

        // 주어진 시간 동안 회전
        while (Time.time - startTime < duration)
        {
            // 현재 시간에 따른 회전 진행률 계산
            float t = (Time.time - startTime) / duration;

            // 회전 진행률에 따라 카메라 회전
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null; // 한 프레임 대기
        }

        // 회전 완료 후 목표 회전 각도로 설정
        mainCamera.transform.rotation = targetRotation;

        // 첫 번째 회전이 완료되면 두 번째 회전 시작
        StartCoroutine(RotateCamera2(3f, 90f));
    }

    IEnumerator RotateCamera2(float duration, float angle)
    {
        // 회전 시작 시간
        float startTime = Time.time;

        // 회전을 시작하기 전의 카메라의 현재 회전 각도
        Quaternion startRotation = mainCamera.transform.rotation;

        // 회전을 시작하기 전의 카메라의 현재 회전 각도를 기준으로 목표 회전 각도 계산
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, angle, 0);

        // 주어진 시간 동안 회전
        while (Time.time - startTime < duration)
        {
            // 현재 시간에 따른 회전 진행률 계산
            float t = (Time.time - startTime) / duration;

            // 회전 진행률에 따라 카메라 회전
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null; // 한 프레임 대기
        }

        // 회전 완료 후 목표 회전 각도로 설정
        mainCamera.transform.rotation = targetRotation;

        // 두 번째 회전이 완료되면 isRotating 변수 업데이트
        isRotating = false;
    }

    void RotateCameraRight()
    {
        StartCoroutine(RotateCamera2(3f, 90f));

    }

    IEnumerator MoveToZPosition(float targetZ)
    {
        // 현재 위치
        Vector3 startPosition = transform.position;
        // 목표 위치
        Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y, targetZ);

        // 이동 속도 및 시간
        float moveSpeed = 5f;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float totalTime = journeyLength / moveSpeed;

        // 이동 시작 시간
        float startTime = Time.time;

        // 이동 중일 때까지 반복
        while (Time.time < startTime + totalTime)
        {
            // 현재 시간에 따른 이동 진행률 계산
            float fractionOfJourney = (Time.time - startTime) / totalTime;

            // 새 위치 계산하여 이동
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null; // 한 프레임 대기
        }

        // 목표 위치에 도착한 후 최종 위치 설정 (보정)
        transform.position = targetPosition;

        // 회전 완료 후 isRotating 변수 업데이트
        isRotating = false;
    }

}
