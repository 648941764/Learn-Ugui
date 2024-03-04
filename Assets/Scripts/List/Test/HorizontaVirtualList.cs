using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ListRefresh(int index, ListItem listItem);

[RequireComponent(typeof(RectTransform), typeof(VerticalLayoutGroup))]

public class HorizontaVirtualList : MonoBehaviour
{
    private ListItem[] _items;
    private int[] _itemIndcies;
    private int _itemCount;
    private float _itemDistance;
    private HorizontalLayoutGroup _horizontalLayoutGroup;
    private ScrollRect _scrollRect;
    private RectTransform _viewport;
    private RectTransform _rectTransform;

    public ListRefresh listRefresh;

    public ListItem[] Items => _items;

    private int _itemDataNum;
    private float _scrollRectNormalizedPosLast;

    private Vector3[] viewportCorners = new Vector3[4];
    private Vector3[] listItemCorners = new Vector3[4];

    public int ItemDataNum
    {
        get => _itemDataNum;
        set
        {
            _itemDataNum = value;
            ResetContentInfo();
            RenderAllListItem();
            _scrollRectNormalizedPosLast = 0.0f;
            _scrollRect.verticalNormalizedPosition = 1.0f;
        }
    }

    private void Awake()
    {
        _rectTransform = transform as RectTransform;
        _items = GetComponentsInChildren<ListItem>();
        _itemIndcies = new int[_items.Length];
        _itemCount = _items.Length;
        _horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        _horizontalLayoutGroup.enabled = false;

        //设置锚点的位置和让物体中心对其
        Vector2 anchoredPos = _items[0].rectTransform.anchoredPosition;
        anchoredPos.x = _items[0].rectTransform.sizeDelta.x * 0.5f + _horizontalLayoutGroup.padding.top - _horizontalLayoutGroup.padding.bottom;
        for (int i = -1; ++i < _itemCount;)
        {
            _items[i].rectTransform.anchorMin = _items[i].rectTransform.anchorMax = new Vector2(0.0f, 1.0f);
            _items[0].rectTransform.anchoredPosition = anchoredPos;
        }

        _scrollRect = transform.parent.GetComponentInParent<ScrollRect>();
        _viewport = transform.parent as RectTransform;
        _scrollRect.onValueChanged.AddListener(OnScrollRectValueChange);
        _itemDistance = _items[0].rectTransform.sizeDelta.x + _horizontalLayoutGroup.spacing;
    }

    private void OnScrollRectValueChange(Vector2 value)
    {
        if (_scrollRectNormalizedPosLast == value.x)
        {
            return;
        }

        _viewport.GetWorldCorners(viewportCorners);

        if (_scrollRectNormalizedPosLast < value.x)
        {
            // 左滑检测超出最左边
            for (int i = -1; ++i < _itemCount;)
            {
                _items[i].rectTransform.GetWorldCorners(listItemCorners);
                if (listItemCorners[2].x < viewportCorners[1].x)
                {
                    _itemIndcies[i] -= _itemCount;
                    Vector2 currentPos = _items[i].rectTransform.anchoredPosition;
                    currentPos.x += (_itemDistance * _itemCount);//TODO
                    _items[i].rectTransform.anchoredPosition = currentPos;
                    RenderListItem(i);
                }
            }
        }
        else if (_scrollRectNormalizedPosLast > value.x)
        {
            // 右边滑检测超出最右边 
            for (int i = -1; ++i < _itemCount;)
            {
                _items[i].rectTransform.GetWorldCorners(listItemCorners);
                if (listItemCorners[1].x > viewportCorners[2].x)
                {
                    _itemIndcies[i] += _itemCount;
                    Vector2 currentPos = _items[i].rectTransform.anchoredPosition;
                    currentPos.x -= (_itemDistance * _itemCount);//TODO
                    _items[i].rectTransform.anchoredPosition = currentPos;
                    RenderListItem(i);
                }
            }
        }

        _scrollRectNormalizedPosLast = _scrollRect.horizontalNormalizedPosition;
    }

    private void RenderAllListItem()
    {
        for (int i = -1; ++i < _itemCount;)
        {
            RenderListItem(i);
        }
    }

    private void RenderListItem(int index)
    {
        int dataIndex = _itemIndcies[index];
        if ((uint)dataIndex >= (uint)_itemDataNum)
        {
            _items[index].gameObject.SetActive(false);
        }
        else
        {
            _items[index].gameObject.SetActive(true);
            listRefresh(_itemIndcies[index], _items[index]);
        }
    }

    private void ResetContentInfo()
    {
        for (int i = -1; ++i < _itemCount;)
        {
            _itemIndcies[i] = i;
        }

        Vector2 size = _rectTransform.sizeDelta;
        size.x =
            (_items[0].rectTransform.sizeDelta.y + _horizontalLayoutGroup.spacing) * _itemDataNum
            - _horizontalLayoutGroup.spacing
            + _horizontalLayoutGroup.padding.right
            + _horizontalLayoutGroup.padding.left;
        _rectTransform.sizeDelta = size;

        // 排列位置
        Vector3 localPos = _items[0].rectTransform.anchoredPosition;
        localPos.x = (_horizontalLayoutGroup.padding.left + _items[0].rectTransform.sizeDelta.x * 0.5f);//TODO
        _items[0].rectTransform.anchoredPosition = localPos;
        for (int i = 0; ++i < _itemCount;)
        {
            localPos.x += _itemDistance;//TODO
            Debug.Log(localPos);
            _items[i].rectTransform.anchoredPosition = localPos;
        }
    }
}
