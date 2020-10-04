using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

    public int health = 5;

    public ParticleSystem[] bloodParticles;

    public Animator animator;
    public Collider trigger;

    public AudioSource audioSource;
    public AudioClip deathClip;


    public void Start()
    {
        ShipManager.instance.SetMonster(this);
        target = ShipManager.instance.playerTransform;
    }

    public void Update()
    {
        agent.destination = target.position;


        if (health == 0)
        {
            if (!ShipManager.instance.monsterKilled)
            {
                audioSource.loop = false;
                audioSource.clip = deathClip;
                audioSource.Play();

                animator.SetTrigger("Die");
                agent.isStopped = true;
                trigger.enabled = false; 
                ShipManager.instance.textEvents[19].segmentToTrigger = ShipManager.instance.currentPlayerSegment + 1;
                ProgressManager.instance.lockedDoorRooms.Add(ShipManager.instance.currentPlayerSegment + 1);
            }

            ShipManager.instance.monsterKilled = true; 
        }
    }

    public void Shot()
    {
        bloodParticles[health - 1].Play();
        health -= 1;
        Debug.Log("HIT");
    }


    public void OnTriggerEnter(Collider other)
    {
        ProgressManager.instance.ReloadScene();
    }
}
