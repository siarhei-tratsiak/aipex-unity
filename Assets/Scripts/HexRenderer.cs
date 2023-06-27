using System.Collections.Generic;
using UnityEngine;

public class HexRenderer : MonoBehaviour
{
    public float height;
    public float innerSize;
    public bool isFlatTopped;
    public Material material;
    public float outerSize;

    private List<Face> m_faces;
    private Mesh m_mesh;
    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;

    private void Awake()
    {
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();

        m_mesh = new Mesh
        {
            name = "Hex"
        };

        m_meshFilter.mesh = m_mesh;
        m_meshRenderer.material = material;
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    private void OnEnable()
    {
        DrawMesh();
    }

    private void OnValidate()
    {
        if (Application.isPlaying && m_mesh != null)
        {
            DrawMesh();
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }

    private void DrawFaces()
    {
        m_faces = new List<Face>();

        // Top faces
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, point));
        }

        // Bottom faces
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, outerSize, -height / 2f, -height / 2f, point, true));
        }

        // Outer faces
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(outerSize, outerSize, height / 2f, -height / 2f, point, true));
        }

        // Inner faces
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, innerSize, height / 2f, -height / 2f, point));
        }
    }

    private Face CreateFace(
        float innerRad,
        float outerRad,
        float heightA,
        float heightB,
        int point,
        bool reverse = false
    )
    {
        Vector3 pointA = GetPoint(innerRad, heightB, point);
        Vector3 pointB = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRad, heightA, point);

        List<Vector3> vertices = new() { pointA, pointB, pointC, pointD };
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

    protected Vector3 GetPoint(float size, float height, int index)
    {
        float angleDegrees = isFlatTopped ? 60 * index : (60 * index) - 30;
        float angleRadians = Mathf.PI / 180f * angleDegrees;

        float x = size * Mathf.Cos(angleRadians);
        float y = height;
        float z = size * Mathf.Sin(angleRadians);

        return new Vector3(x, y, z);
    }

    private void CombineFaces()
    {
        List<Vector3> vertices = new();
        List<int> tris = new();
        List<Vector2> uvs = new();

        for (int i = 0; i < m_faces.Count; i++)
        {
            // Add the vertices
            vertices.AddRange(m_faces[i].vertices);
            uvs.AddRange(m_faces[i].uvs);

            // Offset the triangles
            int offset = 4 * i;
            foreach (int triangle in m_faces[i].triangles)
            {
                tris.Add(triangle + offset);
            }
        }

        Vector3[] verticesArray = vertices.ToArray();
        _ = m_mesh.vertices;
        m_mesh.vertices = verticesArray;
        m_mesh.triangles = tris.ToArray();
        m_mesh.uv = uvs.ToArray();
        m_mesh.RecalculateNormals();
    }
}

public struct Face
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
