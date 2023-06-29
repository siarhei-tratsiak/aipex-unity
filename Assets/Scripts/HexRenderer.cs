using System;
using System.Collections.Generic;
using UnityEngine;

public class HexRenderer : MonoBehaviour
{
    public float height;
    public float innerSize;
    public bool isFlatTopped;
    public Material material;
    public float outerSize;

    private List<Face> faces;
    private Mesh mesh;

    // Start is called before the first frame update
    private void Start()
    {
        DrawMesh();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void DrawMesh()
    {
        mesh = new()
        {
            name = "Hex"
        };

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = material;

        DrawFaces();
        CombineFaces();
    }

    private void DrawFaces()
    {
        Face topFace(int faceIndex)
        {
            return GetFace(innerSize, outerSize, height, height, faceIndex);
        }

        Face bottomFace(int faceIndex)
        {
            return GetFace(innerSize, outerSize, 0, 0, faceIndex, true);
        }

        Face outerFace(int faceIndex)
        {
            return GetFace(outerSize, outerSize, height, 0, faceIndex, true);
        }

        Face innerFace(int faceIndex)
        {
            return GetFace(innerSize, innerSize, height, 0, faceIndex);
        }

        Func<int, Face>[] faceGetters = { topFace, bottomFace, outerFace, innerFace };
        faces = new List<Face>();

        foreach (Func<int, Face> getFace in faceGetters)
        {
            for (int faceIndex = 0; faceIndex < 6; faceIndex++)
            {
                faces.Add(getFace(faceIndex));
            }
        }
    }

    private Face GetFace(
        float innerRadius,
        float outerRadius,
        float heightA,
        float heightB,
        int faceNumber,
        bool reverse = false
    )
    {
        int nextFaceNumber = (faceNumber < 5) ? faceNumber + 1 : 0;
        Vector3 point0 = GetPoint(innerRadius, heightB, faceNumber);
        Vector3 point1 = GetPoint(innerRadius, heightB, nextFaceNumber);
        Vector3 point2 = GetPoint(outerRadius, heightA, nextFaceNumber);
        Vector3 point3 = GetPoint(outerRadius, heightA, faceNumber);

        List<Vector3> vertices = new() { point0, point1, point2, point3 };
        List<int> triangles = new() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new() {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

        if (reverse)
        {
            vertices.Reverse();
        }

        return new Face(vertices, triangles, uvs);
    }

    protected Vector3 GetPoint(float radius, float height, int faceNumber)
    {
        float angleDegrees = isFlatTopped ? 60 * faceNumber : (60 * faceNumber) - 30;
        float angleRadians = Mathf.PI / 180f * angleDegrees;

        float x = radius * Mathf.Cos(angleRadians);
        float y = height;
        float z = radius * Mathf.Sin(angleRadians);

        return new Vector3(x, y, z);
    }

    private void CombineFaces()
    {
        List<Vector3> vertices = new();
        List<int> triangles = new();
        List<Vector2> uvs = new();

        for (int i = 0; i < faces.Count; i++)
        {
            Face face = faces[i];

            vertices.AddRange(face.vertices);
            uvs.AddRange(face.uvs);

            int offset = 4 * i;

            foreach (int triangle in face.triangles)
            {
                triangles.Add(triangle + offset);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
    }
    private struct Face
    {
        public List<Vector3> vertices { get; private set; }
        public List<int> triangles { get; private set; }
        public List<Vector2> uvs { get; private set; }

        public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
        {
            this.vertices = vertices;
            this.triangles = triangles;
            this.uvs = uvs;
        }
    }
}


