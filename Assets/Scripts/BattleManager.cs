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

    //[SerializeField] private BattleUI battleUI;

    private float distance;

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
            .Where(s => s.StateInfo.IsName("Kick"))
            //.Subscribe(_ => StartCoroutine(enemy.GetKicked()));
            .Delay(TimeSpan.FromSeconds(0.5))
            .Subscribe(_ => enemy.GetComponent<Animator>().SetTrigger("IsKicked"));

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
