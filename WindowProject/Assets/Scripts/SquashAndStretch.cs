using UnityEngine;

public class SquashAndStretch : MonoBehaviour
{
    [Header("Configuración")]
    public float intensidad = 1f;    // Ajustado un poco más bajo para suavidad
    public float suavidad = 10f;

    private Vector3 escalaOriginal;
    private Vector3 posicionAnterior;

    void Start()
    {
        escalaOriginal = transform.localScale;
        posicionAnterior = transform.position;
    }

    void Update()
    {
        // --- SEGURIDAD ANTI-NaN ---
        // Si el tiempo es 0 (pausa o primer frame), no calculamos nada
        if (Time.deltaTime <= 0) return;

        // 1. Calculamos la velocidad actual
        Vector3 velocidadActual = (transform.position - posicionAnterior) / Time.deltaTime;

        // Guardamos la posición para el próximo frame
        posicionAnterior = transform.position;

        float velocidadX = Mathf.Abs(velocidadActual.x);
        float velocidadY = Mathf.Abs(velocidadActual.y);

        // 2. Calculamos los factores de escala
        // Reducimos el impacto de la velocidad para que no "explote" el tamaño
        float factorX = 1 + (velocidadX * intensidad * 0.01f) - (velocidadY * intensidad * 0.01f);
        float factorY = 1 + (velocidadY * intensidad * 0.01f) - (velocidadX * intensidad * 0.01f);

        // Limitamos para que la deformación sea sutil (entre 0.8 y 1.2)
        factorX = Mathf.Clamp(factorX, 0.8f, 1.2f);
        factorY = Mathf.Clamp(factorY, 0.8f, 1.2f);

        Vector3 escalaObjetivo = new Vector3(
            escalaOriginal.x * factorX,
            escalaOriginal.y * factorY,
            escalaOriginal.z
        );

        // 3. Aplicamos la escala suavemente
        transform.localScale = Vector3.Lerp(transform.localScale, escalaObjetivo, Time.deltaTime * suavidad);
    }
}