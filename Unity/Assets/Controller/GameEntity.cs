using System;
using UnityEngine;

namespace Hardly.Unity {
    public delegate void UpdateHandler();

    public class GameEntity : MonoBehaviour {
        public event UpdateHandler OnUpdate;

        public virtual void Update() {
            if(OnUpdate != null) {
                OnUpdate();
            }
        }
    }
}
