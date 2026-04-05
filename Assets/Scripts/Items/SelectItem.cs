using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SelectItem : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Material _defaultMat;
    [SerializeField] private Material _outlineMat;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMat = _spriteRenderer.material;
    }
    public void OnMouseEnter()
    {        
        _spriteRenderer.material = _outlineMat;
    }
    public void OnMouseExit()
    {
        _spriteRenderer.material = _defaultMat;
    }
}
