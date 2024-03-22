using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class FrostWalrusController : ObjecController
{
    private bool ActionGame = false;
    private Vector3 KhoangCach;
    private Vector3 Dir;
    private Animator animator;
    private bool BatTu = false;
    // gào thét
    [Header("gào thét")]
    public bool scream;
    public float TimeToScream;
    //Shoot
    [Header("Shoot")]
    public Transform transhoot2;
    public bool CanShoot = false;
    public int CountShoot;
    public bool Chuan_Bi_Ban = false;
    public int CountHit;
    public float TimeToShoot = 0;
    //Walk
    public bool Walk = false;
    // run
    [Header("run")]
    public bool Run;
    public float TimeToRun;
    public bool CanRun = false;
    public bool CompleteToRun;
    public GameObject RunningState;
    // ice ball
    [Header("Effect")]
    public GameObject Smoker;
    public Transform SmokerDir;
    public GameObject Cloud;
    private float TimeEffect;
    [Header("ice ball")]
    public GameObject IceBall;
    public Transform TransformIceBall;
    private bool Qua_Bong_Bang = true;
    private float TimeIceBall = 0;
    public bool punch = false;
    //Die
    [Header("Die")]
    public bool Die = false;

    void Start()
    {
        TimeToScream = 2f;
        Dir = new Vector3(0, 0, 0);
        animator = GetComponent<Animator>();
        scream = false;
    }
    private void Awake()
    {
        this.RegisterListener(EventID.SwichCamera1, (sender, param) =>
        {
            Dir = new Vector3(-1, 0, 0);
            Walk = true;
            ActionGame = true;
        });
        this.RegisterListener(EventID.ChuyenGiaoTrangThai, (sender, param) => //dam
        {
            scream = false;
            punch = true;
        });
        this.RegisterListener(EventID.CompleteIceBall, (sender, param) => // di lai binh thuong
        {
            Walk = true;
            punch = false;
        });
        this.RegisterListener(EventID.Die, (sender, param) =>
        {
            Die = true;
        });
    }

    void Update()
    {
        if (ActionGame == true && Die == false)
        {
            //bất tử
            if (scream == true || CanShoot == true || Chuan_Bi_Ban == true)
            {
                BatTu = true;
            }
            else
            {
                BatTu = false;
            }
            //tinh khoang cach
            KhoangCach = MegamanController.Instance.transform.position - this.gameObject.transform.position;
            //walk
            WalKing();
            Chay();
            //di chuyen
            if (Run || Walk && Die == false)
            {
                Move(Dir);
            }
            AnimFrost();
            //scream
            GaoThet();
            // shoot
            Ban_Goi_Bang();
            // ice ball
            CreateIceBall();
            if (!scream || !CanShoot)
            {
                Flip();
            }
            Shooting();
            if(CanShoot == true)
            {
                TimeToRun = 0;
                // bo sung time to scream =2f neu bi trung lap hanh dong
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "start pos")
        {
            if (Run == true)
            {
                RunningState.SetActive(false);
                CompleteToRun = true;
                CanRun = false;
            }
            TimeToRun = 0;
        }
        if (collision.gameObject.tag == "end pos")
        {
            if (Run == true)
            {
                RunningState.SetActive(false);
                CompleteToRun = true;
                CanRun = false;
            }
            TimeToRun = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bulletplayer")
        {
            if (BatTu == false && Run == false && Die == false)
            {
                CountHit++;
            }
        }
    }
    //Shoot
    private void Ban_Goi_Bang()
    {   
        if (Walk == true && CanShoot == false)
        {
            TimeToShoot += Time.deltaTime;
        }
        if (TimeToShoot >= 17.5f)
        {
            CanShoot = true;
            TimeToShoot = 0;
        }
        if (CanShoot == true)
        {
            Run = false;
            Walk = false;
        }
        if (CountShoot == 3)
        {
            this.PostEvent(EventID.bang_roi);
            CanShoot = false;
            Walk = true;
            Chuan_Bi_Ban = false;
            CountShoot = 0;
        }
    }
    private void Shooting()
    {
        
        if (CanShoot == true && Chuan_Bi_Ban == false)
        {
            CountShoot++;
            CreateBulletEnemy();
            SmartPool.Instance.Spawn(bullet, transhoot2.position, transhoot2.rotation);
            CanShoot = false;
            Chuan_Bi_Ban = true;
        }
    }
    private void CompleteToShoot()
    {
        CanShoot = true;
        Chuan_Bi_Ban = false;
    }

    public void AnimFrost()
    {
        if (Die == false)
        {
            if (Run)
            {
                animator.Play("run");
            }
            if (Walk)
            {
                animator.Play("walk");
            }
            if (scream)
            {
                animator.Play("gào");
            }

            if (CanShoot == true)
            {
                animator.Play("shoot");
            }
            if (Chuan_Bi_Ban == true)
            {
                animator.Play("ready to shoot");
            }
            if (punch)
            {
                animator.Play("punch");
            }
            if (CompleteToRun)
            {
                animator.Play("complete run");
            }
        }
        else
        {
            animator.Play("die");
        }


    }
    //Walk
    private void WalKing()
    {
        {
            if (Walk == true)
            {
                if (KhoangCach.x < 0)
                {
                    Dir = new Vector3(-1, 0, 0);
                }
                else if (KhoangCach.x > 0)
                {
                    Dir = new Vector3(1, 0, 0);
                }
            }
        }
    }
    public void Chay()
    {
        if (scream == false && punch == false && Die == false && CanShoot == false)
        {
            if(Run == false)
            {   
                TimeToRun += Time.deltaTime;
            }
            if (TimeToRun >= 6)
            {
                CanRun = true;
            }
        }
        if (CanRun == true)
        {
            if ((KhoangCach.x >= -40 || KhoangCach.x <= 40) && Die == false)
            {
                if (scream == false && punch == false)
                {
                    RunningState.SetActive(true);
                    Run = true;
                }
            }
        }
        if (Run == true && scream == false && punch == false)
        {
            Walk = false;
        }
        if (CompleteToRun)
        {
            Run = false;
            Walk = false;
        }
    }
    public void CreateEffect()
    {
        SmartPool.Instance.Spawn(Smoker, SmokerDir.transform.position, SmokerDir.transform.rotation);
        SmartPool.Instance.Spawn(Cloud, SmokerDir.transform.position, SmokerDir.transform.rotation);
    }
    public void CreateIceBall()
    {
        if (scream)
        {
            Walk = false;
            Run = false;

            TimeEffect += Time.deltaTime;
            if (TimeEffect >= 0.2f)
            {
                CreateEffect();
                TimeEffect = 0;
                Qua_Bong_Bang = true;
            }
            if (Qua_Bong_Bang == true)
            {
                TimeIceBall += Time.deltaTime;
                if (TimeIceBall >= 0.9f)
                {
                    SmartPool.Instance.Spawn(IceBall, TransformIceBall.position, TransformIceBall.rotation);
                    TimeIceBall = 0;
                    Qua_Bong_Bang = false;
                }
            }
        }
    }
    private void GaoThet()
    {
        if (scream || punch)
        {
            Walk = false; // gao thi tat di
        }
        if (Walk == true)
        {
            TimeToScream += Time.deltaTime;
            if (TimeToScream >= 10 && CanShoot == false)
            {
                scream = true;
                TimeToScream = 0;
            }
        }
    }

    private void Flip()
    {
        if (Dir.x > 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            this.PostEvent(EventID.quaybenphai);
        }
        else
        {
            this.PostEvent(EventID.quaybentrai);
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void ComplePunch()
    {
        this.PostEvent(EventID.punch);
    }
    private void CompleteRun()
    {
        CompleteToRun = false;
        Walk = true;
        speed = 5;
    }
    private void AddSpeed()
    {
        speed = 20;
    }
}

