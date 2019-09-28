using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform lookAt;
    public Transform character;
	public LayerMask layermask;
    public float min;
    public float max;

    private BoxCollider lookAtCollider;
    private Weapons weaponsScript;
    private Camera cam;
    private Vector3 camAdditioner;
    private const float DISTANCE = 2.2f;
	private float mouseX;
	private float mouseY;
    private float SWITCHING_SPEED = 9f;
    private bool isSwitching;
    private bool switchedMarker;
    // Aiming
    private const byte NOT_ZOOM = 0;
    private const float CAM_VERTICAL_NORMAL = 1.2f;
    private const float CAM_HORIZONTAL_DEFAULT = 0.6f;
    private const float CAM_HORIZONTAL_SWITCHED = -0.68f;
    // Clipping
    private const float NORMAL_CLIP_PLANE = 0.05f;
    private const float CLIP_PLANE_FROM_THE_WALL = 0.4f;

    public float getDistance {
        get { return DISTANCE; }
    }
    public bool getswitchedMarker {
        get { return switchedMarker; }
    }

	private void Start() {
        cam = gameObject.GetComponent<Camera>();
        lookAtCollider = lookAt.GetComponent<BoxCollider>();
        weaponsScript = character.GetComponent<Weapons>();
    }

	private void Update() {
        Cursor.visible = false;
		mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y");
        mouseY = Mathf.Clamp(mouseY, min, max);

        lookAt.rotation = Quaternion.Euler(0, mouseX, 0);

        if (Input.GetKeyDown(KeyCode.Q) && weaponsScript.getAimingMarker == NOT_ZOOM) {
            isSwitching = true;
            switchedMarker = switchedMarker ? false : true;

            if (IsInvoking("SetSwitchingOff"))
                CancelInvoke("SetSwitchingOff");

            Invoke("SetSwitchingOff", 1.5f);
        }

        if (isSwitching)
            SwitchCamera();
    }

	private void LateUpdate() {
	    Vector3 offset = new Vector3 (0, 1.8f, -DISTANCE);
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, -DISTANCE);

        transform.position = lookAt.position + rotation * offset;
        transform.LookAt(lookAt.position);

        RaycastHit hit;

        if (Physics.Linecast(character.position + Vector3.up, transform.position, out hit, layermask)) {
            lookAtCollider.enabled = true;

            if (Physics.Linecast(lookAt.position, transform.position, out hit, layermask)) {
                transform.position = hit.point + (transform.InverseTransformDirection(Vector3.forward / 2.5f));
                cam.nearClipPlane = CLIP_PLANE_FROM_THE_WALL;
            }

        } else {
            cam.nearClipPlane = NORMAL_CLIP_PLANE;
            lookAtCollider.enabled = false;
        }
    }

    private void SwitchCamera() {
        float newHorizontalPosition = this.switchedMarker ? CAM_HORIZONTAL_SWITCHED : CAM_HORIZONTAL_DEFAULT;
        lookAt.position = Vector3.Lerp(lookAt.position, character.TransformPoint(new Vector3(newHorizontalPosition, CAM_VERTICAL_NORMAL)), Time.deltaTime * SWITCHING_SPEED);
    }

    private void SetSwitchingOff() {
        this.isSwitching = false;
    }
}
