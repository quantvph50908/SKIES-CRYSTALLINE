using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int speed = 300;
    bool isMoving = false;
    private bool gameOver = false;

    private Button upButton;
    private Button downButton;
    private Button rightButton;
    private Button leftButton;
    private Button resetButton;

    private MapGenerator mapGenerator;
    private Image winImage;
    private Image loseImage;

    public GameObject winParticlePrefab;
    public GameObject loseParticlePrefab;

    void Start()
    {
        upButton = GameObject.Find("Bt_Up")?.GetComponent<Button>();
        downButton = GameObject.Find("Bt_Down")?.GetComponent<Button>();
        rightButton = GameObject.Find("Bt_Right")?.GetComponent<Button>();
        leftButton = GameObject.Find("Bt_Left")?.GetComponent<Button>();
        resetButton = GameObject.Find("ResetButton")?.GetComponent<Button>();

        if (upButton != null) upButton.onClick.AddListener(() => { if (!gameOver) StartCoroutine(Roll(Vector3.forward)); });
        if (downButton != null) downButton.onClick.AddListener(() => { if (!gameOver) StartCoroutine(Roll(Vector3.back)); });
        if (rightButton != null) rightButton.onClick.AddListener(() => { if (!gameOver) StartCoroutine(Roll(Vector3.right)); });
        if (leftButton != null) leftButton.onClick.AddListener(() => { if (!gameOver) StartCoroutine(Roll(Vector3.left)); });
        if (resetButton != null) resetButton.onClick.AddListener(ResetGame);

        mapGenerator = FindObjectOfType<MapGenerator>();
        winImage = GameObject.Find("WinImage")?.GetComponent<Image>();
        loseImage = GameObject.Find("LoseImage")?.GetComponent<Image>();

        if (winImage != null) winImage.gameObject.SetActive(false);
        if (loseImage != null) loseImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isMoving || gameOver)
        {
            return;
        }
    }

    IEnumerator Roll(Vector3 direction)
    {
        isMoving = true;
        float remainingAngle = 90;
        Vector3 rotationCenter = transform.position + (direction / 2f);
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }
        isMoving = false;

        Vector3 newPos = new Vector3(transform.position.x, 1.3f, transform.position.z);
        transform.position = newPos;

        Debug.Log($"Player Position: {newPos}");

        if (mapGenerator != null && IsAtDestination())
        {
            Win();
        }

        if (mapGenerator != null && !IsOnMap())
        {
            Lose();
        }
    }

    bool IsAtDestination()
    {
        if (mapGenerator == null) return false;
        Vector2Int destPos = mapGenerator.destinationPosition;
        float tolerance = 0.1f;
        bool xMatch = Mathf.Abs(transform.position.x - destPos.x) < tolerance;
        Debug.Log($"Destination Check - x: {xMatch}");
        return xMatch;
    }

    bool IsOnMap()
    {
        if (mapGenerator == null || mapGenerator.walkablePositions == null) return false;
        int playerX = Mathf.RoundToInt(transform.position.x);
        int playerZ = Mathf.RoundToInt(transform.position.z);
        bool isOnMap = mapGenerator.walkablePositions.Contains(new Vector2Int(playerX, playerZ));
        Debug.Log($"IsOnMap Check - x: {playerX}, z: {playerZ}, Result: {isOnMap}");
        return isOnMap;
    }

    void Win()
    {
        if (!gameOver && winImage != null)
        {
            gameOver = true;
            winImage.gameObject.SetActive(true);
            if (mapGenerator != null && winParticlePrefab != null)
            {
                Vector2Int destPos = mapGenerator.destinationPosition;
                Instantiate(winParticlePrefab, new Vector3(destPos.x, 1.3f, destPos.y), Quaternion.identity);
            }
            LevelManager.Instance.OnLevelWin();
            Debug.Log("You Win!");
        }
    }

    void Lose()
    {
        if (!gameOver && loseImage != null)
        {
            gameOver = true;
            loseImage.gameObject.SetActive(true);
            if (loseParticlePrefab != null)
            {
                Instantiate(loseParticlePrefab, transform.position, Quaternion.identity);
            }
            LevelManager.Instance.ResetCurrentLevel(); // Reset level khi thua
            Debug.Log("You Lose!");
        }
    }

    void ResetGame()
    {
        if (gameOver)
        {
            gameOver = false;
            if (winImage != null) winImage.gameObject.SetActive(false);
            if (loseImage != null) loseImage.gameObject.SetActive(false);
            LevelManager.Instance.ResetCurrentLevel();
        }
    }
}