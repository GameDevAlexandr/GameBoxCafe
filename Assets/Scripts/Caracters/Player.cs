using DG.Tweening;
using UnityEngine;
using Zenject;

public class Player : CharacterBase
{
    public System.Action onChangePosition;
    public void Init()
    {
        RoadPosition = _pFinder.GetRoadPosition(transform.position);
        transform.position = _pFinder.RoadPoints[RoadPosition];
    }
    private void Update()
    {
        if (Input.GetMouseButton(1) && CurrentItem != null)
        {
            CurrentItem.Drop();
            CurrentItem = null;
        }
    }
    protected override void TakeStep()
    {
        onChangePosition?.Invoke();
        base.TakeStep();
    }
}
