using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillFrame : MonoBehaviour
{
    [SerializeField] Image IconImage;
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] Image SkillFill;
    [SerializeField] TextMeshProUGUI TimerText;
    float saveCooltime;
    public void Initalize(SelectCard selectCard)
    {
        IconImage.sprite = MANAGER.DB.GetSprite(selectCard.db.name);
        LevelText.text = "Lv." + selectCard.Level.ToString();
        if (selectCard.db.state == CardState.Active)
        {
            SkillFill.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(CooltimeCoroutine(selectCard.db, selectCard.Level));
        }
    }

    IEnumerator CooltimeCoroutine(CardDB card, int level)
    {
        float cooltime = card.baseCooldown - card.cooldownPerLevel * (level - 1);
        float currentCooldown = saveCooltime > 0 ? saveCooltime : cooltime;
        while (currentCooldown > 0.0f)
        {
            currentCooldown -= Time.deltaTime;
            saveCooltime = currentCooldown;
            TimerText.text = string.Format("{0:0.0}", currentCooldown);

            SkillFill.fillAmount = currentCooldown / cooltime;

            yield return null;
        }

        StartCoroutine(CooltimeCoroutine(card, level));
    }
}
