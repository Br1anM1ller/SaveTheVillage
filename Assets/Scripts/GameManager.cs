using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ImageTimer HarvestTimer;
    public ImageTimer EatingTimer;
    public Image RaidTimerImg;
    public Image PeasantTimerImg;
    public Image WarriorTimerImg;

    public Button peasantButton;
    public Button warriorButton;

    public Text resourcesText;
    public Text resultWheatText;
    public Text resultWarriorsText;
    public Text resultRaidsText;
    public Text resultPeasantsText;

    public int peasantCount;
    public int warriorsCount;
    public int wheatCount;

    public int wheatPerPeasant;
    public int wheatTowarriors;

    public int peasantCost;
    public int warriorCost;

    public float peasantCreateTime;
    public float warriorCreateTime;
    public float raidMaxTime;
    public int raidIncrease;
    public int nextRaid;

    public GameObject GameOverScreen;
    public GameObject GameVictoryScreen;
    public GameObject GamePausedScreen;

    public AudioSource backgroundMusic;
    public AudioSource buttonClickSound;
    public AudioSource harvestSound;
    public AudioSource raidSound;
    public AudioSource eatSound;
    public AudioSource peasantReadySound;
    public AudioSource warriorReadySound;

    private float peasantTimer = -2;
    private float warriorTimer = -2;
    private float raidTimer;

    private int raidCycles = 0;
    private bool paused;

    void Start()
    {
        UpdateText();
        raidTimer = raidMaxTime;
        backgroundMusic.Play();
    }

    void Update()
    {
        raidTimer -= Time.deltaTime;
        RaidTimerImg.fillAmount = 1 - (raidTimer / raidMaxTime);

        if (raidTimer <= 0)
        {
            raidTimer = raidMaxTime;
            if (raidCycles >= 3)
            {
                warriorsCount -= nextRaid;
                nextRaid += raidIncrease;
                raidSound.Play();
            }
            else
            {
                raidCycles++;
            }
        }

        if (HarvestTimer.Tick)
        {
            wheatCount += peasantCount * wheatPerPeasant;
            harvestSound.Play();
        }

        if (EatingTimer.Tick)
        {
            wheatCount -= warriorsCount * wheatTowarriors;
            eatSound.Play();
        }

        if (peasantTimer > 0)
        {
            peasantTimer -= Time.deltaTime;
            PeasantTimerImg.fillAmount = 1 - (peasantTimer / peasantCreateTime);
        }
        else if (peasantTimer > -1)
        {
            PeasantTimerImg.fillAmount = 1;
            peasantButton.interactable = true;
            peasantCount += 1;
            peasantTimer = -2;

            if (peasantReadySound != null)
            {
                peasantReadySound.Play();
            }
        }

        if (warriorTimer > 0)
        {
            warriorTimer -= Time.deltaTime;
            WarriorTimerImg.fillAmount = 1 - (warriorTimer / warriorCreateTime);
        }
        else if (warriorTimer > -1)
        {
            WarriorTimerImg.fillAmount = 1;
            warriorButton.interactable = true;
            warriorsCount += 1;
            warriorTimer = -2;

            if (warriorReadySound != null)
            {
                warriorReadySound.Play();
            }
        }

        UpdateText();

        if (wheatCount >= 500 && warriorsCount > 0)
        {
            Time.timeScale = 0;
            GameOverScreen.SetActive(false);
            GameVictoryScreen.SetActive(true);
        }
        else if (warriorsCount < 0)
        {
            Time.timeScale = 0;
            GameOverScreen.SetActive(true);
            GameVictoryScreen.SetActive(false);

            resultWheatText.text = wheatCount.ToString();
            resultWarriorsText.text = warriorsCount.ToString();
            resultRaidsText.text = raidCycles.ToString();
            resultPeasantsText.text = peasantCount.ToString();
        }
    }

    public void CreatePeasant()
    {
        if (wheatCount >= peasantCost)
        {
            wheatCount -= peasantCost;
            peasantTimer = peasantCreateTime;
            peasantButton.interactable = false;
            buttonClickSound.Play();
        }
    }

    public void CreateWarrior()
    {
        if (wheatCount >= warriorCost)
        {
            wheatCount -= warriorCost;
            warriorTimer = warriorCreateTime;
            warriorButton.interactable = false;
            buttonClickSound.Play();
        }
    }

    private void UpdateText()
    {
        resourcesText.text = peasantCount + "\n" + warriorsCount + "\n\n" + wheatCount + "\n\n" + nextRaid;
    }

    public void RestartGame()
    {
        peasantCount = 3;
        warriorsCount = 0;
        wheatCount = 10;
        raidCycles = 0;
        raidTimer = raidMaxTime;
        nextRaid = 0;

        peasantTimer = -2;
        warriorTimer = -2;
        peasantButton.interactable = true;
        warriorButton.interactable = true;
        PeasantTimerImg.fillAmount = 0;
        WarriorTimerImg.fillAmount = 0;

        UpdateText();
        Time.timeScale = 1;
        GameOverScreen.SetActive(false);
        GameVictoryScreen.SetActive(false);
    }

    public void PauseGame()
    {
        if (paused)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
            GamePausedScreen.SetActive(true);
        }

        paused = !paused;
        buttonClickSound.Play();
    }

    public void TimeDown()
    {
        Time.timeScale = 0.2f;
        buttonClickSound.Play();
    }

    public void TimeUp()
    {
        Time.timeScale = 5;
        buttonClickSound.Play();
    }

    public void TimeNormal()
    {
        Time.timeScale = 1;
        buttonClickSound.Play();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        GamePausedScreen.SetActive(false);
        paused = false;
        buttonClickSound.Play();
    }
}

