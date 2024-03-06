using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


/// <summary>
/// 实现思路，通过改变item的位置关系来实现轮番图效果，每一个Item都放在——itemList里面，只需要遍历列表，改变每个Item的
/// </summary>
public class carousel : MonoBehaviour
{
    [SerializeField] private SlideItem _itemPrefab;
    [SerializeField] private Sprite[] _sprites;
    private List<SlideItem> _itemList;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Vector2 _size;

    private void Awake()
    {
        _itemList = new List<SlideItem>();
    }

    private void Start()
    {
        ShowSprites();
        FreshPos();
    }

    public void ShowSprites()
    {
        Vector2 localPos = Vector2.zero;
        Vector2 startPos = -(_size + _offset);
        for (int i = -1; ++i < _sprites.Length;)
        {
            SlideItem item = Instantiate(_itemPrefab, transform);
            item.SetSprite(_sprites[i]);
            localPos = startPos + (_offset + _size) * i;
            localPos.y = 0;
            item.transform.localPosition = localPos;
            item.gameObject.name = i.ToString();
            _itemList.Add(item);
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeImage(-1);
            FreshPos();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeImage(1);
            FreshPos();
        }
    }

    private void FreshPos()
    {
        Vector2 localPos = Vector2.zero;
        Vector2 startPos = -(_size + _offset);

        for (int i = -1; ++i < _itemList.Count;)
        {
            localPos = startPos + (_offset + _size) * i;
            localPos.y = 0;
            _itemList[i].SetPos(localPos);

            if (i > _itemList.Count / 2)
            {
                _itemList[i].transform.SetAsFirstSibling();
            }

            if (i == _itemList.Count / 2)
            {
                _itemList[i].transform.localScale = Vector3.one * 1.5f;
            }
            else
            {
                _itemList[i].transform.localScale = Vector3.one * 1f;
            }
        }
        _itemList[_itemList.Count / 2].transform.SetAsLastSibling();
    }

    private void ChangeImage(int dir)
    {
        List<SlideItem> newList = new List<SlideItem>();
        if (dir == 1)
        {
            newList.Add(_itemList[_itemList.Count - 1]);
            for (int i = -1; ++i < _itemList.Count - 1;)
            {
                newList.Add(_itemList[i]);
            }
        }
        else if (dir == -1)
        {
            for (int i = 0; ++i < _itemList.Count;)
            {
                newList.Add(_itemList[i]);
            }
            newList.Add(_itemList[0]);
        }
        _itemList = newList;
    }
}
