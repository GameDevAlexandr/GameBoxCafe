using UnityEngine;
using Zenject;

public abstract class ItemBase : MonoBehaviour
{
    public int RoadNumber { get; protected set; }
    [field: SerializeField] public SpriteRenderer SRenderer { get; private set; }
    [SerializeField] private float _dropRadius;    
    private CharacterBase _char;
    [Inject] private PathFinder _pFinder;
    public virtual void Drop()
    {
        SRenderer.sortingLayerName = "Gound";
        SRenderer.sortingOrder = 1;
        Vector2 pos = transform.position;
        pos += new Vector2(Random.Range(-_dropRadius, _dropRadius), Random.Range(-_dropRadius, _dropRadius));
        transform.position = pos;
        transform.SetParent(null);
        RoadNumber = _pFinder.GetRoadPosition(pos);
    }
}
