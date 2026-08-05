using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCConversation : MonoBehaviour
{
    [Header("Conversation")]
    public float searchRadius = 2.5f;
    public float conversationDistance = 1.2f;
    public float conversationChance = 0.2f;
    public float conversationDuration = 8f;
    public float cooldown = 20f;
    public float arrivalDistance = 0.15f;

    [HideInInspector]
    public bool isTalking;

    private float nextConversationTime;

    private Animator animator;
    private NavMeshAgent agent;
    private NPCWander wander;


    private NPCConversation currentPartner;

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

        currentPartner = other;
        other.currentPartner = this;

        nextConversationTime = Time.time + cooldown;
        other.nextConversationTime = Time.time + cooldown;


        // Midpoint between both NPCs
        Vector3 center = (transform.position + other.transform.position) * 0.5f;

        // Direction from this NPC to the other
        Vector3 dir = (other.transform.position - transform.position).normalized;
        dir.y = 0;

        // Calculate where each NPC should stand
        Vector3 myPos = center - dir * (conversationDistance * 0.5f);
        Vector3 otherPos = center + dir * (conversationDistance * 0.5f);

        // Snap them into place
        agent.Warp(myPos);
        other.agent.Warp(otherPos);

        // Stop moving
        agent.isStopped = true;
        other.agent.isStopped = true;

        // Face each other
        Vector3 lookDir = other.transform.position - transform.position;
        lookDir.y = 0;

        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);

        lookDir = transform.position - other.transform.position;
        lookDir.y = 0;

        if (lookDir != Vector3.zero)
            other.transform.rotation = Quaternion.LookRotation(lookDir);

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

        currentPartner = null;
        other.currentPartner = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
    public void ForceEndConversation()
    {
        if (!isTalking)
            return;

        StopAllCoroutines();

        animator.SetBool("ConvoA", false);
        animator.SetBool("ConvoB", false);

        agent.isStopped = false;
        isTalking = false;

        NPCConversation partner = currentPartner;
        currentPartner = null;

        if (partner != null)
        {
            partner.StopAllCoroutines();

            partner.animator.SetBool("ConvoA", false);
            partner.animator.SetBool("ConvoB", false);

            partner.agent.isStopped = false;
            partner.isTalking = false;
            partner.currentPartner = null;

            partner.wander.SendMessage("GoToRandomPoint");
        }

        wander.SendMessage("GoToRandomPoint");
    }
}