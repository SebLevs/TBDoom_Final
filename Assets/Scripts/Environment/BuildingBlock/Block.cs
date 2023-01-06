using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private bool isBreakable = false;
    //[SerializeField] private int spriteWidth = 32;
    //[SerializeField] private int spriteHeight = 32;

    public bool IsBreakable { get => isBreakable; set => isBreakable = value; }

    //private void Awake()
    //{
    //    var meshRenderer = GetComponent<MeshRenderer>();
    //    var cubeMesh = GetComponent<MeshFilter>();
        
    //    float height = meshRenderer.material.mainTexture.height;
    //    Debug.Log(height);
    //    int yQuantity = (int) (height / (spriteHeight + 2));
    //    int randomY = Random.Range(0, yQuantity);
    //    int positionY = (spriteHeight + 2) * randomY;
    //    float bottomY = (1 + positionY) / height;
    //    float topY = (33 + positionY) / height;

    //    var mesh = cubeMesh.mesh;
    //    Vector2[] uvMap = mesh.uv;
    //    Debug.Log(mesh.uv);

    //    // Front
    //    uvMap[0] = new Vector2(1 / 102f, bottomY);
    //    uvMap[1] = new Vector2(33 / 102f, bottomY);
    //    uvMap[2] = new Vector2(1 / 102f, topY);
    //    uvMap[3] = new Vector2(33 / 102f, topY);

    //    // Top
    //    uvMap[8] = new Vector2(35 / 102f, bottomY);
    //    uvMap[9] = new Vector2(67 / 102f, bottomY);
    //    uvMap[4] = new Vector2(35 / 102f, topY);
    //    uvMap[5] = new Vector2(67 / 102f, topY);

    //    // Back
    //    uvMap[7] = new Vector2(1 / 102f, bottomY);
    //    uvMap[6] = new Vector2(33 / 102f, bottomY);
    //    uvMap[11] = new Vector2(1 / 102f, topY);
    //    uvMap[10] = new Vector2(33 / 102f, topY);

    //    // Bottom
    //    uvMap[12] = new Vector2(69 / 102f, bottomY);
    //    uvMap[15] = new Vector2(101 / 102f, bottomY);
    //    uvMap[13] = new Vector2(69 / 102f, topY);
    //    uvMap[14] = new Vector2(101 / 102f, topY);

    //    // Left
    //    uvMap[16] = new Vector2(1 / 102f, bottomY);
    //    uvMap[19] = new Vector2(33 / 102f, bottomY);
    //    uvMap[17] = new Vector2(1 / 102f, topY);
    //    uvMap[18] = new Vector2(33 / 102f, topY);

    //    // Right        
    //    uvMap[20] = new Vector2(1 / 102f, bottomY);
    //    uvMap[23] = new Vector2(33 / 102f, bottomY);
    //    uvMap[21] = new Vector2(1 / 102f, topY);
    //    uvMap[22] = new Vector2(33 / 102f, topY);

    //    mesh.uv = uvMap;
    //}
}
