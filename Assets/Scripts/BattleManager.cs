using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BattleManager : MonoBehaviour
{

    private PlayerMovement player;
    private EnemyController enemy;


    public ReactiveProperty<bool> playerIsAttackingSubject = new ReactiveProperty<bool>();

    // Start is called before the first frame update
    void Start()
    {
        //playerとenemyの橋渡し役
        //どちらかが攻撃しているときは他方は攻撃行動がとれない
        //プレイヤーが攻撃開始を通知してきたら、それを敵に伝える
        player.playerIsAttackingSubject
            .Where(x=>x)
               .subscribe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
