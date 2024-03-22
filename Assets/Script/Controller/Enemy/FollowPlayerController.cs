using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class FollowPlayerController : ObjecController
{
    [Header("Enemy follow ")]
    protected bool isHitBullet = false;
    private float timeCooldown;
    public Animator animator;
    private Vector3 direc;
    public Transform check;
    public LayerMask layermask;
    //attack
    [Header(" Attack")]
    public Transform checkAttack;
    public float distanceAttack;
    private bool CheckAttack;
    public bool run;

    [Header(" Speed")]
    public bool SpeedEnemy;
    private bool CheckPlayer_Right;
    private bool CheckPlayer_Left;
    public float distanceRight;
    public float distanceLeft;

    private float TimeAttack;
    public bool CanAttack = true;
    private bool CanCheck = true;
    // patrolling
    [Header(" Patrolling")]
    private float distance;
    public GameObject DiemA;
    public float TimeComback = 0;
    private void Start()
    {
        SpeedEnemy = false;
        run = true;
        animator = GetComponent<Animator>();
        direc = new Vector3(1, 0, 0);
    }
    void Update()
    {
        SetAnim();
        Flip();
        //move
        CheckMove();
        Move(direc);
        if (CheckPlayer_Left || CheckPlayer_Right)
        {
            SpeedEnemy = true;
        }
        else
        {
            SpeedEnemy = false;
        }

        if ((CheckPlayer_Left || CheckPlayer_Right) && SpeedEnemy == true && CheckAttack == false) //thấy player và player trong tầm đánh
        {
            if (CheckPlayer_Left == true)
            {
                direc = new Vector3(-1, 0, 0);
            }
            else
            {
                direc = new Vector3(1, 0, 0);
            }
            speed = 15;
            run = false;
        }
        else if ((!CheckPlayer_Left || !CheckPlayer_Right) && SpeedEnemy == false && CheckAttack == false && run == false) // không thấy player và run = false
        {
            Debug.Log("di ve a");
            run = true;
            SpeedEnemy = false;
            float kc = this.gameObject.transform.position.x - DiemA.transform.position.x;
            if (kc > 0)
            {
                direc = new Vector3(-1, 0, 0);
            }
            else
            {
                direc = new Vector3(1, 0, 0);
            }
            speed = 5;
        }
        else if (CheckAttack == true) // player trong tầm đánh thì dừng lại
        {
            direc = new Vector3(0, 0, 0);
        }
        //StopEnemy();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(check.position, new Vector3((check.position.x + distanceRight * this.gameObject.transform.localScale.x),
                                                    check.position.y,
                                                    check.position.z));
        Gizmos.DrawLine(check.position, new Vector3((check.position.x + distanceLeft * this.gameObject.transform.localScale.x),
                                                    check.position.y,
                                                    check.position.z));

        Gizmos.DrawWireSphere(checkAttack.position, distanceAttack);

    }
    public void Flip()
    {
        if (direc.x != 0)
        {
            if (direc.x < 0)
            {
                this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direc.x > 0)
            {
                this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bulletplayer")
        {
            isHitBullet = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Diem 1")
        {
            if (SpeedEnemy == false)
            {
                this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
                direc = new Vector3(1, 0, 0);
                run = true;
            }
        }
        if (collision.gameObject.tag == "Diem 2")
        {
            if (SpeedEnemy == false)
            {
                this.gameObject.transform.localScale = new Vector3(1, 1, 1);
                direc = new Vector3(-1, 0, 0);
                run = true;
            }
        }
    }
    public void SetAnim()
    {
        animator.SetFloat("run", Mathf.Abs(direc.x));
        animator.SetBool("attack", CheckAttack);
        animator.SetBool("speed", SpeedEnemy);
        animator.SetBool("DuocPhepDanh", CanAttack);
    }
    public void AttackCooldown()
    {
        CanAttack = false;
        CheckAttack = false;
    }

    private void CheckMove()
    {
        CheckPlayer_Right = Physics2D.Raycast(check.position, Vector2.right, distanceRight, layermask);
        CheckPlayer_Left = Physics2D.Raycast(check.position, Vector2.right, distanceLeft, layermask);

        if (CanAttack == true)
        {
            CheckAttack = Physics2D.OverlapCircle(checkAttack.position, distanceAttack, layermask);
        }
        else
        {
            TimeAttack += Time.deltaTime;
            if (TimeAttack >= 3)
            {
                CanAttack = true;
                TimeAttack = 0;
            }
        }


        //public void StopEnemy()
        //{
        //    if (isHitBullet == true)
        //    {
        //        direc = new Vector3(0, 0, 0);
        //        SpeedEnemy = false;
        //        run = false;
        //        CanCheck = false;
        //        TimeComback += Time.deltaTime;
        //        if (TimeComback >= 1.2f)
        //        {
        //            isHitBullet = false;
        //            CanCheck = true;
        //            SpeedEnemy = true;
        //            run = true;
        //        }
        //    }
        //}
    }
}
