using System.Collections;
using UnityEngine;
using Zenject;

public class TrashCar : CharacterBase
{
    [SerializeField] private int _spawnDelay;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [Inject] private TrashCan _trashCan;
    private int _endPosition;
    private int _startPosition;
    [Inject]
    private void Init()
    {
        _pFinder.onRoadComplete += () =>
        {
            _endPosition = _pFinder.GetRoadPosition(_endPoint.position);
            _startPosition = _pFinder.GetRoadPosition(_startPoint.position);
            StartCoroutine(Tic());
        };
    }
    private IEnumerator Tic()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);
            transform.position = _startPoint.position;
            RoadPosition = _startPosition;
            Move(_trashCan.RoadPosition);
            onMoveComplete += EndMove;
        }
    }

    private void EndMove()
    {
        if (_trashCan.ClearCan())
        {
            Invoke("MoveOut", 2f);
        }
        else
        {
            MoveOut();
        }
        
    }
    private void MoveOut()
    {
        Move(_endPosition);
        onMoveComplete -= EndMove;
    }
}
