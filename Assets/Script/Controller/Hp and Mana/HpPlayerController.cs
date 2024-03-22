using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using static UnityEditorInternal.ReorderableList;

public class HpPlayerController : SetHpManaController
{
    public float MaxHealth;
    public float CurrentHp;
    private float SaveCrutentHP = 500;
    private float TimeFireCoolDown;
    private void Awake()
    {     
        this.RegisterListener(EventID.ThoatGame, (sender, param) =>
        {
            Debug.Log("xoa key");
            PlayerPrefs.DeleteKey("hp");
        });
        this.RegisterListener(EventID.HoiManaVaMau, (sender, param) =>
        {
            HutMau();
        });
        this.RegisterListener(EventID.Heart, (sender, param) =>
        {
            Debug.Log("Tangmau");
            CurrentHp += 200;
        });
        this.RegisterListener(EventID.QuaMan, (sender, param) =>
        {
            SaveCrutentHP = CurrentHp;
            PlayerPrefs.SetFloat("hp", SaveCrutentHP);
        });
    }
    private void Start()
    {

        if (PlayerPrefs.HasKey("hp"))
        {
            SaveCrutentHP = PlayerPrefs.GetFloat("hp");
            CurrentHp = SaveCrutentHP;
        }
        else
        {
            CurrentHp = MaxHealth;
        }

        SetMaxIndex(MaxHealth);
        SetIndex(CurrentHp);
    }
    private void Update()
    {
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            Destroy(this.gameObject);
            SceneManager.LoadScene("Loser Game");
            Debug.Log("death");
        }
    }
    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
        SetIndex(CurrentHp);
        this.PostEvent(EventID.GetHit);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage(25);
        }
        if (collision.gameObject.tag == "saw")
        {
            TakeDamage(50);
        }
        if (collision.gameObject.tag == "dan enemy")
        {
            TakeDamage(35);
        }
        if (collision.gameObject.tag == "gai")
        {
            TakeDamage(30);
        }
        if (collision.gameObject.tag == "bom")
        {
            TakeDamage(10);
            TimeFireCoolDown++;
            if (TimeFireCoolDown <= 100)
            {
                if (TimeFireCoolDown % 2 == 0)
                    TakeDamage(2);
            }
            else
            {
                TimeFireCoolDown = 0;
                return;
            }
        }
        if (collision.gameObject.tag == "Boss")
        {
            TakeDamage(60);
        }
        if (collision.gameObject.tag == "bos")
        {
            TakeDamage(50);
        }
    }
    public void HutMau()
    {
        CurrentHp += 5;
    }

}
