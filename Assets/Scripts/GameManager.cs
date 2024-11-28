using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button Deal;
    public Button Hit;
    public Button Stand;
    public Button Bet;

    private int standClicks = 0;

    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    public TMP_Text scoreText;
    public TMP_Text dealerScoreText;
    public TMP_Text betsText;
    public TMP_Text cashText;
    public TMP_Text mainText;
    public TMP_Text standBtnText;

    public GameObject hideCard;
    int pot = 0;
    void Start()
    {
        Deal.onClick.AddListener(() => DealClicked());
        Hit.onClick.AddListener(() => HitClicked());
        Stand.onClick.AddListener(() => StandClicked());
    }

    public void StandClicked()
    {
        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        standBtnText.text = "Call";
    }

    public void HitClicked()
    {
        if (playerScript.cardIndex <= 6)
        {
            playerScript.GetCard();
            scoreText.text = "Hand: " + playerScript.handValue.ToString();
            if (playerScript.handValue > 20) RoundOver();
        }
    }

    public void DealClicked()
    {
        playerScript.ResetHand();
        dealerScript.ResetHand();
        dealerScoreText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        hideCard.GetComponent<Renderer>().enabled = true;
        Deal.gameObject.SetActive(false);
        Hit.gameObject.SetActive(true);
        Stand.gameObject.SetActive(true);
        standBtnText.text = "Stand";
        pot = 40;
        betsText.text = "Bets: £" + pot.ToString();
        playerScript.AdjustMoney(-20);
        cashText.text = "£" + playerScript.GetMoney().ToString();
    }

    private void HitDealer()
    {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 8)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20) RoundOver();
        }
    }
    public void BetClicked()
    {
        TMP_Text newBet = Bet.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
        int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));
        playerScript.AdjustMoney(-intBet);
        cashText.text = "£" + playerScript.GetMoney().ToString();
        pot += (intBet * 2);
        betsText.text = "Bets: £" + pot.ToString();
    }
    void RoundOver()
    {
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;
        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer wins!";
        }
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
            playerScript.AdjustMoney(pot);
        }
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Push: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }
        if (roundOver)
        {
            Hit.gameObject.SetActive(false);
            Stand.gameObject.SetActive(false);
            Deal.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            cashText.text = "£" + playerScript.GetMoney().ToString();
            standClicks = 0;
        }
    }

}
