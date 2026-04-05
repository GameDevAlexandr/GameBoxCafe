using UnityEngine;
using Zenject;

public abstract class ItemForPlayerBase: ItemBase
{
    [Inject] private Player _player;
    private void OnMouseDown()
    {
        if (_player.RoadPosition == RoadNumber)
        {
            _player.PicItem(this);
        }
        else
        {
            _player.Move(RoadNumber);
            _player.onMoveComplete += Pic;
        }
    }
    private void Pic()
    {
        _player.onMoveComplete -= Pic;
        if(RoadNumber == _player.RoadPosition)
        {
            SRenderer.sortingLayerName = "Player";
            SRenderer.sortingOrder = 1;
            var trs = this as Trash; 
            if(trs)
            {
                trs.Dissolve(false);
            }
            _player.PicItem(this);
        }
    }
}
