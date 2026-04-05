using UnityEngine;

public class Food : Trash
{
    [SerializeField] SpriteRenderer _sRenderer;
    [SerializeField] Sprite _trashSprite;
    public override void Drop()
    {
        _sRenderer.sprite = _trashSprite;
        base.Drop();
    }
}
