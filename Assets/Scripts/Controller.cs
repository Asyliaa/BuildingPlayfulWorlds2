using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//de onderdelen over de first person controller komen van het script uit de les. Ik heb in dit script zelf er nog veel bijgeschreven,
//het grootste deel heb ik zelf bedacht, maar sommige dingen komen van verschillende tutorials.

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

    //texts 
    public Text countText;
    public Text winText;
    public Text infoText;
    public Text controlText;
    public Text scoreText;
    public Text gieterText;
    public Text collectedText;
    public Text neededText;
    public Text tempText;
    public Text enemyText;
    public Text healthText;

    //booleans
    public bool attackBool;
    public bool plantBool;
    public bool groundBool;
    public bool caudronBool;
    public bool lastSceneBool;

    //ints
    public int objectCount;
    public int triggerCount;
    private int damage = 5;

    //overig
    public GameObject enemy;
    public AudioClip hurtClip;
    public AudioClip collectClip;
    public AudioClip attackClip;
    private int playerHealth;


    void Start()
    {

        //Set Cursor to not be visible
        Cursor.visible = false;


        //texten leeg zetten of invoeren wat er moet staan
        rigidBody = GetComponent<Rigidbody>();
        objectCount = 0;
        playerHealth = 100;
        triggerCount = 0;
        infoText.text = "";
        controlText.text = "";
        gieterText.text = "";
        neededText.text = "Needed: seed flower crystal";
        collectedText.text = "";
        healthText.text = "Health = 100";
        tempText.text = "";
        enemyText.text = "";



    }

    //deze IEnumerator zorgt ervoor dat als je de ingredienten in de ketel doet, dat dan niet gelijk de volgende scene wordt geladen
    //dit vond ik er namelijk niet mooi uitzien.
    IEnumerator WaitForIt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(4);
    }


    // Update is called once per frame
    void Update()

    {

        //Booleans

        //Deze boolean zorgt dat je de enemy kan aanvallen door op E te drukken
        if (attackBool == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                AudioSource.PlayClipAtPoint(attackClip, transform.position);
                enemy.GetComponent<EnemyScript>().enemyHealth -= damage;
                enemyText.text = "Enemy health =" + enemy.GetComponent<EnemyScript>().enemyHealth.ToString();
                gieterText.text = "Enemy took 10 damage!";
                print("It works");
                if (enemy.GetComponent<EnemyScript>().enemyHealth <= 0)
                {
                    gieterText.text = "Enemy died!";
                }
            }

        }


        //Deze boolean zorgt ervoor dat je de paddestoel water kan geven zodat je 2 zaadjes krijgt door op F te drukken als je de gieter hebt
        if (plantBool == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                AudioSource.PlayClipAtPoint(collectClip, transform.position);
                collectedText.text = "Collected: seed";
                neededText.text = "Needed: flower crystal";
                gieterText.text = "Obtained 2 seeds";
                print("it works!");
                objectCount = objectCount + 1;

            }
        }

        //Deze boolean zorgt ervoor dat je een zaadje kan planten door op F te drukken als je alle benodigde objecten hebt
        if (groundBool == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                AudioSource.PlayClipAtPoint(collectClip, transform.position);
                gieterText.text = "Planted 1 seed";
                collectedText.text = "Collected: seed";
                neededText.text = "Needed: flower crystal";
                print("it wooorks!");
                objectCount = objectCount + 1;
            }
        }

        //Deze boolean zorgt ervoor dat je het huisje ingaat
        if (lastSceneBool == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        //Deze boolean zorgt ervoor dat je als de ingredienten in de ketel worden gedaan je naar de eindscene gaat (na 1 seconde)
        if (caudronBool == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(WaitForIt(1.0f));
            }
        }




        //Get input from WASD
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");

        //Get Input from the mouse movement
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");


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


    //trigger voor pick ups en voor de informatie. 
    void OnTriggerEnter(Collider other)
    {
        //als je een pick up object oppakt dan gaat het object weg en telt je score op.

        //ROOM ONE
        if (other.gameObject.CompareTag("Pick Up Gieter"))
        {
            AudioSource.PlayClipAtPoint(collectClip, transform.position);
            other.gameObject.SetActive(false);
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

            if (objectCount >= 3)
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

            //triggercount kijkt of je door de trigger bent gelopen die aangeeft dat je plant is gegroeid
            if (triggerCount >= 1)
            {
                AudioSource.PlayClipAtPoint(collectClip, transform.position);
                tempText.text = "";
                collectedText.text = "Collected: seed flower";
                neededText.text = "Needed: crystal";
                gieterText.text = "Obtained 1 flower";
                other.gameObject.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("Grown"))
        {
            if (objectCount >= 4)
            {
                AudioSource.PlayClipAtPoint(collectClip, transform.position);
                gieterText.text = "Your plant has fully grown! Return to collect it.";
                triggerCount = triggerCount + 1;
                other.gameObject.SetActive(false);
            }

        }



        //ROOM TWO
        if (other.gameObject.CompareTag("Feed"))
        {
            tempText.text = "I'm hungry. In return for green food I will give you this crystal I found.";
            if (objectCount >= 5)
            {
                AudioSource.PlayClipAtPoint(collectClip, transform.position);
                tempText.text = "Thank you! Have my crystal";
                collectedText.text = "Collected: seed flower crystal";
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


        //ROOM THREE
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
            if (objectCount <= 5)
            {
                tempText.text = "Key needed to open this door";
            }
            if (objectCount >= 6)
            {
                other.gameObject.SetActive(false);
            }
        }



        //ROOM FOUR
        //Boolean voor enemy zodat je in Update damage kan doen. Hier zorg ik ervoor dat hij true wordt. 
        if (other.gameObject.CompareTag("Enemy"))
        {
            attackBool = true;
            enemy = other.gameObject;
            enemyText.text = "Enemy health =" + enemy.GetComponent<EnemyScript>().enemyHealth.ToString();
        }

        //Zorgt ervoor dat de aanvallen van de enemy damage doen en zorgt voor een message als de player dood gaat. 
        if (other.gameObject.CompareTag("MagicBall"))
        {
            AudioSource.PlayClipAtPoint(hurtClip, transform.position);
            playerHealth = playerHealth - 10;
            healthText.text = "Health = " + playerHealth.ToString();

            if (playerHealth <= 0)
            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

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

        //als je in de trigger voor informatie loopt, roept hij die functie aan en krijg je informatie te zien. 
        if (other.gameObject.CompareTag("Info"))
        {
            SetInfoText();
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

        if (other.gameObject.CompareTag("Door"))
        {
            tempText.text = "";
        }
    }


    void SetInfoText()
    {

        if (objectCount < 3)
        {
            //de informatie text die je krijgt als je de info trigger inloopt.
            infoText.text = "Hi there, you are a witch but you're not feeling well. You have to make a potion to get better, but you'll need to collect a seed, a flower and a crystal to make it. ";
            controlText.text = "Use wasd to move and your mouse to look around. If other keys have to be pressed, it will show up on screen.";
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