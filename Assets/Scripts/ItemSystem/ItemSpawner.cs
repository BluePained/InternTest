using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;
    
    [SerializeField] private Transform playerPos;
    [SerializeField] private Vector3 playerSpawnOffset;
    [SerializeField] private GameObject itemDefaultPrefab;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnResetScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnResetScene;
    }

    private void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Vector3 GetPlayerSpawnOffset()
    {
        return playerPos.localPosition + playerSpawnOffset;
    }
    
    public void SpawnItem(Item itemData, int amount, Vector3 position)
    {
        GameObject obj = Instantiate(itemDefaultPrefab, position, Quaternion.identity);
        ItemInfo itemInfo = obj.GetComponent<ItemInfo>();
        itemInfo.OnItemSpawn(itemData, amount);
    }
    
    #region Reset Scene

    private void OnResetScene(Scene scene, LoadSceneMode mode)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    #endregion
}
