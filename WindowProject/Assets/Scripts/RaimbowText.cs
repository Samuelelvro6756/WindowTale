using UnityEngine;
using TMPro; // Necesario para TextMeshPro

public class RainbowTextUI : MonoBehaviour
{
    [Header("Configuración Rainbow")]
    public float velocidadCiclo = 0.5f;

    // Podemos ajustar la saturación y el brillo desde el inspector
    [Range(0f, 1f)] public float saturacion = 0.8f;
    [Range(0f, 1f)] public float brillo = 1f;

    private TextMeshProUGUI textoUI;

    void Start()
    {
        // Obtenemos el componente de texto
        textoUI = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (textoUI == null) return;

        // Calculamos el tono (Hue) basado en el tiempo
        float h = (Time.time * velocidadCiclo) % 1f;

        // Convertimos de HSV a RGB para obtener el color
        Color colorArcoiris = Color.HSVToRGB(h, saturacion, brillo);

        // Aplicamos el color al texto
        textoUI.color = colorArcoiris;
    }
}
