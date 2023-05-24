using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseControl : MonoBehaviour
{
    protected BaseBrain brain;

    protected Transform model;
    protected Transform foot1;
    protected Transform foot2;

    protected Rigidbody2D rb;
    protected Animator animator;

    [SerializeField] protected Image HPImage;
    [SerializeField] protected Image webImage;

    protected virtual void Awake()
    {
        brain = GetComponentInChildren<BaseBrain>();

        model = transform.GetChild(0);
        //body = model.Find("Body");
        //eye = model.Find("eye");
        foot1 = model.Find("Foot1");
        foot2 = model.Find("Foot2");

        rb = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }
    public virtual void Initialize()
    {

    }
    public virtual void Open()
    {

    }
    public virtual void Close()
    {

    }
}
