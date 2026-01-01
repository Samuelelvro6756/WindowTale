using UnityEngine;

public class RainbowDashSync : MonoBehaviour
{
    [Header("Ajustes Rainbow")]
    public float velocidadCiclo = 0.5f;
    public float intensidadGlow = 2.5f;

    [Header("Ajustes Dash")]
    public Color colorDashBlanco = Color.white; // Color durante el dash
    public float intensidadGlowDash = 4.0f; // Un brillo más fuerte al hacer dash

    private SpriteRenderer spriteRenderer;
    private ParticleSystem rastro;
    private ParticleSystem.MainModule mainModule;

    // REFERENCIA AL SCRIPT DE MOVIMIENTO
    private PlayerDashMovement scriptMovimiento;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rastro = GetComponent<ParticleSystem>();
        if (rastro != null) mainModule = rastro.main;

        // BUSCAMOS EL SCRIPT DE MOVIMIENTO EN ESTE MISMO OBJETO
        scriptMovimiento = GetComponent<PlayerDashMovement>();
    }

    void Update()
    {
        Color colorFinalSprite;
        Color colorFinalRastro;

        // --- COMPROBACIÓN CLAVE: ¿ESTAMOS EN DASH? ---
        if (scriptMovimiento != null && scriptMovimiento.estaEnDash)
        {
            // MODO DASH: Blanco puro y brillo intenso
            colorFinalSprite = colorDashBlanco;
            // Multiplicamos por una intensidad mayor para un "flash" blanco
            colorFinalRastro = colorDashBlanco * intensidadGlowDash;
        }
        else
        {
            // MODO NORMAL: Arcoíris
            float h = (Time.time * velocidadCiclo) % 1f;
            Color colorBase = Color.HSVToRGB(h, 0.8f, 1f);

            colorFinalSprite = colorBase;
            colorFinalRastro = colorBase * intensidadGlow;
        }

        // --- Aplicar los colores calculados ---
        spriteRenderer.color = colorFinalSprite;
        if (rastro != null)
        {
            mainModule.startColor = colorFinalRastro;
        }
    }
}