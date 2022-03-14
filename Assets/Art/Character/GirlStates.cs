using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlStates : MonoBehaviour
{
    public GameObject girlRig;
    public Animator girlAnim;

    public bool activateRagdoll = false;
    public Vector3 inheritedVelocity;


    private void Awake()
    {
        girlAnim = GetComponent<Animator>();

        GetRagdollParts();

        RagdollModeOff();
    }


    Collider[] ragdollColliders;
    Rigidbody[] limbRigids;
    void GetRagdollParts()
    {
        ragdollColliders = girlRig.GetComponentsInChildren<Collider>();
        limbRigids = girlRig.GetComponentsInChildren<Rigidbody>();
    }

    public void RagdollModeOn()
    {
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rigid in limbRigids)
        {
            rigid.isKinematic = false;
            rigid.AddForce(inheritedVelocity, ForceMode.Impulse);
        }

        girlAnim.enabled = false;
    }

    public void RagdollModeOff()
    {
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rigid in limbRigids)
        {
            rigid.isKinematic = true;
        }

        girlAnim.enabled = true;
    }
}
