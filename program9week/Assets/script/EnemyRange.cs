using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    public Vector3 center;
    public Vector3 size;
    void Start()
    {
        center = new Vector3(2f, 0f, -15.5f); // 중심 위치 설정
        size = new Vector3(38f, 0f, 1f); // 크기 설정
    }

    void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireCube(center, size);
    }
   
   
    void Update()
    {
        
    }
}
