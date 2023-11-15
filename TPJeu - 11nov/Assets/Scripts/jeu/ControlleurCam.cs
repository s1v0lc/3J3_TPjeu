using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gère les caméras
public class ControlleurCam : MonoBehaviour
{
    public GameObject[] cams; // Tableau des caméras
    public GameObject cible; // objet qui sera visé par la caméra
    public Vector3 distanceCamera; // distance entre la cible et la caméra

    // Start is called before the first frame update
    void Start()
    {
        // cam par défaut au démarrage
        ActiverCam(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Gestion Caméra 3e personne
        cams[0].transform.position = cible.transform.position + distanceCamera;
        cams[0].transform.LookAt(cible.transform.position);
        // Gestion Caméra Drone
        if(cams.Length > 1)
        {
            cams[2].transform.position = new Vector3(0f, cible.transform.position.y + distanceCamera.y,  cible.transform.position.z + distanceCamera.z);
            cams[2].transform.LookAt(new Vector3(0f, cible.transform.position.y, cible.transform.position.z));
        }

        // si le joueur appuie sur 1
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActiverCam(0); // cam 3e personne
        }
        // si le joueur appuie sur 2
        if(Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            ActiverCam(1); // cam 1ère personne
        }
        // si le joueur appuie sur 3
        if(Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            ActiverCam(2); // cam drone
        }
    }

    // Active la bonne caméra selon la touche pressée
    void ActiverCam(int index)
    {
        // Désactive toutes les caméras
        foreach(var cam in cams)
        {
            cam.SetActive(false);  
        }
        // puis réactive celle demandée par l'index
        cams[index].SetActive(true); 
    }
}
