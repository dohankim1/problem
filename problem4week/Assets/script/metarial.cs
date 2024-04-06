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

        // �ﰢ�� �ε��� ����
        List<int> triangles = new List<int>();

        // ��� ���� �ﰢ�� �ε��� ����
        for (int i = 1; i <= numPoints; i++)
        {
            triangles.Add(0);
            triangles.Add(i % numPoints + 1);
            triangles.Add(i);
        }

        // �ϴ� ���� �ﰢ�� �ε��� ����
        for (int i = numPoints + 1; i <= numPoints * 2; i++)
        {
            triangles.Add(i);
            triangles.Add((i % numPoints + 1) + numPoints);
            triangles.Add(i % numPoints + numPoints + 1);
        }

        // ������ �ﰢ�� �ε��� ����
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

        // MeshFilter�� MeshRenderer �߰� �� ����
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

