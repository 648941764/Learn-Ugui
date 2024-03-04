using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualList : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private int _startIndex = 0;
    [SerializeField] protected int _endIndex = 0;
    public RectTransform content;
    public List<VirtualListItem> items = new List<VirtualListItem>();
    private VerticalLayoutGroup _verticalLayoutGroup;
    private List<string> strings = new List<string>();
    private Vector3 _lastPostion;
    private float _offset;

    private float _size;
    private float _initHeight;

    private void Awake()
    {
        _verticalLayoutGroup = transform.GetChild(0).GetComponent<VerticalLayoutGroup>();
        _size = items[0].GetComponent<RectTransform>().rect.size.y;//获取物体的高；
        _initHeight = items[0].transform.localPosition.y;
        for (int i = 0; i <= 10 ; i++)
        {
            strings.Add(i.ToString());
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].GetComponentInChildren<Text>().text = strings[i];
        }

        _startIndex = 0;
        _endIndex = items.Count - 2;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastPostion = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _offset = eventData.position.y - _lastPostion.y;
        _lastPostion = eventData.position;

        for (int i = -1; ++i < items.Count;)
        {
            items[i].transform.localPosition += Vector3.up * _offset;

        }

        if (_offset > 0)
        {

            if (items[0].transform.localPosition.y >= _initHeight + _size )
            {
                //把最后一个的item位置放倒数第二个item的后面
                items[items.Count - 1].transform.localPosition = items[items.Count - 2].transform.localPosition - Vector3.up * (_size + _verticalLayoutGroup.spacing);
                _endIndex++;

                if (_endIndex > strings.Count - 1)
                {
                    _endIndex = 0;
                }
                items[items.Count - 1].GetComponentInChildren<Text>().text = strings[_endIndex];
                VirtualListItem item = items[0];
                item.transform.localPosition = items[items.Count - 1].transform.localPosition - Vector3.up * (_size + _verticalLayoutGroup.spacing);
                for (int i = 0; ++i < items.Count;)
                {
                    items[i - 1] = items[i];
                }
                items[items.Count - 1] = item;
            }
        }

        else if(_offset < 0)
        {
            if (items[0].transform.localPosition.y <= _initHeight + _size)
            {
                items[items.Count - 1].transform.localPosition = items[0].transform.localPosition + Vector3.up * (_size + _verticalLayoutGroup.spacing);
                _startIndex--;
                if (_startIndex < 0)
                {
                    _startIndex = strings.Count - 1;
                }
                items[items.Count - 1].GetComponentInChildren<Text>().text = strings[_startIndex];
                VirtualListItem item = items[items.Count - 1];
                item.transform.localPosition = items[0].transform.localPosition + Vector3.up * (_size + _verticalLayoutGroup.spacing);
                for(int i = items.Count - 1; i >= 1; i--)
                {
                    items[i] = items[i - 1];
                }
                items[0] = item;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}
