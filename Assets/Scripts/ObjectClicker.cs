using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ObjectClicker : MonoBehaviour {

    public Ray ray;
    public Canvas mainUI;
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + ray.direction * 1000.0f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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

            UIManager uiManager = mainUI.GetComponent<UIManager>();
            uiManager.manageUI(tile);
        }
    }


}
