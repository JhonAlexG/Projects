using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicsPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] Transform holdArea;
    [SerializeField] private GameObject heldObj;
    private CodeBlock codeBlock;
    [SerializeField] private Rigidbody heldObjRb;
    [SerializeField] private LayerMask pickupLayerMask;

    [Space]

    [Header(" Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;
    public bool IsholdingObj = false;

    [Header("Canvas Pointer")]
    public Image pointerImg;

    private void Start()
    {
        pointerImg.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, pickupRange, pickupLayerMask))
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                Drop();
            }
        }
        if (heldObj != null)
        {
            MoveObject();
        }
    }

    void PickupObject(GameObject pickObj)
    {
        Rigidbody rb = pickObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            codeBlock = pickObj.GetComponent<CodeBlock>();

            if (codeBlock != null)
            {
                pointerImg.enabled = true;
                heldObjRb = rb;
                heldObjRb.useGravity = false;
                heldObjRb.velocity = Vector3.zero;
                heldObjRb.drag = 20;
                heldObjRb.constraints = RigidbodyConstraints.FreezeRotation;

                codeBlock.isHolding = true;
                heldObjRb.transform.parent = holdArea;
                heldObj = pickObj;
            }
            else
            {
                // Si el objeto no tiene el componente CodeBlock, simplemente lo recogemos sin hacer nada especial.
                pointerImg.enabled = true;
                heldObjRb = rb;
                heldObjRb.useGravity = false;
                heldObjRb.velocity = Vector3.zero;
                heldObjRb.drag = 20;
                heldObjRb.constraints = RigidbodyConstraints.FreezeRotation;
                heldObjRb.transform.parent = holdArea;
                heldObj = pickObj;
                IsholdingObj = true;
            }
        }
    }
    void Drop()
    {

        if (codeBlock != null)
        {
            heldObjRb.useGravity = true;
            heldObjRb.drag = 1;
            heldObjRb.constraints = RigidbodyConstraints.None;

            heldObjRb.transform.parent = null;
            heldObj = null;
            pointerImg.enabled = false;
            codeBlock.isHolding = false;
        }
        else
        {
            heldObjRb.useGravity = true;
            heldObjRb.drag = 1;
            heldObjRb.constraints = RigidbodyConstraints.None;

            heldObjRb.transform.parent = null;
            heldObj = null;
            pointerImg.enabled = false;
            IsholdingObj = false;
        }

    }

    void MoveObject()
    {
        if (Vector3.Distance(holdArea.position, heldObj.transform.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position).normalized;
            heldObjRb.AddForce(moveDirection * pickupForce);

            // Alinea la rotación del objeto con la del holdArea
            Quaternion targetRotation = Quaternion.LookRotation(holdArea.forward, Vector3.up);
            heldObjRb.MoveRotation(targetRotation);
        }
    }
}
