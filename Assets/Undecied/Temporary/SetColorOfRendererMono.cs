using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorOfRendererMono: MonoBehaviour
{
    public Renderer m_targetRenderer;
    public Material m_targetMaterial;
    public void SetColor(Color color) {
        if (m_targetRenderer)
        {
            Color c = m_targetRenderer.material.color;
            c= color;
            m_targetRenderer.material.color = c;
        }
        if (m_targetMaterial)
        {
            Color c = m_targetMaterial.color;
            c = color;
            m_targetMaterial.color = c;
        }

    }
}
