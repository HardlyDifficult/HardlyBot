using UnityEngine;
using System.Collections;
namespace Hardly.Unity {

    public class SpawDuplicates : MonoBehaviour {

        [SerializeField]
        private GameObject objectToDupe;

        [SerializeField]
        private GameObject worldBounds;

        // Use this for initialization
        void Start() {
            var bounds = new WorldBoundHelpers(worldBounds);
            for(int i = 0; i < 100; i++) {
                var newBall = Instantiate(objectToDupe);
                newBall.transform.localPosition = bounds.RandomLocationInBounds();
            }
        }

        // Update is called once per frame
        void Update() {

        }
    }
}