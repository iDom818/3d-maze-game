using UnityEngine;

public class GravityController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Physics.gravity = new Vector3(0, 9.8f, 0);
            GetComponent<MouseLook>().originalRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            Physics.gravity = new Vector3(0, -9.8f, 0);
            GetComponent<MouseLook>().originalRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Physics.gravity = new Vector3(9.8f, 0, 0);
            GetComponent<MouseLook>().originalRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Physics.gravity = new Vector3(-9.8f, 0, 0);
            GetComponent<MouseLook>().originalRotation = Quaternion.Euler(new Vector3(0, 0, 270));
        } else if (Input.GetKeyDown(KeyCode.PageUp)) {
            Physics.gravity = new Vector3(0, 0, -9.8f);
            GetComponent<MouseLook>().originalRotation = Quaternion.Euler(new Vector3(90, 0, 0));
        } else if (Input.GetKeyDown(KeyCode.PageDown)) {
            Physics.gravity = new Vector3(0, 0, 9.8f);
            GetComponent<MouseLook>().originalRotation = Quaternion.Euler(new Vector3(270, 0, 0));
        }
    }
}
