using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Trash : ItemForPlayerBase
{
    [SerializeField] private int _pollutionCount;
    [SerializeField] private int _dissolutionSeconds;
    [SerializeField] private Image _bar;
    [SerializeField] private GameObject _mud;
    [Inject] private DataManager _data;
    private float _dissolution;
    private bool _isStart;
    public void Dissolve(bool isStart)
    {
        _isStart = isStart;
        _bar.enabled =isStart;
    }
    private void Dissolution()
    {
        _data.ChangePollution(_pollutionCount);
        Instantiate(_mud, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void Update()
    {
        if (_isStart)
        {
            _dissolution += Time.deltaTime;
            _bar.fillAmount = 1f - _dissolution / _dissolutionSeconds;
            if (_dissolution >= _dissolutionSeconds)
            {               
                Dissolution();
            }
        }
        
    }
    public override void Drop()
    {
        Dissolve(true);
        base.Drop();
    }
}
