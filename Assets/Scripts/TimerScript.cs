using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    public Text counterText;
    public float seconds, minutes;
    

	void Start () {
        //hier kan je het textveld in droppen om het in beeld te laten zien.
        counterText = GetComponent<Text>() as Text;
        counterText.text = "00" + ":" + "00"; 
	}

    void Update()
    {
        //hiermee wordt de tijd bijgehouden vanaf het moment dat je de game opstart en wordt het op je scherm weergegeven.
        minutes = (int)(Time.time / 60f);
        seconds = (int)(Time.time % 60f);
        counterText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    //hier probeerde ik de timer te laten beginnen als je in de trigger liep maar dat wil niet echt werken helaas. 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("startTimer"))
        {
            Start();

        }
    }



}
