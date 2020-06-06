using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Txt_poderes : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //[SerializeField]
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    public float holdTime = 1f;
    
    //TEXTO UI
    private GameObject UI_texto;
 
    private bool held = false;
    public UnityEvent onClick = new UnityEvent();
 
    public UnityEvent onLongPress = new UnityEvent();

    void Start() {
        this.UI_texto = this.transform.GetChild(1).gameObject;
        this.UI_texto.SetActive(false);
    }
 
    public void OnPointerDown(PointerEventData eventData)
    {
        held = false;
        Invoke("OnLongPress", holdTime);
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(held);
        CancelInvoke("OnLongPress");
        this.UI_texto.SetActive(false);
        if (!held) onClick.Invoke();
    }
 
    void OnLongPress()
    {
        held = true;
        this.UI_texto.SetActive(true);
        onLongPress.Invoke();
    }
}