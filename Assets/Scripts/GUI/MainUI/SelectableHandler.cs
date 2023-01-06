using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectableHandler : MonoBehaviour, IPointerEnterHandler, IDeselectHandler, IPointerDownHandler
{
    private Selectable selectable;

    // Start is called before the first frame update
    void Start()
    {
        selectable = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectable.Select();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectable.OnPointerExit(null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.selectedObject.GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
}
