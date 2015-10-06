using UnityEngine;


namespace Hardly.Unity {
    public class MovementKeys {
        ConstantForce force;
        float speed;

        public MovementKeys(ConstantForce force, float speed) {
            this.force = force;
            this.speed = speed;
        }
        
        public void Update() {
            MovementKey(KeyCode.UpArrow, new Vector3(0, 0, speed));
            MovementKey(KeyCode.DownArrow, new Vector3(0, 0, -1 * speed));
            MovementKey(KeyCode.RightArrow, new Vector3(speed, 0, 0));
            MovementKey(KeyCode.LeftArrow, new Vector3(-1 * speed, 0, 0));
        }

        void MovementKey(KeyCode key, Vector3 velocity) {
            if(Input.GetKeyDown(key)) {
                force.force += velocity;
            }

            if(Input.GetKeyUp(key)) {
                force.force -= velocity;
            }
        }
    }
 }