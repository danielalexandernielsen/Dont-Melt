﻿using System;
using UnityEngine;

public class ChasingSun : MonoBehaviour
{
	[SerializeField] private LayerMask layerMask = -1;
	[SerializeField] private float maxSpeed = 10;
	[SerializeField] private float smoothTime = 1;
	[SerializeField] private float damagePerSecond = 10;
	[SerializeField] private Transform rays;

	private Player player;
	private Transform playerTransform;
	private Vector3 offsetFromPlayer;
	private Vector3 rayDirection;
	private Vector3 currentVelocity;
	private Vector3 raycastHitPoint;
	private bool raycastHitPlayer;

	private void Start()
	{
		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
		player = playerObject.GetComponent<Player>();
		playerTransform = playerObject.transform;

		offsetFromPlayer = transform.position - playerTransform.position;
	}

	private void Update()
	{
		Vector3 targetPosition = playerTransform.position + offsetFromPlayer;
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, maxSpeed);

		rayDirection = playerTransform.position - transform.position;

		float rayAngle = Mathf.Atan2(rayDirection.y, rayDirection.x) * Mathf.Rad2Deg;
		rays.eulerAngles = new Vector3(0, 0, rayAngle);

		UpdateRaycast();

		if (raycastHitPlayer)
		{
			player.TakeDamage(damagePerSecond * Time.deltaTime);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = raycastHitPlayer ? Color.red : Color.yellow;
		Gizmos.DrawLine(transform.position, raycastHitPoint);
	}

	private void UpdateRaycast()
	{
		raycastHitPlayer = false;

		RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 100, layerMask);
		if (hit.collider != null)
		{
			if (hit.collider.CompareTag("Player"))
				raycastHitPlayer = true;

			raycastHitPoint = hit.point;
		}
	}
}