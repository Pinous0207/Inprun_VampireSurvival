using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class DamageTMP : MonoBehaviour
{
    GameObject Critical;
    private TextMeshProUGUI m_Text;
    private RectTransform rectTransform;

    private Vector2 velocity; // 초기 속도 (포물선 운동)
    private float gravity = -1000.0f; // 중력 효과 (UI 이동이므로 값 조정 필요)
    private float lifetime = 1.0f; // 지속 시간

    private Color textColor;

    private void Awake()
    {
        Critical = transform.Find("Critical").gameObject;
        m_Text = GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initalize(Transform parent, Vector3 pos, string temp,  Color color,bool critical = false)
    {
        Critical.SetActive(critical);
        transform.SetParent(parent);

        m_Text.text = temp;
        m_Text.color = color;

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        rectTransform.position = screenPosition;

        velocity = new Vector2(Random.Range(-50.0f, 50.0f), Random.Range(150.0f, 250.0f));

        textColor = m_Text.color;

        StartCoroutine(MoveAndFade());
    }

    IEnumerator MoveAndFade()
    {
        float elapsedTime = 0.0f;
        
        while(elapsedTime < lifetime)
        {
            velocity.y += gravity * Time.deltaTime;

            rectTransform.anchoredPosition += velocity * Time.deltaTime;

            textColor.a = Mathf.Lerp(1.0f, 0.0f, elapsedTime / lifetime);
            m_Text.color = textColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        MANAGER.POOL.m_pool_Dictionary["DamageTMP"].Return(this.gameObject);
    }
}
