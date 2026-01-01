using UnityEngine;
using UnityEngine.UI; // Necesario para la Imagen
using TMPro; // Necesario para el Texto
using System.Collections;

public class HealthUIAnimator : MonoBehaviour
{
    [Header("Referencias UI")]
    public Image imagenCorazon;
    public TextMeshProUGUI textoVidas;

    [Header("Configuración del Pulso")]
    public float duracionPulso = 0.3f;
    public Vector3 escalaMaximo = new Vector3(1.3f, 1.3f, 1f); // Qué tan grande se pone
    public Color colorCuracion = Color.green;
    public Color colorDaño = Color.red;

    private Vector3 escalaOriginal;
    private Color colorOriginalCorazon;
    private Color colorOriginalTexto;
    private Coroutine corrutinaActual;

    void Start()
    {
        // Guardamos el estado inicial para poder volver a él
        if (imagenCorazon != null)
        {
            escalaOriginal = imagenCorazon.transform.localScale;
            colorOriginalCorazon = imagenCorazon.color;
        }
        if (textoVidas != null)
        {
            // Asumimos que el texto tiene la misma escala que el corazón
            colorOriginalTexto = textoVidas.color;
        }
    }

    // --- Funciones Públicas para llamar desde GameManager ---

    public void AnimarCuracion()
    {
        // Si ya hay una animación corriendo, la detenemos para empezar la nueva
        if (corrutinaActual != null) StopCoroutine(corrutinaActual);
        corrutinaActual = StartCoroutine(SecuenciaPulso(colorCuracion));
    }

    public void AnimarDaño()
    {
        if (corrutinaActual != null) StopCoroutine(corrutinaActual);
        corrutinaActual = StartCoroutine(SecuenciaPulso(colorDaño));
    }

    // --- La Corrutina que hace la magia ---
    IEnumerator SecuenciaPulso(Color colorObjetivo)
    {
        float tiempoMitad = duracionPulso / 2f;
        float timer = 0f;

        // FASE 1: Crecer y cambiar al color objetivo
        while (timer < tiempoMitad)
        {
            timer += Time.deltaTime;
            float t = timer / tiempoMitad;

            // Interpolamos (Lerp) escala y color
            if (imagenCorazon != null)
            {
                imagenCorazon.transform.localScale = Vector3.Lerp(escalaOriginal, escalaMaximo, t);
                imagenCorazon.color = Color.Lerp(colorOriginalCorazon, colorObjetivo, t);
            }
            if (textoVidas != null)
            {
                textoVidas.transform.localScale = Vector3.Lerp(escalaOriginal, escalaMaximo, t);
                textoVidas.color = Color.Lerp(colorOriginalTexto, colorObjetivo, t);
            }
            yield return null; // Esperar al siguiente frame
        }

        timer = 0f;

        // FASE 2: Encoger y volver al color original
        while (timer < tiempoMitad)
        {
            timer += Time.deltaTime;
            float t = timer / tiempoMitad;

            if (imagenCorazon != null)
            {
                imagenCorazon.transform.localScale = Vector3.Lerp(escalaMaximo, escalaOriginal, t);
                imagenCorazon.color = Color.Lerp(colorObjetivo, colorOriginalCorazon, t);
            }
            if (textoVidas != null)
            {
                textoVidas.transform.localScale = Vector3.Lerp(escalaMaximo, escalaOriginal, t);
                textoVidas.color = Color.Lerp(colorObjetivo, colorOriginalTexto, t);
            }
            yield return null;
        }

        // Asegurar que queden exactamente como al principio
        RestaurarEstadoOriginal();
    }

    void RestaurarEstadoOriginal()
    {
        if (imagenCorazon != null)
        {
            imagenCorazon.transform.localScale = escalaOriginal;
            imagenCorazon.color = colorOriginalCorazon;
        }
        if (textoVidas != null)
        {
            textoVidas.transform.localScale = escalaOriginal;
            textoVidas.color = colorOriginalTexto;
        }
    }
}