using UnityEngine;
using UnityEngine.AI;

public class NPCWander : MonoBehaviour
{

    private NPCConversation conversation;

    public Transform player;

    public float walkSpeed = 2f;
    public float runSpeed = 6f;

    public float scareDistance = 5f;

    private NavMeshAgent agent;
    private Animator animator;

    private bool runningAway;
    private float idleTimer;

    [Header("Phone Calls")]
    public float minTimeBetweenCalls = 10f;
    public float maxTimeBetweenCalls = 30f;

    public float minCallDuration = 4f;
    public float maxCallDuration = 10f;

    private float nextPhoneCallTime;

    [HideInInspector]
    public bool talkingOnPhone;


    public GameObject phoneObject;

    [Header("Run Away SFX")]
    public AudioSource voiceAudio;
    public AudioClip[] runAwaySounds;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        conversation = GetComponent<NPCConversation>();

        GoToRandomPoint();

        ScheduleNextPhoneCall();
    }

    void Update()
    {
        if (!agent.isOnNavMesh || player == null) return;

        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);

        // RUN LOGIC
        if (distanceToPlayer < scareDistance)
        {
            if (!runningAway)
                RunAway();
        }
        else
        {
            runningAway = false;

            // when reached destination, pick new one
            if (!agent.pathPending &&
                agent.remainingDistance <= 0.5f)
            {
                idleTimer += Time.deltaTime;

                if (idleTimer > 1.5f)
                {
                    GoToRandomPoint();
                    idleTimer = 0f;
                }
            }
        }

        // ANIMATION
        animator.SetFloat("Speed", agent.velocity.magnitude);
        animator.SetBool("Running", runningAway);

        // Random phone calls
        if (!runningAway &&
            !talkingOnPhone &&
            (conversation == null || !conversation.isTalking) &&
            Time.time >= nextPhoneCallTime)
        {
            StartPhoneCall();
        }
    }

    void GoToRandomPoint()
    {
        runningAway = false;
        agent.speed = walkSpeed;

        Vector3 randomPoint = GetRandomNavMeshPoint();
        agent.SetDestination(randomPoint);
    }

    Vector3 GetRandomNavMeshPoint()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int randomIndex = Random.Range(0, navMeshData.vertices.Length);
        Vector3 randomPoint = navMeshData.vertices[randomIndex];

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas);

        return hit.position;
    }

    void RunAway()
    {
        // Stop conversation immediately
        if (conversation != null)
        {
            conversation.ForceEndConversation();
        }

        // Hang up phone call immediately
        if (talkingOnPhone)
        {
            CancelInvoke(nameof(EndPhoneCall));
            EndPhoneCall();
        }

        if (conversation != null)
        {
            conversation.ForceEndConversation();
        }

        if (talkingOnPhone)
        {
            CancelInvoke(nameof(EndPhoneCall));
            EndPhoneCall();
        }

        if (!runningAway)
        {
            if (voiceAudio != null &&
                runAwaySounds.Length > 0)
            {
                int index = Random.Range(0, runAwaySounds.Length);

                voiceAudio.PlayOneShot(runAwaySounds[index]);
            }
        }

        runningAway = true;
        agent.speed = runSpeed;

        Vector3 direction =
            (transform.position - player.position).normalized;

        Vector3 runTarget =
            transform.position + direction * 10f;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(runTarget, out hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void ScheduleNextPhoneCall()
    {
        nextPhoneCallTime = Time.time +
            Random.Range(minTimeBetweenCalls, maxTimeBetweenCalls);
    }

    void StartPhoneCall()
    {
        talkingOnPhone = true;

        phoneObject.SetActive(true);

        animator.SetBool("TalkingonPhone", true);

        Invoke(nameof(EndPhoneCall),
            Random.Range(minCallDuration, maxCallDuration));
    }

    void EndPhoneCall()
    {
        talkingOnPhone = false;

        animator.SetBool("TalkingonPhone", false);

        phoneObject.SetActive(false);

        ScheduleNextPhoneCall();
    }
}