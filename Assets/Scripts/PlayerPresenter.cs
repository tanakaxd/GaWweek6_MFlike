using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;
using System;

public class PlayerPresenter : MonoBehaviour
{
    public PlayerView view;
    public PlayerModel model;
    public Collider hitRadius;
    private NavMeshAgent agent;
    private Animator animator;

    [HideInInspector] public ObservableStateMachineTrigger stateMachine;
    public BoolReactiveProperty reactiveIsAttacking = new BoolReactiveProperty(false);
    private bool beingAttacked;
    //public float Distance { private get; set; }
    public FloatReactiveProperty reactiveDistance = new FloatReactiveProperty(0);
    public float maxDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stateMachine = animator.GetBehaviour<ObservableStateMachineTrigger>();

    }

    void Start()
    {
        stateMachine
            .OnStateEnterAsObservable()
            .Where(s => s.StateInfo.IsTag("Attack"))
            .Subscribe(_ =>
            {
                //Debug.Log("Attack begin");
                hitRadius.enabled = true;
                reactiveIsAttacking.Value = true;
            });

        stateMachine
            .OnStateExitAsObservable()
            .Where(s => s.StateInfo.IsTag("Attack"))
            .Subscribe(_ =>
            {
                //Debug.Log("Attack end");
                hitRadius.enabled = false;
                reactiveIsAttacking.Value = false;
            });


        this.UpdateAsObservable()
            .Where(_ => Input.anyKeyDown && !beingAttacked && !reactiveIsAttacking.Value)
            .Select(_ => Input.inputString)
            .Select(s => GetSkillName(s))
            //名前だけでなくスキル使用条件も取得してフィルターする
            .Where(s=>s!=null)
            .Subscribe(s =>
            {
                animator.SetTrigger(s);
                model.reactiveStamina.Value -= 20;
            });
        /*
        //Skills
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown("w"))
            .Where(_ => !reactiveIsAttacking.Value)
            .Where(_ => !beingAttacked)
            .Subscribe(_ =>
            {
                animator.SetTrigger("Attack");
            });

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown("q") && !beingAttacked && !reactiveIsAttacking.Value)
            .Subscribe(_ =>
            {
                animator.SetTrigger("JumpAttack");
            });

*/
        //Kick
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space) && !beingAttacked && !reactiveIsAttacking.Value)
            .Subscribe(_ =>
            {
                animator.SetTrigger("Kick");
            });

        //Movement
        this.UpdateAsObservable()
            .Select(_ => Input.GetKey("d"))
            .DistinctUntilChanged()
            .Subscribe(isPressed => animator.SetBool("IsWalkingForward", isPressed)); 

        this.UpdateAsObservable()
            .Select(_ => Input.GetKey("a"))
            .DistinctUntilChanged()
            .Subscribe(isPressed => animator.SetBool("IsWalkingBack", isPressed));


        //View
        reactiveDistance
            .Subscribe(d => view.playerDistanceSlider.value = d/maxDistance);

        model.reactiveStamina
            .Subscribe(f => view.stamina.value = f / model.stamina);//TODO DOTweenを合成できるか？

    }

    private string GetSkillName(string s)
    {
        switch (s)
        {
            case "q":
                if (model.skills.Count >= 1) return model.skills[0];
                break;
            case "w":
                if (model.skills.Count >= 2) return model.skills[1];
                break;
            case "e":
                if (model.skills.Count >= 3) return model.skills[2];
                break;
            case "r":
                if (model.skills.Count >= 4) return model.skills[3];
                break;
            default:
                return null;
        }
        return null;
    }

    void Update()
    {
        
    }
}
