using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStaff : MonoBehaviour {


    public Transform gunEnd;
    public GameObject bullet;

    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.tag == "Player")
        {
            StartCoroutine("Shooting");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopCoroutine("Shooting");
        }
    }

    IEnumerator Shooting()
    {
        while (true)
        {
            Instantiate(bullet, gunEnd.position, gunEnd.rotation);
            yield return new WaitForSeconds(1);
        }
    }
}
