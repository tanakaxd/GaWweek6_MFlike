using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerModel : MonoBehaviour
{
    //戦闘外用の静的なモデルと戦闘用の動的なモデルに分ける？

    private float attack;
    private float defense;
    public float stamina = 100;
    private float health;
    private float concentration;
    private float guard;
    //private float fatigue;

    public FloatReactiveProperty reactiveStamina = new FloatReactiveProperty();
    public FloatReactiveProperty reactiveHealth = new FloatReactiveProperty();

    public List<string> skills;
    //private Weapon weapon;
    //private Armor armor;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
