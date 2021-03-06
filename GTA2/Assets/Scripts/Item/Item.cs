﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemStatus
{
    // 아이템
    ActiveItemStartIndex,
    Heath,
    CopBribe,
    KillFrenzy,
    ActiveItemEndIndex,

    // 총
    GunStartIndex,
    PistolGun,
    DoublePistolGun,
    MachineGun,
    SleepMachineGun,
    RocketLauncherGun,
    ElectricGun,
    ShotGun,
    FireGun,
    FireBottleGun,
    GranadeGun,
    GunEndIndex,

    // TODO: 아래는 차 아이템 구현할 것
}

public class Item : MonoBehaviour
{
    [Header("Value")]
    // Start is called before the first frame update
    public ItemStatus itemType;
    public int itemCount;
    public float RespawnTime;


    [Header("Sound")]
    public AudioClip soundClip;

    float RespawnDelta;

    [SerializeField]
    protected Sprite[] spriteAnimation;
    protected Player userPlayer;
    
    
    protected SpriteRenderer spriteRender;
    protected SphereCollider sphereCollider;
    protected float animationTime = .3f;
    protected float animationDelta = .0f;
    protected int aniIdx = 0;




    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        sphereCollider = GetComponent<SphereCollider>();
        userPlayer = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        UpdateSprite();
        UpdateRespawn();
    }

    void UpdateSprite()
    {
        animationDelta += Time.deltaTime;
        if (animationDelta > animationTime)
        {
            animationDelta = .0f;
            aniIdx++;
        }

        if (aniIdx >= spriteAnimation.Length)
        {
            aniIdx = 0;
        }

        spriteRender.sprite = spriteAnimation[aniIdx];
    }

    void UpdateRespawn()
    {
        RespawnDelta += Time.deltaTime;
        if (RespawnTime < RespawnDelta)
        {
            ActiveOn();

            RespawnDelta = .0f;
        }
    }

    void ActiveOn()
    {
        spriteRender.enabled = true;
        sphereCollider.enabled = true;
    }

    protected void ActiveOff()
    {
        RespawnDelta = .0f;
        spriteRender.enabled = false;
        sphereCollider.enabled = false;
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject != userPlayer.gameObject)
        {
            return;
        }

        // 해당아이템이 수치 아이템
        if (itemType > ItemStatus.ActiveItemStartIndex && itemType < ItemStatus.ActiveItemEndIndex)
        {
            switch (itemType)
            {
                case ItemStatus.Heath:
                    userPlayer.SetHpDefault();
                    break;
                case ItemStatus.CopBribe:
                    WantedLevel.instance.ResetWantedLevel();
                    break;
                case ItemStatus.KillFrenzy:
                    QuestUIManager.Instance.ToastStartQuest("KILL FRENZY", ""); 
                    SoundManager.Instance.PlayClip(soundClip, SoundPlayMode.UISFX);
                    ActiveOff();
                    return;
                default:
                    break;
            }
        }

        // 해당아이템이 총 아이템일 경우
        if (itemType > ItemStatus.GunStartIndex && itemType < ItemStatus.GunEndIndex)
        {
            userPlayer.gunList[(int)itemType - (int)ItemStatus.GunStartIndex].bulletCount += itemCount;
        }

        ActiveOff();
        SoundManager.Instance.PlayClip(soundClip, SoundPlayMode.UISFX);
        UIManager.Instance.TurnOnItemText(itemType);
    }
}
