using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovee : MonoBehaviour
{
    float speed = 5;
    bool isgo = true;
    bool backgo = false;
    public Camera mainCamera; // ī�޶� ����
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
            // �÷��̾ �þ߿� ���� ��
            if (playerInSightr)
            {
                // ������ �÷��̾ �����߾��ٸ�
                playerInSightr = false;
                // ī�޶� �������� ȸ��
                RotateCameraLeft();
            }

        }
        if (!isRotating && !IsPlayerInSight())
        {
            StartCoroutine(MoveToZPosition(-15.5f));
        }

        bool playerInSight = IsPlayerInSight();

        // �÷��̾ �þ� ���� ������ ������ ������ ó��
        if (playerInSight)
        {
            Debug.Log("�÷��̾� ����!");
            // �߰� �۾� ����
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
        Vector3 cameraPosition = mainCamera.transform.position; // ī�޶� ��ġ
        Vector3 playerPosition = player.position; // �÷��̾� ��ġ

        // ī�޶󿡼� �÷��̾������ ���� ����
        Vector3 directionToPlayer = (playerPosition - cameraPosition).normalized;

        // ī�޶��� �þ� ����
        float fieldOfView = mainCamera.fieldOfView;

        // �÷��̾ ���ϴ� ���Ͱ� ī�޶��� �þ� ���� ���� �ִ��� Ȯ��
        if (Vector3.Angle(mainCamera.transform.forward, directionToPlayer) < fieldOfView / 2)
        {
            // ī�޶� �þ� �ȿ� �÷��̾ �ִ��� ����ĳ��Ʈ�� Ȯ��
            RaycastHit hit;
            if (Physics.Raycast(cameraPosition, directionToPlayer, out hit))
            {
                // ����ĳ��Ʈ�� �÷��̾ �浹�ϸ�
                if (hit.collider.CompareTag("Player"))
                {
                    return true; // �÷��̾ �þ� ���� �ִٰ� �Ǵ�
                }
            }
        }

        return false; // �÷��̾ �þ� ���� ���ٰ� �Ǵ�
    }
    void ChasePlayer()
    {
        // �÷��̾��� ��ġ�� �̵�
        transform.LookAt(player);
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // �÷��̾� ������ �̵�
    }

    void RotateCameraLeft()
    {
        // �������� 3�� ���� ȸ�� (90��)
        StartCoroutine(RotateCamera(3f, -90f));
    }

    IEnumerator RotateCamera(float duration, float angle)
    {
        // ȸ�� ���� �ð�
        float startTime = Time.time;

        // ȸ���� �����ϱ� ���� ī�޶��� ���� ȸ�� ����
        Quaternion startRotation = mainCamera.transform.rotation;

        // ȸ���� �����ϱ� ���� ī�޶��� ���� ȸ�� ������ �������� ��ǥ ȸ�� ���� ���
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, angle, 0);

        // �־��� �ð� ���� ȸ��
        while (Time.time - startTime < duration)
        {
            // ���� �ð��� ���� ȸ�� ����� ���
            float t = (Time.time - startTime) / duration;

            // ȸ�� ������� ���� ī�޶� ȸ��
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null; // �� ������ ���
        }

        // ȸ�� �Ϸ� �� ��ǥ ȸ�� ������ ����
        mainCamera.transform.rotation = targetRotation;

        // ù ��° ȸ���� �Ϸ�Ǹ� �� ��° ȸ�� ����
        StartCoroutine(RotateCamera2(3f, 90f));
    }

    IEnumerator RotateCamera2(float duration, float angle)
    {
        // ȸ�� ���� �ð�
        float startTime = Time.time;

        // ȸ���� �����ϱ� ���� ī�޶��� ���� ȸ�� ����
        Quaternion startRotation = mainCamera.transform.rotation;

        // ȸ���� �����ϱ� ���� ī�޶��� ���� ȸ�� ������ �������� ��ǥ ȸ�� ���� ���
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, angle, 0);

        // �־��� �ð� ���� ȸ��
        while (Time.time - startTime < duration)
        {
            // ���� �ð��� ���� ȸ�� ����� ���
            float t = (Time.time - startTime) / duration;

            // ȸ�� ������� ���� ī�޶� ȸ��
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null; // �� ������ ���
        }

        // ȸ�� �Ϸ� �� ��ǥ ȸ�� ������ ����
        mainCamera.transform.rotation = targetRotation;

        // �� ��° ȸ���� �Ϸ�Ǹ� isRotating ���� ������Ʈ
        isRotating = false;
    }

    void RotateCameraRight()
    {
        StartCoroutine(RotateCamera2(3f, 90f));

    }

    IEnumerator MoveToZPosition(float targetZ)
    {
        // ���� ��ġ
        Vector3 startPosition = transform.position;
        // ��ǥ ��ġ
        Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y, targetZ);

        // �̵� �ӵ� �� �ð�
        float moveSpeed = 5f;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float totalTime = journeyLength / moveSpeed;

        // �̵� ���� �ð�
        float startTime = Time.time;

        // �̵� ���� ������ �ݺ�
        while (Time.time < startTime + totalTime)
        {
            // ���� �ð��� ���� �̵� ����� ���
            float fractionOfJourney = (Time.time - startTime) / totalTime;

            // �� ��ġ ����Ͽ� �̵�
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null; // �� ������ ���
        }

        // ��ǥ ��ġ�� ������ �� ���� ��ġ ���� (����)
        transform.position = targetPosition;

        // ȸ�� �Ϸ� �� isRotating ���� ������Ʈ
        isRotating = false;
    }

}
