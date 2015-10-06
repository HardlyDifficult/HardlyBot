using System;
using UnityEngine;

namespace Hardly.Unity {
    public class Ball : GameEntity {

        [SerializeField]
        float speed;

        // Use this for initialization
        void Start() {
            var keys = new MovementKeys(GetComponent<ConstantForce>(), speed);
            OnUpdate += keys.Update;
        }

        // Update is called once per frame
        public override void Update() {
            base.Update();

        }

        void OnCollisionEnter(Collision col) {
            var weHit = col.gameObject;
            if(weHit.tag.Equals("Pickupable")) {
                weHit.transform.position = new Vector3(weHit.transform.position.x, 100, weHit.transform.position.z);
            }
        }
    }

}