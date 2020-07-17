using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class BattleManager : MonoBehaviour
{

    [SerializeField]private PlayerMovement player;
    [SerializeField] private EnemyController enemy;
    private Transform playerTransform;
    private Transform enemyTransform;

    private float distance;


    //public ReactiveProperty<bool> playerIsAttackingSubject = new ReactiveProperty<bool>();

    private void Awake()
    {
        playerTransform = player.transform;
        enemyTransform = enemy.transform;
    }

    void Start()
    {
        //playerとenemyの橋渡し役
        //どちらかが攻撃しているときは他方は攻撃行動がとれない
        //プレイヤーが攻撃開始を通知してきたら、それを敵に伝える
        player.attackingSubject
            .Subscribe(b=>enemy.Defend(b));

        //player.kickSubject
        //    .Where(b=>b)
        //    .Subscribe(_=> StartCoroutine(enemy.GetKicked()));

        player.stateMachine.OnStateEnterAsObservable()
            .Where(s=>s.StateInfo.IsName("Kick"))
            .Subscribe(_ => StartCoroutine(enemy.GetKicked()));

        this.UpdateAsObservable()
            .Select(_ => Vector3.Distance(playerTransform.position, enemyTransform.position))
            .Subscribe(d => {
                player.Distance = d;
                enemy.Distance = d;
            });
    }

    void Update()
    {
        
    }
}
