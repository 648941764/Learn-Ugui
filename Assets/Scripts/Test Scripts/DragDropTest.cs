using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropTest : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Text _text;
    private float _topPadding;
    private float _spacing;
    private float _height;
    private int  _sibling;
    private GameObject _tempGameObject;

    private void OnEnable()
    {
        _topPadding = transform.parent.GetComponent<VerticalLayoutGroup>().padding.top;
        _spacing = transform.parent.GetComponent<VerticalLayoutGroup>().spacing;
        _height = GetComponent<RectTransform>().sizeDelta.y;
    } 

    private void Start()
    {
        _text = transform.GetChild(0).GetComponent<Text>();
        _text.text = transform.name;
        transform.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
     
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetSiblingIndex(transform.parent.GetSiblingIndex() - 1);
        _tempGameObject = GameObject.Find("EmptyBtn");
        transform.GetComponent<LayoutElement>().ignoreLayout = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector2(transform.position.x, eventData.position.y);
        _sibling = -(int)((transform.localPosition.y - _topPadding) / (_height + _spacing));
        _tempGameObject.transform.SetSiblingIndex(_sibling);
        Debug.Log(_sibling);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.GetComponent<LayoutElement>().ignoreLayout = false;
        transform.SetSiblingIndex(_sibling);
        _tempGameObject.transform.SetAsLastSibling();
    }
}
