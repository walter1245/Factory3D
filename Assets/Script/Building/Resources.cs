using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public bool hasExtractor=false;  // Controlla se c'è già un estrattore
    private GameObject player;   // Riferimento al giocatore
     
     private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Player")
        {
            player=other.gameObject;
              Debug.Log("Giocatore vicino alla risorsa!");
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag=="Player")
        {
            player=null;
            Debug.Log("Giocatore lontano dalla risorsa!");
        }
    }

    public bool CanInteract() {
       return player != null&& !hasExtractor;
    }
}
