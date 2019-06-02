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

    public IEnumerator getDestTile(System.Action<TileClass> passedTile)
    {
        this.UIM.isOnDestTileSelection = true;
        UIM.showPopup("Select a destination tile");
        while (true)
        {
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000.0f);
            TileClass tile = null;
            string tile_type = "";
            if (hit.transform == null)
            {
                tile = null;
                tile_type = "None";
            }
            else
            {
                tile_type = hit.transform.gameObject.name.Substring(4);    
                tile = (TileClass)hit.transform.GetComponent(tile_type);
            }
            UIM.hoverTile(tile);
            if (Input.GetMouseButtonDown(0) && !UIM.isMouseOnUI)
            {
                print("SELECTED " + tile_type);
                passedTile(tile);
                this.UIM.isOnDestTileSelection = false;
                yield break;        
            }
            if(Input.GetKey(KeyCode.Escape))
            {
                passedTile(null);
                this.UIM.isOnDestTileSelection = false;
                yield break;
            }
            yield return null;
        }
    }

    private void Update()
    {
        UIManager uiManager = this.GetComponent<UIManager>();
        TileClass tile = null;
        if (!UIM.isMouseOnUI)
        {
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000.0f);  
            if (hit.transform == null)
                tile = null;
            else
            {
                string tile_type = hit.transform.gameObject.name.Substring(4);
                //print(tile_type);
                tile = (TileClass) hit.transform.GetComponent(tile_type);
            }
            if(!UIM.isOnTileSelected)
                uiManager.hoverTile(tile);
        }
        if (Input.GetMouseButton(0) && !UIM.isMouseOnUI && !UIM.isOnDestTileSelection)
        {
            if (!UIM.isOnTileSelected)
            {
                uiManager.manageUI(tile);
                if (tile != null)
                    UIM.isOnTileSelected = true;
            }
            else
            {
                UIM.isOnTileSelected = false;
            }
        }
        //ON CLICK MOUSE2 (MIDDLEMOUSE) CAMERA CAN MOVE (VIEW SCRIPT IN THE CAMERA)
    }

    private void Start()
    {
        this.UIM = this.GetComponent<UIManager>();
    }

}
