using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float jumpForce;

    private bool _isGrounded;

    [Header("References")] 
    public Rigidbody2D rig;
    public Animator anim;
    public BoxCollider2D coll;
    
    private bool _isInvincible = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _isGrounded = true;
        TryGetComponent(out rig);
        TryGetComponent(out anim);
        TryGetComponent(out coll);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            rig.AddForceY(jumpForce, ForceMode2D.Impulse);
            anim.SetInteger("state", 1);
            _isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("platform"))
        {
            if (!_isGrounded)
            {
                anim.SetInteger("state", 2);
            }
            _isGrounded = true;
        }
    }

    private void Hit()
    {
        GameManager.Instance.lives -= 1;
    }

    private void Heal()
    {
        GameManager.Instance.lives = Mathf.Min(3, GameManager.Instance.lives + 1);
    }

    private void StartInvincible()
    {
        _isInvincible = true;
        Invoke("StopInvincible", 5f);
    }

    private void StopInvincible()
    {
        _isInvincible = false;
    }

    public void KillPlayer()
    {
        coll.enabled = false;
        anim.enabled = false;
        rig.AddForceY(jumpForce, ForceMode2D.Impulse);
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("enemy"))
        {
            if(!_isInvincible)
                Destroy(other.gameObject);
            Hit();
        }
        else if (other.gameObject.tag.Equals("food"))
        {
            Destroy(other.gameObject);
            Heal();
        }
        else if (other.gameObject.tag.Equals("golden"))
        {
            Destroy(other.gameObject);
            StartInvincible();
        }
    }
}
