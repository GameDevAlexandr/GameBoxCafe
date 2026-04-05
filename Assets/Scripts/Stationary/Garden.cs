using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Garden : StationaryBase
{
    [SerializeField] private int _secondsToCreate;
    [SerializeField] private Image[] _vegImages;
    [SerializeField] private Vegetable _vegPrefab;
    [Inject] private DiContainer _container;
    private int _vegCount;
    private GameObject _content;
    private void Start()
    {
        StartCoroutine(Tic());
        _content = _vegImages[0].transform.parent.gameObject;
    }
    protected override void PlayerHere()
    {
        if(_player.CurrentItem == null && _vegCount>0)
        {
            var veg = _container.InstantiatePrefabForComponent<Vegetable>(_vegPrefab);
            _player.PicItem(veg);
            _vegCount--;            
            _vegImages[_vegCount].gameObject.SetActive(false);
            if (_vegCount == 0)
                _content.SetActive(false);
        }
    }

    private IEnumerator Tic()
    {
        while (true)
        {
            yield return new WaitForSeconds(_secondsToCreate);
            if (_vegCount < _vegImages.Length)
            {
                _content.SetActive(true);
                _vegImages[_vegCount].gameObject.SetActive(true);
                _vegCount++;                
            }
        }
    }
}
