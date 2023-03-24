using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemBox : MonoBehaviour
{
    [SerializeField] private List<GameObject> ItemList; //Input weapon prefabs that swap current weapons
    [SerializeField] private InputAction openBox;       //Input as so u can press E to open
    [SerializeField] private GameObject boxObject;      //The actual object. will spawn the funky item in front of it

    private bool opened = false, isPlayer = false;
    private void OnEnable()
    {
        openBox.Enable();

        openBox.performed += onBoxOpen;
    }
    private void OnDisable()
    {
        openBox.performed -= onBoxOpen;

        openBox.Disable();
    }
    
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            isPlayer = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            isPlayer = false;
        }
    }
    private void onBoxOpen(InputAction.CallbackContext context)
    {
        if(isPlayer && !opened)
        {
            opened = true;
            int itemNum = Random.Range(0,ItemList.Count);
            GameObject chosenItem = ItemList[itemNum];
            //Vector3 newPosition = new Vector3(boxObject.transform.position.x, )
            Instantiate(chosenItem, boxObject.transform.position, Quaternion.identity);
            //Random number from 0 to list size
            //Select object from list
            //Spawn object (weapon, item, etc) in front of box
        }
    }
}
