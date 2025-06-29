using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LevelManager : MonoBehaviour
{
    private GameObject[] levelPrefabs;
    private int currentLevel = 0;
    private GameObject currentLevelInstance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AutoLoadPrefabs();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public static LevelManager Instance { get; private set; }

    void AutoLoadPrefabs()
    {
        string levelsPath = "Assets/Prefabs/Level";
        if (!Directory.Exists(levelsPath))
        {
            return;
        }

        levelPrefabs = new GameObject[15];
        for (int i = 0; i < 15; i++)
        {
            string prefabName = $"Level{i + 1}.prefab";
            string prefabPath = Path.Combine(levelsPath, prefabName);

#if UNITY_EDITOR
            levelPrefabs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
#else
#endif

            if (levelPrefabs[i] == null)
            {
                return;
            }
        }
    }

    void Start()
    {
        if (levelPrefabs == null || levelPrefabs.Length != 15)
        {
            return;
        }
        if (currentLevel >= levelPrefabs.Length)
        {
            return;
        }
        LoadLevel(currentLevel);
    }

    public void OnLevelWin()
    {
        if (currentLevel < levelPrefabs.Length - 1)
        {
            currentLevel++;
            CleanupCurrentLevel();
            LoadLevel(currentLevel);
        }
        else
        {
            CleanupCurrentLevel();
        }
    }

    void CleanupCurrentLevel()
    {
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
        }
    }

    void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelPrefabs.Length)
        {
            CleanupCurrentLevel();
            currentLevelInstance = Instantiate(levelPrefabs[levelIndex], Vector3.zero, Quaternion.identity);
            currentLevelInstance.transform.SetParent(transform);
            currentLevelInstance.SetActive(true);
        }
    }

    public void ResetCurrentLevel()
    {
        if (currentLevelInstance != null)
        {
            CleanupCurrentLevel();
            LoadLevel(currentLevel);
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}