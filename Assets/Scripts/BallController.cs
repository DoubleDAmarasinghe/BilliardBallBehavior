using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float Power;
    [SerializeField] private float MaxPower;
    [SerializeField] private float MinSpeed;
    private Rigidbody rb;
    private LineRenderer lr;
    private float ShotPower;
    private Vector3 StartPosition, EndPosition, Direction;
    private bool CanShot, ShotStarted;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponentInChildren<LineRenderer>();
        CanShot = true;
        rb.sleepThreshold = MinSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanShot)
        {
            StartPosition = MousePositionInWorld();
            ShotStarted = true;
            lr.gameObject.SetActive(true);
            lr.SetPosition(0, lr.transform.localPosition);
        }
        if (Input.GetMouseButton(0) && ShotStarted)
        {
            EndPosition = MousePositionInWorld();
            ShotPower = Mathf.Clamp(Vector3.Distance(EndPosition, StartPosition), 0, MaxPower);
            lr.SetPosition(1, transform.InverseTransformPoint(EndPosition));
        }
        if (Input.GetMouseButtonUp(0) && ShotStarted)
        {
            CanShot = false;
            ShotStarted = false;
            lr.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!CanShot)
        {
            Direction = StartPosition - EndPosition;
            rb.AddForce(Vector3.Normalize(Direction) * ShotPower *Power, ForceMode.Impulse);
            StartPosition = EndPosition = Vector3.zero;
        }

        if (rb.IsSleeping())
        {
            CanShot = true;
        }
    }

    private Vector3 MousePositionInWorld()
    {
        Vector3 Position = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            Position = hit.point;
        }
        return Position;

    }
 
}
