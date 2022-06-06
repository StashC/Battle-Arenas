using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellAttack : MonoBehaviour
{
    public int _manaCost;
    public float _coolDown;
    public int _damage;

    [HideInInspector] public bool isSelected = false;
    public bool onCooldown;
    private float cooldownTimer;
    SpriteRenderer sr;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        onCooldown = false;
    }
    public bool handleAttack() {
        if(!onCooldown) {
            attack();
            cooldownTimer = _coolDown;
            onCooldown = true;
            return true;
        }
        return false;
    }
    public abstract void attack();

    private void Update() {
        sr.enabled = isSelected;
            cooldownTimer -= Time.deltaTime;
        if(cooldownTimer <= 0) onCooldown = false;
    }
}
