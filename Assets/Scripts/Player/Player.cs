using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IHittable
{

    private CharacterController2D _controller;
    private GameManager gm;
    [Header("Info")]
    public IItem selectedItem;

    [Header("Health")]
    public float maxHealth;
    public float health;
    public bool invulnerable;
    public float invulnerabilityTime;
    public bool isHit;
    public bool dead;
    public GameObject healthUI;
    private List<GameObject> healthUIs;
    public GameObject healthUIParent;

    [Header("Move Settings")]
    private bool _isFacingRight;
    public float maxSpeed;
    public float accelerationOnGround;
    public float accelerationInAir;
    public bool handleInput;

    [Header("Animation")]
    public Animator spriteAni;
    public ParticleSystem hitEffect;
    public ParticleSystem deathEffect;

    [Header("Attack")]
    public AttackManager meleeAttack;
    public float attackSpeed;
    private float attackTimer;

    private Vector2 _moveDir;


    [Header("Jump")]
    public float jumpHeight;

    void Start()
    {
        gm = GameManager.Instance;
        health = maxHealth;
        _controller = GetComponent<CharacterController2D>();
        _isFacingRight = transform.localScale.x > 0;
        healthUIs = new List<GameObject>();
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject healthImage = Instantiate(healthUI);
            healthImage.transform.SetParent(healthUIParent.transform, false);
            healthImage.transform.localPosition = Vector2.right * 60 * i;
            healthUIs.Add(healthImage);
        }
    }

    void Update()
    {
        if (handleInput)
        {
            HandleInputs();
        }
        else
        {
            _moveDir = Vector2.zero;
        }
        HandleMovement();
        UpdateFacingDirection();
        UpdateAnimations();
        UpdateHealthUI();
        TimerVariables();
    }

    private void TimerVariables()
    {
        if (attackTimer < attackSpeed)
        {
            attackTimer += Time.deltaTime;
        }
    }

    private void UpdateAnimations()
    {
        if (_moveDir.x != 0f && _controller.State.IsGrounded)
        {
            spriteAni.SetBool("Walking", true);
        }
        else
        {
            spriteAni.SetBool("Walking", false);
        }
    }

    private void UpdateFacingDirection()
    {
        if (_controller.Velocity.x > 0)
        {
            _isFacingRight = true;
            //remove when animations are added
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (_controller.Velocity.x < 0)
        {
            _isFacingRight = false;
            //remove when animations are added
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void UpdateHealthUI()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        for (int i = 0; i < health; i++)
        {
            healthUIs[i].SetActive(true);
        }
        for(int i = (int)health; i < healthUIs.Count; i++)
        {
            healthUIs[i].SetActive(false);
        }
    }

    private void HandleInputs()
    {
        _moveDir.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _controller.Jump(jumpHeight);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && _controller.State.IsJumping && !_controller.State.IsDashing)
        {
            _controller.CutJump();
            if(Input.GetAxis("Jump") < 0.9f)
            {
                _controller.CutJump();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && (_moveDir.x != 0 || _moveDir.y != 0))
        {
            _controller.Dash(_moveDir);
        }

        if (Input.GetMouseButtonDown(0) && attackTimer >= attackSpeed)
        {
            Attack();
        }

        if(selectedItem != null && Input.GetKeyDown(KeyCode.E))
        {
            selectedItem.Collect();
        }
    }

    private void HandleMovement()
    {
        var accelertionMultiplier = _controller.State.IsGrounded ? accelerationOnGround : accelerationInAir;
        if (_controller.State.SlopeAngle > _controller.Parameters.SlopeLimit)
        {
            _moveDir = Vector2.zero;
        }
        if (!_controller.State.IsDashing && !isHit)
        {
            if (Mathf.Abs(_controller.MovementSpeed) <= maxSpeed * 1.05)
            {
                _controller.SetHorizontalSpeed(Mathf.MoveTowards(_controller.MovementSpeed, _moveDir.x * maxSpeed, Time.deltaTime * accelertionMultiplier));
            }

            if (_controller.Velocity.magnitude > maxSpeed * 1.05)
            {
                if ((Mathf.Abs(_controller.Velocity.x) > _controller.Velocity.y) && Mathf.Sign(_moveDir.x * _controller.Velocity.x) == 1)
                {
                    _controller.SetDrag(1f);
                }
                else if (!_controller.State.IsJumping)
                {
                    _controller.SetDrag(3f);
                }

            }
            else
            {
                _controller.SetDrag(0f);
            }
        }

    }

    public void Attack()
    {
        meleeAttack.dir.Set(_isFacingRight ? 1 : -1, _moveDir.y);
        meleeAttack.gameObject.SetActive(true);
        attackTimer = 0;
    }

    public void Hit(float dmg, Vector2 dir, float knockbackMult)
    {
        if (invulnerable)
        {
            return;
        }
        health -= dmg;
        health = Mathf.Max(health, 0);
        isHit = true;
        invulnerable = true;
        handleInput = false;
        _controller.Move(Vector2.zero);
        spriteAni.SetBool("Hit", true);
        StartCoroutine(HitUpdate(dir));
        hitEffect.Play();
        if (health <= 0f)
        {
            healthUIs.ForEach(obj => obj.SetActive(false));
            health = 0;
            Die();
            return;
        }
        healthUIs[(int)health].SetActive(false);
    }

    public void Die()
    {
        dead = true;
        deathEffect.Play();
        _controller.Freeze();
        spriteAni.SetTrigger("Death");
        StartCoroutine(Restart());
    }

    IEnumerator HitUpdate(Vector2 hitDir)
    {
        float hitTime = 0.2f;
        Vector2 moveDir = (hitDir + Vector2.up * 0.5f).normalized;
        while (hitTime >= 0)
        {
            hitTime -= Time.deltaTime;
            transform.position += (Vector3)moveDir * 5 * Time.deltaTime;
            yield return null;
        }

        isHit = false;
        if (health > 0f)
        {
            handleInput = true;
        }
        float invulnTimer = invulnerabilityTime;
        while (invulnTimer >= 0)
        {
            invulnTimer -= Time.deltaTime;
            yield return null;
        }

        if (health > 0f)
        {
            invulnerable = false;
        }

        spriteAni.SetBool("Hit", false);
    }

    IEnumerator Restart()
    {
        float restartTime = 3f;
        while (restartTime >= 0)
        {
            restartTime -= Time.deltaTime;
            yield return null;
        }
        gm.Restart();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.GetComponent<IItem>() != null && other.isTrigger)
        {
            selectedItem = other.GetComponent<IItem>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.GetComponent<IItem>() != null)
        {
            selectedItem = null;
        }
    }
}
