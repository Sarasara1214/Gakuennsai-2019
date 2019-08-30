﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //変数定義
    bool runnable = true;
    public float flap = 500f;
    Rigidbody2D rb2d;
    bool isGround = true; //trueであればジャンプ可能
    bool stst; //リスポーン時にfalseで止まる
    int coinCnt=0; //コインの枚数
    private bool isMuteki;
    [SerializeField]
    private ItemController itemCtl;

    KinectController kinect;
    // Use this for initialization
    void Start()
    {
        //コンポーネント読み込み
        rb2d = GetComponent<Rigidbody2D>();
        stst = true;
        kinect = new KinectController();
    }


    // Update is called once per frame
    void Update()
    {
        kinect.KinectUpdate();
        if (stst)
        {
            if (runnable)
            {
                //自動で移動
                this.gameObject.transform.Translate(0.1f, 0, 0);
            }
            else
            {
                Debug.Log("停止中");
            }
     
        }

        //ジャンプ判定
        if ((Input.GetKeyDown("space") || kinect.getJumping()) && isGround && rb2d.velocity.y == 0)
        {
            isGround = false;
            rb2d.AddForce(Vector2.up * flap);
        }

        //Debug.Log(isGround);

        if (rb2d.velocity.y < 0) //ジャンプ制御
        {
            Vector2 v = rb2d.velocity;

            if (v.y > -10.0f)
            {
                v.y -= 0.6f;
                rb2d.velocity = v;
            }
            
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("floor"))
        {
            isGround = true;
            stst = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("stop_left"))
        {
            Debug.Log("壁から離れる");
            runnable = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //respawn消す
        if (other.gameObject.CompareTag("death"))
        {
            stst = false;
            isGround = false;
        }

        if (other.gameObject.CompareTag("stop_left"))
        {
            Debug.Log("壁にぶつかる");
            runnable = false;
            //rb2d.velocity = Vector2.zero;
        }

        /*if (other.gameObject.name.Equals("GoalGate"))
        {
            Debug.Log("脱出成功");
            Destroy(this.gameObject);
        }*/
    }

    public void CoinCounter()
    {
        coinCnt++;
        Debug.Log(coinCnt+"枚");
    }

    public void MutekiTime()
    {
        isMuteki = true;
        Debug.Log("無敵");
        itemCtl.SetItem("Muteki");
        Invoke("MutekiEnd", 10);

    }

    public void MutekiEnd()
    {
        isMuteki = false;
        Debug.Log("無敵解除");
        itemCtl.LoseItem();
    }

    public bool GetMuteki()
    {
        return isMuteki;
    }

}