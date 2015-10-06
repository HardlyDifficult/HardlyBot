using UnityEngine;
using System.Collections;

namespace Hardly.Unity {
    public class HideObject : MonoBehaviour {

        [SerializeField]
        private GameObject worldBounds;

        // Use this for initialization
        void Start() {
            var bounds = new WorldBoundHelpers(worldBounds);
            transform.position = bounds.RandomLocationInBounds();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}