using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Atributos")]
    public float life = 100f;
    public float atack;
    public float speed;
    public float lockradius;
    public float coliderradius = 2f;    
    
    [Header("componentes")] 
    private Animator anim;
    private CapsuleCollider capsule;
    private BoxCollider box;
    private NavMeshAgent agent;

    [Header("outros")] 
    public Transform player;
    private bool atacking;
    private bool walking;
    private bool waitfor;
    private bool hitting;
    void Start()
    {
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (life > 0)
        {

            float distance = Vector3.Distance(player.position, transform.position);

            if (distance <= lockradius)
            {
                agent.isStopped = false;

                if (!atacking)
                {
                    agent.SetDestination(player.position);
                    anim.SetBool("Slither Forward", true);
                    walking = true;
                }

                if (distance <= agent.stoppingDistance)
                {
                    StartCoroutine("Matack");
                }
                else
                {
                    atacking = false;
                }
            }
            else
            {
                agent.isStopped = true;
                anim.SetBool("Slither Forward", false);
                atacking = false;
                walking = false;
            }
        }
    }
    
    IEnumerator Matack()
    {
        if(!waitfor && !hitting)
        {
            waitfor = true;
            atacking = true;
            walking = false;
            anim.SetBool("Slither Forward", false);
            anim.SetBool("Bite Attack", true);
            yield return new WaitForSeconds(1.2f);
            GetPlayer();
            waitfor = false;
        }
    }

    void GetPlayer()
    {
        foreach(Collider c in Physics.OverlapSphere((transform.position + transform.forward * coliderradius),coliderradius))
        {
            if (c.gameObject.CompareTag("Player"))
            {
                Debug.Log("Aiinnnn");
            }
        }
    }

    public void getHit(int dmg)
    {
        life -= dmg;
        if (life > 0)
        {
            StopCoroutine("Matack");
            anim.SetTrigger("Take Damage");
            hitting = true;
            StartCoroutine("recovery");
        }
        else
        {
            anim.SetTrigger("Die");
        }
    }

    IEnumerator recovery()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Slither Forward", false);
        anim.SetBool("Bite Attack", false);
        hitting = false;
        waitfor = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, lockradius);
    }
}

