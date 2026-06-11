using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnDragItem : MonoBehaviour
{
    public static OnDragItem Instance;
    [SerializeField] private Image dragItem;
    public bool IsDragging { get; private set; }
    private ItemInfo _itemInfo;
    

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
        dragItem = GameObject.Find("DragItem").GetComponent<Image>();
        dragItem.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            dragItem.transform.localPosition = Mouse.current.position.ReadValue();
        }
    }

    public void OnDragging(Sprite sprite, ItemInfo itemInfo)
    {
        IsDragging = true;
        _itemInfo = itemInfo;
        dragItem.sprite = sprite;
        dragItem.gameObject.SetActive(true);
    }

    public void OnCompleteDrag()
    {
        IsDragging = false;
        _itemInfo = null;
        dragItem.sprite = null;
        dragItem.gameObject.SetActive(false);
    }
    
    #region Reset Scene

    private void OnResetScene(Scene scene, LoadSceneMode mode)
    {
        dragItem = GameObject.Find("DragItem").GetComponent<Image>();
        dragItem.gameObject.SetActive(false);
    }

    #endregion
}
