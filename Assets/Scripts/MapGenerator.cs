using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    public GameObject walkableTilePrefab; // Prefab cho ô đi được
    public GameObject playerPrefab; // Prefab cho player
    public GameObject destinationPrefab; // Prefab cho điểm đích
    public List<Vector2Int> walkablePositions; // Danh sách vị trí ô đi được
    public Vector2Int destinationPosition; // Vị trí đích

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        // Xóa bản đồ cũ (nếu có)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Tạo ô đi được với vị trí tùy chỉnh
        foreach (Vector2Int pos in walkablePositions)
        {
            Vector3 position = new Vector3(pos.x, 0.25f, pos.y); // Đặt y = 0.25 để nổi lên
            Instantiate(walkableTilePrefab, position, Quaternion.identity, transform);
        }

        // Sinh player tại vị trí cố định (1, 1.3, 0)
        Vector3 playerSpawnPos = new Vector3(1f, 1.3f, 0f); // x = 1, y = 1.3, z = 0
        Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity, transform);

        // Sinh điểm đích tại vị trí chỉ định với y = 1.3 và rotation X = 90 độ
        Vector3 destinationPos = new Vector3(destinationPosition.x, 1.3f, destinationPosition.y);
        Quaternion destinationRotation = Quaternion.Euler(90f, 0f, 0f); // X = 90, Y = 0, Z = 0
        Instantiate(destinationPrefab, destinationPos, destinationRotation, transform);
    }

    public void Home() => SceneManager.LoadScene(0);
}