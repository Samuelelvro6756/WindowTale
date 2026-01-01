using System.Collections;
using UnityEngine;

public class PlayerDashMovement : MonoBehaviour
{
    [Header("Movimiento Normal")]
    public float velocidadNormal = 500f;

    [Header("Configuración del Dash (Correr)")]
    public float velocidadDash = 1500f; // ¡Mucho más rápido!
    public float duracionDash = 0.2f;   // Un impulso corto
    public float cooldownDash = 0.5f;   // Tiempo de espera para usarlo de nuevo
    public KeyCode teclaDash = KeyCode.Space; // La tecla para activarlo
    public KeyCode botonDashMando = KeyCode.Joystick1Button0; // La tecla para activarlo

    [Header("Efectos Visuales Dash")]
    public Vector3 escalaPulso = new Vector3(1.3f, 1.3f, 1f); // Cuánto crece en el pulso

    // --- Variables de Estado Públicas (Para que el otro script las lea) ---
    [HideInInspector] public bool estaEnDash = false;
    [HideInInspector] public bool puedeHacerDash = true;

    private float velocidadActual;
    private Vector3 escalaOriginal;
    private SpriteRenderer spriteRenderer;
    private Camera cam;
    private float mitadAncho, mitadAlto;

    void Start()
    {
        cam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        velocidadActual = velocidadNormal;
        escalaOriginal = transform.localScale;

        if (spriteRenderer != null)
        {
            mitadAncho = spriteRenderer.bounds.extents.x;
            mitadAlto = spriteRenderer.bounds.extents.y;
        }
    }

    void Update()
    {
        // 1. Detectar Input del Dash
        if (Input.GetKeyDown(teclaDash) || Input.GetKeyDown(botonDashMando) && puedeHacerDash && !estaEnDash)
        {
            StartCoroutine(SecuenciaDash());
        }

        // 2. Movimiento Básico (Usa velocidadActual, que cambia si hay dash)
        float moverHorizontal = Input.GetAxis("Horizontal");
        float moverVertical = Input.GetAxis("Vertical");
        // Si estamos en dash, normalizamos el vector para que diagonal no sea más rápido
        Vector3 movimiento = new Vector3(moverHorizontal, moverVertical, 0);
        if (estaEnDash) movimiento.Normalize();

        transform.position += movimiento * velocidadActual * Time.deltaTime;

        // 3. Lógica de pantalla infinita (La que ya tenías)
        AtravesarBordes();
    }

    // --- CORRUTINA PRINCIPAL DEL DASH ---
    IEnumerator SecuenciaDash()
    {
        estaEnDash = true;
        puedeHacerDash = false;
        velocidadActual = velocidadDash;

        // EFECTO DE PULSO (Crecer rápidamente)
        transform.localScale = escalaPulso;

        // Esperamos lo que dura el dash
        yield return new WaitForSeconds(duracionDash);

        // FIN DEL DASH
        estaEnDash = false;
        velocidadActual = velocidadNormal;
        // Volver al tamaño original
        transform.localScale = escalaOriginal;

        // Esperamos el tiempo de enfriamiento
        yield return new WaitForSeconds(cooldownDash);
        puedeHacerDash = true;
    }

    void AtravesarBordes()
    {
        // (Pega aquí exactamente la misma función AtravesarBordes que tenías en el script anterior)
        // La he omitido para no hacer este código kilométrico, pero es necesaria.
        Vector3 limiteIzqAbajo = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 limiteDerArriba = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Vector3 posActual = transform.position;
        float nuevaX = posActual.x;
        float nuevaY = posActual.y;
        if (posActual.x > limiteDerArriba.x + mitadAncho) nuevaX = limiteIzqAbajo.x - mitadAncho;
        else if (posActual.x < limiteIzqAbajo.x - mitadAncho) nuevaX = limiteDerArriba.x + mitadAncho;
        if (posActual.y > limiteDerArriba.y + mitadAlto) nuevaY = limiteIzqAbajo.y - mitadAlto;
        else if (posActual.y < limiteIzqAbajo.y - mitadAlto) nuevaY = limiteDerArriba.y + mitadAlto;
        transform.position = new Vector3(nuevaX, nuevaY, posActual.z);
    }
}