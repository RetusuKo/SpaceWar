using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
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
    private void Update ()
    {
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
        if (inputX > 0)
        {
            _spriteRenderer.flipX = false;
            _facingDirection = 1;
            _animator.SetBool("DirectionLeft", true);
        }

        else if (inputX < 0)
        {
            _spriteRenderer.flipX = true;
            _facingDirection = -1;
            _animator.SetBool("DirectionLeft", false);
        }
    }
    private void Move(float inputX)
    {
        if (!_rolling)
            _rigidbody.velocity = new Vector2(inputX * _speed, _rigidbody.velocity.y);
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
        if (_animator.GetBool("WallSlide"))
            _rigidbody.drag = 2;
        else if (!_animator.GetBool("WallSlide"))
            _rigidbody.drag = 0.5f;
    }
    private void PlayerMovement(float inputX)
    {
        if (Input.GetButton("Fire1") && PlayerInfo.TimeSinceAttack > 0.3f && !_rolling)
            Atack();
        else if (Input.GetButtonDown("Block") && !_rolling  && !PlayerInfo.DoNotTakeDamage)
            Block();
        else if (Input.GetButtonDown("Roll") && !_rolling && !_animator.GetBool("WallSlide"))
            Roll();
        else if (Input.GetButtonDown("Jump") && Input.GetButton("Vertical") && !_grounded && PlayerUpgrade.UpgradeCheck("JumpDownUpgrade"))
            Drop();
        else if (Input.GetButtonDown("Jump") && _grounded && !_rolling || Input.GetButtonDown("Jump") && _animator.GetBool("WallSlide") && PlayerUpgrade.UpgradeCheck("WallJumpUpgrade"))
            Jump();
        else if (Input.GetButtonDown("ChangeWeapon") && PlayerUpgrade.UpgradeCheck("HaveGun"))
            ChangeWeapon();
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            Run();
        else
            Idle();
    }
    #region
    public void Dead()
    {
        _animator.SetBool("noBlood", _noBlood);
        _animator.SetTrigger("Death");
        Destroy(GetComponent<Player>());
        Destroy(GetComponent<PlayerHealth>());
        gameObject.layer = 8;
    }
    public void Hurt()
    {
        _animator.SetTrigger("Hurt");
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
        //gameObject.layer = 9;
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
    }
    private void Jump()
    {
        _animator.SetTrigger("Jump");
        _grounded = false;
        _animator.SetBool("Grounded", _grounded);
        if (_rigidbody.drag == 0.5)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
        else if(_rigidbody.drag == 2)
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
        {
            _animator.SetBool("Drop", false);
        }
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
}
