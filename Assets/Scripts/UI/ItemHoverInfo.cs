using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private GameObject _infoCanvasPrefab;
    private ItemInfoCanvas _infoCanvas;
    private Item _item;
    
    
    public void Init(Item item)
    {
        if (_infoCanvas)
        {
            Destroy(_infoCanvas.gameObject);
        }
        _item = item;
        var go  = Instantiate(_infoCanvasPrefab);
        _infoCanvas = go.GetComponent<ItemInfoCanvas>();
        _infoCanvas.Init(item, this);
        _infoCanvas.gameObject.SetActive(false);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_infoCanvas)
        _infoCanvas.gameObject.SetActive(true);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        if(_infoCanvas)
            _infoCanvas.gameObject.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if(_infoCanvas)
            _infoCanvas.UpdatePanelPosition(eventData.position);
    }

    public void Destroy()
    {
        Destroy(_infoCanvas.gameObject);
        _infoCanvas = null;
    }
}
