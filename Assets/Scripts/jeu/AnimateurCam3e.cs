using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Anime la caméra au début de la partie puis active le controlleur de caméra
public class AnimateurCam3e : MonoBehaviour
{
    public GameObject cible; // endroit visé par la caméra
    public Vector3 distanceInitiale; // distance initiale entre la cible et la cam
    public Vector3 distanceFinale; // distance finale entre la cible et la cam
    float transition; // % de la transition entre les distances
    float dureeAnimation = 2.0f; // temps de l'animation
    static public bool animationTerminee = false;
    public GameObject ControlleurCam; // activé après l'animation

    // Update is called once per frame
    void Update()
    {
        // Si l'animation est terminée
        if(animationTerminee)
        {
            ControlleurCam.SetActive(true);
            CancelInvoke();
        }
        // Sinon on anime
        else
        {
            AnimerCam();
            Invoke("terminerAnimation", 2.5f);
        }
    }

    // Animation de la position de la caméra
    void AnimerCam()
    {
        // endroit visé par la cam
        transform.LookAt(cible.transform.position); 
        // position de la cam
        if(transition > 1.0f)
        {
            transform.position = cible.transform.position + distanceFinale;
        }
        else
        {
            transform.position = Vector3.Lerp(cible.transform.position + distanceInitiale, cible.transform.position + distanceFinale, transition);
            transition += Time.deltaTime * 1 / dureeAnimation;
        }
    }

    // Change la booléeenne une fois que l'animation est terminé
    void terminerAnimation()
    {
        animationTerminee = true;
    }
}
