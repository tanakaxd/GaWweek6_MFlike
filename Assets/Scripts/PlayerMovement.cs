using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private bool isAttacking;
    private bool isAttacked;
    private double downTime = 2;
    [SerializeField]private Transform enemy;
    [SerializeField]
    private Collider hitRadius;


   private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

        var stateMachine = animator.GetBehaviour<ObservableStateMachineTrigger>();

        stateMachine
            .OnStateExitAsObservable()
            .Where(b => b.StateInfo.IsName("Standing Melee Attack Horizontal"))
            //.SkipWhile(b => b.StateInfo.normalizedTime <= 1.0f)
            .Subscribe(_ => {
               //transform.rotation = Quaternion.Euler(90,90,90);//回転を受け付けない。原因不明。下の方法ならできる。
                Debug.Log("Attack end");
                //Debug.Log(transform.rotation);
                //Debug.Log(defaultRotation);
                //transform.LookAt(enemy);
                hitRadius.enabled = false;
            });
            

        var updateObservable = this.UpdateAsObservable();

        updateObservable
            .Where(_ => Input.GetKeyDown("w"))
            .ThrottleFirst(TimeSpan.FromSeconds(downTime))//subscribeされたときにdownTimeは固定されるっぽい
            .Subscribe(_ => { 
                animator.SetTrigger("Attack");
                hitRadius.enabled = true;
                    
            }); 

        updateObservable
             .Where(_ => Input.GetKeyDown("q"))
             .ThrottleFirst(TimeSpan.FromSeconds(5))//subscribeされたときにdownTimeは固定されるっぽい
             .Subscribe(_ => animator.SetTrigger("JumpAttack"));

        //updateObservable
        //    //.Where(_ => Input.GetKeyDown("p"))
        //    .Where(_ => animator.GetCurrentAnimatorStateInfo(0).IsName("Standing Idle"))
        //    .Subscribe(_ => transform.LookAt(enemy));


        //updateObservable
        //    .Where(_ => Input.GetKeyDown("a"))
        //    .Subscribe(_ => animator.SetBool("WalkBackward",true));

        updateObservable
            .Where(_ => Input.GetKeyDown("d"))
            .Subscribe(_ => animator.SetBool("IsWalkingForward",true));

        updateObservable
            .Where(_ => Input.GetKeyUp("d"))
            .Subscribe(_ => animator.SetBool("IsWalkingForward", false));

        updateObservable
            .Where(_ => Input.GetKeyDown("a"))
            .Subscribe(_ => animator.SetBool("IsWalkingBack", true));

        updateObservable
            .Where(_ => Input.GetKeyUp("a"))
            .Subscribe(_ => animator.SetBool("IsWalkingBack", false));
        updateObservable
            .Where(_ => Input.GetKeyUp("g"))
            .Subscribe(_ => animator.SetTrigger("IsDamaged"));

        //this.OnTriggerEnterAsObservable()
        //    //.Where(collider => collider.transform == enemy)
        //    //.Select(other => other.GetComponent<Animator>())
        //    .Subscribe(_ => Debug.Log("jhit"));
        //    //.Subscribe(a=>a.SetTrigger("IsDamaged"));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("d"))
        {
            transform.position += transform.forward * Time.deltaTime;

        }
        else if (Input.GetKey("a"))
        {
            transform.position -= transform.forward * Time.deltaTime;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Debug.Log(transform.name+":subject");
        Debug.Log(other.transform.name+":object");

        
        //other.GetComponent<Animator>().SetTrigger("IsDamaged");
    }



}

//var clickStream = Observable.EveryUpdate()
//    .Where(_ => Input.GetMouseButtonDown(0));

//clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
//    .Where(xs => xs.Count >= 2)
//    .Subscribe(xs => Debug.Log("DoubleClick Detected! Count:" + xs.Count));
