using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject railsObject;

    public void GenerateLevel(Level_Info level)
    {
        RailGenerate(level);
    }
    
    private void RailGenerate(Level_Info level)
    {
		var scenographyManager = GetComponent<ScenographyManager>();

        Color[] railColors = level.railTex.GetPixels();
        int width = level.railTex.width;
        int height = level.railTex.height;

        int[] tilemap = new int[width * height];
		int[] rotation = new int[width * height];
		Vector2Int start = Vector2Int.zero, target = Vector2Int.zero;
        for(int i = 0; i < tilemap.Length; i++)
        {
			int x = i / width;
			int y = i % height;

			var r = (int)(railColors[i].r*255.0f);
            switch(r)
			{
				case 0:
					tilemap[i] = 0;
					break;
				case 64:
					tilemap[i] = 1;
					break;
				case 128:
					tilemap[i] = 2;
					break;
				case 192:
					tilemap[i] = 3;
					scenographyManager.AddElement(ScenographyType.Switch2, new Vector3(y + 0.5f, 0.0f, x + 0.5f), 135.0f, new Vector2Int(x,y));
					break;
				case 255:
					tilemap[i] = 4;
					scenographyManager.AddElement(ScenographyType.Switch3, new Vector3(y + 0.5f, 0.0f, x + 0.5f), 135.0f, new Vector2Int(x,y));
					break;
			}

			var g = (int)(railColors[i].g*255.0f);
            switch(g)
			{
				case 0:
					rotation[i] = 0;
					break;
				case 64:
					rotation[i] = 1;
					break;
				case 128:
					rotation[i] = 2;
					break;
				case 192:
					rotation[i] = 3;
					break;
			}

			var b = (int)(railColors[i].b*255.0f);
		    switch(b)
			{
				case 32:
					scenographyManager.AddElement(ScenographyType.Tree, new Vector3(y+ 0.5f, 0.0f, x+0.5f), 0.0f, Vector2Int.zero);
					break;
				case 64:
					scenographyManager.AddElement(ScenographyType.CoalThumper, new Vector3(y+ 0.5f, 0.0f, x+0.5f), rotation[i] * 90.0f, Vector2Int.zero);
					break;
				case 96:
					scenographyManager.AddElement(ScenographyType.Generator, new Vector3(y + 0.5f, 0.0f, x + 0.5f), rotation[i] * 90.0f, Vector2Int.zero);
					break;
				case 128:
					break;
				case 160:
					break;
				case 192:
					break;			
				case 224:
					start = new Vector2Int(y, x);				
					break;
				case 255:				
					target = new Vector2Int(y, x);				
					break;
			}
        }
        
        int railsCount = 0;
        foreach (var tile in tilemap)
        {
            if (tile == 0)
                continue;

            railsCount++;
        }

        Vector3[] vertices = new Vector3[railsCount * 4];
        for (int x = 0, c = -1, vertNum = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                c++;
                if (tilemap[c] == 0)
                    continue;
                
                vertices[vertNum + 0] = new Vector3(y, 0.0f, x);
                vertices[vertNum + 1] = new Vector3(y + 1.0f, 0.0f, x);
                vertices[vertNum + 2] = new Vector3(y, 0.0f, x + 1.0f);
                vertices[vertNum + 3] = new Vector3(y + 1.0f, 0.0f, x + 1.0f);
                vertNum += 4;
                
            }
        }


        int[] triangles = new int[railsCount * 6];
        for (int i = 0, j = 0; i < railsCount * 4; i+=4, j+=6)
        {
            triangles[j + 0] = i + 2;
            triangles[j + 1] = i + 1;
            triangles[j + 2] = i + 0;
            
            triangles[j + 3] = i + 2;
            triangles[j + 4] = i + 3;
            triangles[j + 5] = i + 1;
        }

        Vector2[] uvs = new Vector2[railsCount * 4];

        for (int i = 0, j = 0; i < tilemap.Length; i++)
        {
            if (tilemap[i] == 0)
                continue;

			switch(rotation[i])
			{
				case 0:
					uvs[j + 0] = new Vector2(0.0f, 0.0f);
            		uvs[j + 1] = new Vector2(0.5f, 0.0f);
            		uvs[j + 2] = new Vector2(0.0f, 0.5f);
            		uvs[j + 3] = new Vector2(0.5f, 0.5f);
					break;
				case 1:
					uvs[j + 0] = new Vector2(0.0f, 0.5f);
            		uvs[j + 1] = new Vector2(0.0f, 0.0f);
            		uvs[j + 2] = new Vector2(0.5f, 0.5f);
            		uvs[j + 3] = new Vector2(0.5f, 0.0f);
					break;
				case 2:
					uvs[j + 0] = new Vector2(0.5f, 0.5f);
            		uvs[j + 1] = new Vector2(0.0f, 0.5f);
            		uvs[j + 2] = new Vector2(0.5f, 0.0f);
            		uvs[j + 3] = new Vector2(0.0f, 0.0f);
					break;
				case 3:
					uvs[j + 0] = new Vector2(0.5f, 0.0f);
            		uvs[j + 1] = new Vector2(0.5f, 0.5f);
            		uvs[j + 2] = new Vector2(0.0f, 0.0f);
            		uvs[j + 3] = new Vector2(0.0f, 0.5f);
					break;
			}
			j+=4;
		}
        
        for (int i = 0, j = 0; i < tilemap.Length; i++)
        {
            if (tilemap[i] == 0)
                continue;

            float offsetX = ((tilemap[i] - 1) / 2 == 0) ? 0.5f : 0.0f;
            float offsetY = ((tilemap[i] - 1) % 2 == 0) ? 0.5f : 0.0f;
            uvs[j + 0] += new Vector2(offsetX, offsetY);
            uvs[j + 1] += new Vector2(offsetX, offsetY);
            uvs[j + 2] += new Vector2(offsetX, offsetY);
            uvs[j + 3] += new Vector2(offsetX, offsetY);
            j += 4;
        }

        Mesh mesh = new Mesh();
        mesh.name = "rails";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        
        railsObject.GetComponent<MeshFilter>().mesh = mesh;

		RailCrawler(tilemap, rotation, width, height, start, target);
    }

	private void RailCrawler(int[] tilemap, int[] rotation, int width, int height, Vector2Int start, Vector2Int target)
	{
		RailNode[,] nodes = new RailNode[width, height];
		
		for(int x = 0, i = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
				nodes[y,x] = new RailNode();
				switch(tilemap[i])
				{
					case 0:
						nodes[y,x].isValid = false;
						nodes[y,x].position = new Vector3(y + 0.5f, 0.0f, x + 0.5f);
						break;
					case 1:
						nodes[y,x].isValid = true;
						nodes[y,x].position = new Vector3(y + 0.5f, 0.0f, x + 0.5f);
						nodes[y,x].maxDir = 0;
						nodes[y,x].dir = 0;
						nodes[y,x].rotation = (ushort)rotation[i];
						nodes[y,x].type = 1;
						break;
					case 2:
						nodes[y,x].isValid = true;
						nodes[y,x].position = new Vector3(y + 0.5f, 0.0f, x + 0.5f);
						nodes[y,x].maxDir = 0;
						nodes[y,x].dir = 0;
						nodes[y,x].rotation = (ushort)rotation[i];
						nodes[y,x].type = 2;
						break;
					case 3:
						nodes[y,x].isValid = true;
						nodes[y,x].position = new Vector3(y + 0.5f, 0.0f, x + 0.5f);
						nodes[y,x].maxDir = 2;
						nodes[y,x].dir = 0;
						nodes[y,x].rotation = (ushort)rotation[i];
						nodes[y,x].type = 3;
						break;
					case 4:
						nodes[y,x].isValid = true;
						nodes[y,x].position = new Vector3(y + 0.5f, 0.0f, x + 0.5f);
						nodes[y,x].maxDir = 3;
						nodes[y,x].dir = 0;
						nodes[y,x].rotation = (ushort)rotation[i];
						nodes[y,x].type = 4;
						break;
				}
				i++;
			}		
		}
		
		GetComponent<RailManager>().CreateNodes(nodes, start, target);
	}
}
