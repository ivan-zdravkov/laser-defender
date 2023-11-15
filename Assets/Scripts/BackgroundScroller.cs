using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.5f;

    Material material;
    Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        this.material = GetComponent<Renderer>().material;
        this.offset = new Vector2(x: 0f, y: this.scrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        this.material.mainTextureOffset += this.offset * Time.deltaTime;
    }
}
