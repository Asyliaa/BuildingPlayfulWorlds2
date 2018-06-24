using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    public bool plantBool;
    public bool groundBool;
    public bool caudronBool;
    public bool lastSceneBool;
    public GameObject enemy;
    private int damage = 10; 
    public Text healthText;
    public AudioClip hurtClip;
    public AudioClip collectClip;
    public AudioClip attackClip;


    //andere script van de timer importeren zodat ik die kan gebruiken om de eindscore weer te geven
    public TimerScript timerScript;

    private int playerHealth;
    

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
        neededText.text = "Needed: 1 seed, 1 flower, 1 crystal";
        healthText.text = "Health = 100";
        tempText.text = "";
        playerHealth = 100;
        enemyText.text = "Enemy Health";


    }

    // Update is called once per frame
    void Update()
    {
           if (attackBool == true)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                AudioSource.PlayClipAtPoint(attackClip, transform.position);
                enemy.GetComponent<EnemyScript>().enemyHealth -= damage;
                    enemyText.text = "Enemy health =" + enemy.GetComponent<EnemyScript>().enemyHealth.ToString();
                gieterText.text = "Enemy took 10 damage!";
                    print("It workds");
                }

            }
            

        if (plantBool == true)
        {

            if (Input.GetKeyDown(KeyCode.F))
            {
                AudioSource.PlayClipAtPoint(collectClip, transform.position);
                collectedText.text = "Collected: 2 Seeds";
                neededText.text = "Needed: 1 flower, 1 crystal";
                gieterText.text = "Obtained 2 seeds";
                print("it works!");
                objectCount = objectCount + 1; 
                
            }

        }

        if (groundBool == true)
        {

            if (Input.GetKeyDown(KeyCode.F))
            {
                gieterText.text = "Planted 1 seed";
                collectedText.text = "Collected: 1 Seed";
                neededText.text = "Needed: 1 flower, 1 crystal";
                print("it wooorks!");
                objectCount = objectCount + 1;
            }
        }

        if (caudronBool == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                winText.text = "YOU DID IT!";
                gieterText.text = "";
                collectedText.text = "";
                neededText.text = "";
                //op het eind timer laten zien

                scoreText.text = "Your score is: " + timerScript.counterText.text;
            }
        }

        if (lastSceneBool == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

   
        //als je een pick up object oppakt dan gaat het object weg en telt je score op.
        //Room 1 
        if (other.gameObject.CompareTag("Pick Up Gieter"))
        {
            AudioSource.PlayClipAtPoint(collectClip, transform.position);
            other.gameObject.SetActive(false);
            count = count + 1;
            objectCount = objectCount + 1;
            gieterText.text = "Obtained watering can";

            //object count is 1 
        }
        
        if (other.gameObject.CompareTag("Plant"))
        {         
            if (objectCount >= 1)
            {
                tempText.text = "Water me and I'll give you two seeds! (Press F to water)";
                plantBool = true;
                }

            if (objectCount >= 2)
            {
                plantBool = false;
                tempText.text = "";
            }

            if (objectCount < 1)
            {
                tempText.text = "Water me and I'll give you two seeds! (You need a watering can.)";
            }
               

        }


        if (other.gameObject.CompareTag("Schep"))
        {
            AudioSource.PlayClipAtPoint(collectClip, transform.position);
            other.gameObject.SetActive(false);
                objectCount = objectCount + 1;
                gieterText.text = "Obtained shovel";

                //object count is 3
                    }

        if (other.gameObject.CompareTag("Grond"))
        {
            if (objectCount >= 3)
            {
                groundBool = true;
                tempText.text = "Press F to plant seed";
               
            }

            if (objectCount >= 4)
            {
                groundBool = false; 
            }
            if (objectCount < 3)
            {
                tempText.text = "Needed to plant a seed: seed, watering can, shovel.";
            }

            if (triggerCount >= 1)
            {
                AudioSource.PlayClipAtPoint(collectClip, transform.position);
                tempText.text = "";
                collectedText.text = "Collected: 1 Seed, 1 Flower";
                neededText.text = "Needed: 1 crystal";
                gieterText.text = "Obtained 1 flower";
            }
        }

        if (other.gameObject.CompareTag("Grown"))
        {
            gieterText.text = "Your plant has fully grown! Return to collect it.";
            triggerCount = triggerCount + 1;
            other.gameObject.SetActive(false);
        }



        //Room 2

        if (other.gameObject.CompareTag("Feed"))
        {
            tempText.text = "I'm hungry. In return for green food I will give you this crystal I found.";
            if (objectCount >= 5)
            {
                AudioSource.PlayClipAtPoint(collectClip, transform.position);
                tempText.text = "Thank you! Have my crystal";
                collectedText.text = "Collected: 1 Seed, 1 Flower, 1 Crystal.";
                neededText.text = "";
                gieterText.text = "You collected all the ingredients! Find the cauldron and put them in there to finish the potion.";

            }
        }

        if (other.gameObject.CompareTag("Green Food"))
        {
            other.gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(collectClip, transform.position);
            gieterText.text = "Obtained: Green Food";
            objectCount = objectCount + 1;

            //object count is 5
        }

        //Room 3

        if (other.gameObject.CompareTag("Key"))
        {
            AudioSource.PlayClipAtPoint(collectClip, transform.position);
            other.gameObject.SetActive(false);
            objectCount = objectCount + 1;
            gieterText.text = "Obtained: Key";

            
            // object count is 6
        }

        if (other.gameObject.CompareTag("Door"))
        {
            if (objectCount >= 6)
            {
                other.gameObject.SetActive(false);
            }
        }

        //als je in de trigger voor informatie loopt, roept hij die functie aan en krijg je informatie te zien. 
        if (other.gameObject.CompareTag("Info"))
        {
            SetInfoText();
        }

        //room 4
        //Boolean voor enemy zodat je in Update damage kan doen. Hier zorg ik ervoor dat hij true wordt. 
        if (other.gameObject.CompareTag("Enemy"))
        {
            attackBool = true;
            enemy = other.gameObject;
        }

        //Zorgt ervoor dat de aanvallen van de enemy damage doen en zorgt voor een message als de player dood gaat. 
        if (other.gameObject.CompareTag("MagicBall"))
        {
            AudioSource.PlayClipAtPoint(hurtClip, transform.position);
            playerHealth = playerHealth - 10;
            healthText.text = "Health = " + playerHealth.ToString();

            if (playerHealth <= 0)
            {
                gieterText.text = "YOU DIED";
            }
        }

        if (other.gameObject.CompareTag("Cauldron"))
        {
         
            {
                caudronBool = true;
                tempText.text = "Press F to put in all the ingredients!";

            }

        }

        if (other.gameObject.CompareTag("House"))
        {
            lastSceneBool = true;
            tempText.text = "Press F to go in";
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

        if (other.gameObject.CompareTag("Plant"))
        {
        
                tempText.text = "";
            plantBool = false;
        }

        if (other.gameObject.CompareTag("Grond"))
        {
            tempText.text = "";
            groundBool = false; 
        }
    }


    void SetInfoText()
    {

        if ( objectCount < 3)
        {
            //de informatie text die je krijgt als je de info trigger inloopt.
            infoText.text = "Hi there, you are a witch but you're losing you're not feeling well. Make a potion to feel better! Collect the following things: ";
            controlText.text = "Use wasd to move and your mouse to look around. You can jump by pressing the spacebar and double jump by pressing it twice. ";
        }

        if (objectCount > 3)
        {
            infoText.text = "Watch out! There's and enemy ahead!";
            controlText.text = "Avoid his magic balls and attack by pressing E.";
        }
     
    }

    void SetInfoDelText()
    {
        //deze functie zet de informatie text weer op leeg, zodat hij niet meer in je beeld staat als je uit de info trigger loopt.
        infoText.text = "";
        controlText.text = "";
    }


    


}