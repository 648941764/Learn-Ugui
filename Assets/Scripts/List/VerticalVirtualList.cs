using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ListRenderer(int index, ListItem listItem);

[RequireComponent(typeof(RectTransform), typeof(VerticalLayoutGroup))]
public sealed class VerticalVirtualList : MonoBehaviour
{
    private ListItem[] _items;
    private int[] _itemIndcies;
    private int _itemCount;
    private float _itemDistance;
    private VerticalLayoutGroup _verticalLayoutGroup;
    private ScrollRect _scrollRect;
    private RectTransform _viewport;
    private RectTransform _rectTransform;

    public event ListRenderer listRenderer;

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
            _verticalLayoutGroup.enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
            _verticalLayoutGroup.enabled = false;
            RecalculateContentHeight();
            ResetItemIndices();
            RenderAllListItem();
            _scrollRectNormalizedPosLast = 1.0f;
            _scrollRect.verticalNormalizedPosition = 1.0f;
        }
    }

    private void Awake()
    {
        _rectTransform = transform as RectTransform;
        _items = GetComponentsInChildren<ListItem>();
        _itemIndcies = new int[_items.Length];
        _itemCount = _items.Length;
        _itemDistance = _items[0].transform.position.y - _items[1].transform.position.y;
        _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        _scrollRect = transform.parent.GetComponentInParent<ScrollRect>();
        _viewport = transform.parent as RectTransform;
        _scrollRect.onValueChanged.AddListener(OnScrollRectValueChange);
    }

    private void OnScrollRectValueChange(Vector2 value)
    {
        if (_scrollRectNormalizedPosLast == value.y)
        {
            return;
        }

        _viewport.GetWorldCorners(viewportCorners);

        if (_scrollRectNormalizedPosLast > value.y)
        {
            // 上滑检测超出顶部
            for (int i = -1; ++i < _itemCount;)
            {
                _items[i].rectTransform.GetWorldCorners(listItemCorners);
                if (listItemCorners[0].y > viewportCorners[1].y)
                {
                    _itemIndcies[i] += _itemCount;
                    Vector2 currentPos = _items[i].rectTransform.anchoredPosition;
                    currentPos.y -= (_itemDistance * _itemCount);
                    _items[i].rectTransform.anchoredPosition = currentPos;
                    RenderListItem(i);
                }
            }
        }
        else if (_scrollRectNormalizedPosLast < value.y)
        {
            // 下滑检测超出顶部 
            for (int i = -1; ++i < _itemCount;)
            {
                _items[i].rectTransform.GetWorldCorners(listItemCorners);
                if (listItemCorners[1].y < viewportCorners[0].y)
                {
                    _itemIndcies[i] -= _itemCount;
                    Vector2 currentPos = _items[i].rectTransform.anchoredPosition;
                    currentPos.y += (_itemDistance * _itemCount);
                    _items[i].rectTransform.anchoredPosition = currentPos;
                }
            }
        }

        _scrollRectNormalizedPosLast = _scrollRect.verticalNormalizedPosition;
    }

    private void ResetItemIndices()
    {
        for (int i = -1; ++i < _itemCount;)
        {
            _itemIndcies[i] = i;
        }
    }

    private void RenderAllListItem()
    {
        for (int i = -1; ++i < _itemCount;)
        {
            RenderListItem(i);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"> ListItem的索引 </param>
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
            listRenderer(_itemIndcies[index], _items[index]);            
        }
    }

    private void RecalculateContentHeight()
    {
        float height = (_items[1].rectTransform.sizeDelta.y + _verticalLayoutGroup.spacing) * _itemDataNum
            - _verticalLayoutGroup.spacing
            + _verticalLayoutGroup.padding.bottom
            + _verticalLayoutGroup.padding.top; 
        Vector2 size = _rectTransform.sizeDelta;
        size.y = height;
        _rectTransform.sizeDelta = size;
    }
}
