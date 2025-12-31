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
        // Responsible for keeping stuff highlighted visually
        highLightStuff();

        // When a piece is selected
        if(selection != null)
        {
            // curSelected is the gameobject that is currently selected
            curSelected = selection.gameObject;

            // Shoots ray and gets all objects it hits
            Vector2 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            
            // Iterates through each hit
            foreach (RaycastHit hit in hits)
            {
                // if the hit has a tag space (space is the colliders in the box where the noodles go)
                if (hit.transform.CompareTag("Space"))
                {   
                    // If you click on space
                    if (Input.GetMouseButtonDown(0))
                    {
                        // Turns off gravity and hitbox for object selected
                        curSelected.GetComponent<Rigidbody>().isKinematic = true;
                        curSelected.GetComponent<Rigidbody>().useGravity = false;

                        // Undoes highlight efftect
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
                    else // Until clicked the piece selected will snap to the center of the space youre mousing over
                    {
                         SnapToSpace(hit);
                    }
                }
            }
        }
        else
        {
            // If no piece is selected and button clicked. Selects piece
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

    void SnapToSpace(RaycastHit hit)
    {
        // center of the space collider in world space
        Vector3 target = hit.collider.bounds.center;

        Transform anchor = curSelected.transform.Find("Anchor");

        // move so anchor lands on target
        Vector3 offset = anchor.position - curSelected.transform.position;
        curSelected.transform.position = target - offset;

        // optional: keep piece flat
        curSelected.transform.rotation = Quaternion.identity;
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
    }
}
