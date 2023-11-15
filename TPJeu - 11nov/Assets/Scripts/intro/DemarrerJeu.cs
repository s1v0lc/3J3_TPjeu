using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemarrerJeu : MonoBehaviour
{
    public GameObject ecranNoir;
    // Update is called once per frame
    public void Demarrer()
    {
        ecranNoir.SetActive(true);
        Invoke("ChargerProchaineScene", 1f);
    }

    void ChargerProchaineScene()
    {
        SceneManager.LoadScene(1);
    }
}
