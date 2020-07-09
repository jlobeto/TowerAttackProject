using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMinion : Minion
{
    Rigidbody rigidBody;
    Collider _col;
    bool _isFalling;

    protected override void Start()
    {
        base.Start();
        rigidBody = GetComponent<Rigidbody>();

        if (rigidBody == null)
            Debug.LogError(minionType + " does not have rigidbody");

        _col = GetComponent<Collider>();
    }


    protected override void Update()
    {
        base.Update();
        
        if(!_isFalling  && minionManager.MinionHasToFall(this, pNextNode))
        {
            _isFalling = true;
            rigidBody.useGravity = true;
            rigidBody.isKinematic = false;
            _col.isTrigger = false;

            if (infoCanvas != null)
                Destroy(infoCanvas.gameObject);
        }

        if(_isFalling)
        {
            rigidBody.AddForce(Vector3.down * 10, ForceMode.Acceleration);
            rigidBody.AddTorque(transform.forward * 5, ForceMode.Force);
            rigidBody.AddTorque(transform.up * 3, ForceMode.Force);
        }
            

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isFalling) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("LevelSkillFloor"))
        {
            DoExplotion();
        }
    }
    

}
