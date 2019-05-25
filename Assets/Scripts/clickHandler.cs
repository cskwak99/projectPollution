using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class clickHandler : MonoBehaviour {

    public Ray ray;
    UIManager UIM;
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + ray.direction * 1000.0f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UIM.isMouseOnUI)
        {
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000.0f);
            TileClass tile;
            if (hit.transform == null)
                tile = null;
            else
            {
                string tile_type = hit.transform.gameObject.name.Substring(4);
                print(tile_type);
                tile = (TileClass) hit.transform.GetComponent(tile_type);
            }

            UIManager uiManager = this.GetComponent<UIManager>();
            uiManager.manageUI(tile);
        }

        //ON CLICK MOUSE2 (MIDDLEMOUSE) CAMERA CAN MOVE (VIEW SCRIPT IN THE CAMERA)
    }

    private void Start()
    {
        this.UIM = this.GetComponent<UIManager>();
    }

}
