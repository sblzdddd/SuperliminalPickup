using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SuperEnginal : MonoBehaviour
{
    public GameObject player;
    public GameObject playerObj;
    public LayerMask targetMask;
    public LayerMask collisionMask;
    public int enabledLayer;
    [Range(10,1000)]
    public float maxPickUpDistance;
    [Range(50,1000)]
    public float maxScaleDistance;
    [Range(8,256)]
    public int sample;
    public bool enableLerp;
    [Range(0,5)]
    public float lerpTime;
 
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public bool Recycle;
    float originalDistance;
    Vector3 originalSize;
    Vector3 originalBoundSize;
    float originalYRotation;
    private FPSController pcs;
    private float initialSens;
    private Vector3 startDirectionOffset;
    private float lerpStart;
    private bool isLerping;
    private int originalLayer;
    private Quaternion rotOffset;
    const float pi = 3.141592653589793238f;

    void Start(){pcs = player.GetComponent<FPSController>();initialSens = pcs.mouseSensitivity;}
    void Update() {HandleInput();}
    void LateUpdate() {ResizeTarget();}
 
    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (target == null) {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, maxScaleDistance, targetMask)) {
                    lerpStart = Time.time;
                    target = hit.transform;
                    Physics.IgnoreCollision(target.GetComponent<Collider>(), playerObj.GetComponent<Collider>(), true);
                    startDirectionOffset = (target.position - transform.position).normalized - transform.forward;
                    target.GetComponent<Rigidbody>().isKinematic = true;
                    originalDistance = Vector3.Distance(transform.position, target.position);
                    originalSize = target.localScale;
                    var bound = target.GetComponent<Collider>().bounds.size;
                    originalBoundSize = new Vector3(bound.x, bound.z, bound.y);
                    originalYRotation = target.localEulerAngles.y - transform.localEulerAngles.y;
                    // rotOffset = Quaternion.inverse(Quaternion.Inverse(transform.rotation) * transform.rotation);
                    originalLayer = target.gameObject.layer;
                    target.gameObject.layer = enabledLayer;
                    isLerping = enableLerp;
                }
            } else {
                target.GetComponent<Rigidbody>().isKinematic = false;
                Physics.IgnoreCollision(target.GetComponent<Collider>(), playerObj.GetComponent<Collider>(), false);
                pcs.mouseSensitivity = initialSens;
                target.gameObject.layer = originalLayer;
                target = null;
            }
        }
        if (Input.GetMouseButton(1) && target != null) {RotateObj();} else if (target != null) {pcs.mouseSensitivity = initialSens;}
        if (Input.GetMouseButtonUp(1) && target != null) {originalYRotation = target.localEulerAngles.y - transform.localEulerAngles.y;}
    }

    private float OutSine(float x) {
        if (x > 1f) {return 1f;}
        return Mathf.Sin(0.5f * pi * x);
    }

    private void ResizeTarget()
    {
        if (target == null) {return;}
        Vector3 targetPos = BoxCast(transform.forward);
        if (isLerping) {
            float lerpPercentage = (Time.time - lerpStart) / lerpTime;
            if (lerpPercentage > 1) {lerpPercentage = 1;isLerping = false;}
            Vector3 startPos = BoxCast(transform.forward + startDirectionOffset);
            float progress = OutSine(lerpPercentage);
            target.position = Vector3.Lerp(startPos, targetPos, progress);
        } else {
            target.position = targetPos;
        }
        target.localScale = CalcScale(target.position, originalSize);
        // target.rotation = transform.rotation * rotOffset;
        // if (!Input.GetMouseButton(1))
            // target.localEulerAngles = new Vector3(target.localEulerAngles.x, originalYRotation + transform.localEulerAngles.y, target.localEulerAngles.z);
    }

    private Vector3 CalcScale(Vector3 tarPos, Vector3 originalSize) {
        float currentDistance = Vector3.Distance(transform.position, tarPos);
        float scale = currentDistance / originalDistance;
        return scale * originalSize;
    }

    private void RotateObj() {
        pcs.mouseSensitivity = 0;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * initialSens * 10f;
        target.localEulerAngles = new Vector3(target.localEulerAngles.x, target.localEulerAngles.y - mouseX, target.localEulerAngles.z);
    }

    private Vector3 BoxCast(Vector3 Direction)
    {
        Vector3 boxCenter = new Vector3(0, 0, 0);
        Vector3 boxSize;
        boxCenter = Direction * 0.5f + transform.position;
        Collider[] collider = new Collider[0];
        while (collider.Length == 0) {
            boxCenter += Direction * (1f / sample);
            boxSize = CalcScale(boxCenter, originalBoundSize) * 0.5f;
            collider = Physics.OverlapBox(boxCenter, boxSize, target.rotation, collisionMask);
            if (Vector3.Distance(transform.position, boxCenter) > maxScaleDistance)
                return Direction * maxScaleDistance + transform.position;
        }
        while (collider.Length != 0) {
            boxCenter -= Direction * (1f / sample);
            boxSize = CalcScale(boxCenter, originalBoundSize) * 0.5f;
            collider = Physics.OverlapBox(boxCenter, boxSize, target.rotation, collisionMask);
        }
        return boxCenter;
    }
}
