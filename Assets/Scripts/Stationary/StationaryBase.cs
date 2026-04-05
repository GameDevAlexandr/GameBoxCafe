using UnityEngine;
using Zenject;

public abstract class StationaryBase : MonoBehaviour
{
    public int RoadPosition { get; private set; }
    [SerializeField] private Transform _entry;
    [Inject] private PathFinder _pFinder;
    [Inject] protected Player _player;
    [Inject] protected DataManager _data;

    [Inject]
    private void Init()
    {
        _pFinder.onRoadComplete+=()=> 
        RoadPosition = _pFinder.GetRoadPosition(_entry.position);
    }
    protected virtual void OnMouseDown()
    {
        _player.Move(RoadPosition);
        _player.onMoveComplete += MoveFinish;
    }
    
    private void MoveFinish()
    {
        _player.onMoveComplete -= MoveFinish;

        if (RoadPosition == _player.RoadPosition)
            PlayerHere();
    }
    protected abstract void PlayerHere();
}
