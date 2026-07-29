using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCConversation : MonoBehaviour
{
    [Header("Conversation")]
    public float searchRadius = 2.5f;
    public float conversationChance = 0.2f;
    public float conversationDuration = 8f;
    public float cooldown = 20f;


    [HideInInspector]
    public bool isTalking;

    private float nextConversationTime;

    private Animator animator;
    private NavMeshAgent agent;
    private NPCWander wander;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        wander = GetComponent<NPCWander>();

        nextConversationTime = Time.time + Random.Range(5f, 15f);
    }

    void Update()
    {
        if (isTalking)
            return;

        if (wander.talkingOnPhone)
            return;

        if (Time.time < nextConversationTime)
            return;

        Collider[] nearby = Physics.OverlapSphere(transform.position, searchRadius);

        foreach (Collider c in nearby)
        {
            if (c.gameObject == gameObject)
                continue;

            NPCConversation other = c.GetComponent<NPCConversation>();

            if (other == null)
                continue;

            if (other.isTalking)
                continue;

            if (other.wander.talkingOnPhone)
                continue;

            if (Random.value > conversationChance)
                continue;

            StartCoroutine(Conversation(other));

            break;
        }
    }

    IEnumerator Conversation(NPCConversation other)
    {
        isTalking = true;
        other.isTalking = true;

        nextConversationTime = Time.time + cooldown;
        other.nextConversationTime = Time.time + cooldown;

        agent.isStopped = true;
        other.agent.isStopped = true;

        Vector3 dir = other.transform.position - transform.position;
        dir.y = 0;

        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        Vector3 dir2 = transform.position - other.transform.position;
        dir2.y = 0;

        if (dir2 != Vector3.zero)
            other.transform.rotation = Quaternion.LookRotation(dir2);

        bool thisIsA = Random.value < 0.5f;

        if (thisIsA)
        {
            animator.SetBool("ConvoA", true);
            other.animator.SetBool("ConvoB", true);
        }
        else
        {
            animator.SetBool("ConvoB", true);
            other.animator.SetBool("ConvoA", true);
        }

        yield return new WaitForSeconds(conversationDuration);

        animator.SetBool("ConvoA", false);
        animator.SetBool("ConvoB", false);

        other.animator.SetBool("ConvoA", false);
        other.animator.SetBool("ConvoB", false);

        agent.isStopped = false;
        other.agent.isStopped = false;

        wander.SendMessage("GoToRandomPoint");
        other.wander.SendMessage("GoToRandomPoint");

        isTalking = false;
        other.isTalking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}