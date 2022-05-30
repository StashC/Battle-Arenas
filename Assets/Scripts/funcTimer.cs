using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class funcTimer
    //Dont extend monobehaviour to decrease coupling.  Keep as simple class
{
    public static funcTimer Create(Action action, float time) {
        funcTimer ft = new funcTimer(action, time);

        //creates a gameobject called functionTimer, of the type MB (a mono behaviour)
        GameObject gameObject = new GameObject("functionTimer", typeof(MB));
        gameObject.GetComponent<MB>().onUpdate = ft.Update;

        return ft;
    }
    //class which has access to update
    public class MB : MonoBehaviour{
        public Action onUpdate;
        private void Update() {
            if(onUpdate != null) onUpdate();
        }
    }
    private Action callback;
    private float time;
    private bool isDestroyed;
    private funcTimer(Action callback, float time) {
        this.callback = callback;
        this.time = time;
    }
    
    public void Update() {
        if(!isDestroyed) {
            time -= Time.deltaTime;
            if(time <= 0) {
                callback();
                Destruct();
            }
        }
    }

    private void Destruct() {
        isDestroyed = true;
    }
}
