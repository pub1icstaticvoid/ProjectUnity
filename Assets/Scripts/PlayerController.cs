using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed;
	private bool isMoving;
	private Vector2 input;
	private Animator animator;
	public LayerMask solidObjectsLayer;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Update() 
	{
		if (!isMoving) {
			input.x = Input.GetAxisRaw("Horizontal");
			input.y = Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero) 
			{
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                //            var targetPos = transform.position;
                //targetPos.x += input.x;
                //targetPos.y += input.y;
                Vector3 targetPos = transform.position;
                targetPos.x += input.x * moveSpeed * Time.deltaTime;
                targetPos.y += input.y * moveSpeed * Time.deltaTime;

                if (isWalkable(targetPos))
				StartCoroutine(Move(targetPos));
			}
		}

		animator.SetBool("isMoving", isMoving);
	}

	IEnumerator Move(Vector3 targetPos) 
	{
		isMoving = true;
		while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
			transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
			yield return null;
		}
		transform.position = targetPos;

		isMoving = false;
	}

	private bool isWalkable(Vector3 targetPos)
	{
		return Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) == null;
	}
}
