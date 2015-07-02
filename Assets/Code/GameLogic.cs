﻿//.     .       .  .   . .   .   . .    +  .
//  .     .  :     .    .. :. .___---------___.
//       .  .   .    .  :.:. _".^ .^ ^.  '.. :"-_. .
//    .  :       .  .  .:../:            . .^  :.:\.
//        .   . :: +. :.:/: .   .    .        . . .:\
// .  :    .     . _ :::/:               .  ^ .  . .:\
//  .. . .   . - : :.:./.                        .  .:\
//  .      .     . :..|:                    .  .  ^. .:|
//    .       . : : ..||        .                . . !:|
//  .     . . . ::. ::\(                           . :)/
// .   .     : . : .:.|. ######              .#######::|
//  :.. .  :-  : .:  ::|.#######           ..########:|
// .  .  .  ..  .  .. :\ ########          :######## :/
//  .        .+ :: : -.:\ ########       . ########.:/
//    .  .+   . . . . :.:\. #######       #######..:/
//     :: . . . . ::.:..:.\           .   .   ..:/
//  .   .   .  .. :  -::::.\.       | |     . .:/
//     .  :  .  .  .-:.":.::.\             ..:/
// .      -.   . . . .: .:::.:.\.           .:/
//.   .   .  :      : ....::_:..:\   ___.  :/
//   .   .  .   .:. .. .  .: :.:.:\       :/
//     +   .   .   : . ::. :.:. .:.|\  .:/|
//     .         +   .  .  ...:: ..|  --.:|
//.      . . .   .  .  . ... :..:.."(  ..)"
// .   .       .      :  .   .: ::/  .  .::\


//      __       ___  ___  ___  ___      ___       ___      ___       __        ______    
//     /""\     |"  \/"  ||"  \/"  |    |"  |     |"  \    /"  |     /""\      /    " \   
//    /    \     \   \  /  \   \  /     ||  |      \   \  //   |    /    \    // ____  \  
//   /' /\  \     \\  \/    \\  \/      |:  |      /\\  \/.    |   /' /\  \  /  /    ) :) 
//  //  __'  \    /   /     /   /        \  |___  |: \.        |  //  __'  \(: (____/ //  
// /   /  \\  \  /   /     /   /        ( \_|:  \ |.  \    /:  | /   /  \\  \\        /   
//(___/    \___)|___/     |___/          \_______)|___|\__/|___|(___/    \___)\"_____/    
//--------------------------------------------------------------------------------
using UnityEngine.Advertisements;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
//-------------------------------------------------------------------------------
public class GameLogic : MonoBehaviour
{
    //-------------------------------------------------------------------------------
    public enum GameState
    {
        NotStarted,
        OnGoing,
        Paused,
        Ended
    };
    //-------------------------------------------------------------------------------
    private static GameState m_eState;
    public static GameState State
    {
        get { return m_eState; }
        set { m_eState = value; }
    }
    //-------------------------------------------------------------------------------
    private WaveHandler m_WaveHandler;
    private FormationHandler m_FormationHandler;
    private InputHandler m_InputHandler;
    public MenuController MC;
    //-------------------------------------------------------------------------------
    public GameObject ScorePanel;
    private Animator m_SPAnimator;
    public GameObject ButtonPanel;
    public Animator m_TPAnimator;
    //-------------------------------------------------------------------------------
    private bool m_bShowAds = false;
    private int m_iAdCounter = 0;
    private int AD_COUNT_PER_DEATH = 4;
    private bool m_bAdCounter = false;
    //-------------------------------------------------------------------------------
    public int m_iScore;
    //-------------------------------------------------------------------------------
    public void Start()
    {
        m_eState = GameState.NotStarted;
        Application.targetFrameRate = 30;

        m_WaveHandler = GetComponent<WaveHandler>();
        m_FormationHandler = GetComponent<FormationHandler>();

        m_InputHandler = GetComponent<InputHandler>();

        Advertisement.Initialize("49224");

        m_SPAnimator = ScorePanel.GetComponent<Animator>();
        m_iAdCounter = PlayerPrefs.GetInt("AdCounter", 0);
    }
    //-------------------------------------------------------------------------------
    public void Restart()
    {
        if(!(GameLogic.State == GameState.OnGoing))
        {
            
            //ScorePanel.SetActive(false);
            //m_SPAnimator.SetBool("gameEnded", false);
            m_SPAnimator.Play("ScorePanelOutAnim");
           
            ButtonPanel.SetActive(false);
        }
        m_TPAnimator.Play("TopPanelEnterAnim");
        GameLogic.State = GameLogic.GameState.NotStarted;
        m_WaveHandler.Restart();
        m_InputHandler.Restart();
        m_FormationHandler.Restart();

        m_iScore = 0;
        MC.UpdateScoreboard(m_iScore);

       
    }
    //-------------------------------------------------------------------------------
    public void Update()
    {
        if (State == GameState.OnGoing)
            m_bAdCounter = false;
        
        if (State == GameLogic.GameState.Ended && !m_bAdCounter)
        {
            m_iAdCounter++;
            m_bAdCounter = true;
            PlayerPrefs.SetInt("AdCounter", m_iAdCounter);
        }
        if (m_iAdCounter % AD_COUNT_PER_DEATH == 0 && m_iAdCounter != 0)
        {
            m_bShowAds = true;
            m_iAdCounter = 0;
            PlayerPrefs.SetInt("AdCounter", m_iAdCounter);
        }
        if (m_bShowAds)
        {
            //InterstitialAd ins = new InterstitialAd("ca-app-pub-7907761596585940/5317772971");
            //AdRequest request = new AdRequest.Builder().Build();
            //// Load the interstitial with the request.
            //ins.LoadAd(request);
            //if (ins.IsLoaded())
            //{
            //    ins.Show();
            //    m_bShowAds = false;
            //}
            if (Advertisement.isReady())
            {
                Advertisement.Show();
                m_bShowAds = false;
            }
        }
    }
    //-------------------------------------------------------------------------------
    public void IncrementScore(int s)
    {
        m_iScore += s;
        MC.UpdateScoreboard(m_iScore);
    }
    //-------------------------------------------------------------------------------

}
