using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MyScore : MonoBehaviour
{
    private TextMeshProUGUI m_Text;
    private int sc;
   
    void Start()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }
    public void Scoring(int score)
    {
        sc+=score;
       m_Text.text = sc.ToString();
    }
}
