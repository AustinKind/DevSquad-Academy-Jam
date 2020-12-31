using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialValueAnimation : MonoBehaviour
{
    public AnimatedFloat setValueVariable;
    [SerializeField] private string adjustVariable = "_Variable";


    new Renderer renderer;
    Image uiImage;
    Material mat;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        uiImage = GetComponent<Image>();
        mat = (renderer) ? renderer.material : uiImage.material;
    }

    private void Update()
    {
        mat.SetFloat(adjustVariable, setValueVariable.animatedValue);
    }
}
