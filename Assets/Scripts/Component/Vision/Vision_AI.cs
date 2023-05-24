using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vision_AI : Vision_Men
{
    public UnityAction onHalfOnGround = delegate { };
    public UnityAction<Transform> onFindPlayer = delegate { };
    public UnityAction onLostPlayer = delegate { };
    public UnityAction<int> onBelowGround = delegate { };
    //
    Transform eye;

    bool isOrientRight;
    bool isPlayerVisiable;

    Transform player;

    [SerializeField] float height;
    
    [SerializeField] float eyeRayLength;
    [SerializeField] float eyeRayAngle;
    float cosEyeAngle;
    [SerializeField] float chaseRange;
    [SerializeField] float headRayLength;
    float sqrVisionRange;
    //
    LayerMask playerLayer;
    LayerMask eyeLayer;

    public override void Initialize<T0, T1,T2>(T0 arg0, T1 arg1,T2 t2)
    {
        foot1 = arg0.transform;
        foot2 = arg1.transform;
        eye = t2.transform;
        //
        sqrVisionRange = chaseRange * chaseRange;
        cosEyeAngle = Mathf.Cos(eyeRayAngle);
        //
        collisionLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player");
        eyeLayer = LayerMask.GetMask("Player", "Ground");
    }
    public override void Open()
    {
        base.Open();
        isOrientRight = true;
        isPlayerVisiable = false;
    }
    protected override void Update()
    {
        CheckOnGround();
        CheckFindPlayer();
        CheckBelowGround();
    }
    protected override void CheckOnGround()
    {
        base.CheckOnGround();

        var hit1 = Physics2D.Raycast(foot1.position, Vector2.down, footRayLength, collisionLayer);
        var hit2 = Physics2D.Raycast(foot2.position, Vector2.down, footRayLength, collisionLayer);
        if (hit1 && !hit2 && isOrientRight)
        {
            onHalfOnGround.Invoke();
        }
        else if (!hit1 && hit2 && !isOrientRight)
        {
            onHalfOnGround.Invoke();
        }
    }
    void CheckFindPlayer()
    {
        if (isPlayerVisiable)
            return;
        var hit0 = Physics2D.OverlapCircle(eye.position, eyeRayLength, playerLayer);
        if (!hit0)
            return;
        Vector2 ori = (hit0.transform.position - eye.position).normalized;
        if(Vector2.Dot(ori, (isOrientRight ? Vector2.right : Vector2.left)) > cosEyeAngle)
        {
            //player is in eyeAngle
            var hit = Physics2D.Raycast(eye.position, ori, eyeRayLength, eyeLayer);
            if (hit && hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                isPlayerVisiable = true;
                player = hit.transform;
                onFindPlayer.Invoke(player.transform);
                StopCoroutine("CheckPlayer");
                StartCoroutine("CheckPlayer");
            }
        }
    }
    IEnumerator CheckPlayer()
    {
        while (isPlayerVisiable)
        {
            Vector3 minus = player.position - eye.position;
            if (minus.sqrMagnitude > sqrVisionRange)
            {
                isPlayerVisiable = false;
                onLostPlayer.Invoke();
            }
            yield return null;
        }
    }
    void CheckBelowGround()
    {
        var hit1 = Physics2D.Raycast(foot1.position+new Vector3(0,height,0), Vector2.up, headRayLength, collisionLayer);
        var hit2 = Physics2D.Raycast(foot2.position + new Vector3(0, height, 0), Vector2.up, headRayLength, collisionLayer);
        if (!hit1 && !hit2)
        {
            onBelowGround.Invoke(0);
        }
        else if (hit1 && hit2)
        {
            onBelowGround.Invoke(2);
        }
        else if (hit1)
        {
            onBelowGround.Invoke(-1);
        }
        else if (hit2)
        {
            onBelowGround.Invoke(1);
        }
    }
}

