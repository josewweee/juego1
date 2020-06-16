using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Txt_buffos : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //[SerializeField]
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    public float holdTime = 1f;
    
    //TEXTO UI
    private GameObject UI_texto;
    public UnityEvent onLongPress = new UnityEvent();

    void Start() {
        this.UI_texto = this.transform.GetChild(1).gameObject;
        this.UI_texto.SetActive(false);
    }
 
    public void OnPointerDown(PointerEventData eventData)
    {
        Invoke("OnLongPress", holdTime);
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
        this.UI_texto.SetActive(false);
    }
 
    void OnLongPress()
    {
        this.UI_texto.SetActive(true);
        onLongPress.Invoke();
    }
}
