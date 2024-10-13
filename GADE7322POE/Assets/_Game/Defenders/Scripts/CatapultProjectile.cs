using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CatapultProjectile : MonoBehaviour
{
    [Header("Settings")] public float speed = 3f;
    public float height;

    private bool _init;
    private bool _midpointReached;

    private Vector3 _startPoint = Vector3.zero;
    private Vector3 _midpoint = Vector3.zero;
    private Vector3 _endPoint = Vector3.zero;

    public void InitializeProjectile(Vector3 startPos, Vector3 endPos)
    {
        _startPoint = startPos;
        _midpoint = (startPos + endPos) / 2;
        _midpoint.y *= height;
        _endPoint = endPos;

        _init = true;

        FlightForce(startPos, endPos);
    }

    private void FlightForce(Vector3 startPoint, Vector3 endPoint)
    {
        var rb = GetComponent<Rigidbody>();

        // Calculate the direction to the target
        Vector3 direction = endPoint - startPoint;
        
        direction.Normalize();

        rb.velocity = direction * speed;

        // // Calculate the horizontal distance (ignoring height)
        // float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;
        //
        // // Calculate the required velocity using a launch angle (adjust the angle as necessary)
        // float launchAngle = 45f * Mathf.Deg2Rad; // 45 degrees
        // float gravity = Physics.gravity.magnitude;
        //
        // // Calculate the velocity needed to reach the target
        // float velocity = Mathf.Sqrt(horizontalDistance * gravity / Mathf.Sin(2 * launchAngle));
        //
        // // Set the velocity components
        // Vector3 velocityVector = direction.normalized * velocity;
        // velocityVector.y = Mathf.Sin(launchAngle) * velocity; // Apply vertical component of velocity for the arc
        //
        // // Apply force to the Rigidbody
        // rb.AddForce(velocityVector, ForceMode.VelocityChange);
        //rb.AddRelativeForce(velocityVector);
    }

    private void Update()
    {
        if (!_init) return;

        // Vector3 direction = _endPoint - _startPoint;
        // Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
        // Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);
        //
        // float heightTest = Mathf.Max(1, (_endPoint.y + _startPoint.y) / 2f);
        // float angle;
        // float v0;
        // float time;
        //
        // CalculatePathWithHeight(_endPoint, heightTest, out v0,out angle,out time);
        //
        // StopAllCoroutines();
        // StartCoroutine(ProjectileMovement(groundDirection.normalized, v0, angle, time));
    }

    private void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));

        time = xt / (v0 * Mathf.Cos(angle));
    }

    private float QuadraticEquation(float a, float b, float c, float sin)
    {
        return (-b + sin * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }

    private void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
    {
        float g = Physics.gravity.magnitude;
        float dx = targetPos.x; // horizontal distance to target
        float dy = targetPos.y; // vertical distance to target

        // Calculate the required initial velocity
        float v1 = Mathf.Pow(dx, 2) * g;
        float v2 = 2 * dx * Mathf.Sin(Mathf.Deg2Rad * 45f) * Mathf.Cos(Mathf.Deg2Rad * 45f); // at 45 degrees
        float v3 = 2 * dy * Mathf.Pow(Mathf.Cos(Mathf.Deg2Rad * 45f), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));

        // Time to target
        time = dx / (v0 * Mathf.Cos(Mathf.Deg2Rad * 45f));
        angle = Mathf.Deg2Rad * 45f; // 45 degree launch angle
    }


    IEnumerator ProjectileMovement(Vector3 direction, float v0, float angle, float time)
    {
        float t = 0;
        while (t < time)
        {
            // Calculate the projectile's new position
            float x = v0 * t * Mathf.Cos(angle); // horizontal displacement
            float y = v0 * t * Mathf.Sin(angle) - (0.5f * Physics.gravity.y * Mathf.Pow(t, 2)); // vertical displacement

            // Move the projectile
            transform.position = _startPoint + direction * x + Vector3.up * y;

            t += Time.deltaTime;
            yield return null;
        }
    }
}