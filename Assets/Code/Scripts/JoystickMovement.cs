using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickMovement : MonoBehaviour
{
    public GameObject joystick;
    public GameObject joystickBG;
    public Vector2 joystickVector;
    private Vector2 joystickTouchPos;
    private Vector2 joystickOriginalPos;
    private float joystickRadius;

    void Start()
    {
        joystickOriginalPos = joystick.transform.position;
        joystickRadius = joystickBG.GetComponent<RectTransform>().sizeDelta.y / 4;
    }

    public void PointerDown() {
        joystick.transform.position = Input.mousePosition;
        joystickBG.transform.position = Input.mousePosition;
        joystickTouchPos = joystick.transform.position;
    }

    public void Drag(BaseEventData baseEventData) {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;
        joystickVector = (dragPos - joystickTouchPos).normalized;

        float joystickDist = Vector2.Distance(dragPos, joystickTouchPos);

        if(joystickDist < joystickRadius) {
            joystick.transform.position = joystickTouchPos + joystickVector * joystickDist;
        } else {
            joystick.transform.position = joystickTouchPos + joystickVector * joystickRadius;
        }
    }

    public void PointerUp() {
        joystickVector = Vector2.zero;
        joystick.transform.position = joystickOriginalPos;
    }

}
