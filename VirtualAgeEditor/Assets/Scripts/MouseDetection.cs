using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MouseDetection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public bool isOver = false;
    public GameObject numbers;
    public GameObject video;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse enter");
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit");
        isOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOver)
        {
      
            video.GetComponent<VideoEditor>().ScrollZoom();
        }

    }
}
