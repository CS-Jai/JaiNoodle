using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class raycasting : MonoBehaviour
{
    [Header("DEBUG")]
    public GameObject curSelected;

    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }

        highLightStuff();
        if(selection != null)
        {
            curSelected = selection.gameObject;

            Vector2 mousePos = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag("Space"))
                {
                    curSelected.transform.rotation = Quaternion.identity;
                    curSelected.transform.position = Vector3.zero;

                    Debug.Log(hit.collider.gameObject.name);

                    float x = hit.transform.position.x;
                    float y = hit.transform.position.y;
                    float z = hit.transform.position.z;

                    curSelected.transform.position = new Vector3(x, y, z + 10);


                    //Makes piece flat
                    
                }
            }
        }
        
    }

    void highLightStuff()
    {
        // Highlight
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight)
            {
                if (selection != null)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }
                selection = raycastHit.transform;
                selection.gameObject.GetComponent<Outline>().enabled = true;
                highlight = null;
            }
            else
            {
                if (selection)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;
                }
            }
        }
    }
}
