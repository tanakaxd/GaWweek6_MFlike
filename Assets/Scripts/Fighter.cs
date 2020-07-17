using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public abstract class Fighter : MonoBehaviour
{
    protected Animator animator;
    protected NavMeshAgent agent;

    protected bool isAttacking;
    protected bool beingAttacked;

    [SerializeField] protected Transform enemy;
    [SerializeField] protected Collider hitRadius;

    
    protected ReactiveProperty<bool> isAttackingSubject = new ReactiveProperty<bool>();

    public ReactiveProperty<bool> IsAttackingSubject { get; private set; }

}
