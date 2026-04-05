using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Cafe : StationaryBase
{
    [SerializeField] private float _coockTime;
    [SerializeField] private int _pollution;
    [SerializeField] private int _foodPrise;
    [SerializeField] private int _foodsAtVegatable;
    [SerializeField] private int _upgradePrice;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Text _upgradePriceText;
    [SerializeField] private Text _pollutionText;
    [SerializeField] private Image[] _queueImages;
    [SerializeField] private Image[] _foodsImages;
    [SerializeField] private Image[] _vegatablesImages;
    [SerializeField] private Food _foodPrefab;
    private int _vegCount;
    private int _foodCount;
    private bool _isUpgrade;
    private Queue<Client> _clients = new Queue<Client>();
    [Inject] private DiContainer _container;
    private void Start()
    {
        StartCoroutine(Tic());
        _upgradePriceText.text = _upgradePrice.ToString();
        _pollutionText.text = _pollution.ToString();
        _player.onChangePosition += () =>
        {
            if (_player.RoadPosition != RoadPosition)
                _upgradeButton.gameObject.SetActive(false);
        };
        _data.onChangeCoins += () => _upgradeButton.interactable=_data.Coins >= _upgradePrice;
        _upgradeButton.onClick.AddListener(() =>
        {
            _isUpgrade = true;
            _pollutionText.text = "0"; 
            _upgradeButton.gameObject.SetActive(false);
        });
    }
    protected override void PlayerHere()
    {
        if(_player.CurrentItem is Vegetable)
        {
            _player.RemoveItem();
            _vegCount = Mathf.Min(_vegatablesImages.Length,_vegCount+ _foodsAtVegatable);
            SetImage(_vegatablesImages, _vegCount);
        }
        if(!_isUpgrade)
            _upgradeButton.gameObject.SetActive(true);
    }
    private bool SetImage(Image[] arrImg, int count)
    {
        if (count >= arrImg.Length)
            return false;
        for (int i = 0; i < arrImg.Length; i++)
        {
            arrImg[i].gameObject.SetActive(i < count);
        }
        return true;
    }
    public void ClientHere(Client client)
    {
        if (_queueImages.Length == _clients.Count)
        {
            client.GoHome();
        }
        else
        {
            _clients.Enqueue(client);
            _queueImages[_clients.Count - 1].gameObject.SetActive(true);
        }
    }
    private void SellFood()
    {
        if (_clients.Count > 0 && _foodCount > 0)
        {
            var f = _container.InstantiatePrefabForComponent<Food>(_foodPrefab);
            _clients.Dequeue().PicItem(f);
            _foodCount--;
            SetImage(_foodsImages, _foodCount);
            SetImage(_queueImages, _clients.Count);
            _data.ChangeCoins(_foodPrise);
        }
    }
    private IEnumerator Tic()
    {
        while (true)
        {
            yield return new WaitForSeconds(_coockTime);
            if (_vegCount > 0 && _foodCount<_foodsImages.Length)
            {
                _vegCount--;
                SetImage(_vegatablesImages, _vegCount);
                _foodCount++;
                SetImage(_foodsImages, _foodCount);
                if(!_isUpgrade)
                    _data.ChangePollution(_pollution);
            }
            SellFood();
        }
    }
}
