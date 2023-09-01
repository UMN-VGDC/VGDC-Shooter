using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private Image m_Image;
    [SerializeField] private bool playOnAwake, restartScene;
    [SerializeField] private Sprite[] m_SpriteArray;
    [SerializeField] private int m_Speed = 20;

    private int m_IndexSprite;

    private void Start()
    {
        if (playOnAwake)
        {
            m_Image.color = Color.white;
            PlayAnimation();
        }
    }

    public async void PlayAnimation()
    {
        await Task.Delay(m_Speed);
        if (m_IndexSprite >= m_SpriteArray.Length)
        {
            if (restartScene)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        m_Image.color = Color.white;
        m_Image.sprite = m_SpriteArray[m_IndexSprite];
        m_IndexSprite++;

        PlayAnimation();
    }
}
