using DG.Tweening;
using UnityEngine;
using Zenject;

public abstract class CharacterBase : MonoBehaviour
{
    public ItemBase CurrentItem { get; protected set; }
    public int RoadPosition { get; protected set; }
    public System.Action onMoveComplete;
    [Inject] protected PathFinder _pFinder;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _itemPosition;

    private int[] _path;
    private int _step;
    private Tween _tween;

    public virtual void SetPosition(int positionNumber)
    {
        RoadPosition = positionNumber;
        transform.position = _pFinder.RoadPoints[positionNumber];
    }
    public void Move(int target)
    {
        _path = _pFinder.FindPath(RoadPosition, target).ToArray();
        if (_path == null)
        {
            return;
        }
        if( _path.Length == 1)
        {
            onMoveComplete?.Invoke();
            return;
        }
        _step = 1;
        TakeStep();
    }
    protected virtual void TakeStep()
    {
        _tween?.Kill();
        _tween = transform.DOMove(_pFinder.RoadPoints[_path[_step]], 1 / _speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            RoadPosition = _path[_step];
            _step++;
            if (_step == _path.Length)
            {
                onMoveComplete?.Invoke();
                Debug.Log(name + " finish");
                return;
            }
            TakeStep();
        });
    }
    public virtual void PicItem(ItemBase item)
    {
        CurrentItem = item;
        CurrentItem.SRenderer.sortingLayerName = "Player";
        CurrentItem.SRenderer.sortingOrder = 1;
        CurrentItem.transform.position = _itemPosition.position;
        CurrentItem.transform.SetParent(transform);
    }
    public void RemoveItem()
    {
        Destroy(CurrentItem.gameObject);
        CurrentItem = null;       
    }
}
