using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FI.PP
{
    [ExecuteInEditMode]
    public class OutlineEffect : MonoBehaviour
    {
        [NonSerialized]
        private Material outlineMat;

        [SerializeField]
        private float width = 1, threshold = 0.1f;
        [SerializeField, Range(1, 2)]
        private float offset = 1;

        [SerializeField]
        private Color outlineColor = Color.white;

        // ---------------------
        private void Start() { }

        // ------------------------------------------------------------------------
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (outlineMat == null)
            {
                outlineMat = new Material(Shader.Find("FI/OutlineEffect"));
                outlineMat.hideFlags = HideFlags.HideAndDontSave;
            }

            UpdateMaterialProperties();

            Graphics.Blit(null, destination, outlineMat);
        }

        // ----------------------------------------------------
        private void UpdateMaterialProperties()
        {
            outlineMat.SetFloat("width", width);
            outlineMat.SetFloat("offset", offset);
            outlineMat.SetFloat("threshold", threshold);
            outlineMat.SetColor("_OutlineColor", outlineColor);
        }
    }

}