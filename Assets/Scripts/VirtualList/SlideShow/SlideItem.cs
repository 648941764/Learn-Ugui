using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG;
using DG.Tweening;

public class SlideItem : MonoBehaviour
{
    [SerializeField] private Image _img;
    public void SetSprite(Sprite sprite)
    {
        _img.sprite = sprite;
    }

    public void SetPos(Vector2 pos)
    {
        DOTween.To(
            () => (Vector2)transform.localPosition,
             _ => transform.localPosition = _,
            pos, 0.2f
            );
    }
}
