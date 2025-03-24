using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public bool hasExtractor=false;  // Controlla se c'è già un estrattore
    public static Resources currentResources; // Riferimento alla risorsa corrente
   
     
     private void OnTriggerEnter(Collider other) {
       
        if(other.gameObject.tag=="Player")
        {

            currentResources=this;
            Debug.Log(currentResources);
              Debug.Log("Giocatore vicino alla risorsa!1111");
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag=="Player")
        {
            if(currentResources==this)
            {
             currentResources=null;
             Debug.Log("Giocatore lontano dalla risorsa!");
            }
           
        }
    }

    public bool CanInteract() {
       return !hasExtractor;
    }
}
