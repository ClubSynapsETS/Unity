using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    CharacterController cc;
    Rigidbody RB;

    [SerializeField]
    float movementSpeed = 4.5f;

    void Awake() {
        cc = this.GetComponent<CharacterController>();
        RB = this.GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        LockMouse();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape)) UnlockMouse();
	}

    // Update is called once per frame
    void FixedUpdate() {
        Control();
    }

    public void Control() {
        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");

        float aimingX = Input.GetAxis("Mouse X");
        float aimingZ = Input.GetAxis("Mouse Y");
        //Debug.Log ("X : " + movementX.ToString("#.##") + "  |  Z : " + movementZ.ToString("#.##"));
        //Debug.Log ("X : " + aimingX.ToString("#.##") + "  |  Z : " + aimingZ.ToString("#.##"));

        RB.angularVelocity = new Vector3(aimingZ, aimingX, 0f);
        //RB.velocity = new Vector3(-Mathf.Sin(transform.localEulerAngles.y * Mathf.Deg2Rad) * movementX, 0, Mathf.Cos(transform.localEulerAngles.y * Mathf.Deg2Rad) * movementZ) * movementSpeed;
        Vector3 desiredMove = transform.forward * movementZ + transform.right * movementX * 0.5f;
        cc.Move(desiredMove.normalized * movementSpeed * Time.fixedDeltaTime);

        
    }

    public void LockMouse() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockMouse() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (cc.collisionFlags == CollisionFlags.Below) {
            return;
        }

        if (body == null || body.isKinematic) {
            return;
        }
        body.AddForceAtPosition(cc.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }

}
