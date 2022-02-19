using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Image foreground;
    public Image background;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 direction = (target.position - Camera.main.transform.position).normalized;
        bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;
        foreground.enabled = !isBehind;
        background.enabled = !isBehind;
        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }

    public void setHealthBarPercentage(float percentage)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        foreground.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
  
}
