using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InputManager : Single<InputManager>
{
    [SerializeField] bool canMouseInput = true;
    
    [SerializeField] Vector3 minTouchPort;
    
    [SerializeField] Vector3 maxTouchPort = new Vector3(1920, 1080, 0);

    [Range(0,int.MaxValue)]
    [SerializeField] float dragOffset=100f;

    [SerializeField] float doubleClickTime;


    public bool CanInput
    {
        get { return canMouseInput; }
        set
        {
            canMouseInput = value;
            lastPressPoint = Input.mousePosition;
        }
    }

    public static UnityAction<Vector3, Vector3> onDrag = delegate { };
    public static UnityAction<Vector3, Vector3> onDragEnd = delegate { };
    public static UnityAction<Vector3> onClick = delegate { };
    public static UnityAction<Vector3> onDoubleClick = delegate { };
    public static UnityAction<Vector3> onStayTouchPortEdge = delegate { };
    //
#if UNITY_STANDALONE_WIN
    public static UnityAction onClickW = delegate { };
    public static UnityAction onPressA = delegate { };
    public static UnityAction onPressD = delegate { };
    public static UnityAction onRedoPressA = delegate { };
    public static UnityAction onRedoPressD = delegate { };
    public static UnityAction onClickP = delegate { };
    public static UnityAction onClickL = delegate { };
#endif
    //
    float lastClickTime;
    Vector3 lastPressPoint;
    bool isPressValid;



    private void Update()
    {
        if (!canMouseInput)
        {
            return;
        }
        CheckMouse();
#if UNITY_STANDALONE_WIN
        CheckKeyboard();
#endif
    }
    void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0) && TouchPortCheck(Input.mousePosition))
        {
            //DoubleClick
            isPressValid = true;
            if ((lastPressPoint - Input.mousePosition).sqrMagnitude > dragOffset)
            {
                lastClickTime = 0;
            }
            lastPressPoint = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && TouchPortCheck(Input.mousePosition) && isPressValid)
        {
            isPressValid = false;
            if ((lastPressPoint - Input.mousePosition).sqrMagnitude < dragOffset)
            {
                if (lastClickTime != 0 && Time.time - lastClickTime < doubleClickTime)
                {
                    onDoubleClick.Invoke(Input.mousePosition);
                    lastClickTime = 0;
                }
                else
                {
                    onClick.Invoke(Input.mousePosition);
                    lastClickTime = Time.time;
                }
            }
            else
            {
                onDragEnd.Invoke(lastPressPoint, Input.mousePosition);
            }
        }
        if (!Input.GetMouseButton(0))
        {
            if (TouchPortCheck(Input.mousePosition))
            {
                var a = maxTouchPort - Input.mousePosition;
                var b = Input.mousePosition - minTouchPort;
                Vector3.Min(a, b);
            }
        }
        else
        {
            if ((lastPressPoint - Input.mousePosition).sqrMagnitude > dragOffset)
            {
                onDrag.Invoke(lastPressPoint, Input.mousePosition);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {

        }
    }
    void CheckKeyboard()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            onClickW.Invoke();
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            onPressA.Invoke();
        }
        else
        {
            onRedoPressA.Invoke();
        }
        if (Input.GetKey(KeyCode.D))
        {
            onPressD.Invoke();
        }
        else
        {
            onRedoPressD.Invoke();
        }
        
    }
    bool TouchPortCheck(Vector3 mousePos)
    {
        return mousePos.x > minTouchPort.x && mousePos.x < maxTouchPort.x && mousePos.y > minTouchPort.y && mousePos.y < maxTouchPort.y;
    }
}
