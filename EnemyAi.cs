using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{

    public float minX = -13.5f;
    public float minZ = -7f;
    public float maxX = 9f;
    public float maxZ = 7f;
    public float waitTime;
    public float startWaitTime;

    private NavMeshAgent agent;
    public Transform startingPos;

    private PlayerController Enemyplayer;

    public float lineOfSight;

    private float AttackInterval = 1f;
    private float FieldOfView = 60f;

    public float health;
    public float AIDamage;

    private Animator animator;
    public AudioSource AudioS;


    public enum states
    {
        Idle,
        WonderAround,
        ChasingEnemy,
        AttackEnemy,
        Dead

    }

    private states currentState = states.Idle;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        AudioS = GetComponent<AudioSource>();
        SetState(states.Idle);
        //waitTime = startWaitTime;
        startingPos.position = new Vector3(Random.Range(minX, maxX), .5f, Random.Range(minZ, maxZ));
    }

    private void Update()
    {
        if (health <= 0)
        {
            SetState(states.Dead);
        }
    }

    private void SetState(states newState)
    {
        StopAllCoroutines();

        currentState = newState;

        if (currentState == states.Idle)
        {
            StartCoroutine(onIdle());
        }
        else if (currentState == states.WonderAround)
        {
            StartCoroutine(onWonderAround());
        }
        else if (currentState == states.ChasingEnemy)
        {
            StartCoroutine(onChasingEnemy());
        }
        else if (currentState == states.AttackEnemy)
        {
            StartCoroutine(onAttackEnemy());
        }
        else if (currentState == states.Dead)
        {
            StartCoroutine(onDead());
        }
    }

    IEnumerator onIdle()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (startingPos != null)
            {
                SetState(states.WonderAround);
            }
        }
    }
    IEnumerator onWonderAround()
    {
        agent.SetDestination(startingPos.transform.position);
        while (true)
        {
            yield return new WaitForFixedUpdate();
            WonderingAround();

            LookForEnemies();
            if (Enemyplayer != null)
            {
                SetState(states.ChasingEnemy);
            }
        }
    }
    IEnumerator onChasingEnemy()
    {
        agent.ResetPath();

        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (Enemyplayer.health <= 0)
            {
                Enemyplayer = null;
                SetState(states.Idle);
                yield break;
            }

            float distanceToEnemy = Vector3.Distance(transform.position, Enemyplayer.transform.position);

            if (distanceToEnemy <= lineOfSight || !canSee(Enemyplayer))
            {
                //if the enemy is out of range move closer
                agent.SetDestination(Enemyplayer.transform.position);
                if (distanceToEnemy <= lineOfSight)
                {
                    SetState(states.AttackEnemy);
                }
            }
            else
            {

            }
        }
    }
    IEnumerator onAttackEnemy()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            float distanceToEnemy = Vector3.Distance(transform.position, Enemyplayer.transform.position);
            if (distanceToEnemy <= 1)
            {
                animator.SetBool("IsAttacking", true);
            }
            else if (distanceToEnemy >= 1 || !canSee(Enemyplayer))
            {
                animator.SetBool("IsAttacking", false);
                SetState(states.ChasingEnemy);
            }
        }
    }
    IEnumerator onDead()
    {
        while (true)
        {
            animator.SetTrigger("IsDead");
            StopAllCoroutines();
            Destroy(this);
            Destroy(agent);
            Destroy(GetComponent<Collider>());
            AudioS.Stop();
            yield break;
        }
    }

    private void WonderingAround()
    {
        transform.position = Vector3.MoveTowards(transform.position, startingPos.position, agent.speed * Time.deltaTime);
        agent.SetDestination(startingPos.transform.position);
        if (Vector3.Distance(transform.position, startingPos.position) <= 1.5f)
        {
            if (waitTime <= 0)
            {
                SetState(states.Idle);
                startingPos.position = new Vector3(Random.Range(minX, maxX), .5f, Random.Range(minZ, maxZ));
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    private void LookForEnemies()
    {
        Collider[] surrondingColliders = Physics.OverlapSphere(transform.position, lineOfSight);
        foreach (Collider c in surrondingColliders)
        {
            PlayerController player = c.GetComponent<PlayerController>();
            if (player != null && canSee(player) && player.health > 0)
            {
                Enemyplayer = player;
            }
        }
    }

    private bool canSee(PlayerController player)
    {
        Vector3 dirToEnemy = player.transform.position - transform.position;

        float Angle = Vector3.Angle(transform.forward, dirToEnemy);
        if (Angle <= FieldOfView)
        {
            return true;
        }
        return false;
    }

    public void EnableDamagePlayer()
    {
        //add damage to player here
        Enemyplayer.health -= AIDamage;
    }

}
