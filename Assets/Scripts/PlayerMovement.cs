using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    //private bool isAttacking;
    private bool beingAttacked;

    private double downTime = 2;
    [SerializeField] private Transform enemy;
    private NavMeshAgent agent;

    [SerializeField]
    private Collider hitRadius;

    public ReactiveProperty<bool> attackingSubject = new ReactiveProperty<bool>();

    //public ReactiveProperty<bool> kickSubject = new ReactiveProperty<bool>();
    [HideInInspector] public ObservableStateMachineTrigger stateMachine;

    private float distance;
    public float Distance { private get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine = animator.GetBehaviour<ObservableStateMachineTrigger>();
        //agent.updatePosition = false;
        //agent.updateRotation = false;
    }

    private void Start()
    {
        stateMachine
            .OnStateExitAsObservable()
            .Where(s => s.StateInfo.IsTag("Attack"))
            //.SkipWhile(b => b.StateInfo.normalizedTime <= 1.0f)
            .Subscribe(_ =>
            {
                Debug.Log("Attack end");
                hitRadius.enabled = false;
                attackingSubject.Value = false;
            });

        stateMachine
            .OnStateEnterAsObservable()
            .Where(s => s.StateInfo.IsTag("Attack"))
            .Subscribe(_ =>
            {
                Debug.Log("Attack begin");
                hitRadius.enabled = true;
                attackingSubject.Value = true;
            });

        //stateMachine
        //    .OnStateExitAsObservable()
        //    .Where(s => s.StateInfo.IsName("Kick"))
        //    .Subscribe(_=>kickSubject.Value = false);

        var updateObservable = this.UpdateAsObservable();

        updateObservable
            .Where(_ => Input.GetKeyDown("w") && !beingAttacked&& !attackingSubject.Value)
            //.ThrottleFirst(TimeSpan.FromSeconds(downTime))//subscribeされたときにdownTimeは固定されるっぽい
            .Subscribe(_ =>
            {
                animator.SetTrigger("Attack");
            });

        updateObservable
             .Where(_ => Input.GetKeyDown("q") && !beingAttacked && !attackingSubject.Value)
             //.ThrottleFirst(TimeSpan.FromSeconds(5))//subscribeされたときにdownTimeは固定されるっぽい
             .Subscribe(_ =>
             {
                 animator.SetTrigger("JumpAttack");
             });

        //updateObservable
        //    //.Where(_ => Input.GetKeyDown("p"))
        //    .Where(_ => animator.GetCurrentAnimatorStateInfo(0).IsName("Standing Idle"))
        //    .Subscribe(_ => transform.LookAt(enemy));

        //updateObservable
        //    .Where(_ => Input.GetKeyDown("a"))
        //    .Subscribe(_ => animator.SetBool("WalkBackward",true));

        updateObservable
            .Where(_ => Input.GetKeyDown("d"))
            .Subscribe(_ => animator.SetBool("IsWalkingForward", true));

        updateObservable
            .Where(_ => Input.GetKeyUp("d"))
            .Subscribe(_ => animator.SetBool("IsWalkingForward", false));

        updateObservable
            .Where(_ => Input.GetKeyDown("a"))
            .Subscribe(_ => animator.SetBool("IsWalkingBack", true));

        updateObservable
            .Where(_ => Input.GetKeyUp("a"))
            .Subscribe(_ => animator.SetBool("IsWalkingBack", false));
        //updateObservable
        //    .Where(_ => Input.GetKeyUp("g"))
        //    .First()
        //    .Take(200)
        //    .Concat()
        //    .First()
        //    .Subscribe(_=> GetKicked());

        updateObservable
            .Where(_ => Input.GetKeyDown(KeyCode.Space) && !beingAttacked && !attackingSubject.Value)
            .Subscribe(_ =>
            {
                //kickSubject.Value = true;
                animator.SetTrigger("Kick");
            });

        //this.OnTriggerEnterAsObservable()
        //    //.Where(collider => collider.transform == enemy)
        //    //.Select(other => other.GetComponent<Animator>())
        //    .Subscribe(_ => Debug.Log("jhit"));
        //    //.Subscribe(a=>a.SetTrigger("IsDamaged"));
    }

    // Update is called once per frame
    private void Update()
    {
        //if (Input.GetKey("d"))
        //{
        //    transform.position += transform.forward * Time.deltaTime;

        //}
        //else if (Input.GetKey("a"))
        //{
        //    transform.position -= transform.forward * Time.deltaTime;

        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(transform.name+":subject");
        //Debug.Log(other.transform.name+":object");

        //other.GetComponent<Animator>().SetTrigger("IsDamaged");
    }

    //public IEnumerator Kicked()
    //{
    //    animator.SetBool("IsKicked", true);
    //    yield return new WaitForSeconds(1);
    //    animator.SetBool("IsKicked", false);

    //}
}

//var clickStream = Observable.EveryUpdate()
//    .Where(_ => Input.GetMouseButtonDown(0));

//clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
//    .Where(xs => xs.Count >= 2)
//    .Subscribe(xs => Debug.Log("DoubleClick Detected! Count:" + xs.Count));