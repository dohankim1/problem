using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stackreal : MonoBehaviour
{
    public GameObject Bullet;

    private Queue<GameObject> queue; 
    private int maxBulletCount = 10;

    void Start()
    {
        queue = new Queue<GameObject>();
    }

    GameObject InstantiateBullet(Vector3 spawnPosition)
    {
        GameObject bulletInstance = Instantiate(Bullet, spawnPosition, Quaternion.identity);
        return bulletInstance;
    }

 
    public void Push(GameObject bullet)
    {
        if (queue.Count < maxBulletCount)
        {
            queue.Enqueue(bullet);
           
            for (int i = 0; i < queue.Count - 1; i++)
            {
                queue.Enqueue(queue.Dequeue());
            }
        }
    }


    public GameObject Pop()
    {
        if (IsEmpty())
        {
            Debug.LogError("Stack is empty. Cannot pop item.");
            return null;
        }
        return queue.Dequeue(); 
    }

    // ������ ����ִ��� Ȯ��
    public bool IsEmpty()
    {
        return queue.Count == 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedBlock"))
        {
            GameObject bullet = Pop();
            Push(bullet);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && queue.Count < maxBulletCount)
        {
            Vector3 spawnPosition = new Vector3(-10, 0, 0); // ���ϴ� ��ġ�� ����
            GameObject bullet = InstantiateBullet(spawnPosition);
            Push(bullet);
        }
    }
}
