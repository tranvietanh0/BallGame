using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [Header("Wall Settings")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float wallThickness = 0.5f;

    [Header("Screen Offset (optional)")]
    [SerializeField] private float screenOffset = 0f;

    [Header("Auto Spawn")]
    [SerializeField] private bool spawnOnStart = true;

    private Camera mainCamera;

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnWallsManually();
        }
    }

    public void SpawnWallsManually()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        if (wallPrefab == null)
        {
            Debug.LogError("Wall Prefab is not assigned!");
            return;
        }

        SpawnWalls();
    }

    public void ClearWalls()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            #if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(i).gameObject);
            #else
            Destroy(transform.GetChild(i).gameObject);
            #endif
        }
    }

    void SpawnWalls()
    {
        // Get screen bounds in world space
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        float screenWidth = topRight.x - bottomLeft.x;
        float screenHeight = topRight.y - bottomLeft.y;

        float left = bottomLeft.x - screenOffset;
        float right = topRight.x + screenOffset;
        float top = topRight.y + screenOffset;
        float bottom = bottomLeft.y - screenOffset;

        // Spawn 4 walls
        SpawnWall("TopWall", new Vector2(0, top), new Vector2(screenWidth + wallThickness * 2, wallThickness));
        SpawnWall("BottomWall", new Vector2(0, bottom), new Vector2(screenWidth + wallThickness * 2, wallThickness));
        SpawnWall("LeftWall", new Vector2(left, 0), new Vector2(wallThickness, screenHeight));
        SpawnWall("RightWall", new Vector2(right, 0), new Vector2(wallThickness, screenHeight));
    }

    void SpawnWall(string wallName, Vector2 position, Vector2 size)
    {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
        wall.name = wallName;

        // Set collider size
        BoxCollider2D collider = wall.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = wall.AddComponent<BoxCollider2D>();
        }
        collider.size = size;

        // Optional: Adjust sprite size if wall has SpriteRenderer
        SpriteRenderer spriteRenderer = wall.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            wall.transform.localScale = new Vector3(size.x, size.y, 1);
        }
    }
}