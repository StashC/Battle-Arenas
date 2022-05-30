using System.Collections.Generic;
using UnityEngine;

public class damageZone : MonoBehaviour { 

    [Header("Customize")]
    [SerializeField] private int _damagePerTick;
    [SerializeField] private float _timeBetweenTick;
    [SerializeField] private float _lifetime;

    [HideInInspector] public int _team;

    private List<healthScript> objectsToDamage;
    private float damageTimer;

    private void Awake() {
        objectsToDamage = new List<healthScript>();
    }

    private void FixedUpdate() {
        if(damageTimer <= 0) {
            DealDamage();
            damageTimer = _timeBetweenTick;
        }
        damageTimer -= Time.fixedDeltaTime;
        if(_lifetime <= 0) Destroy(gameObject);
        _lifetime -= Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent<healthScript>(out healthScript hs)) {
            if(hs._team != _team && !objectsToDamage.Contains(hs)) objectsToDamage.Add(hs);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.TryGetComponent<healthScript>(out healthScript hs)) {
            if(hs._team != _team && objectsToDamage.Contains(hs)) objectsToDamage.Remove(hs);
        }
    }

    void DealDamage() {
        foreach(healthScript unit in objectsToDamage) {
            unit.DealDamage(_damagePerTick);
        }
    }
}
