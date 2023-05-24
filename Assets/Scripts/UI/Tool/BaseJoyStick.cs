using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace BFramework.UI
{
    public class BaseJoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public UnityAction<Vector2> onDragStick = delegate { };
        public UnityAction onStopDrag = delegate { };


        [Header("������")]
        [SerializeField] RectTransform BG;
        [SerializeField] RectTransform stick;
        //
        [Header("��������")]
        [SerializeField] float BGRadius;

        //
        [Header("����")]
        [SerializeField] float deadZone;
        //[SerializeField] bool isFlipX;
        //[SerializeField] bool isFlipY;
        
        //
        [Header("���")]
        [SerializeField] Vector2 input;
        //
        [Header("�����Ĭ��main���")]
        [SerializeField] new Camera camera;
        Vector2 fingerPos;
        Vector2 formerInput;

        private void Awake()
        {
            if (!camera)
                camera = Camera.main;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            isDown = true;
            fingerPos = eventData.position;
        }
        public void OnDrag(PointerEventData eventData)
        {
            fingerPos = eventData.position;
        }
        bool isDown = false;
        private void Update()
        {
            if (isDown)
            {
                input = fingerPos - (Vector2)BG.position;
                //
                Normalize();
                SetHandle();
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isDown = false;
            input = Vector2.zero;
            if (formerInput != Vector2.zero)
            {
                formerInput = input;
                onStopDrag.Invoke();
            }
            SetHandle();
        }
        void Normalize()
        {
            if (input.sqrMagnitude < deadZone * deadZone)
            {
                input = Vector2.zero;
                if (formerInput != Vector2.zero)
                {
                    formerInput = input;
                    onStopDrag.Invoke();
                }
            }
            else
            {
                if (input.sqrMagnitude > BGRadius * BGRadius)
                {
                    input = input.normalized;
                }
                else
                {
                    input = input / BGRadius;
                }
                formerInput = input;
                onDragStick.Invoke(input);
            }
        }
        void SetHandle()
        {
            stick.localPosition = input * BGRadius;
        }
    }
}

