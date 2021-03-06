﻿using UnityEngine;
using System.Collections;

/*
 * 장애물 클래스(View)
 * 장애물 데이터를 기반으로 장애물을 화면에 표현하며
 * 캐릭터와 충돌을 일으키는 충돌체를 가지고 있고
 * IG_ObjectPool에 의해 관리된다.
 */

public class IG_Object : MonoBehaviour
{
    public Data_Object Data;
    public Common.ObjectType Type;
    public BoxCollider2D Col;
    public Rigidbody2D Rigid;
    public IG_Flow Flow;
    public bool IsUse;

    public UISprite SP_Image;


    public bool IsColandFly { get; set; }

    void Update()
    {
        if (Data == null) { Clear(); return; }
        if (IG_Manager.Instance.IsPause || IG_Manager.Instance.IsGameOver) return;

        float curXPos = Camera.main.WorldToViewportPoint(this.transform.position).x;
        GetComponent<Collider2D>().enabled = ((curXPos < 1) && (curXPos > -0.5) && !IsColandFly);
        FlyObejctCheck();

        if (IsColandFly && curXPos > Common.FlyClear_Pos_x)
        {
            Clear();
        }
    }

    public void SetData(Data_Object data)
    {
        //위치 설정
        Data = data;
        float pos_y = 0;
        switch (data.PosType)
        {
            case Common.PosType.Down_Full:
                pos_y = Common.Down_Full_Pos_y;
                break;

            case Common.PosType.Up_Full:
                pos_y = Common.Up_Full_Pos_y;
                break;

            case Common.PosType.Down_Jump:
                pos_y = Common.Down_Pos_y;
                break;

            case Common.PosType.Up_Jump:
                pos_y = Common.Up_Pos_y;
                break;

            case Common.PosType.Down_Fly:
                pos_y = -Common.Fly_pos_y;
                break;

            case Common.PosType.Up_Fly:
                pos_y = Common.Fly_pos_y;
                break;

        }
        transform.localPosition = new Vector2(data.Pos_x, pos_y);

        //데이터 타입&태그 설정, 사이즈, 이미지 설정
        if (data is Data_BuildObject)
        {
            Type = Common.ObjectType.Build;
            tag = Common.Tag_Build;
            SetBuild();
        }
        else if (data is Data_GetObject)
        {
            Type = Common.ObjectType.Get;
            tag = Common.Tag_Get;
            SetGet();
        }
        else if (data is Data_FlyObject)
        {
            Type = Common.ObjectType.FlyBuild;
            tag = Common.Tag_Build;
            SetFly(data as Data_FlyObject);
        }
        else if (data is Data_FlyGetObject)
        {
            Type = Common.ObjectType.FlyGet;
            tag = Common.Tag_Get;
            SetFly(data as Data_FlyGetObject);
        }

        gameObject.SetActive(true);
        IsUse = true;
    }

    public void Clear()
    {
        IG_Manager.Instance.ObjectPool.ReturnObject(this);
        Data = null;
        SP_Image.gameObject.SetActive(true);
        Flow.enabled = true;
        Rigid.isKinematic = true;
        IsColandFly = false;
        IsUse = false;

        gameObject.SetActive(false);
    }

    public void GetClear()
    {
        SP_Image.gameObject.SetActive(false);
    }

    public void PlaySound(AnimalControl.sound index, AudioClip clip)
    {
        switch (index)
        {
            case AnimalControl.sound.Booster:
            case AnimalControl.sound.GetCoin:
            case AnimalControl.sound.Collide:
            case AnimalControl.sound.BBuzik:
                GetComponent<AudioSource>().clip = clip;
                GetComponent<AudioSource>().Play();
                break;
        }
    }

    void SetBuild()
    {
        switch ((Data as Data_BuildObject).PosType)
        {
            case Common.PosType.Down_Jump:
                SP_Image.spriteName = string.Format("{0}{1}", 1, Common.Sprite_DJ);
                SetImage(42, 42);
                break;

            case Common.PosType.Up_Jump:
                SP_Image.spriteName = string.Format("{0}{1}{2}",1 ,Common.Sprite_UJ, Random.Range(0, 2));
                SetImage(42, 42);
                break;

            case Common.PosType.Up_Full:
                SP_Image.spriteName = string.Format("{0}{1}", 1, Common.Sprite_UF);
                //SP_Image.height = (int)Common.FullObj_y_Size;
                //Col.size = new Vector2(Col.size.x, Common.FullObj_y_Size);
                SetImage(128, (int)Common.FullObj_y_Size);
                break;

            default:
            case Common.PosType.Down_Full:
                SP_Image.spriteName = string.Format("{0}{1}",1,Common.Sprite_DF);
                //SP_Image.height = (int)Common.FullObj_y_Size;
                //Col.size = new Vector2(Col.size.x, Common.FullObj_y_Size);
                SetImage(64, (int)Common.FullObj_y_Size);
                break;

            case Common.PosType.Up_Fly:
                SP_Image.spriteName = string.Format("{0}{1}", 1, Common.Sprite_UFly);
                SetImage(50, 50);
                break;

            case Common.PosType.Down_Fly:
                SP_Image.spriteName = string.Format("{0}{1}", 1, Common.Sprite_DFly);
                SetImage(50, 50);
                break;
        }
        //타입 맞춰서 이미지 늘리기, 콜라이더 조정
    }

    void SetGet()
    {
        switch ((Data as Data_GetObject).GetType)
        {
            case Common.GetType.Gold:
                SP_Image.spriteName = Common.Sprite_Gold;
                SetImage(40, 40);
                break;

            case Common.GetType.Speed:
                SP_Image.spriteName = Common.Sprite_Booster;
                SetImage(50, 50);
                break;
        }
        //이미지 변경
    }

    void SetFly(Data_FlyObject _data)
    {
        switch ((Data as Data_FlyObject).PosType)
        {
            case Common.PosType.Up_Fly:
                SP_Image.spriteName = string.Format("{0}{1}", 1, Common.Sprite_UFly);
                break;

            default:
            case Common.PosType.Down_Fly:
                SP_Image.spriteName = string.Format("{0}{1}", 1, Common.Sprite_DFly);
                break;
        }
        //이미지 변경
    }

    void SetFly(Data_FlyGetObject _data)
    {
        switch ((Data as Data_FlyGetObject).GetType)
        {
            default:
            case Common.GetType.Gold:
                SP_Image.spriteName = Common.Sprite_Gold;
                SetImage(40, 40);
                break;

            case Common.GetType.Speed:
                SP_Image.spriteName = Common.Sprite_Booster;
                SetImage(50, 50);
                break;
        }
        //이미지 변경
    }

    void SetImage(int width, int height)
    {
        SP_Image.width = width;
        SP_Image.height = height;
        Col.size = new Vector2(width, height - 10f); // 약간의 완충?
    }

    void FlyObejctCheck()
    {
        if (Type == Common.ObjectType.FlyBuild)
        {
            transform.Translate((Data as Data_FlyObject).Direction * Time.fixedDeltaTime);
        }
        else if (Type == Common.ObjectType.FlyGet)
        {
            transform.Translate((Data as Data_FlyGetObject).Direction * Time.fixedDeltaTime);
        }
    }

    public void CollideGone()
    {
        Flow.enabled = false;
        IsColandFly = true;
        Rigid.isKinematic = false;
        Rigid.velocity = (new Vector2(50, 30).normalized * 7);
    }
}
