using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Gère les routes (génération et suppression)
public class ControlleurRoute : MonoBehaviour
{
    Transform positionJoueur; // variable pour enregistrer la position du joueur
    // Gabarit d'une route
    [Serializable]
    public struct route
    {
        public GameObject prefabRoute; // prefab route
        public int difficulte; // niveau de difficulté
    }
    public List<route> routes; // liste des différentes routes à générer
    float longueurPrefab = 75f;
    int nbPrefabMax = 4; // Pour s'assurer de ne pas surchager le jeu
    float spawnZ = 0f; // où placer la prochaine instantiation
    float zoneActive = 100f; // zone où le joueur se trouve
    List<GameObject> routesActives = new List<GameObject>(); // liste regroupant les routes actives
    int dernierIndexRoute = 0; // index de la dernière route générée
    public float difficulte; // niveau de difficulté actuel
    static public float score; // score du joueur

    // Start is called before the first frame update
    void Start()
    { 
        // Enregistre la position du joueur
        positionJoueur = GameObject.FindGameObjectWithTag("voitureJoueur").transform;
        // Génération initiale de la route
        for (int i = 0; i < nbPrefabMax; i++)
        {
            if(i < 1) // la première route
            {
                SpawnRoute(0); // route par défaut
            }
            else
            {
                SpawnRoute(1); // route aléatoire
            }
        }
        // Actualisation du score selon le temps
        InvokeRepeating("ActualiserScore", 2.5f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
        // Actualisation de la difficulté selon le score
        // difficulte = Mathf.Round(score/1000);
        // Gestion dynamique de la route si le joueur dépasse une certaine position en z
        if(positionJoueur.position.z - zoneActive > (spawnZ - nbPrefabMax * longueurPrefab))
        {
            SpawnRoute(1); // Génération
            SupprimerRoute(); // Suppression
        }

        // Si la voiture tombe de la map TEMPORAIRE
        if(positionJoueur.position.y < -30)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    // Génération dynamique de la route
    void SpawnRoute(int index)
    {
        GameObject routeClonee;
        if(index == 0) // génère la route par défaut
        {
            routeClonee = Instantiate(routes[0].prefabRoute) as GameObject; // clone la route 
        }
        else // génère une route aléatoire
        {
            routeClonee = Instantiate(routes[IndexRouteAleatoire()].prefabRoute) as GameObject; // clone la route 
        }
            routeClonee.SetActive(true); // active le clone puisque l'original est désactivé
            routeClonee.transform.SetParent(transform); // place la route clonée comme enfant du controlleur
            routeClonee.transform.position = Vector3.forward * spawnZ; // place la route au bon endroit 
            spawnZ += longueurPrefab; // Incrémente la variable de spawn
            routesActives.Add(routeClonee); // ajout de la route au tableau des routes actives
    }

    // Suppression dynamique de la route
    void SupprimerRoute()
    {
        Destroy (routesActives[0]); // destruction de la première route
        routesActives.RemoveAt(0); // retrait de la route détruite
    }

    // Retourne aléatoirement un entier qui correspond au prefab d'une route
    // toutefois, ce nombre est toujours différent du dernier
    // et celui-ci correspond au niveau de difficulté actuel
    int IndexRouteAleatoire()
    {
        // Si le tableau de prefab est inférieur ou égal à 1
        if (routes.Count <= 1)
        {
            return 0; // le prefab en index 0 est la route par défaut sans obstacles
        }
        // Sinon, un nombre aléatoire est généré jusqu'à ce qu'il ne soit plus identique au précédent
        // Ou qu'il ne difficulté ne soit pas suffisante
        int IndexRouteAleatoire = dernierIndexRoute; 
        while(IndexRouteAleatoire == dernierIndexRoute || difficulte > routes[IndexRouteAleatoire].difficulte)
        {
            // l'écart aléatoire dépend du nb de prefab de routes
            IndexRouteAleatoire = UnityEngine.Random.Range(1,routes.Count);
        }
        dernierIndexRoute = IndexRouteAleatoire; // Actualisela variable en dehors de la fonction
        return IndexRouteAleatoire; // Retourne le nouvel index

    }

    void ActualiserScore()
    {
        score++;
    }
}
