using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
    
    //eerst neem je in de animator de deur animatie op, deze code zorgt voor het uitvoeren ervan. 

    Animator anim;

    void Start() {

        anim = GetComponent<Animator>();
    }


    void OnTriggerEnter(Collider other)
    {
       //als je in de trigger komt, opent de deur.
        anim.SetTrigger("OpenDoor");
    }

    void OnTriggerExit(Collider other)
    {
        //als je uit de trigger komt, gaat de deur dicht.
        anim.enabled = true;
    }

    void pauseAnimationEvent()
    {
        //de deur stopt met bewegen als je in de trigger bent, anders zou hij steeds open en dicht blijven gaan.
        anim.enabled = false ;
    }

}
