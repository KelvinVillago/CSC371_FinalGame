using UnityEngine;

public class TerrainRemoval : MonoBehaviour
{
    public Terrain terrain;

    void Start()
    {
        TrimTerrain();
    }

    void TrimTerrain()
    {
        int width = terrain.terrainData.heightmapResolution;
        int height = terrain.terrainData.heightmapResolution;

        int startX = (width - 250) / 2; // Start X index of the area to keep
        int startY = (height - 250) / 2; // Start Y index of the area to keep

        float[,] heights = terrain.terrainData.GetHeights(startX, startY, 250, 250);

        terrain.terrainData.SetHeights(0, 0, heights);
    }
}
