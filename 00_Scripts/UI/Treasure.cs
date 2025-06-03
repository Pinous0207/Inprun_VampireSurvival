using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering;
public class Treasure : MonoBehaviour
{
    [SerializeField] Image ChestImage;
    [SerializeField] Sprite[] ChestSprites;

    [SerializeField] Treasure_Card[] cards;

    [SerializeField] GameObject ConfirmBtn;
    int valueCount = 0;
    Animator animator;
    CanvasGroup canvasGroup;

    private void Start()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Initalize(int chestValue)
    {
        CanvasGroupCheck(true);
        animator.Play("Selector_Open");
        ChestImage.sprite = ChestSprites[chestValue];
        valueCount = chestValue;
        switch(chestValue)
        {
            case 0:
                cards[0].gameObject.SetActive(true);
                cards[0].Initalize(lists());
                break;
            case 1:
                for (int i = 0; i < 3; i++)
                {
                    cards[i].gameObject.SetActive(true);
                    cards[i].Initalize(lists());
                }
                break;
            case 2:
                for (int i = 0; i < 5; i++)
                {
                    cards[i].gameObject.SetActive(true);
                    cards[i].Initalize(lists());
                }
                break;
        }
    }

    private void CanvasGroupCheck(bool B)
    {
        canvasGroup.interactable = B;
        canvasGroup.blocksRaycasts = B;
    }

    public List<SelectCard> lists()
    {
        List<SelectCard> lists = new List<SelectCard>();

        foreach (var selected in MANAGER.SESSION.SelectedCards)
        {
            if (selected.Value.Level < 5)
                lists.Add(selected.Value);
        }

        if(lists.Count == 0)
        {
            for(int i = 0; i < MANAGER.DB.NoneCards.Count; i++)
            {
                lists.Add(new SelectCard
                {
                    db = MANAGER.DB.NoneCards[i],
                    Level = 0
                });
            }
        }
        return lists;
    }

    public void ConfirmCheck()
    {
        switch (valueCount)
        {
            case 0:
                if (!cards[0].isFinished)
                {
                    return;
                }
                break;
            case 1:
                for(int i = 0; i < 3; i++)
                {
                    if (!cards[i].isFinished)
                        return;
                }
                break;
            case 2:
                for(int i = 0; i < 5; i++)
                {
                    if (!cards[i].isFinished)
                        return;
                }
                break;
        }
        ConfirmBtn.transform.localScale = Vector3.one;
    }

    public void Confirm()
    {
        Time.timeScale = 1.0f;
        for (int i = 0; i < cards.Length; i++) cards[i].gameObject.SetActive(false);
        ConfirmBtn.transform.localScale = Vector3.zero;
        CanvasGroupCheck(false);
        animator.Play("Selector_Close");
    }
}
