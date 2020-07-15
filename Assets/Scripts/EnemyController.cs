using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private bool isAttacking;
    [SerializeField]private bool isAttacked;
    private double downTime = 2;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerWeapon;
    [SerializeField]
    private Collider hitRadius;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    void Start()
    {
        var stateMachine = animator.GetBehaviour<ObservableStateMachineTrigger>();

        stateMachine
            .OnStateExitAsObservable()
            .Where(b => b.StateInfo.IsName("Standing React Large Gut"))
            .Subscribe(_ => {
                isAttacked = false;
            });

        stateMachine
            .OnStateEnterAsObservable()
            .Where(b=>b.StateInfo.IsName("Standing React Large Gut"))
            .Subscribe(_ => {
                isAttacked = true;
            });

        this.OnTriggerEnterAsObservable()
            .Where(collider => collider.transform == playerWeapon&&!isAttacked)
            //.ThrottleFirst(TimeSpan.FromSeconds(2))
            .Subscribe(_=>animator.SetTrigger("IsDamaged"));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
