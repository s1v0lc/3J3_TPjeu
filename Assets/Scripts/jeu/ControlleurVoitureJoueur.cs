using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.SceneManagement;

// Gère la voiture du joueur (déplacements, animations, collisions)
public class ControlleurVoitureJoueur : MonoBehaviour
{
    public AudioSource crashSFX;
    // Variables Déplacements
    public float vitesse = 30f; // force d'accelération
    public float brake = 50f; // force de frein
    float axeV; // pour avancer/reculer
    float axeH; // pour tourner
    public float sensibiliteRotation; 
    public float angleMaxRoue; // angle maximum de rotation des roues

    // Variables Rigibody
    Rigidbody rb;
    public Vector3 centreMasse;

    // Propriété des roues et des phares
    public enum Axe
    {
        Avant,
        Arriere
    }

    // Gabarit d'une roue
    [Serializable]
    public struct roue
    {
        public GameObject modeleRoue; // Mesh
        public WheelCollider colliderRoue; // Collider
        public Axe axe; // Essieu
    }
    public List<roue> roues; // liste des roues

    // Gabarit d'un phare
    [Serializable]
    public struct phare
    {
        public GameObject objPhare; // gameObject
        public Axe axe; // avant/arrière

    }
    public List<phare> phares; // liste des phares
    bool pharesAllumes = false; // éteint ou allumé

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centreMasse; // Pour éviter que la voiture se renverse
    }

    // Update is called once per frame
    void Update()
    {
        // Gestion des phares avants avec la touche E
        // Désactivation
        if(pharesAllumes && Input.GetKeyDown(KeyCode.E))
        {
            pharesAllumes = !pharesAllumes; // Inversion de la booléenne
            foreach(var phare in phares)
            {
                if(phare.axe == Axe.Avant) // Uniquement les phares avants
                {
                    phare.objPhare.SetActive(false);
                }
            }
        }
        // Activation
        else if(!pharesAllumes && Input.GetKeyDown(KeyCode.E))
        {
            pharesAllumes = !pharesAllumes; // Inversion de la booléenne
            foreach(var phare in phares)
            {
                if(phare.axe == Axe.Avant) // Uniquement les phares avants
                {
                    phare.objPhare.SetActive(true);
                }
            }                
        }
    }

    void FixedUpdate()
    {
        // Contrôles
        if(AnimateurCam3e.animationTerminee) 
        // les controles sont bloqués jusqu'à la fin de l'animation de la caméra
        {
            axeV = Input.GetAxis("Vertical"); // W/S
            axeH = Input.GetAxis("Horizontal"); // A/D
        }

        // Pitch audio du moteur
        GetComponent<AudioSource>().pitch = 1f + axeV/3;
        // Gestion des roues
        foreach(var roue in roues) 
        {
            // Mouvement des roues
            roue.colliderRoue.motorTorque = axeV * vitesse * 600;

            // Rotation des roues avants
            if(roue.axe == Axe.Avant)
            {
                float angleRoues = axeH * sensibiliteRotation * angleMaxRoue; // angle de rotation des roues
                roue.colliderRoue.steerAngle = Mathf.Lerp(roue.colliderRoue.steerAngle, angleRoues, 0.6f);
            }

            // Animation des roues
            // les coordonnées de position et de rotation de chaque roue sont imprimées
            // dans leurs variables respectives puis assignées à leur mesh respectifs
            Vector3 position;
            Quaternion rotation;
            roue.colliderRoue.GetWorldPose(out position, out rotation);
            roue.modeleRoue.transform.position = position;
            roue.modeleRoue.transform.rotation = rotation;

            // Freinage
            if(axeV == 0 || Input.GetKey(KeyCode.Space)) // si le joueur n'appuie pas sur W/S ou qu'il appuie sur SPACE
            {
                roue.colliderRoue.brakeTorque = 2500 * brake; // applique une force de freinage
            }
            else
            {
                roue.colliderRoue.brakeTorque = 0; // retire la force de freinage
            }
        }

        // Gestion des phares arrières
        foreach(var phare in phares)
        {
            // Allume les phares arrières au freinage
            if(phare.axe == Axe.Arriere && (Input.GetKey(KeyCode.Space) || axeV == 0))
            {
                phare.objPhare.SetActive(true);
            }
            // Éteint les phares arrières dès que la voiture avance
            else if (phare.axe == Axe.Arriere)
            {
                phare.objPhare.SetActive(false);
            }
        }
    }

    // Gestion des collisions
    void OnCollisionEnter(Collision infoCollision)
    {
        // son de la collision
        if(infoCollision.gameObject.tag != "route" && infoCollision.gameObject.tag != "pieton" && infoCollision.gameObject.tag != "pietonEmpty")
        {
            crashSFX.time = 0.1f;
            crashSFX.Play();
        }
        // Si le joueur entre en collision avec un obstacle mouvant
        if(infoCollision.gameObject.tag == "obstacle")
        {
            // Désactive l'animator pour que l'objet cesse son animation
            // pour que le rigidbody simule un accident de voiture
            infoCollision.gameObject.GetComponent<Animator>().enabled = false;
        }
        // si le joueur happe un piéton
        if(infoCollision.gameObject.tag == "pieton")
        {
            // Déclenche l'animation de mort
            infoCollision.gameObject.GetComponent<Animator>().SetBool("mort", true);
            // son de la collision
            infoCollision.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    void OnTriggerEnter(Collider infoCollision)
    {
        // Désactive l'animator si le joueur happe un piéton
        if(infoCollision.gameObject.tag == "pietonEmpty")
        {
            infoCollision.gameObject.GetComponent<Animator>().enabled = false;
            // infoCollision.gameObject.GetComponentInChildren<Animator>().SetBool("marche", false); // ne fonctionne pas
        }
    }
}
