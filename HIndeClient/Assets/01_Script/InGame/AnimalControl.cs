﻿using UnityEngine;
using System.Collections;

public class AnimalControl : MonoBehaviour
{
    /*
     애니메이션 상황
     * Walk(Idle)
     * RunReady
     * Run // Hang하다가도 먹으면 자연스럽게 공으로 올라오도록
     * Flip
     * Jump
     * Hang
     * HangJump
     * Die
    */
    //Data변수 추가

    public bool IsStart { get; set; }
    public bool IsUp { get; set; }
    public bool IsJumping { get; set; }
    public bool IsPower { get; set; }
    public bool IsRunning { get; set; }
    Animator anim;
    Rigidbody2D rigid;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();
        IsUp = true;

        //temp
        anim.Play("Walk");
    }

    public void Flip()
    {
        if (IsJumping) return;

        if (IsUp)
        {
            //temp
            transform.localRotation = Quaternion.identity;
            transform.Rotate(180, 0, 0);
            rigid.gravityScale = -0.5f;
            IsUp = false;
        }
        else
        {
            transform.localRotation = Quaternion.identity;
            rigid.gravityScale = 0.5f;
            IsUp = true;
        }
    }

    public void Jump()
    {
        if (IsJumping) return;

        IsJumping = true;
        rigid.velocity = new Vector2(0, IsUp ? 2 : -2);

        //temp
        anim.SetTrigger("Jump");
    }

    public void Running()
    {
        //temp
        anim.SetTrigger("Run");
    }

    public void Die()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.collider.name + " / " + col.collider.tag);
        switch (col.collider.tag)
        {
            case "Ground":
                OnCollide_Ground();
                break;
        }
    }

    void OnCollide_Ground()
    {
        IsJumping = false;

        //temp
        anim.Play("Walk");
    }

    void OnCollide_Get()
    {
        //switch(Object type)
    }
}
