using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages:
/// 1. Player movement and flipping
/// 2. Player animations
/// </summary>
public class PlayerCtrl : MonoBehaviour 
{

	[Tooltip("This makes character move faster")]
	public int speedBoost = 5;
	public float jumpSpeed = 600;
	public float boxWidth;
	public float boxHeight;

	public bool isGrounded;
	public Transform feet;
	public float feetRadius;
	public LayerMask whatIsGround;
	public Transform leftBulletSpawnPosition, rightBulletSpawnPosition;
	public GameObject leftBullet, rightBullet;
	bool leftPressed, rightPressed;

	Animator anim;
	Rigidbody2D rb;
	SpriteRenderer sr;
	bool isJumping;


	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
	}
	

	void Update () 
	{
		//isGrounded = Physics2D.OverlapCircle (feet.position, feetRadius, whatIsGround);

		isGrounded = Physics2D.OverlapBox (new Vector2 (feet.position.x, feet.position.y), new Vector2 (boxWidth, boxHeight), 360.0f, whatIsGround);

		float playerSpeed = Input.GetAxisRaw ("Horizontal"); //value 0, -1 or 1	
		playerSpeed *= speedBoost;

		if (playerSpeed != 0)
			MoveHorizontal (playerSpeed);
		else
			StopMoving ();
			
		if (Input.GetButtonDown ("Jump"))
			Jump ();

		if (Input.GetButtonDown ("Fire1")) {
			FireBullets ();
		}
		showFalling ();

		if (leftPressed)
			MoveHorizontal (-speedBoost);

		if (rightPressed)
			MoveHorizontal (speedBoost);

		if (transform.position.y <= -2.8)
			DisableCameraFollow ();
	}

	void OnDrawGizmos()
	{
		//Gizmos.DrawWireSphere (feet.position, feetRadius);
		Gizmos.DrawWireCube(feet.position, new Vector3(boxWidth,boxHeight,0));
	}
		
	void MoveHorizontal(float playerSpeed) 
	{
		rb.velocity = new Vector2 (playerSpeed, rb.velocity.y);

		if (playerSpeed < 0)
			sr.flipX = true;
		else if(playerSpeed > 0)
			sr.flipX = false;

		if (!isJumping)
			anim.SetInteger ("State", 1);

	}

	void StopMoving() 
	{
		rb.velocity = new Vector2 (0, rb.velocity.y);

		if(!isJumping)
			anim.SetInteger ("State", 0);
	}

	void showFalling()
	{
		if (rb.velocity.y < 0) 
		{
			anim.SetInteger ("State", 3);
		}
	}

	void Jump() 
	{
		if (isGrounded) 
		{
			isJumping = true;
			rb.AddForce (new Vector2 (0, jumpSpeed));
			anim.SetInteger ("State", 2);
		}
	}

	void FireBullets()
	{
		if (sr.flipX) {
			Instantiate (leftBullet, leftBulletSpawnPosition.position, Quaternion.identity);
		}

		if (!sr.flipX) {
			Instantiate (rightBullet, rightBulletSpawnPosition.position, Quaternion.identity);
		}
	}

	public void MobileMoveLeft()
	{
		leftPressed = true;
	}

	public void MobileMoveRight()
	{
		rightPressed = true;
	}

	public void MobileStop()
	{
		leftPressed = false;
		rightPressed = false;

		StopMoving ();
	}

	public void MobileFireBullets()
	{
		FireBullets ();
	}

	public void MobileJump()
	{
		Jump ();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		isJumping = false;
	}

	void DisableCameraFollow()
	{
		Camera.main.GetComponent<CameraCtrl> ().enabled = false;	
	}
}
