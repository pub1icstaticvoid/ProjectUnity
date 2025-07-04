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

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Update() 
	{
		if (!isMoving) {
			input.x = Input.GetAxisRaw("Horizontal");
			input.y = Input.GetAxisRaw("Vertical");

			Debug.Log("input x: " + input.x);
            Debug.Log("input y: " + input.y);

            if (input != Vector2.zero) 
			{
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
				targetPos.x += input.x;
				targetPos.y += input.y;

				StartCoroutine(Move(targetPos));
			}
		}
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
}
