using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[Serializable]
public class Options
{
    public string Car_Name;
    public Sprite Car_Image;
}

public class GameController : MonoBehaviour
{
    public List<Options> CarsList;
    public List<Image> Cars;

    private int selectedColorIndex;
    private int scoreCounter;

    public TextMeshProUGUI carLabelText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI scoreDisplay;

    private float roundDuration = 5f;
    private bool isCountdownActive = false;

    public GameObject victoryPanel;
    public GameObject defeatPanel;

    public AudioSource victorySound;
    public AudioSource defeatSound;

    void Start()
    {
        AssignRandomColors();
    }

    void Update()
    {
        HandleCountdown();
    }

    private void AssignRandomColors()
    {
        HashSet<int> selectedIndices = new HashSet<int>();

        // انتخاب چهار عدد تصادفی و غیرتکراری
        while (selectedIndices.Count < 4)
        {
            int randomIndex = UnityEngine.Random.Range(0, CarsList.Count);
            selectedIndices.Add(randomIndex);
        }

        int[] indices = new int[4];
        selectedIndices.CopyTo(indices);

        // اختصاص تصاویر خودروها به عناصر Cars
        for (int i = 0; i < 4; i++)
        {
            Cars[i].sprite = CarsList[indices[i]].Car_Image;
        }

        // انتخاب یکی از چهار تصویر به‌صورت تصادفی
        int selectedRandomIndex = UnityEngine.Random.Range(0, 4);
        selectedColorIndex = indices[selectedRandomIndex];
        carLabelText.text = CarsList[selectedColorIndex].Car_Name;

        Invoke("ActivateRound", 1f);
    }

    private void ActivateRound()
    {
        isCountdownActive = true;
    }

    private void HandleCountdown()
    {
        if (isCountdownActive)
        {
            roundDuration -= Time.deltaTime;

            if (roundDuration <= 0)
            {
                UpdateScore(-1);

                if (scoreCounter > 0)
                {
                    roundDuration = 5f;
                    AssignRandomColors();
                }
            }

            countdownText.text = "Timer: " + roundDuration.ToString("N3");
        }
    }

    private void UpdateScore(int scoreChange)
    {
        scoreCounter += scoreChange;

        if (scoreCounter >= 20)
        {
            isCountdownActive = false;
            victoryPanel.SetActive(true);
            scoreDisplay.text = "Score: " + scoreCounter.ToString();
        }
        else if (scoreCounter <= 0)
        {
            scoreCounter = 0;
            defeatPanel.SetActive(true);
            scoreDisplay.text = "Score: " + scoreCounter.ToString();
            isCountdownActive = false;
        }
        else
        {
            scoreDisplay.text = "Score: " + scoreCounter.ToString();
        }
    }

    public void ValidateColorSelection(Image selectedImage)
    {
        if (selectedImage.sprite == CarsList[selectedColorIndex].Car_Image)
        {
            UpdateScore(1);
            victorySound.Play();

            if (scoreCounter > 0)
            {
                roundDuration = 5f;
                AssignRandomColors();
            }
        }
        else
        {
            UpdateScore(-1);
            defeatSound.Play();

            if (scoreCounter > 0)
            {
                roundDuration = 5f;
                AssignRandomColors();
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
