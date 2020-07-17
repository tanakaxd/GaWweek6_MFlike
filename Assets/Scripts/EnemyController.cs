using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private bool beingAttacked;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerWeapon;
    private Collider hitRadius;

    public ReactiveProperty<bool> attackingSubject = new ReactiveProperty<bool>();

    private float distance;

    [HideInInspector] public ObservableStateMachineTrigger stateMachine;


    public float Distance { private get; set; }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();
        stateMachine = animator.GetBehaviour<ObservableStateMachineTrigger>();

    }
    void Start()
    {
        var stateMachine = animator.GetBehaviour<ObservableStateMachineTrigger>();

        //stateMachine
        //    .OnStateExitAsObservable()
        //    .Where(b => b.StateInfo.IsName("Standing React Large Gut"))
        //    .Subscribe(_ => {
        //        beingAttacked = false;
        //    });

        //stateMachine
        //    .OnStateEnterAsObservable()
        //    .Where(b=>b.StateInfo.IsName("Standing React Large Gut"))
        //    .Subscribe(_ => {
        //        beingAttacked = true;
        //    });

        this.OnTriggerEnterAsObservable()
            .Where(collider => collider.transform == playerWeapon&&beingAttacked)
            //.ThrottleFirst(TimeSpan.FromSeconds(2))
            .Subscribe(_=>animator.SetTrigger("IsDamaged"));

    }

    public void Defend(bool beginning)
    {
        beingAttacked = beginning;
    }

    public IEnumerator GetKicked()
        //animator controllerの方で入るまでの秒数と継続時間を書く方法を採用したので使わない予定
        //やっぱ無理っぽいので使う
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("IsKicked", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("IsKicked", false);

    }
}
