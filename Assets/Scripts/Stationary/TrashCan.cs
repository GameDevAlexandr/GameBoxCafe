using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TrashCan : StationaryBase
{
    public int Level { get; private set; }
    
    
    [SerializeField] private Button _llvUpButton;
    [SerializeField] private Text _priceText;
    [SerializeField] private GameObject _trashCanObj;
    [SerializeField] private TrashBug _bag;
    [SerializeField] private Image[] _trashIcons;
    [SerializeField] private UpgradeData[] _upgrades;
    private int _price;
    private int _trashCount;
    private GameObject _imgContent;
    private List<TrashBug> _bags = new List<TrashBug>();
    private int _needTrashForBug => _trashIcons.Length;
    private bool _isMaxLvl => Level == _upgrades.Length;
    [Inject] DiContainer _container;
    [System.Serializable]
    public struct UpgradeData
    {
        public int price;
        public GameObject newTrashCan;
        public TrashBug bag;
    }
    private void Start()
    {
        _imgContent = _trashIcons[0].transform.parent.gameObject;
        _player.onChangePosition += () =>
          {
              if(RoadPosition != _player.RoadPosition)
              {
                  _llvUpButton.gameObject.SetActive(false);
              }
          };
        _llvUpButton.onClick.AddListener(Upgrade);
        SetPrice();
        _data.onChangeCoins += () =>
              _llvUpButton.interactable = _price <= _data.Coins;
    }

    public void ClientHere(Client client)
    {
        if (client.CurrentItem != null)
        {
            AddTrash();
            client.RemoveItem();
        }
    }
    private void AddTrash()
    {
        _imgContent.SetActive(true);
        _trashIcons[_trashCount].gameObject.SetActive(true);
        _trashCount++;
        if (_trashCount == _needTrashForBug)
        {
            _trashCount = 0;
            foreach (var img in _trashIcons)
            {
                img.gameObject.SetActive(false);
            }
            _imgContent.SetActive(false);
            var bag = _container.InstantiatePrefabForComponent<TrashBug>(_bag);
            _bags.Add(bag);
            bag.Drop();
            return;
        }        
    }
    protected override void PlayerHere()
    {
        if (RoadPosition != _player.RoadPosition)
            return;

        if(Level<_upgrades.Length)
            _llvUpButton.gameObject.SetActive(true);
        
        if(_player.CurrentItem is Food)
        {
            AddTrash();
            _player.RemoveItem();
        }
    }
    private void Upgrade()
    {
        Destroy(_trashCanObj.gameObject);
        _trashCanObj = Instantiate(_upgrades[Level].newTrashCan, transform.position, Quaternion.identity);
        _bag = _upgrades[Level].bag;
        Level++;
        _llvUpButton.gameObject.SetActive(Level < _upgrades.Length);
        int oldPrice = _price;  
        
        if (!_isMaxLvl)
            SetPrice();

        _data.ChangeCoins(-oldPrice);
    }
    private void SetPrice()
    {
        _price = _upgrades[Level].price;
        _priceText.text = _price.ToString();
    }
    public bool ClearCan()
    {
        int cnt = 0;
        if (_bags.Count == 0)
            return false;
        while(cnt<_bags.Count)
        {
            var b = _bags[cnt];
            if (b.RemovePrice <= _data.Coins)
            {
                _data.ChangeCoins(-b.RemovePrice);
                Destroy(b.gameObject);
                _bags.RemoveAt(cnt);
                continue;
            }
            cnt++;
        }
        return true;
    }
    private void OnMouseEnter()=>_trashCanObj.GetComponent<SelectItem>().OnMouseEnter();
    private void OnMouseExit()=>_trashCanObj.GetComponent<SelectItem>().OnMouseExit();

}
