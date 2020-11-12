using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorControl : MonoBehaviour
{
    public Transform normalPTR, validPTR, invalidPTR;

    private Vector3 mPos;
    private bool changePointer = true, isValid = false;
    private string pointerMode = "normal";
    private int roomX, roomY;

    void Awake()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        roomX = Map.MAP.room[GameManager.GAME.mazeX, GameManager.GAME.mazeY].width; roomY = Map.MAP.room[GameManager.GAME.mazeX, GameManager.GAME.mazeY].height;
        mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = new Vector3(mPos.x, mPos.y, Input.mousePosition.z);

        if (changePointer && pointerMode == "normal")
        {
            invalidPTR.gameObject.SetActive(false);
            normalPTR.gameObject.SetActive(true);
            validPTR.gameObject.SetActive(false);
            changePointer = false;
        }
        if (changePointer && pointerMode == "validate")
        {
            if(Mathf.RoundToInt(mPos.x) < 1 || Mathf.RoundToInt(mPos.x) > roomX-1 || Mathf.RoundToInt(mPos.y) < 1 || Mathf.RoundToInt(mPos.y) > roomY-1)
            {
                invalidPTR.gameObject.SetActive(true);
                normalPTR.gameObject.SetActive(false);
                validPTR.gameObject.SetActive(false);
                isValid = false;
            }
            else
            {
                invalidPTR.gameObject.SetActive(false);
                normalPTR.gameObject.SetActive(false);
                validPTR.gameObject.SetActive(true);
                isValid = true;
            }
            if (Input.GetButton("Fire1"))
            {
                if (isValid)
                {
                    GameManager.GAME.mousePos = new Vector2(Mathf.RoundToInt(mPos.x), Mathf.RoundToInt(mPos.y));
                    GameManager.GAME.mousePosRaw = new Vector2(mPos.x, mPos.y);
                    GameManager.GAME.mPosValidated = true;
                }
                pointerMode = "normal";
            }
        }
    }

    public void EnterValidateMode()
    {
        pointerMode = "validate";
        changePointer = true;
    }
}
