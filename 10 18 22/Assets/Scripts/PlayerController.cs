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
    [SerializeField] int itemPlatformCount = 0;
    [SerializeField] float itemDistanceBetween = 0.5f;
    [SerializeField] float itemCubeDistanceBetween = 0.5f;

    [SerializeField] float DetectionRange = 1;
    [SerializeField] LayerMask Layer;
    [SerializeField] float JumpPower;



    [SerializeField] Transform sellTransform;
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

                hit.tag = "collected";
                hit.transform.parent = holdTransform;



                var seq = DOTween.Sequence();

                seq.Append(hit.transform.DOLocalJump(new Vector3(0, itemCount * itemDistanceBetween), 2, 1, 0.2f))
                .Insert(0, hit.transform.DOScale(1.25f, 0.1f))
                .Insert(0.1f, hit.transform.DOScale(0.3f, 0.2f));
                seq.AppendCallback(() => { hit.transform.localRotation = Quaternion.Euler(0, 0, 0); });
                itemCount++;

            }
            if (hit.CompareTag("SellGround"))
            {

                foreach (Transform item in holdTransform)
                {
                    item.transform.parent = sellTransform;
                    var seq = DOTween.Sequence();

                    seq.Append(item.transform.DOLocalJump(new Vector3(0, itemPlatformCount * itemCubeDistanceBetween), JumpPower, 1, 0.2f))
                    .Insert(0.2f, item.transform.DOScale(new Vector3(0.3f, 3f, 0.13f), 0.1f))
                    .Insert(0.1f, item.transform.DOScale(new Vector3(0.13f, 2f, 0.13f), 0.2f));
                    seq.AppendCallback(() => { item.transform.localRotation = Quaternion.Euler(0, 0, 0); });
                    itemPlatformCount++;
                    itemCount--;
                }
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
