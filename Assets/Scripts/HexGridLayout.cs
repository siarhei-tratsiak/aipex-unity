using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize;

    [Header("Tile Settings")]
    public GameObject hexagonPrefab;
    public float outerSize = 1f;
    public float innerSize = 0f;
    public float height = 1f;
    public bool isFlatTopped;
    public Material material;

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            LayoutGrid();
        }
    }
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void LayoutGrid()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = Instantiate(
                    hexagonPrefab,
                    GetPositionForHexFromCoordinate(new Vector2Int(x, y)),
                    Quaternion.identity,
                    transform
                );

                HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                Renderer renderer = tile.GetComponent<Renderer>();

                hexRenderer.isFlatTopped = isFlatTopped;
                hexRenderer.height = height;
                hexRenderer.outerSize = outerSize;
                hexRenderer.innerSize = innerSize;
                //hexRenderer.material = material;
                renderer.SetMaterials(new System.Collections.Generic.List<Material> { material });
                hexRenderer.DrawMesh();
            }
        }
    }

    private Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;
        float width;
        float height;
        bool shouldOffset;
        float horizontalDistance;
        float verticalDistance;
        float xPosition;
        float yPosition;
        float offset;
        float size = outerSize;

        if (!isFlatTopped)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3f) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f / 4f);

            offset = shouldOffset ? width / 2 : 0;

            xPosition = (column * horizontalDistance) + offset;
            yPosition = row * verticalDistance;
        }
        else
        {
            shouldOffset = (column % 2) == 0;
            width = 2f * size;
            height = Mathf.Sqrt(3f) * size;

            horizontalDistance = width * (3f / 4f);
            verticalDistance = height;

            offset = shouldOffset ? height / 2 : 0;

            xPosition = column * horizontalDistance;
            yPosition = (row * verticalDistance) + offset;
        }

        return new Vector3(xPosition, 0, -yPosition);
    }
}
