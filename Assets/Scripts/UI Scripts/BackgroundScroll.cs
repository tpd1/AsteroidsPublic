using UnityEngine;

/// <summary>
/// Handles the parallax scrolling effect for the menu background stars. 
/// </summary>
public class BackgroundScroll : MonoBehaviour
{

    [SerializeField] private Vector2 scrollSpeed;
    private Vector2 currentOffset;
    private Material scrollMaterial;
    

    private void Awake()
    {
        scrollMaterial = GetComponent<SpriteRenderer>().material;
    }
    
    void Update()
    {
        currentOffset = scrollSpeed * Time.deltaTime;
        scrollMaterial.mainTextureOffset += currentOffset;
    }
}
