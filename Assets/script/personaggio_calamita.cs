using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class personaggio_calamita : MonoBehaviour
{
   public float velocita_calamita_animazione=5f;
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Moneta") {
            other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position, transform.position, Time.deltaTime * velocita_calamita_animazione);
        }
    }

}
