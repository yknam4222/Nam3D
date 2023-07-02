using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
	public Animator animator;

	public float speed;      // 캐릭터 움직임 스피드.
	public float jumpSpeed; // 캐릭터 점프 힘.
	public float gravity;    // 캐릭터에게 작용하는 중력.

	private CharacterController controller; // 현재 캐릭터가 가지고있는 캐릭터 컨트롤러 콜라이더.
	private Vector3 MoveDir;                // 캐릭터의 움직이는 방향.
	void Start()
	{
		animator = GetComponent<Animator>();

		speed = 6.0f;
		jumpSpeed = 8.0f;
		gravity = 20.0f;

		MoveDir = Vector3.zero;
		controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.W))
		{
			Walk();
			transform.Translate(0, 0, speed * Time.deltaTime);
		}
		else
			Idle();

		// 현재 캐릭터가 땅에 있는가?
		if (controller.isGrounded)
		{
			// 위, 아래 움직임 셋팅. 
			MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

			// 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
			MoveDir = transform.TransformDirection(MoveDir);

			// 스피드 증가.
			MoveDir *= speed;

			// 캐릭터 점프
			if (Input.GetButton("Jump"))
				MoveDir.y = jumpSpeed;
		}

		// 캐릭터에 중력 적용.
		MoveDir.y -= gravity * Time.deltaTime;

		// 캐릭터 움직임.
		controller.Move(MoveDir * Time.deltaTime);

		transform.Rotate(0f, Input.GetAxis("Mouse X") * speed, 0f, Space.World);
	}

	public void Idle()
	{
		animator = GetComponent<Animator>();
		animator.SetBool("Walk", false);
		animator.SetBool("SprintJump", false);
		animator.SetBool("SprintSlide", false);
	}

	public void Walk()
	{
		animator = GetComponent<Animator>();
		animator.SetBool("Walk", true);
		animator.SetBool("SprintJump", false);
		animator.SetBool("SprintSlide", false);
	}

	public void SprintJump()
	{
		animator = GetComponent<Animator>();
		animator.SetBool("Walk", false);
		animator.SetBool("SprintJump", true);
		animator.SetBool("SprintSlide", false);
	}

	public void SprintSlide()
	{
		animator = GetComponent<Animator>();
		animator.SetBool("Walk", false);
		animator.SetBool("SprintJump", false);
		animator.SetBool("SprintSlide", true);
	}
}
