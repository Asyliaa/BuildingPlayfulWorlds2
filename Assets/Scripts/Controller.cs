using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//de onderdelen over de first person controller komen van het script uit de les. Ik heb in dit script zelf er nog veel bijgeschreven,
//aan de hand van tutorials, maar ook mijn eigen code geschreven.

public class Controller : MonoBehaviour
{
    //movement and such
    public GameObject cam;
    public float moveSpeed = 3f;
    public float mouseSensitivityX, mouseSensitivityY;
    private float verticalAxis;
    private float horizontalAxis;
    private float angleX, angleY;
    private float mouseX, mouseY;
    private Rigidbody rigidBody;

    //jumping 
    private bool jump;
    public float jumpForce = 200;
    public float rayCastRange = 0.25f;

    //texts 
    public Text countText;
    public int count;
    public int objectCount;
    public int schepCount;
    public int triggerCount;
    public Text winText;
    public Text infoText;
    public Text controlText;
    public Text scoreText;
    public Text gieterText;
    public Text collectedText;
    public Text neededText;
    public Text tempText;
    public Text enemyText;

    public bool attackBool; 
    public GameObject enemy;
    private int damage = 10; 
    public Text healthText;


    //andere script van de timer importeren zodat ik die kan gebruiken om de eindscore weer te geven
    public TimerScript timerScript;

    public int playerHealth = 0;
    

    void Start()
    {
        //texten leeg zetten
        rigidBody = GetComponent<Rigidbody>();
        count = 0;
        objectCount = 0;
        schepCount = 0;
        triggerCount = 0;
        countText.text = "Count: " + count.ToString() + " / 10";
        winText.text = "";
        infoText.text = "";
        controlText.text = ""; 
        scoreText.text = "";
        gieterText.text = "";
        collectedText.text = "Collected:";
        neededText.text = "Needed: 1 seed, 1 flower";
        healthText.text = "100";
        tempText.text = "";
        playerHealth = 100;
        enemyText.text = "test";


    }

    // Update is called once per frame
    void Update()
    {
           if (attackBool == true)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    enemy.GetComponent<EnemyScript>().enemyHealth -= damage;
                    enemyText.text = "Enemy took damage";
                    print("It workds");
                }

            }
            

        

        //Get input from WASD
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");

        //Get Input from the mouse movement
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        //Input for Jump
        jump = Input.GetKeyDown(KeyCode.Space);
        Debug.DrawLine(transform.position, transform.position + -transform.up * rayCastRange);
        if (jump)
        {
            DoJump();
            
        }


        //Look Up and Down
        angleY += mouseY * mouseSensitivityY;
        angleY = Mathf.Clamp(angleY, -89f, 89f);
        cam.transform.localRotation = Quaternion.Euler(-angleY, 0, 0);

        //Look Right and Left
        angleX += mouseX * mouseSensitivityX;
        transform.rotation = Quaternion.Euler(0, angleX, 0);

      
    }

    private void FixedUpdate()
    {
        //Move the Player with Physics
        Vector3 forwardMovement = transform.forward * verticalAxis;
        Vector3 sideMovement = transform.right * horizontalAxis;
        rigidBody.MovePosition(rigidBody.position + (forwardMovement + sideMovement).normalized * moveSpeed * Time.deltaTime);
    }

    //jumping 
    private void DoJump()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.1f, -transform.up, out hit, rayCastRange))
        {
            rigidBody.AddForce(transform.up * jumpForce);
            //Debug omdat het jumpen niet wilde werken.
            Debug.Log("HALLO");
        }
    }

    //trigger voor pick ups en voor de informatie. 
    void OnTriggerEnter(Collider other)
    {

        //Boolean voor enemy zodat je in Update damage kan doen. Hier zorg ik ervoor dat hij true wordt. 
        if (other.gameObject.CompareTag("Enemy"))
        {
            attackBool = true;
            enemy = other.gameObject;
        }

        //Zorgt ervoor dat de aanvallen van de enemy damage doen en zorgt voor een message als de player dood gaat. 
        if (other.gameObject.CompareTag("MagicBall"))
        {

            playerHealth = playerHealth - 10;
            healthText.text = "90";

            if (playerHealth <= 0)
            {
                gieterText.text = "YOU DIED";
            }
        }
        //als je een pick up object oppakt dan gaat het object weg en telt je score op.
        //Room 1 
        if (other.gameObject.CompareTag("Pick Up Gieter"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            objectCount = objectCount + 1;
            gieterText.text = "Obtained: Gieter!";

            //object count is 1 
        }

        if (other.gameObject.CompareTag("Plant"))
        {
            if (objectCount >= 1)
            {


                collectedText.text = "Collected: 2 Seeds";
                neededText.text = "Needed: 1 flower";
            }

        }


        if (other.gameObject.CompareTag("Schep"))
        {
            if (objectCount >= 1)
            {
                other.gameObject.SetActive(false);
                objectCount = objectCount + 1;
                gieterText.text = "Obtained: Schep!";

                //object count is 2
            }

        }

        if (other.gameObject.CompareTag("Grond"))
        {
            if (objectCount >= 2)
            {
                gieterText.text = "Planted: 1 Seed!";
                collectedText.text = "Collected: 1 Seed";
            }

            if (triggerCount >= 1)
            {
                collectedText.text = "Collected: 1 Seed, 1 Flower";
                neededText.text = "Needed:";
                gieterText.text = "";
            }
        }



        //Room 2

        if (other.gameObject.CompareTag("Feed"))
        {
            tempText.text = "I'm hungry. In return for green food I will give you this crystal I found.";
            if (objectCount >= 3)
            {
                tempText.text = "Thank you! Have my crystal";
                collectedText.text = "Collected: 1 Seed, 1 Flower, 1 Crystal.";

            }
        }

        if (other.gameObject.CompareTag("Green Food"))
        {
            other.gameObject.SetActive(false);

            gieterText.text = "Obtained: Green Food";
            objectCount = objectCount + 1;

            //object count is 3
        }

        //Room 3

        if (other.gameObject.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
            objectCount = objectCount + 1;
            gieterText.text = "Obtained: Key";

            //op het eind timer laten zien

            scoreText.text = "Your score is: " + timerScript.counterText.text;
            // object count is 4
        }

        if (other.gameObject.CompareTag("Door"))
        {
            if (objectCount >= 4)
            {
                other.gameObject.SetActive(false);
            }
        }

        //als je in de trigger voor informatie loopt, roept hij die functie aan en krijg je informatie te zien. 
        if (other.gameObject.CompareTag("Info"))
        {
            SetInfoText();
        }

        if (other.gameObject.CompareTag("Grown"))
        {
            gieterText.text = "Your plant has fully grown!";
            triggerCount = triggerCount + 1;
            other.gameObject.SetActive(false);
        }


    }
 



    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            attackBool = false;
        }
        //als je uit de info trigger loopt, gaat de text met informatie weer uit je scherm. 
        if (other.gameObject.CompareTag("Info"))
        {
            SetInfoDelText();
        }

        if (other.gameObject.CompareTag("Feed"))
        {
            tempText.text = "";
        }
    }


    void SetInfoText()
    {
        //de informatie text die je krijgt als je de info trigger inloopt.
        infoText.text = "Hi there, you are a witch but you're losing you're not feeling well. Make a potion to feel better! Collect the following things: ";
        controlText.text = "Use wasd to move and your mouse to look around. You can jump by pressing the spacebar and double jump by pressing it twice. ";
    }

    void SetInfoDelText()
    {
        //deze functie zet de informatie text weer op leeg, zodat hij niet meer in je beeld staat als je uit de info trigger loopt.
        infoText.text = "";
        controlText.text = "";
    }


    


}