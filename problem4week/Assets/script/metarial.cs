using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(metarial))]
public class StaticMeshGenEditor : Editor
{
    //버튼만들기 예제
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        metarial script = (metarial)target;

        if (GUILayout.Button("Generate Mesh"))
        {
            script.GenerateMesh();
        }

    }
}

public class metarial : MonoBehaviour
{
    public int numPoints = 5; // 별의 꼭지점 수
    public float radius = 1f; // 별의 반지름
    public float height = 2f; // 기둥의 높이
    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        // 기둥을 그리기 위한 정점 좌표 설정
        List<Vector3> verticesList = new List<Vector3>();

        // 기둥의 상단 중심점
        verticesList.Add(new Vector3(0, height, 0));

        // 기둥의 상단 원의 꼭지점 좌표 설정
        for (int i = 0; i < numPoints; i++)
        {
            float angle = Mathf.PI * 2 * i / numPoints;
            verticesList.Add(new Vector3(Mathf.Cos(angle) * radius, height, Mathf.Sin(angle) * radius));
        }

        // 기둥의 하단 원의 꼭지점 좌표 설정
        for (int i = 0; i < numPoints; i++)
        {
            float angle = Mathf.PI * 2 * i / numPoints;
            verticesList.Add(new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius));
        }

        // 정점 배열로 변환
        Vector3[] vertices = verticesList.ToArray();
        mesh.vertices = vertices;

        // 삼각형 인덱스 설정
        List<int> triangles = new List<int>();

        // 상단 원의 삼각형 인덱스 설정
        for (int i = 1; i <= numPoints; i++)
        {
            triangles.Add(0);
            triangles.Add(i % numPoints + 1);
            triangles.Add(i);
        }

        // 하단 원의 삼각형 인덱스 설정
        for (int i = numPoints + 1; i <= numPoints * 2; i++)
        {
            triangles.Add(i);
            triangles.Add((i % numPoints + 1) + numPoints);
            triangles.Add(i % numPoints + numPoints + 1);
        }

        // 옆면의 삼각형 인덱스 설정
        for (int i = 1; i <= numPoints; i++)
        {
            int next = (i % numPoints) + 1;

            triangles.Add(i);
            triangles.Add(next);
            triangles.Add(i + numPoints);

            triangles.Add(next);
            triangles.Add(next + numPoints);
            triangles.Add(i + numPoints);
        }

        mesh.triangles = triangles.ToArray();

        // MeshFilter와 MeshRenderer 추가 및 설정
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        if (mf == null)
            mf = gameObject.AddComponent<MeshFilter>();

        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        if (mr == null)
            mr = gameObject.AddComponent<MeshRenderer>();
        Material material = new Material(Shader.Find("Custom/NewSurfaceShader"));
        mr.material = material;
        mf.mesh = mesh;
    }
    
}

