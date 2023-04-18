using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayReveal : MonoBehaviour
{
    public GameObject revealArea;

    public TileData tileData;

    //static Grid grid;

    public void RevealHallway()
    {
        revealArea.SetActive(true);

        LayerMask mask = LayerMask.GetMask("Hallway");

        foreach (Connection con in tileData.connections)
        {   
            Collider[] col = Physics.OverlapSphere(new Vector3(con.alignPt.position.x, 1.5f, con.alignPt.position.z), 0.5f, mask, QueryTriggerInteraction.Collide);
            foreach(Collider c in col)
            {
                if(c.GetComponent<HallwayReveal>().revealArea.activeInHierarchy == false)
                {
                    c.GetComponent<HallwayReveal>().RevealHallway();
                }
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            RevealHallway();
        }
        
    }
    
}
