using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FI.PP
{
    [ExecuteInEditMode]
    public class WorldPosInFragment : MonoBehaviour
    {
        private Camera cam;

        [NonSerialized]
        private Material worldMaterial;

        [NonSerialized]
        private Vector3[] frustumCorners;

        [NonSerialized]
        private Vector4[] vectorArray;

        [SerializeField]
        private float speed = 50f, multiplierFactor = 1.01f;

        [SerializeField]
        private Color inRadiusColor = Color.green;

        private float radius;

        private bool effectIsOn;

        // ---------------------
        private void Start() { }

        // ----------------------------------
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                effectIsOn = !effectIsOn;

            UpdateRadiusEffect();
        }

        // ----------------------------------------------------------
        private void UpdateRadiusEffect()
        {
            if (!cam)
                cam = Camera.main;

            if (effectIsOn)
            {
                radius += Time.deltaTime * speed;
                radius = radius > cam.farClipPlane / 4f ?
                    radius * multiplierFactor : radius;
            }
            else
            {
                radius -= Time.deltaTime * speed;
                radius = radius > cam.farClipPlane / 4f ?
                    radius / multiplierFactor : radius;
            }

            radius = Mathf.Clamp(radius, 0, cam.farClipPlane * 4f);
        }

        // ------------------------------------------------------------------------
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!worldMaterial)
            {
                vectorArray = new Vector4[4];
                frustumCorners = new Vector3[4];

                worldMaterial = new Material(Shader.Find("FI/WorldPosInFragment"));
                worldMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            UpdateFrustumCorners();
            UpdateMaterialProperties();

            Graphics.Blit(source, destination, worldMaterial);
        }

        // -------------------------------------
        private void UpdateFrustumCorners()
        {
            if (!cam)
                cam = Camera.main;

            cam.CalculateFrustumCorners(
                   new Rect(0f, 0f, 1f, 1f),
                   cam.farClipPlane,
                   cam.stereoActiveEye,
                   frustumCorners);

            vectorArray[0] = frustumCorners[0];
            vectorArray[1] = frustumCorners[3];
            vectorArray[2] = frustumCorners[1];
            vectorArray[3] = frustumCorners[2];
        }

        // ---------------------------------------------------------------
        private void UpdateMaterialProperties()
        {
            worldMaterial.SetFloat("_Radius", radius);
            worldMaterial.SetColor("_InRadiusColor", inRadiusColor);
            worldMaterial.SetVectorArray("_FrustumCorners", vectorArray);
        }
    }

}