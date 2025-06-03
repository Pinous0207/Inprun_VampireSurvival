using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardDB card;
    [SerializeField] TextMeshProUGUI Title, Description;
    [SerializeField] Image IconImage;
    [SerializeField] Image OutlineImage;
    [SerializeField] Transform StarParent;
    Animator animator;
    public bool isSelected = false;

    public Color[] colors;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetAnimation("Card_PointerDown");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SetAnimation("Card_PointerUp");
    }

    public void Initalize(CardDB cardDB)
    {
        for (int i = 0; i < StarParent.childCount; i++)
            StarParent.GetChild(i).GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 1.0f);

        card = cardDB;

        StarCheck();
        OutlineImage.color = card.state == CardState.Active ? colors[0] : colors[1];
        Title.text = card.id;
        Description.text = string.Format(card.description, card.baseDamage);
        IconImage.sprite = MANAGER.DB.GetSprite(card.name);

        animator.Rebind();
        isSelected = false;
    }

    public void StarCheck()
    {
        if (MANAGER.SESSION.HaveCard(card))
        {
            for (int i = 0; i < MANAGER.SESSION.SelectedCards[card.id].Level; i++)
            {
                StarParent.GetChild(i).GetComponent<Image>().color = Color.white;
            }
        }
        else
        {
            for (int i = 0; i < StarParent.childCount; i++)
                StarParent.GetChild(i).GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        }
    }

    public void SetAnimation(string temp)
    {
        if (isSelected) return;
        animator.Play(temp);
    }


}
