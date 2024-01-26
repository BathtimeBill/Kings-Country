using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPainter : GameBehaviour
{
    public Terrain terrain;
    public Texture2D newTexture; // The texture to apply when clicking

    private void Start()
    {
        terrain = GameObject.FindGameObjectWithTag("Ground").GetComponent<Terrain>();   
        if (terrain == null)
        {
            Debug.LogError("Terrain not assigned!");
            return;
        }

        // Ensure the new texture has the same resolution as the terrain's alphamap
        int textureResolution = terrain.terrainData.alphamapResolution;
        newTexture = ResizeTexture(newTexture, textureResolution, textureResolution);
    }

    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast to the terrain
            if (Physics.Raycast(ray, out hit) && hit.collider.GetComponent<TerrainCollider>() != null)
            {
                // Get the terrain position and paint the new texture
                PaintTexture(hit.point);
            }
        }
    }

    private void PaintTexture(Vector3 position)
    {
        TerrainData terrainData = terrain.terrainData;

        // Get the normalized position on the terrain where the click occurred
        Vector3 normalizedPos = GetNormalizedPosition(position);

        // Get the current alpha map
        float[,,] alphaMap = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        // Apply the new texture to the clicked position
        ApplyTextureToAlphaMap(alphaMap, normalizedPos.x, normalizedPos.z);

        // Set the modified alpha map back to the terrain
        terrainData.SetAlphamaps(0, 0, alphaMap);
    }

    private Vector3 GetNormalizedPosition(Vector3 worldPosition)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = worldPosition - terrain.transform.position;
        Vector3 normalizedPos = new Vector3(
            terrainPos.x / terrainData.size.x,
            terrainPos.y / terrainData.size.y,
            terrainPos.z / terrainData.size.z
        );
        return normalizedPos;
    }

    private void ApplyTextureToAlphaMap(float[,,] alphaMap, float posX, float posY)
    {
        int x = (int)(posX * alphaMap.GetLength(0));
        int y = (int)(posY * alphaMap.GetLength(1));

        // Set the alpha values for the new texture
        for (int i = 0; i < alphaMap.GetLength(2); i++)
        {
            alphaMap[x, y, i] = 0f; // Assuming you want to set all other textures to 0
        }

        // Set the alpha value for the new texture to 1
        alphaMap[x, y, 0] = 1f;
    }

    private Texture2D ResizeTexture(Texture2D texture, int width, int height)
    {
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        RenderTexture.active = rt;
        Graphics.Blit(texture, rt);

        Texture2D resizedTexture = new Texture2D(width, height);
        resizedTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        resizedTexture.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return resizedTexture;
    }

}
