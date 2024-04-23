using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(metarial))]
public class StaticMeshGenEditor : Editor
{
    //��ư����� ����
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
    public int numPoints = 5; // ���� ������ ��
    public float radius = 1f; // ���� ������
    public float height = 2f; // ����� ����

    void AddOutlineToMesh(Mesh mesh)
    {
        List<Vector3> verticesList = new List<Vector3>(mesh.vertices);
        List<int> trianglesList = new List<int>(mesh.triangles);
        List<Vector3> outlines = new List<Vector3>();

        // �� �ﰢ���� ������ �鼱�� �߰�
        for (int i = 0; i < trianglesList.Count; i += 3)
        {
            // �ﰢ���� �� �������� ������
            Vector3 v1 = verticesList[trianglesList[i]];
            Vector3 v2 = verticesList[trianglesList[i + 1]];
            Vector3 v3 = verticesList[trianglesList[i + 2]];

            // �ﰢ���� �� ���� �鼱���� �߰�
            outlines.Add(v1);
            outlines.Add(v2);

            outlines.Add(v2);
            outlines.Add(v3);

            outlines.Add(v3);
            outlines.Add(v1);
        }

        // �鼱�� ������ �޽��� ���� �迭 ����
        List<Vector3> newVertices = new List<Vector3>(verticesList);
        newVertices.AddRange(outlines);
        mesh.vertices = newVertices.ToArray();

        // �ﰢ�� �ε��� �缳��
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

        // ����� �׸��� ���� ���� ��ǥ ����
        List<Vector3> verticesList = new List<Vector3>();

        // ����� ��� �߽���
        verticesList.Add(new Vector3(0, height, 0));

        // ����� ��� ���� ������ ��ǥ ����
        for (int i = 0; i < numPoints; i++)
        {
            float angle = Mathf.PI * 2 * i / numPoints;
            verticesList.Add(new Vector3(Mathf.Cos(angle) * radius, height, Mathf.Sin(angle) * radius));
        }

        // ����� �ϴ� ���� ������ ��ǥ ����
        for (int i = 0; i < numPoints; i++)
        {
            float angle = Mathf.PI * 2 * i / numPoints;
            verticesList.Add(new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius));
        }

        // ���� �迭�� ��ȯ
        Vector3[] vertices = verticesList.ToArray();
        mesh.vertices = vertices;

        List<int> triangles = new List<int>();

        // ��� ���� �ﰢ�� �ε��� ����
        for (int i = 1; i <= numPoints; i++)
        {
            int next = (i % numPoints) + 1;
            triangles.Add(0); // �߽���
            triangles.Add(i); // ���� ������
            triangles.Add(next); // ���� ������
        }

        for (int i = 1; i <= numPoints; i++)
        {
            int next = (i % numPoints) + 1;
            int index1 = i + numPoints;
            int index2 = next + numPoints;
            int nextNext = ((i + 1) % numPoints) + 1;
            int nextNextIndex1 = nextNext + numPoints;

            // �ﰢ�� ��� �簢���� ����
            triangles.Add(i); // ���� ��� ������
            triangles.Add(next); // ���� ��� ������
            triangles.Add(index1); // ���� ��� ������

            triangles.Add(next); // ���� ��� ������
            triangles.Add(index2); // ���� ���� ��� ������
            triangles.Add(index1); // ���� ��� ������

            triangles.Add(i); // ���� ��� ������
            triangles.Add(index1); // ���� ��� ������
            triangles.Add(nextNextIndex1); // ���� ���� ��� ������

            triangles.Add(i); // ���� ��� ������
            triangles.Add(nextNextIndex1); // ���� ���� ��� ������
            triangles.Add(nextNext); // ���� ���� ��� ������
        }

        mesh.triangles = triangles.ToArray();
        AddOutlineToMesh(mesh);
        // MeshFilter�� MeshRenderer �߰� �� ����
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

