using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region attributes

    [SerializeField]
    private AudioSource backSound;

    [SerializeField]
    private Sprite ImagePause;

    [SerializeField]
    private Sprite ImagePlay;

    [SerializeField]
    private Button imageButton;
    
    [SerializeField]
    private Vector3 startP1 = new Vector3(-11.0f, 0.5f, 0.0f);

    [SerializeField]
    private Vector3 startP2 = new Vector3(11.0f, 0.5f, 0.0f);

    [SerializeField]
    private GameObject player1;

    [SerializeField]
    private GameObject player2;

    [SerializeField]
    private Text Resultat;

    [SerializeField]
    private Text Score;

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    private float startTime;



    private bool destruction = false;
    [SerializeField]
    private LayerMask layerMaskWall;
    [SerializeField]
    private GameObject EternalExplosion;

    [SerializeField]
    private Text chrono;

    [SerializeField]
    private GameObject GameWall;

    [SerializeField]
    private GameObject BreakableWall;

    private List<GameObject> currentWallList = new List<GameObject>();
    private List<GameObject> currentBreakableWallList = new List<GameObject>();

    private int mapLimit = 12;

    [SerializeField]
    private GameObject ButtonStart;

    [SerializeField]
    private GameObject PanelBackground;

    private enum gameStatus { P1, P2, EnCours, Nul, EnLancement, Debut };

    private gameStatus currentGameStatus = gameStatus.Debut;

    public int botMode = 0;

    private List<PlayerMovementScript> scriptsPlayers = new List<PlayerMovementScript>();
    private List<BotRandomScript> scriptsBots = new List<BotRandomScript>();

    private List<String> textModes = new List<string>();

    

    [SerializeField]
    private GameObject botButton;

    [SerializeField]
    private Text botText;

    [SerializeField]
    private GameObject pauseText;

    #endregion

    void Start()
    {
        initScripts();
        initScene();
        updateTextBot();
    }

    private void Update()
    {
        pauseGame();
        updateTime();
    }

    /* met a jour le chrono et appelle la fonction destruction de jeu si conditions reunies
     * 
     */
    private void updateTime()
    {
        if (gameStatus.EnCours != currentGameStatus) return;

        var newTime = Time.time - startTime;
        string res = "";

        if (newTime > 3600)
        {
            res = (int)(newTime / 3600) + "h ";
            newTime = newTime % 3600;
        }
        if (newTime > 60)
        {
            res = res + (int)(newTime / 60) + "min ";
            newTime = newTime % 60;
        }
        if (newTime > 0)
        {
            newTime = Mathf.Round(newTime * 10f) / 10f;
            res = res + newTime + "s";
        }
        chrono.text = res;
        if (destruction == false && (Time.time - startTime) > 10.0f) StartCoroutine(endWorld());
    }

    /* fonction de destruction de la map
     * 
     */
    private IEnumerator endWorld()
    {
        destruction = true;
        int sens = 1;
        int tour = 0;
        int i = -mapLimit;
        while (destruction && tour!=(mapLimit*2)+1 && currentGameStatus == gameStatus.EnCours)
        {
            int j = -mapLimit+tour;
            while (destruction && j!=mapLimit*2 && currentGameStatus == gameStatus.EnCours)
            {
                Instantiate<GameObject>(EternalExplosion, new Vector3(i, 0.0f, j), Quaternion.identity);
                 yield return new WaitForSeconds(0.03f);

                if (sens == 1)
                {
                    if (j < (mapLimit - tour )) j++; 
                    else
                    {
                        sens = 2;
                    }
                } 
                if (sens == 2)
                {
                    if (i < (mapLimit - tour)) i++;
                    else
                    {
                        sens = 3;
                    }
                } 
                if (sens == 3)
                {
                    if (j > (-mapLimit + tour)) j--;
                    else
                    {
                        sens = 4;
                    }
                } 
                if (sens == 4)
                {
                    if (i > (-mapLimit + tour)) i--;
                    else
                    {
                        sens = 1;
                        tour++;
                        j = mapLimit * 2;
                    }
                }
            }

        }
    }

    /* initialise la liste de scripts pour chaque joueur
     * 
     */
    private void initScripts()
    {
        scriptsPlayers.Add(player1.GetComponent<PlayerMovementScript>());
        scriptsPlayers.Add(player2.GetComponent<PlayerMovementScript>());
        scriptsBots.Add(player1.GetComponent<BotRandomScript>());
        scriptsBots.Add(player2.GetComponent<BotRandomScript>());

        textModes.Add("Player1 vs Player2");
        textModes.Add("Player1 vs Bot");
        textModes.Add("Bot vs Player2");
        textModes.Add("Bot vs Bot");
    }

    /* desactive le scripts des joueurs et affiche la vue necessaire
     * 
     */
    private void initScene()
    {
        Score.text = scoreP1 + "-" + scoreP2 + " ";
        ButtonStart.SetActive(true);
        botButton.SetActive(true);
        PanelBackground.SetActive(true);
        disableMove();
    }

    /* desactive le mouvement des joueurs
     * 
     */
    private void disableMove()
    {
        for (int i = 0; i < scriptsPlayers.Count; i++)
        {
            scriptsPlayers[i].enabled = false;
        }
        for (int i = 0; i < scriptsBots.Count; i++)
        {
            scriptsBots[i].enabled = false;
        }
    }

    /* renitialise les valeurs de chaque joueurs (vitesse et bomb length
     * 
     */
    private void resetPlayersValues()
    {
        for (int i = 0; i < scriptsPlayers.Count; i++)
        {
            scriptsPlayers[i].resetValues();
        }
        for (int i = 0; i < scriptsBots.Count; i++)
        {
            scriptsBots[i].resetValues();
        }
    }

    /* active le mouvement des joueurs
     * 
     */
    private void enableMove()
    {
        if (botMode == 0)
        {
            scriptsPlayers[0].enabled = true;
            scriptsPlayers[1].enabled = true;
        }
        if (botMode == 1)
        {
            scriptsPlayers[0].enabled = true;
            scriptsBots[1].enabled = true;
        }
        if (botMode == 2)
        {
            scriptsBots[0].enabled = true;
            scriptsPlayers[1].enabled = true;
        }
        if (botMode == 3)
        {
            scriptsBots[0].enabled = true;
            scriptsBots[1].enabled = true;
        }
    }

    /* met ou enleve pause
     * 
     */
    private void pauseGame()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (gameStatus.EnCours == currentGameStatus)
            {
                if (Time.timeScale == 1)
                {
                    pauseText.SetActive(true);
                    disableMove();
                    Time.timeScale = 0;
                } else
                {
                    pauseText.SetActive(false);
                    enableMove();
                    Time.timeScale = 1;
                }
            }
        }
    }

    /* prepare la map ainsi que les joueurs et leurs positions et scripts
     * 
     */
    public void startGame()
    {
        if (currentGameStatus == gameStatus.EnLancement || currentGameStatus == gameStatus.EnCours) return;
        currentGameStatus = gameStatus.EnLancement;
        Time.timeScale = 1;
        removeBombAndExplosions();
        removeBonus();
        destruction = false;
        cleanAllWall();
        putWallOnMap();
        putBreakableOnMap();
        resetPlayers();
        resetPlayersValues();
        startTime = Time.time;
        PanelBackground.SetActive(false);
        botButton.SetActive(false);
        Resultat.text = "";
        currentGameStatus = gameStatus.EnCours;
    }

    /*nettoie tous les murs de la map
     * 
     */
    private void cleanAllWall()
    {
        foreach (GameObject obj in currentWallList)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in currentBreakableWallList)
        {
            Destroy(obj);
        }
        currentWallList.Clear();
        currentBreakableWallList.Clear();
    }

    /*instancie des murs dans la map
     * 
     */
    private void putWallOnMap()
    {
        for (int i = -mapLimit; i <= mapLimit; i+=2)
        {
            for (int j = -mapLimit; j <= mapLimit; j+=2)
            {
                if (UnityEngine.Random.Range(0.0f, 4.0f) < 3.0f)  currentWallList.Add(Instantiate<GameObject>(GameWall, new Vector3(i, 0.5f, j), Quaternion.identity));
            }
        }
    }


    /*instancie des murs que les joueurs peuvent detruire
     * 
     */
    private void putBreakableOnMap()
    {
        for (int i = -mapLimit+3; i <= mapLimit-3; i++)
        {
            
            for (int j = -mapLimit+1; j <= mapLimit-1; j++)
            {
                if (!Physics.CheckSphere(new Vector3(i, 0.5f, j), 0.45f, layerMaskWall) && UnityEngine.Random.Range(0.0f, 5.0f) < 4.0f) currentBreakableWallList.Add(Instantiate<GameObject>(BreakableWall, new Vector3(i, 0.5f, j), Quaternion.identity));
            }
        }
    }

    /* reactive les players et leurs positions/ scripts
     * 
     */
    private void resetPlayers()
    {
        player1.SetActive(true);
        player2.SetActive(true);
        player1.transform.SetPositionAndRotation(startP1, Quaternion.identity);
        player2.transform.SetPositionAndRotation(startP2, Quaternion.identity);
        enableMove();
    }

   

    /* change le botMode 
     * 
     */
    public void changeBotMode()
    {
        botMode++;
        if (botMode == 4) botMode = 0;
        updateTextBot();
    }

    /* met a jour l affichage du texte du botMode
     * 
     */
    private void updateTextBot()
    {
        botText.text = textModes[botMode];
    }

    /* Detruis toutes les bombes et explosions de la map
     * 
     */
    private void removeBombAndExplosions()
    {
        List<GameObject> explosions = (new List<GameObject>(GameObject.FindGameObjectsWithTag("Finish")));
        foreach (GameObject obj in explosions)
        {
            Destroy(obj);
        }
        explosions = (new List<GameObject>(GameObject.FindGameObjectsWithTag("Bomb")));
        foreach (GameObject obj in explosions)
        {
            Destroy(obj);
        };
    }

    /* Detruis tous les bonus
     * 
     */
    private void removeBonus()
    {
        List<GameObject> explosions = (new List<GameObject>(GameObject.FindGameObjectsWithTag("Bonus")));
        foreach (GameObject obj in explosions)
        {
            Destroy(obj);
        }
    }

    /* fin de partie,met le jeu en pause et affiche le vainqueur
     * 
     */
    public void onPlayerDeath(GameObject playerDead)
    {
        if (currentGameStatus == gameStatus.Nul || currentGameStatus == gameStatus.P1 || currentGameStatus == gameStatus.P2) return;


        if (playerDead.name == "Player2" && "Player" == playerDead.name)
        {
            Resultat.text = "Match nul";
            player1.SetActive(false);
            currentGameStatus = gameStatus.Nul;
            player2.SetActive(false);
        }
        else
        if ("Player" == playerDead.name)
        {
            scoreP2++;
            Resultat.text = "Joueur 2 a gagné";
            player1.SetActive(false);
            currentGameStatus = gameStatus.P2;
        }
        else
        if (playerDead.name == "Player2")
        {
            scoreP1++;
            Resultat.text = "Joueur 1 a gagné";
            player2.SetActive(false);
            currentGameStatus = gameStatus.P1;
        }


        initScene();

        Time.timeScale = 0;
    }


    public void clickMute()
    {
        if (backSound.isPlaying)
        {
            backSound.Pause();
            imageButton.image.sprite = ImagePlay;
        }
        else
        {
            backSound.Play();
            imageButton.image.sprite = ImagePause;
        }
    }
}
