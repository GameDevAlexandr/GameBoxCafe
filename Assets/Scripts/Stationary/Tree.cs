using UnityEngine;
using Zenject;

public class Tree : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _crown;
    [SerializeField] private Color _deathColor;
    [Inject] DataManager _data;
    private void Start()
    {
        _data.onChangePollution += () =>
         {
             _crown.color = Color.Lerp(Color.white, _deathColor, Mathf.InverseLerp(0, 50, _data.Pollution));
             _crown.gameObject.SetActive(_data.Pollution < 50);
         };
    }

}
