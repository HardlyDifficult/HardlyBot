using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hardly.Unity {
    public class WorldBoundHelpers {
        GameObject worldBounds;
        public WorldBoundHelpers(GameObject worldBounds) {
            this.worldBounds = worldBounds;
        }

        public Vector3 RandomLocationInBounds() {
            return new Vector3(
                    Hardly.Random.Uint.LessThan((uint)(worldBounds.transform.localScale.x * 18f)) + 10
                    - worldBounds.transform.localScale.x * 10f
                    ,
                    Hardly.Random.Uint.LessThan((uint)(worldBounds.transform.localScale.y * 10f)) + 100
                    ,
                    Hardly.Random.Uint.LessThan((uint)(worldBounds.transform.localScale.y * 18f)) + 10
                    - worldBounds.transform.localScale.z * 10f
                    );
        }
    }
}
