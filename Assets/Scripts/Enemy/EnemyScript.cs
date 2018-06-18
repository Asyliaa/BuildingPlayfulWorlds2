using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{

    public float lookRadius = 10f;

    Transform target;
    NavMeshAgent agent;

    public int enemyHealth = 50;
    private GameObject player; 


    // Use this for initialization
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = 50;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
            player.GetComponent<Controller>().enemy = null;
            player.GetComponent<Controller>().attackBool = false;
            player.GetComponent<Controller>().enemyText.text  = "Enemy died";
            


        }

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                // Attack the target
                FaceTarget();
            }
        }
    }



    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    

}
