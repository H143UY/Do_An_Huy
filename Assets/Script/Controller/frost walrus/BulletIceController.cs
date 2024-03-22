using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletIceController : MonoBehaviour
{
    public Animator anim;
    public bool vacham = false;
    public float TimeDestroy;
    void Start()
    {
        anim =  GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(vacham)
        {
            anim.SetTrigger("vacham");
            TimeDestroy += Time.deltaTime;
        }
        if(TimeDestroy >= 1.5f)
        {
            SmartPool.Instance.Despawn(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "san")
        {
            vacham = true;
        }
    }
}
