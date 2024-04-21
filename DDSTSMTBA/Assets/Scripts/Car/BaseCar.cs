using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class BaseCar : MonoBehaviour
{
    public Transform top;
    public List<Transform> leftWheels;
    public List<Transform> rightWheels;
    public Transform frame;

    public float torque;

    public float roofHorizontalForce;

    public float force;

    
    public IEnumerator Explode()
    {
        //gameObject.AddComponent<Rigidbody>();
        //GetComponent<Rigidbody>().AddForce(Vector3.up*2, ForceMode.Impulse);

        yield return new WaitForSeconds(0.1f);

        //GetComponent<Rigidbody>().mass = 5.0f;

        frame.AddComponent<Rigidbody>();
        //frame.GetComponent<BoxCollider>().enabled = true;

        ExplodeRoof();
        ExplodeWheels(true);
        ExplodeWheels(false);
    }

    private void ExplodeRoof()
    {
        Vector3 forceVector = Vector3.up;
        //forceVector += new Vector3(Random.Range(-roofHorizontalForce, 0), 0, Random.Range(-roofHorizontalForce, roofHorizontalForce));
        forceVector *= force;

        //top.GetComponent<BoxCollider>().enabled = true;
        top.AddComponent<Rigidbody>();
        top.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
        //top.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-torque, torque), Random.Range(-torque, torque), Random.Range(-torque, torque)));
    }

    private void ExplodeWheels(bool left)
    {
        Vector3 forceVector = left ? -1 * transform.right : transform.right;

        float notBoring = 3.0f;

        //forceVector += new Vector3(0, Random.Range(0, notBoring), 0);
        forceVector *= 0; //6.0f;
        List<Transform> wheels = left ? leftWheels : rightWheels;

        foreach (Transform wheel in wheels)
        {
            //wheel.GetComponent<SphereCollider>().enabled = true;
            wheel.AddComponent<Rigidbody>();
            wheel.GetComponent<Rigidbody>().mass = 3.5f;
            wheel.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
            //wheel.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-torque, torque), Random.Range(-torque, torque), Random.Range(-torque, torque)));
        }
    }

}
