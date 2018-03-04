using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FI.PP
{
    [ExecuteInEditMode]
    public class DepthBuffer : MonoBehaviour
    {

        [NonSerialized]
        private Material depthMat;

        // ---------------------
        private void Start() { }

        // -------------------------------------------------------------------------
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (depthMat == null)
            {
                depthMat = new Material(Shader.Find("FI/DepthBuffer"));
                depthMat.hideFlags = HideFlags.HideAndDontSave;
            }

            Graphics.Blit(null, destination, depthMat);
        }
    }

}