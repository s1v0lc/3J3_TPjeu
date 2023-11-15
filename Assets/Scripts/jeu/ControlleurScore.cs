using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Gestion du compteur du score
public class ControlleurScore : MonoBehaviour
{
    public TextMeshProUGUI score;
    // Start is called before the first frame update
    void Start()
    {
        // Fait apparaître le texte après un cours délai
        Invoke("afficherScore", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // Une fois l'animation d'intro terminée
        if(AnimateurCam3e.animationTerminee)
        {
            // Incrémente le compteur selon le score
            score.text = "" + ControlleurRoute.score;
        }
    }

    // Active le gameobject et affiche 0
    void afficherScore()
    {
        score.text = "0";
        score.enabled = true;
    }
}
