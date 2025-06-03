using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        EXPChange(0);
        MANAGER.SESSION.onExpChanged += EXPChange;
        MANAGER.SESSION.onHpChanged += HPChanged;
        MANAGER.SESSION.onMonsterCountChanged += M_CountText;
        MANAGER.SESSION.onSelectedCard += SetSkillFrame;

        SelectCard(true);
    }

    public Transform HOLDERLAYER;

    public Image EXPFill;
    public CardSelector CardObject;
    public Treasure Treasure;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI monsterCountText;
    public TextMeshProUGUI TimerText;

    Coroutine HP_Coroutine;

    public Image HpFIll;
    public Image HPFillSeconds;
    public TextMeshProUGUI HPText;

    public SkillFrame frame;
    public Transform activeFrameContent;
    public Transform passiveFrameContent;
    List<GameObject> SkillFrameGorvage = new List<GameObject>();

    private void SetSkillFrame()
    {
        if (SkillFrameGorvage.Count > 0)
        {
            for (int i = 0; i < SkillFrameGorvage.Count; i++)
                Destroy(SkillFrameGorvage[i]);

            SkillFrameGorvage.Clear();
        }
        
        foreach(var data in MANAGER.SESSION.SelectedCards)
        {
            var go = Instantiate(frame, 
                data.Value.db.state == CardState.Active ? 
                activeFrameContent:
                passiveFrameContent);

            go.Initalize(data.Value);
            SkillFrameGorvage.Add(go.gameObject);
        }
    }

    private void Update()
    {
        TimerText.text = Utils_UI.FormatTime(MANAGER.SESSION.GameTime);
    }

    public void SelectTreasure(int value)
    {
        Time.timeScale = 0;
        Treasure.Initalize(value);
    }

    public void SelectCard(bool AllActive = false)
    {
        Time.timeScale = 0;
        CardObject.Initalize(AllActive);
    }

    private void M_CountText(int value) => monsterCountText.text = value.ToString();
    public void EXPChange(float exp)
    {
        float expPercentage = exp / MANAGER.SESSION.GetRequiredExp();
        EXPFill.fillAmount = expPercentage;
        LevelText.text =
            string.Format(
            "Lv.{0} <color=#FFFF00>{1:0.0}</color>%",
            (MANAGER.SESSION.Level + 1),
            expPercentage * 100.0f); 
    }

    public void HPChanged(float hp)
    {
        float hpPercentage = hp / MANAGER.SESSION.MaxHP;
        HPText.text = string.Format("{0:0}/{1:0}", hp, MANAGER.SESSION.MaxHP);
        HpFIll.fillAmount = hpPercentage;
        
        if(HP_Coroutine != null)
        {
            StopCoroutine(HP_Coroutine);
        }
        HP_Coroutine = StartCoroutine(SecondFillAmount(HpFIll.fillAmount));
    }

    IEnumerator SecondFillAmount(float percentage)
    {
        float speed = 1.0f;
        float t = 0.0f;
        while (HPFillSeconds.fillAmount > percentage)
        {
            t += Time.deltaTime * speed;
            HPFillSeconds.fillAmount = Mathf.Lerp(
                HPFillSeconds.fillAmount, percentage, t);

            yield return null;
        }
        HPFillSeconds.fillAmount = percentage;
    }

    private void OnDestroy()
    {
        MANAGER.SESSION.onExpChanged -= EXPChange;
        MANAGER.SESSION.onMonsterCountChanged -= M_CountText;
        MANAGER.SESSION.onSelectedCard -= SetSkillFrame;
    }
}
