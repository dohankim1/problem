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

    void AddOutlineToMesh(Mesh mesh)
    {
        List<Vector3> verticesList = new List<Vector3>(mesh.vertices);
        List<int> trianglesList = new List<int>(mesh.triangles);
        List<Vector3> outlines = new List<Vector3>();

        // 각 삼각형의 변마다 백선을 추가
        for (int i = 0; i < trianglesList.Count; i += 3)
        {
            // 삼각형의 세 꼭지점을 가져옴
            Vector3 v1 = verticesList[trianglesList[i]];
            Vector3 v2 = verticesList[trianglesList[i + 1]];
            Vector3 v3 = verticesList[trianglesList[i + 2]];

            // 삼각형의 세 변을 백선으로 추가
            outlines.Add(v1);
            outlines.Add(v2);

            outlines.Add(v2);
            outlines.Add(v3);

            outlines.Add(v3);
            outlines.Add(v1);
        }

        // 백선을 포함한 메시의 정점 배열 설정
        List<Vector3> newVertices = new List<Vector3>(verticesList);
        newVertices.AddRange(outlines);
        mesh.vertices = newVertices.ToArray();

        // 삼각형 인덱스 재설정
        int[] newTriangles = new int[trianglesList.Count + outlines.Count];
        trianglesList.CopyTo(newTriangles);
        for (int i = trianglesList.Count, j = 0; j < outlines.Count; i++, j++)
        {
            newTriangles[i] = verticesList.Count + j;
        }
        mesh.triangles = newTriangles;
    }

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

        List<int> triangles = new List<int>();

        // 상단 별의 삼각형 인덱스 설정
        for (int i = 1; i <= numPoints; i++)
        {
            int next = (i % numPoints) + 1;
            triangles.Add(0); // 중심점
            triangles.Add(i); // 현재 꼭지점
            triangles.Add(next); // 다음 꼭지점
        }

        for (int i = 1; i <= numPoints; i++)
        {
            int next = (i % numPoints) + 1;
            int index1 = i + numPoints;
            int index2 = next + numPoints;
            int nextNext = ((i + 1) % numPoints) + 1;
            int nextNextIndex1 = nextNext + numPoints;

            // 삼각형 대신 사각형을 생성
            triangles.Add(i); // 현재 상단 꼭지점
            triangles.Add(next); // 다음 상단 꼭지점
            triangles.Add(index1); // 현재 기둥 꼭지점

            triangles.Add(next); // 다음 상단 꼭지점
            triangles.Add(index2); // 다음 다음 기둥 꼭지점
            triangles.Add(index1); // 현재 기둥 꼭지점

            triangles.Add(i); // 현재 상단 꼭지점
            triangles.Add(index1); // 현재 기둥 꼭지점
            triangles.Add(nextNextIndex1); // 다음 다음 기둥 꼭지점

            triangles.Add(i); // 현재 상단 꼭지점
            triangles.Add(nextNextIndex1); // 다음 다음 기둥 꼭지점
            triangles.Add(nextNext); // 다음 다음 상단 꼭지점
        }

        mesh.triangles = triangles.ToArray();
        AddOutlineToMesh(mesh);
        // MeshFilter와 MeshRenderer 추가 및 설정
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        if (mf == null)
            mf = gameObject.AddComponent<MeshFilter>();

        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        if (mr == null)
            mr = gameObject.AddComponent<MeshRenderer>();
        //Material material = new Material(Shader.Find("Custom/NewSurfaceShader"));
        //mr.material = material;
        mf.mesh = mesh;
    }
    
}

