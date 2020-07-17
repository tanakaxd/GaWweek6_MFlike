using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// All this does is implement the OnAnimatorMove() for our Units
/// </summary>
public class RootMotionNavMesh : MonoBehaviour
{
    private Vector3 worldDeltaPosition = Vector3.zero;
    private Vector3 position = Vector3.zero;
    private NavMeshAgent agent;
    private Animator animator;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        // don't update the agent's position, the animation will do that
        //実際はこの二つはtrueにしておいても問題が顕在しない。問題点が不明
        //発見している違いは他のagentを押しやすいかどうか
        //おそらくtrueにしておくと、obstacleを避けようとして移動に反発が生まれ阻害される
        //本来は認識したagentを避けたいが、このゲームの場合むしろ相手に移動を妨害されたい
        //agent.updatePosition = false;
        //agent.updateRotation = false;//?
        //agent.updatePositionをtrueにしておくと、この場合agentは積極的な移動計算は行わないが、
        //transformがsurfaceを出そうになった時や他のagentとの衝突するときなどにtransformを矯正してくれているっぽい
    }

    private void Update()
    {
        Debug.Log(agent.nextPosition);
        //worldDeltaPosition = agent.nextPosition - transform.position;
        //Debug.Log(worldDeltaPosition);

        // Pull agent towards character
        //if (worldDeltaPosition.magnitude > agent.radius)
        //    agent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
        //上記の方法だとradiusより外にtrasformが移動した場合のみagentの位置を修正するので
        //他のagentとの距離感がおかしくなる
        // agent.nextPosition = transform.position;だとなにが不都合になるのかがわからない
        //agent.nextPosition = transform.position;
        //これはあほっぽいけど、一度agentの方に代入するとnavmesh surface上に限定されるので
        //結果としてsurface上にtransformを限定できる。つまり、この順序である必要がある
        //transform.position = agent.nextPosition;
    }

    private void OnAnimatorMove()
    //このメソッドを書いている時点でoverrideされるっぽい
    //root motionがhandled by scriptになる
    //このメソッドはコメントアウトしても一応動くが、動くが少しカクカクする
    {
        // Update position based on animation movement using navigation surface height
        position = animator.rootPosition;
        //position.y = agent.nextPosition.y;
        transform.position = position;
    }
  

        /*

        //別の方法
        //下の方法でもできなくはないが、agent同士がすり抜けやすくなる
        //おそらく位置情報を更新するタイミングとかの影響だと思う
     private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updatePosition = false;
        agent.updateRotation = false;//?
    }

    private void Update()
    {
        agent.nextPosition = transform.position;
        transform.position = agent.nextPosition;
    }

    private void OnAnimatorMove()
    {
        position = animator.rootPosition;
        transform.position = position;
    }
    */
}