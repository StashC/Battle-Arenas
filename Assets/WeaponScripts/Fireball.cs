using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime;
    [SerializeField] private int _damage;
    [SerializeField] private GameObject _fireZone;

    [SerializeField] private bool _spawnBloodOnHit = false;
    [SerializeField] private GameObject _bloodSplatter;

    public int _team;
    private float timeLeft;

    // Start is called before the first frame update
    void Start() {
        timeLeft = _lifetime;
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {
        if(timeLeft < 0) {
            Destroy(gameObject);
        }
        _transform.Translate(Vector2.right * Time.deltaTime * _speed);
        timeLeft -= Time.deltaTime;
    }

    private void Explode() {
        Instantiate(_fireZone, transform.position, transform.rotation);
        //Debug.Log("exploded");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent<healthScript>(out healthScript hs)) {
            if(hs._team != _team) {
                hs.DealDamage(_damage);
                Explode();
                if(_spawnBloodOnHit && _bloodSplatter != null) {
                    Instantiate(_bloodSplatter, transform.position, Quaternion.identity);
                }                
            }
        }
        //if hit an obstacle, explode
        if(collision.gameObject.layer == 7) Explode();
    }

}
