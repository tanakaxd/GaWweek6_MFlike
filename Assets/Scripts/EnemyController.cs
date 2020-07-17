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
    [SerializeField]private bool beingAttacked;
    private double downTime = 2;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerWeapon;
    [SerializeField]
    private Collider hitRadius;
    private float distance;
    public float Distance { private get; set; }
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
                beingAttacked = false;
            });

        stateMachine
            .OnStateEnterAsObservable()
            .Where(b=>b.StateInfo.IsName("Standing React Large Gut"))
            .Subscribe(_ => {
                beingAttacked = true;
            });

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
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("IsKicked", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("IsKicked", false);

    }
}
