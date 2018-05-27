using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreenHUD : MonoBehaviour {

    public Text winText;
    public Text scoreText;
    public Image imageRef;
    public Sprite winImage;
    public Sprite lostImage;
    public float scoreAnimationTime = 2;
    public int decimalPlaces = 5;
    public Color backGroundImage;
    public GameObject panel;

	//pegando o numero da fase - OBS: Não fiz isso pq ai todas as cenas devem ter um numero no final de seu nome
	// private string s1 = SceneManager.GetActiveScene().name;
	// private string s2 = s1.Substring (s1.Length -1);
	// converter o ultimo valor q eh um numero para um int e colocar na variavel fase
	//private int fasenumber; 

	//solucao temporária
	public int fasenumber; 


    public string winMessage = "You won";
    public string lostMessage = "You lost";
    
    public void endGame(bool hasLost, int score) {
        panel.SetActive(true);
		if (hasLost) {
			winText.text = lostMessage;
			scoreText.transform.parent.gameObject.SetActive (false);
			imageRef.sprite = lostImage;
			//score = 0 ;
        }
        		else
        {
            winText.text = winMessage;
            imageRef.sprite = winImage;
        }


        Image backImage = panel.GetComponent<Image>();
        backImage.color = backGroundImage;

        StartCoroutine(incrementScore(score));
		DataToSave.Scores [fasenumber] = score;
    }

    private IEnumerator incrementScore(int targetScore)
    {
        float increment = (targetScore / scoreAnimationTime);
        int currentScore = 0;
        float currentFloat = 0;

        while (currentScore < targetScore) {
            currentFloat = (currentFloat + increment * Time.deltaTime);
            currentScore = (int)currentFloat;
            scoreText.text = currentScore.ToString("D" + decimalPlaces);
            yield return null;
        }

        scoreText.text = targetScore.ToString("D" + decimalPlaces);
    }

    public void clickRetryButton() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void clickNextButton() {
        string actualStage = SceneManager.GetActiveScene().name;
        //string[] result = actualStage.Split(new string[] { "[Fase]" }, System.StringSplitOptions.None);
		int number = (int)char.GetNumericValue (actualStage, 4);

        number++;
		if (number == 12)
			number = 1;
        SceneManager.LoadScene("Fase" + number);
    }

    public void clickSelectButton() {
        SceneManager.LoadScene("Scenes/MenuPrincipal");
    }
}
