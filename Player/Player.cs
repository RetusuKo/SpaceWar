﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDatePersistance
{
    [Header("PlayerInfo")]
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private float _jumpForce = 7.5f;
    [SerializeField] private float _rollForce = 6.0f;
    [SerializeField] private float _dropForce = 8.5f;
    [SerializeField] private float _damageForce = 3f;

    [Header("Particle")]
    [SerializeField] private ParticleSystem _dropParticle;
    [SerializeField] private ParticleSystem _dustParticle;

    [SerializeField] private bool _noBlood = false;
    [SerializeField] private GameObject _slideDust;
    [SerializeField] private ActivateInventary _inventary;

    private PlayerGun _gun;
    private PlayerSword _sword;
    private Animator _animator;
    private ChangeSize _changeSize;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spriteRenderer;
    private Sensor_HeroKnight _groundSensor;
    private Sensor_HeroKnight _wallSensorR1;
    private Sensor_HeroKnight _wallSensorR2;
    private Sensor_HeroKnight _wallSensorL1;
    private Sensor_HeroKnight _wallSensorL2;

    private bool _grounded = false;
    private bool _rolling = false;
    private int _facingDirection = 1;
    private float _delayToIdle = 0.0f;
    private enum Action
    {
        None,
        Attack,
        Block,
        Roll,
        Jump,
        Drop
    }
    private void Awake()
    {
        _changeSize = gameObject.AddComponent(typeof(ChangeSize)) as ChangeSize;
        _gun = GetComponent<PlayerGun>();
        _sword = GetComponent<PlayerSword>();
        _animator = GetComponent<Animator>();
        _changeSize = GetComponent<ChangeSize>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        _wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        _wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        _wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        _wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();

        gameObject.tag = "Player";
    }
    private void Update()
    {
        if (PlayerInfo.CanMove)
            InputManager();
    }
    private void InputManager()
    {
        ResetColiderAnim();
        PlayerInfo.TimeSinceAttack += Time.deltaTime;
        Loaded();
        IsFalling();
        float inputX = Input.GetAxis("Horizontal");
        Rotation(inputX);
        Move(inputX);
        SetSpeed();
        WallSlide();
        PlayerMovement(inputX);
    }
    private void Loaded()
    {
        if (!_grounded && _groundSensor.State())
        {
            _grounded = true;
            _animator.SetBool("Grounded", _grounded);
        }
    }
    private void IsFalling()
    {
        if (_grounded && !_groundSensor.State())
        {
            _grounded = false;
            _animator.SetBool("Grounded", _grounded);
        }
    }
    private void Rotation(float inputX)
    {
        bool flip = inputX < 0 ? true : false;
        _spriteRenderer.flipX = flip;
        _facingDirection = flip ? -1 : 1;
        _animator.SetBool("DirectionLeft", flip);
    }
    private void Move(float inputX)
    {
        if (!_rolling)
        {
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = inputX * _speed;
            _rigidbody.velocity = velocity;
        }
        //_rigidbody.velocity = new Vector2(inputX * _speed, _rigidbody.velocity.y);
    }
    private void SetSpeed()
    {
        _animator.SetFloat("AirSpeedY", _rigidbody.velocity.y);
        _animator.SetFloat("SpeedX", _rigidbody.velocity.x);
    }
    private void WallSlide()
    {
        bool isOnWall = _wallSensorR1.State() && _wallSensorR2.State() || (_wallSensorL1.State() && _wallSensorL2.State());
        _animator.SetBool("WallSlide", isOnWall);
        _rigidbody.drag = isOnWall ? 2 : 0.5f;
    }
    private void PlayerMovement(float inputX)
    {
        if (Input.GetButton("Fire1"))
        {
            if (PlayerInfo.TimeSinceAttack > 0.3f && !_rolling && _animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                AirAtack();
            else if (Input.GetButton("Fire1") && PlayerInfo.TimeSinceAttack > 0.3f && !_rolling)
                Atack();
        }
        else if (Input.GetButtonDown("Roll"))
        {
            if (!_rolling && !_animator.GetBool("WallSlide"))
                Roll();
        }
        else if (Input.GetButtonDown("Jump"))
        {
            if (Input.GetButton("Vertical") && !_grounded && PlayerUpgrade.UpgradeCheck(PlayerUpgrade.UpgradesName[1]))
                Drop();
            else if (_grounded && !_rolling || Input.GetButtonDown("Jump") && _animator.GetCurrentAnimatorStateInfo(0).IsName("Wall Slide") && PlayerUpgrade.UpgradeCheck(PlayerUpgrade.UpgradesName[3]))
                Jump();
        }
        else if (Input.GetButtonDown("Block"))
        {
            if (!_rolling && !PlayerInfo.DoNotTakeDamage)
                Block();
        }
        else if (Input.GetButtonDown("ChangeWeapon"))
        {
            if (PlayerUpgrade.UpgradeCheck(PlayerUpgrade.UpgradesName[2]))
                ChangeWeapon();
        }
        else if (Input.GetButtonDown("Inventary"))
        {
            ActivateInventary();
        }
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            Run();
        }
        else
        {
            Idle();
        }
    }
    #region
    public void Dead()
    {
        _animator.SetBool("noBlood", _noBlood);
        _animator.SetTrigger("Death");
        Destroy(GetComponent<Player>());
        GetComponent<PlayerHealth>().Dead();
        Destroy(GetComponent<PlayerHealth>());
        gameObject.layer = 8;
        Destroy(gameObject.GetComponent<Player>());
        Destroy(gameObject.GetComponent<PlayerUpgrade>());
    }
    public void Hurt()
    {
        _animator.SetTrigger("Hurt");
    }
    private void AirAtack()
    {
        if (PlayerInfo.CurentWapone == PlayerInfo.WeaponType.Sword)
            _sword.AirAtack();
    }
    private void Atack()
    {
        if (PlayerInfo.CurentWapone == PlayerInfo.WeaponType.Sword)
            _sword.Atack();
        else if (PlayerInfo.CurentWapone == PlayerInfo.WeaponType.Gun)
            _gun.Atack();
    }
    private void Block()
    {
        PlayerInfo.DoNotTakeDamage = true;
        _animator.SetTrigger("Block");
        _animator.SetBool("IdleBlock", true);
        StartCoroutine(WaitForDoNotDamage());
    }
    private void Roll()
    {
        _changeSize.RollChange(_boxCollider);
        _rolling = true;
        _animator.SetTrigger("Roll");
        _rigidbody.velocity = new Vector2(_facingDirection * _rollForce, _rigidbody.velocity.y);
    }

    private bool drop = false;
    private void Drop()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -1 * _dropForce);
        drop = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (drop)
        {
            print("drop");
            _animator.SetBool("Drop", true);
            _dropParticle.Play();
            drop = false;
        }
        else if (_rolling && collision.gameObject.tag == "Enemy")
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_rolling && collision.gameObject.tag == "Enemy")
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }
    private void Jump()
    {
        _animator.SetTrigger("Jump");
        _grounded = false;
        _animator.SetBool("Grounded", _grounded);
        if (_rigidbody.drag == 0.5)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
        else if (_rigidbody.drag == 2)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce * 1.3f);
        _groundSensor.Disable(0.2f);
    }
    private void ChangeWeapon()
    {
        if (PlayerInfo.CurentWapone == PlayerInfo.WeaponType.Sword)
            PlayerInfo.CurentWapone = PlayerInfo.WeaponType.Gun;
        else if (PlayerInfo.CurentWapone == PlayerInfo.WeaponType.Gun)
            PlayerInfo.CurentWapone = PlayerInfo.WeaponType.Sword;
    }
    private void ActivateInventary()
    {
        _inventary.ActivateInventory();
    }
    private void Run()
    {
        _delayToIdle = 0.05f;
        _animator.SetInteger("AnimState", 1);
    }
    private void Idle()
    {
        _delayToIdle -= Time.deltaTime;
        if (_delayToIdle < 0)
            _animator.SetInteger("AnimState", 0);
    }
    private void ResetColiderAnim()
    {
        if (_grounded)
            _animator.SetBool("Drop", false);
    }
    #endregion
    private IEnumerator WaitForDoNotDamage()
    {
        yield return new WaitForSeconds(PlayerInfo.NoDomageMiliSec);
        PlayerInfo.DoNotTakeDamage = false;
    }
    private IEnumerator WaitTimeToLayerPlayer()
    {
        yield return new WaitForSeconds(0.9f);
        gameObject.layer = 3;
    }
    private void AE_ResetRoll()
    {
        _rolling = false;
        _changeSize.StandartSize(_boxCollider);
        gameObject.layer = 3;
        PlayerInfo.DoNotTakeDamage = false;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void AE_SlideDust()
    {
        Vector3 spawnPosition;
        if (_facingDirection == 1)
            spawnPosition = _wallSensorR2.transform.position;
        else
            spawnPosition = _wallSensorL2.transform.position;

        if (_slideDust != null)
        {
            GameObject dust = Instantiate(_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            dust.transform.localScale = new Vector3(_facingDirection, 1, 1);
        }
        AE_ResetRoll();
    }

    public void LoadDate(GameData data)
    {
        gameObject.transform.position = data.PlayerPosition;
    }

    public void SaveData(GameData data)
    {
        data.PlayerPosition = gameObject.transform.position;
    }
}