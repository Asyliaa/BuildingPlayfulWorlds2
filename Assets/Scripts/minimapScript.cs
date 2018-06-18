using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapScript : MonoBehaviour {

    public Transform player;

    private void LateUpdate()
    {
        //hier zoek je de positie van de player, zodat de minimap die informatie heeft en de player steeds in het midden kan laten zien.
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
