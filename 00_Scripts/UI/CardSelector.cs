using System.Collections;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    public Card[] cards;
    Animator animator;
    bool isSelected = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Initalize(bool AllActive = false)
    {
        isSelected = false;
        animator.Play("Selector_Open");
        var Cards = MANAGER.DB.GetRandomCardSet(AllActive);
        for (int i = 0; i < cards.Length; i++) cards[i].Initalize(Cards[i]);
    }

    public void SelectCard(int value)
    {
        if (isSelected) return;
        isSelected = true;
        for (int i = 0; i < cards.Length; i++)
        {
            if (i == value)
            {
                cards[i].SetAnimation("Card_Select");
                MANAGER.SESSION.SelectedCard(cards[i].card);
                cards[i].StarCheck();
            }
            else cards[i].SetAnimation("Card_NoneSelect");
            cards[i].isSelected = true;
        }
        StartCoroutine(GameStartCoroutine());
    }

    IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        animator.Play("Selector_Close");
        Time.timeScale = 1.0f;
    }
}
