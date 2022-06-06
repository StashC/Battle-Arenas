using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpell : SpellAttack
{
    public GameObject _attack;
    public override void attack() {
        Instantiate(_attack, transform.position, transform.rotation);
    }
}
