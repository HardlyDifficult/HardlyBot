using UnityEngine;
using System.Collections;

public class ObjectFollower : MonoBehaviour {

    [SerializeField]
    private GameObject objectToFollow;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.localPosition = objectToFollow.transform.localPosition;
	}
}
