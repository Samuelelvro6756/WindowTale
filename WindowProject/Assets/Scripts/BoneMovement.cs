using UnityEngine;

// Asegúrate de que el enum tenga los nuevos tipos:
public enum TipoHueso { Blanco, Gris, Verde, Azul, Naranja, Rojo, Amarillo, Morado, MoradoPequeño }

public class HuesoMovimiento : MonoBehaviour
{
    public TipoHueso tipo;
    public float velocidadMover = 5f;

    private Vector3 direccion;
    private int rebotesRestantes = 3;
    private bool haEntradoEnPantalla = false;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void ConfigurarHueso(TipoHueso t, Vector3 dir, Color c)
    {
        tipo = t;
        direccion = dir.normalized;
        GetComponent<SpriteRenderer>().color = c;

        // Si es pequeño, que viva menos tiempo
        Destroy(gameObject, 15f);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        switch (tipo)
        {
            case TipoHueso.Amarillo:
                ManejarMovimientoCurvo();
                break;

            case TipoHueso.Rojo:
            case TipoHueso.MoradoPequeño: // Reutilizamos la lógica de rebote
                ManejarMovimientoRebote();
                break;

            case TipoHueso.Morado:
                ManejarMovimientoMorado();
                break;

            default: // Blanco, Verde, Azul, Naranja, Gris
                transform.position += direccion * velocidadMover * Time.deltaTime;
                break;
        }

        transform.Rotate(Vector3.forward * 200f * Time.deltaTime);
    }

    // --- LÓGICAS ESPECÍFICAS ---

    void ManejarMovimientoCurvo()
    {
        Vector3 perpendicular = new Vector3(-direccion.y, direccion.x, 0);
        transform.position += direccion * velocidadMover * Time.deltaTime;
        transform.position += perpendicular * Mathf.Sin(Time.time * 1.5f) * 0.03f;
    }

    // Esta función ahora sirve tanto para el ROJO como para los MORADOS PEQUEÑOS
    void ManejarMovimientoRebote()
    {
        transform.position += direccion * velocidadMover * Time.deltaTime;

        Vector3 posPunto = cam.WorldToViewportPoint(transform.position);

        if (!haEntradoEnPantalla)
        {
            // Verificar si ya entró en la zona visible (margen de 0.1 a 0.9)
            if (posPunto.x > 0.1f && posPunto.x < 0.9f && posPunto.y > 0.1f && posPunto.y < 0.9f)
                haEntradoEnPantalla = true;
            return;
        }

        if (rebotesRestantes > 0)
        {
            // Si toca un borde, invierte la dirección y resta un rebote
            if (posPunto.x <= 0 || posPunto.x >= 1) { direccion.x *= -1; rebotesRestantes--; }
            if (posPunto.y <= 0 || posPunto.y >= 1) { direccion.y *= -1; rebotesRestantes--; }
        }
    }

    void ManejarMovimientoMorado()
    {
        transform.position += direccion * velocidadMover * Time.deltaTime;
        Vector3 posPunto = cam.WorldToViewportPoint(transform.position);

        // Similar al rojo, primero debe entrar en pantalla
        if (!haEntradoEnPantalla)
        {
            if (posPunto.x > 0.1f && posPunto.x < 0.9f && posPunto.y > 0.1f && posPunto.y < 0.9f)
                haEntradoEnPantalla = true;
            return;
        }

        // Si ya estaba dentro y vuelve a tocar un borde... ¡SE ROMPE!
        if (posPunto.x <= 0 || posPunto.x >= 1 || posPunto.y <= 0 || posPunto.y >= 1)
        {
            RomperEnPedazos();
        }
    }

    void RomperEnPedazos()
    {
        // La dirección opuesta a la que venía
        Vector3 direccionBase = -direccion;
        Color colorMorado = GetComponent<SpriteRenderer>().color;

        for (int i = 0; i < 4; i++)
        {
            // Creamos una copia del hueso actual en la misma posición
            GameObject miniHueso = Instantiate(gameObject, transform.position, Quaternion.identity);

            // Lo hacemos más pequeño (el 60% del tamaño original)
            miniHueso.transform.localScale = transform.localScale * 0.6f;

            // Calculamos una dirección con un poco de ángulo al azar para que se dispersen (efecto escopeta)
            float anguloRandom = Random.Range(-30f, 30f);
            Vector3 direccionFinal = Quaternion.Euler(0, 0, anguloRandom) * direccionBase;

            // Configuramos el nuevo hueso como PEQUEÑO y rebotador
            miniHueso.GetComponent<HuesoMovimiento>().ConfigurarHueso(TipoHueso.MoradoPequeño, direccionFinal, colorMorado);
        }

        // Destruimos el hueso grande original
        Destroy(gameObject);
    }
}