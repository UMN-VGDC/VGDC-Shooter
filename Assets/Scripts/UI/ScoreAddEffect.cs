using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreAddEffect : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    [SerializeField] private Color32 redCol, redBorderCol;
    // Start is called before the first frame update
    void Awake()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        float yPos = transform.position.y;
        transform.DOMoveY(yPos + 120f, 1f).OnComplete(() => Destroy(gameObject));
        Color col = m_TextMeshProUGUI.color;
        m_TextMeshProUGUI.DOColor(new Color(col.r, col.g, col.b, 0), 1f);
    }

    public void SetText(int amount)
    {
        if (amount < 0)
        {
            m_TextMeshProUGUI.color = redCol;
            m_TextMeshProUGUI.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, redBorderCol);
        }
        string sign = amount >= 0 ? "+" : "";
        m_TextMeshProUGUI.text = sign + amount.ToString();
    }

}
