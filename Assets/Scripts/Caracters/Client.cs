using UnityEngine;
using Zenject;

public class Client : CharacterBase
{
    private int _startRoadNumber;
    [SerializeField] private Vector2 _dropTimeInterval;
    [Inject] private  Cafe _cafe;
    [Inject] private ClientSpawner _spawner;
    [Inject] private Bench _bench;
    [Inject] private TrashCan _trashCan;
    private float _dropTime;
    public override void SetPosition(int positionNumber)
    {
        base.SetPosition(positionNumber);
        _startRoadNumber = RoadPosition;
    }
    public void MoveToCafe()
    {
        gameObject.SetActive(true);        
        Move(_cafe.RoadPosition);
        onMoveComplete += InCafe;
    }
    private void InCafe()
    {
        if (RoadPosition == _cafe.RoadPosition)
        {
            onMoveComplete -= InCafe;
            gameObject.SetActive(false);
            _cafe.ClientHere(this);
        }
    }
    public override void PicItem(ItemBase item)
    {
        _dropTime = Random.Range(_dropTimeInterval.x, _dropTimeInterval.y);
        gameObject.SetActive(true);
        base.PicItem(item);
        if (_trashCan.Level > 0)
        {
            Move(_trashCan.RoadPosition);
            onMoveComplete += InTrashCan;
        }
        else
        {
            Move(_bench.RoadPosition);
            onMoveComplete += InBench;
            Invoke("DropTrash", _dropTime);
        }        
    }
    private void DropTrash()
    {
        if (CurrentItem == null)
            return;

        CurrentItem.Drop();
        CurrentItem = null;
    }
    public void GoHome()
    {
        gameObject.SetActive(true);
        Move(_startRoadNumber);
        onMoveComplete += InHome;
    }
    private void InHome()
    {
        if(_startRoadNumber == RoadPosition)
        {
            _spawner.Comeback(this);
            gameObject.SetActive(false);
        }
    }
    private void InBench()
    {
        if(RoadPosition == _bench.RoadPosition)
        {
            if (CurrentItem != null)
            {
                DropTrash();
            }
            gameObject.SetActive(false);
            _bench.ClientHere(this);
            onMoveComplete -= InBench;
        }
    }
    private void InTrashCan()
    {
        if (RoadPosition == _trashCan.RoadPosition)
        {
            _trashCan.ClientHere(this);
            onMoveComplete -= InTrashCan;
            Move(_bench.RoadPosition);
            onMoveComplete += InBench;
        }
    }
}
