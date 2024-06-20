using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AI : MonoBehaviour
{
 
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navmesh;

    [SerializeField] float reachDistance;
    Transform destinationPoint;

    public GameObject player;
    public float searchRadius=3f;



    bool started = false;
    // Start is called before the first frame update
    void Start()
    {
       
        SetDestination();
    
        started = true;
    }
    void SetDestination()
    {
      
            //destinationPoint = GamePlayHandler.Instance.AI_Poses[Random.Range(0, GamePlayHandler.Instance.AI_Poses.Length)].transform;
     
    }
    // Update is called once per frame
    void Update()
    {
        if (!started) return;

        if(player != null)
        {
            navmesh.destination = player.transform.position;
            if (Vector3.Distance(transform.position, player.transform.position) <= reachDistance)
            {
                if (!animator.GetBool("Attack")) animator.SetBool("Attack", true);

            }
            else
            {
                if (animator.GetBool("Attack")) animator.SetBool("Attack", false);
            }
        }
        else
        {
            if (animator.GetBool("Attack")) animator.SetBool("Attack", false);
            Collider[] cols = Physics.OverlapSphere(transform.position, searchRadius);

         

            foreach (Collider col in cols)
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    player = col.gameObject;
                    return;
                }
            }
           



            navmesh.destination = destinationPoint.position;
            if (Vector3.Distance(transform.position, destinationPoint.position) <= reachDistance)
            {
                SetDestination();

            }

        }


    }

    public void Attack()
    {
        player?.GetComponent<DamageRecieve>().Damage(5f);
    }

  
}
