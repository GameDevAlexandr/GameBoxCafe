using UnityEngine;
using UnityEngine.UI;

public class Road : MonoBehaviour
{
    private int _number;
    private PathFinder _pFinder;
    public void SetData(int number, PathFinder pathFinder)
    {
        _number = number;
        _pFinder = pathFinder;
    }
    private void OnMouseDown()
    {
        _pFinder.SelectCell(_number);
    }
}
