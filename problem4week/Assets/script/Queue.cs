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

    private Node<GameObject> head; // 큐의 맨 앞을 가리키는 노드
    private Node<GameObject> Lastnode; // 큐의 맨 뒤를 가리키는 노드
    public int count; // 큐에 들어있는 데이터 개수
    public int maxBulletCount = 10; // 최대 총알 개수

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

    // 큐에 데이터 추가
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

    // 큐에서 데이터 추출
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

    // 큐가 비어있는지 확인
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
            Vector3 spawnPosition = new Vector3(-10, 0, 0); // 원하는 위치로 변경
            GameObject bullet = InstantiateBullet(spawnPosition);
            Enqueue(bullet);
        }
    }

}
