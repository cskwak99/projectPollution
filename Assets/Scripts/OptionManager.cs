using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class OptionManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject createOptionPanel(string name, GameObject parent, string[] optionList, Vector3 iniPosition)
    {
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        panel.name = name;
        int n = 0;
        foreach (string option in optionList)
        {
            GameObject opt = Instantiate(buttonPrefab);
            opt.name = option;
            this.GetComponent<UIManager>().addMouseHoverListener(opt);
            opt.GetComponentInChildren<Text>().text = option;
            opt.transform.SetParent(panel.transform);
            float height = opt.GetComponent<RectTransform>().rect.height;
            opt.GetComponent<RectTransform>().anchoredPosition -= new Vector2(-20, n * height);
            n++;
        }
        panel.transform.SetParent(parent.transform, false);
        panel.transform.position = iniPosition + new Vector3(-25, 0, 0);
        return panel;
    } 
   
}
