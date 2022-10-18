using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerController : MonoBehaviour
{
    [Header("Joystick Settings")]
    float horizontal;
    float vertical;
    [SerializeField] FloatingJoystick Joystick;
    Vector3 Direction;
    float CurrentTurnAngle;
    float SmoothTurnTime = 0.1f;
    Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] Transform DetectTransform;
    [SerializeField] Transform holdTransform;
    [SerializeField] int itemCount = 0;
    [SerializeField] float itemDistanceBetween = 0.5f;
    [SerializeField] float DetectionRange = 1;
    [SerializeField] LayerMask Layer;
    Collider[] colliders;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.6f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(DetectTransform.position, DetectionRange);

    }

    private void Update()
    {
        colliders = Physics.OverlapSphere(DetectTransform.position, DetectionRange, Layer);
        foreach (var hit in colliders)
        {
            if (hit.CompareTag("collectable"))
            {
                Debug.Log(hit.name);
                hit.tag = "collected"; //deydiyi objenin tagini deyismek
                hit.transform.parent = holdTransform;

                // var seq = DOTween.Sequence();

                // seq.Append(hit.transform.DOLocalJump(new Vector3(0,itemCount*itemDistanceBetween), 2, 1, 0.2f))
                // .Join(hit.transform.DOScale(0.5f,0.2f));
                // seq.AppendCallback(() => {hit.transform.localRotation= Quaternion.Euler(0,0,0);} );
                // itemCount++;
                
                var seq = DOTween.Sequence();

                seq.Append(hit.transform.DOLocalJump(new Vector3(0,itemCount*itemDistanceBetween), 2, 1, 0.2f))
                .Insert(0,hit.transform.DOScale(1.25f,0.1f)) //obje goturende boyuyur
                .Insert(0.1f,hit.transform.DOScale(0.3f,0.2f)); //obje boyuyenden sonra kicilir
                seq.AppendCallback(() => {hit.transform.localRotation= Quaternion.Euler(0,0,0);} );
                itemCount++;
            }
        }




    }



    private void FixedUpdate()
    {
        horizontal = Joystick.Horizontal;
        vertical = Joystick.Vertical;

        Direction = new Vector3(horizontal, 0, vertical);

        if (Direction.magnitude > 0.01f)
        {
            float TargetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
            float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetAngle, ref CurrentTurnAngle, SmoothTurnTime);
            transform.rotation = Quaternion.Euler(0, Angle, 0);

            rb.MovePosition(transform.position + (Direction * speed * Time.deltaTime));
        }

    }
}
