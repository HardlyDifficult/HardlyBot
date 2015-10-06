using UnityEngine;
using System.Collections;

namespace Hardly.Unity {

    public class ChaseTarget : MonoBehaviour {

        [SerializeField]
        private GameObject objectToChase;

        [SerializeField]
        float speed;

        // Use this for initialization
        void Start() {

        }

        float angle = 0;

        // Update is called once per frame
        void FixedUpdate() {
            ConstantForce force = GetComponent<ConstantForce>();
            var velocity = new Vector3(0, 0, 0);

            if(transform.position.x < objectToChase.transform.position.x) {
                velocity += new Vector3(1, 0, 0);
            } else if(transform.position.x > objectToChase.transform.position.x) {
                velocity += new Vector3(-1 , 0, 0);
            }
            if(transform.position.z < objectToChase.transform.position.z) {
                velocity += new Vector3(0, 0, 1);
            } else if(transform.position.z > objectToChase.transform.position.z) {
                velocity += new Vector3(0, 0, -1 );
            }

            //if(Mathf.Abs(velocity.x) + Mathf.Abs(velocity.y) + Mathf.Abs(velocity.z) > 1) {
            //    velocity = new Vector3(velocity.x / 2, velocity.y / 2, velocity.z / 2);
            //}

            //force.force = velocity;
            transform.rotation =  Quaternion.AngleAxis(angle, velocity);
            angle += speed;
            if(angle > 360) {
                angle = 0;
            }
        }
    }
}