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
	public LayerMask interactablesLayer;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void HandleUpdate() 
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

                if (IsWalkable(targetPos))
				StartCoroutine(Move(targetPos));
			}
		}

		animator.SetBool("isMoving", isMoving);

		if (Input.GetKeyDown(KeyCode.Z)) Interact();
	}

	void Interact()
	{
		var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
		var interactPos = transform.position + facingDir;

		var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactablesLayer);
		if (collider != null)
		{
			collider.GetComponent<Interactable>()?.Interact();
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

	private bool IsWalkable(Vector3 targetPos)
	{
		return Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactablesLayer) == null;
	}
}
