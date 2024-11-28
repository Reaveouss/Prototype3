using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int CurrentIndex = 0;
    void Start()
    {
        GetCardValues();
    }

    
    void GetCardValues()
    {
        int num = 0;
        for(int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            num %= 13;
            if(num > 10 || num == 0)
            {
                num = 10;
            }
            cardValues[i] = num++;
        }
    }
    public void Shuffle()
    {
        for(int i = cardSprites.Length - 1; i >= 0; i--)
        {
            int n = Mathf.FloorToInt(Random.Range(0.0f,1.0f) * cardSprites.Length - 1) + 1;
            Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[n];
            cardSprites[n] = face;

            int value = cardValues[i];
            cardValues[i] = cardValues[n];
            cardValues[n] = value;
        }
        CurrentIndex = 1;
    }
    public int DealCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[CurrentIndex]);
        cardScript.SetValue(cardValues[CurrentIndex]);
        CurrentIndex++;
        return cardScript.GetValueOfCard();
    }
    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }
}
