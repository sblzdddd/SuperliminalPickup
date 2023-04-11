using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public GameObject tooptipText;
    public GameObject cursorDefault;
    public GameObject cursorHand;
    public GameObject cursorGrabbed;
    private SuperEnginal superEnginal;
    private bool haveOutline = false;
    private GameObject target;
    void Start() {
        superEnginal = GetComponent<SuperEnginal>();
    }

    void Update() {
        if (superEnginal.target != null) {
            target = null;
            SuperProp sp = superEnginal.target.GetComponent<SuperProp>();
            if (sp != null) {
                if (sp.outline) {
                    Outline OutLine = superEnginal.target.GetComponent<Outline>();
                    OutLine.OutlineColor = sp.holdColor;
                    OutLine.enabled = true;
                }
            }
            tooptipText.GetComponent<CanvasGroup>().alpha = 0;
            cursorDefault.GetComponent<CanvasGroup>().alpha = 0;
            cursorHand.GetComponent<CanvasGroup>().alpha = 0;
            cursorGrabbed.GetComponent<CanvasGroup>().alpha = 1;
        } else {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, superEnginal.maxScaleDistance, superEnginal.targetMask)) {
                if (target == null) {
                    target = hit.transform.gameObject;
                    SuperProp sp = target.GetComponent<SuperProp>();
                    if (sp != null) {
                        Outline OutLine = target.GetComponent<Outline>();
                        haveOutline = sp.outline && OutLine != null;
                        if (haveOutline) {
                            OutLine.OutlineColor = sp.pointedColor;
                            OutLine.enabled = true;
                        }
                        tooptipText.GetComponent<TextMeshProUGUI>().text = sp.objectName;
                        tooptipText.GetComponent<CanvasGroup>().alpha = 1;
                        cursorDefault.GetComponent<CanvasGroup>().alpha = 0;
                        cursorHand.GetComponent<CanvasGroup>().alpha = 1;
                        cursorGrabbed.GetComponent<CanvasGroup>().alpha = 0;
                    }
                }
            } else {
                if (target != null && haveOutline) {
                    target.GetComponent<Outline>().enabled = false;
                    haveOutline = false;
                }
                tooptipText.GetComponent<CanvasGroup>().alpha = 0;
                cursorDefault.GetComponent<CanvasGroup>().alpha = 1;
                cursorHand.GetComponent<CanvasGroup>().alpha = 0;
                cursorGrabbed.GetComponent<CanvasGroup>().alpha = 0;
                target = null;
            }
        }
    }
}
