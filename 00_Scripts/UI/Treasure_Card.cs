using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Treasure_Card : MonoBehaviour
{
    public RectTransform rollerParent;
    public GameObject cardPrefab;
    public float cardHeight = 240.0f;
    private float rollDuration;
    private int loopCount;

    public bool isFinished = false;

    private List<RectTransform> cards = new();

    [SerializeField] Treasure treasure;
    Dictionary<int, SelectCard> ActiveCards = new Dictionary<int, SelectCard>();
    public void Initalize(List<SelectCard> candidateCards)
    {
        isFinished = false;
        rollDuration = Random.Range(5.0f, 8.0f);
        loopCount = Random.Range(5, 10);

        foreach (Transform child in rollerParent)
            Destroy(child.gameObject);
        cards.Clear();

        ActiveCards.Clear();
        for(int i = 0; i < 20; i++)
        {
            int value = Random.Range(0, candidateCards.Count);
            var pick = candidateCards[value];
            int index = i;
            ActiveCards.Add(index, pick);
            GameObject go = Instantiate(cardPrefab, rollerParent);
            go.SetActive(true);
            go.GetComponent<Image>().sprite = MANAGER.DB.GetSprite(pick.db.name);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -i * cardHeight);
            cards.Add(rt);
        }

        rollerParent.anchoredPosition = Vector2.zero;

        int targetIndex = Random.Range(0, cards.Count);
        SelectCard card = ActiveCards[targetIndex];
        if (MANAGER.SESSION.SelectedCards.ContainsKey(card.db.id))
        {
            MANAGER.SESSION.SelectedCards[card.db.id].Level++;
        }
  
        StartCoroutine(RollingCoroutine(targetIndex));
    }

    IEnumerator RollingCoroutine(int targetIndex)
    {
        int totalSteps = loopCount * cards.Count + targetIndex;
        float totalDistance = totalSteps * cardHeight;

        Vector2 startPos = rollerParent.anchoredPosition;
        Vector2 endPos = startPos - new Vector2(0, totalDistance);
        float elapsed = 0.0f;

        Vector2 previousPos = startPos;
        float threshold = 1.5f;

        while (elapsed < rollDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = EaseOutQuart(elapsed / rollDuration);
            Vector2 currentPos = Vector2.Lerp(startPos, endPos, t);

            if ((currentPos - previousPos).magnitude < threshold && (endPos - currentPos).magnitude < 10.0f)
            {
                rollerParent.anchoredPosition = endPos;
                break;
            }

            rollerParent.anchoredPosition = currentPos;
            previousPos = currentPos;

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].anchoredPosition.y + rollerParent.anchoredPosition.y < -cardHeight)
                {
                    float highestY = GetHighestCardY();
                    cards[i].anchoredPosition = new Vector2(0, highestY + cardHeight);
                }
            }
            yield return null;
        }
        isFinished = true;

        SelectCard card = ActiveCards[targetIndex];
        if (MANAGER.SESSION.SelectedCards.ContainsKey(card.db.id))
        {
            MANAGER.SESSION.RegisterSkill(card.db);
        }
        else
        {
            MANAGER.SESSION.HP += 25;
        }

        treasure.ConfirmCheck();
        GetComponent<Animator>().Play("Effect");
        rollerParent.anchoredPosition = endPos;
    }

    // ī����� �Ʒ��� ������, �Ʒ��� ��� ī�带 �ٽ� ���� ���ġ
    float GetHighestCardY()
    {
        float maxY = float.MinValue;
        foreach(var card in cards)
        {
            if (card.anchoredPosition.y > maxY)
                maxY = card.anchoredPosition.y;
        }
        return maxY;
    }

    float EaseOutQuart(float t)
    {
        // ó������ ������ -> �������� �������� (����)
        // Mathf.Pow(x, y) -> �ŵ�����
        // x -> ���� ����, y -> ���� (�� �� ���Ұ���)
        // (1 - t)^4 
        // t�� Ŀ������ �ش� ���� �۾���
        // (1 - (1 - t)^4)�� ó������ ������, ���߿��� ���� õõ��
        return 1 - Mathf.Pow(1 - t, 4);
    }
}
