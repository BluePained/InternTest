using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxValue;
    
    private Vector3 _startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = cameraTransform.position.x * parallaxValue;
        
        transform.position = new Vector3(_startPosition.x + distance, transform.position.y, transform.position.z);
    }
}
