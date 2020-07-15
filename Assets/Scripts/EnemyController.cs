using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private bool isAttacking;
    private bool isAttacked;
    private double downTime = 2;
    [SerializeField] private Transform enemy;
    [SerializeField]
    private Collider hitRadius;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
