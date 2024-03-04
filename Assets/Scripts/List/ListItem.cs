using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ListItem : MonoBehaviour
{
    private RectTransform _rectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null) { _rectTransform = transform as RectTransform; }
            return _rectTransform;
        }
    }

    public int recordIndex;

    public bool IsOpen;
}
