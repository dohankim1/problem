using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public GameObject Bullet;
    
    private class Node<T>
    {
        public T data;
        public Node<T> next;

        public Node(T data)
        {
            this.data = data;
            next = null;
        }
    }

    private Node<GameObject> head; // ť�� �� ���� ����Ű�� ���
    private Node<GameObject> Lastnode; // ť�� �� �ڸ� ����Ű�� ���
    public int count; // ť�� ����ִ� ������ ����
    public int maxBulletCount = 10; // �ִ� �Ѿ� ����

    void Start()
    {
        head = null;
        Lastnode = null;
        count = 0;
    }

    GameObject InstantiateBullet(Vector3 spawnPosition)
    {
        GameObject bulletInstance = Instantiate(Bullet, spawnPosition, Quaternion.identity);
        return bulletInstance;
    }

    // ť�� ������ �߰�
    public void Enqueue(GameObject bullet)
    {
        if (count < maxBulletCount)
        {
            Node<GameObject> newNode = new Node<GameObject>(bullet);
            if (Lastnode == null)
            {
                head = newNode;
                Lastnode = newNode;
            }
            else
            {
                Lastnode.next = newNode;
                Lastnode = newNode;
            }
            count++;
        }
    }

    // ť���� ������ ����
    public GameObject Dequeue()
    {
        if (IsEmpty())
        {
            return null;
        }

        GameObject data = head.data;
        head = head.next;
        if (head == null)
        {
            Lastnode = null;
        }
        count--;
        return data;
    }

    // ť�� ����ִ��� Ȯ��
    bool IsEmpty()
    {
        return count == 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedBlock"))
        {
            
            GameObject bullet = Dequeue();
            Enqueue(bullet);
        }
    }
   

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && count < maxBulletCount)
        {
            Vector3 spawnPosition = new Vector3(-10, 0, 0); // ���ϴ� ��ġ�� ����
            GameObject bullet = InstantiateBullet(spawnPosition);
            Enqueue(bullet);
        }
    }

}
