using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool CoChiaKhoa;
    public bool MoCua;
    private Vector3 Direc;
    private Rigidbody2D rb;
    public float TocDo;
    private float TimeCloseDoor;
    private Vector3 KhoangCach;
    private void Awake()
    {
        this.RegisterListener(EventID.ChiaKhoaMoCua, (sender, param) =>
        {
            CoChiaKhoa = true;
        });
        this.RegisterListener(EventID.SwichCamera1, (sender, param) =>
        {
            rb.gravityScale = 1;
            MoCua = false;
        });
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MoCua = false;
        CoChiaKhoa = false;
    }
    private void Update()
    {
        KhoangCach = this.gameObject.transform.position - MegamanController.Instance.transform.position;
        if (KhoangCach.x <= 3 && CoChiaKhoa)
        {
            MoCua = true;
            CoChiaKhoa = false;
            rb.gravityScale = 0;
        }
        if (MoCua == true)
        {
            OpenDoor();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "san")
        {
            TimeCloseDoor = 0;
        }

    }
    private void OpenDoor()
    {
        this.gameObject.transform.position += new Vector3(0, 1, 0) * TocDo * Time.deltaTime;
    }
}
