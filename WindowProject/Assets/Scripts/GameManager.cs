using System.Collections;
using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

[System.Serializable]
public struct ProbabilidadHueso
{
    public TipoHueso tipo;
    [Range(0, 100)] public int peso; // Cuanto más alto, más común
}
public class GameManager : MonoBehaviour
{
    public static GameManager instancia; // Para poder acceder desde el jugador fácilmente

    [Header("Referencias de Audio")]
    public AudioSource bgmSource; // Arrastra el objeto que tiene la música en loop
    public AudioClip sfxSpawn;
    public AudioClip sfxDanio;
    public AudioClip sfxCuracion;
    public AudioClip sfxMuerte;

    [Header("UI Animación")]
    public HealthUIAnimator healthAnimator;

    [Header("Sistema de Vidas")]
    public int vidas = 5;
    public TextMeshProUGUI textoVidasUI;
    public float tiempoInvulnerabilidad = 2f;
    private bool esInvulnerable = false;

    [Header("Efectos de Daño")]
    public GameObject prefabParticulasDanio;

    [Header("Configuración de Probabilidades")]
    public ProbabilidadHueso[] tablaProbabilidades = new ProbabilidadHueso[] {
        new ProbabilidadHueso { tipo = TipoHueso.Blanco, peso = 40 },
        new ProbabilidadHueso { tipo = TipoHueso.Azul,   peso = 15 },
        new ProbabilidadHueso { tipo = TipoHueso.Naranja, peso = 15 },
        new ProbabilidadHueso { tipo = TipoHueso.Amarillo, peso = 11 },
        new ProbabilidadHueso { tipo = TipoHueso.Rojo,     peso = 10 },
        new ProbabilidadHueso { tipo = TipoHueso.Gris,     peso = 8 },
        new ProbabilidadHueso { tipo = TipoHueso.Verde,    peso = 1 }
    };

    [Header("Referencias")]
    public GameObject prefabHueso;
    public TextMeshProUGUI textoTiempoUI;
    public Transform jugador; // Arrastra tu corazón aquí

    [Header("Dificultad")]
    public float tiempoEntreSpawnsInicial = 2f;
    public float tiempoMinimoSpawn = 0.5f;
    public float reduccionTiempoPorSegundo = 0.05f;

    [Header("Game Over")]
    public float segundosAntesDeCerrar = 3f;

    private float tiempoJuego;
    private float tiempoParaSiguienteSpawn;
    private float tiempoActualEntreSpawns;
    private bool juegoTerminado = false;
    private Camera cam;

    void Awake()
    {
        instancia = this; // Configuración del Singleton
    }

    void Start()
    {
        cam = Camera.main;
        tiempoActualEntreSpawns = tiempoEntreSpawnsInicial;
        tiempoParaSiguienteSpawn = tiempoActualEntreSpawns;
        ActualizarVidasUI();
    }

    void Update()
    {
        if (juegoTerminado) return;

        // 1. Actualizar reloj
        tiempoJuego += Time.deltaTime;
        ActualizarUI();

        // 2. Aumentar dificultad (reducir tiempo entre spawns)
        // Mathf.Max asegura que nunca baje del mínimo establecido
        tiempoActualEntreSpawns = Mathf.Max(tiempoMinimoSpawn, tiempoActualEntreSpawns - (reduccionTiempoPorSegundo * Time.deltaTime));

        // 3. Generador de huesos
        tiempoParaSiguienteSpawn -= Time.deltaTime;
        if (tiempoParaSiguienteSpawn <= 0)
        {
            SpawnHueso();
            tiempoParaSiguienteSpawn = tiempoActualEntreSpawns;
        }
    }

    public void ModificarVida(int cantidad)
    {
        if (cantidad < 0)
        {
            if (esInvulnerable) return; // No hace daño si está parpadeando
            if (prefabParticulasDanio != null)
            {
                GameObject particulas = Instantiate(prefabParticulasDanio, jugador.position, Quaternion.identity);

                // Si quieres que las partículas tengan el color actual del arcoíris:
                var main = particulas.GetComponent<ParticleSystem>().main;
                main.startColor = jugador.GetComponent<SpriteRenderer>().color;

                Destroy(particulas, 1.5f); // Limpieza
            }
            bgmSource.PlayOneShot(sfxDanio); // Sonido de daño
            StartCoroutine(EfectoDañado());
            if (healthAnimator != null) healthAnimator.AnimarDaño();
        } else if (cantidad > 0) {
            bgmSource.PlayOneShot(sfxCuracion); // Sonido de curación
            if (healthAnimator != null) healthAnimator.AnimarCuracion();
        }

        vidas += cantidad;
        ActualizarVidasUI();

        if (vidas <= 0) ActivarGameOver();
    }

    void ActualizarVidasUI()
    {
        textoVidasUI.text = "" + vidas;
    }

    IEnumerator EfectoDañado()
    {
        esInvulnerable = true;
        SpriteRenderer sr = jugador.GetComponent<SpriteRenderer>();
        float tiempoPasado = 0;

        while (tiempoPasado < tiempoInvulnerabilidad)
        {
            sr.enabled = !sr.enabled; // Apaga y prende el sprite (titileo)
            yield return new WaitForSeconds(0.1f);
            tiempoPasado += 0.1f;
        }

        sr.enabled = true;
        esInvulnerable = false;
    }
    void ActualizarUI()
    {
        // Calculamos los minutos y segundos a partir del tiempo total
        // Mathf.FloorToInt redondea hacia abajo para tener números enteros
        int minutos = Mathf.FloorToInt(tiempoJuego / 60);
        int segundos = Mathf.FloorToInt(tiempoJuego % 60);

        // string.Format nos permite darle el formato "00:00"
        // {0:00} significa: usa el primer valor (minutos) y asegúrate de que siempre tenga 2 dígitos
        textoTiempoUI.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void SpawnHueso()
    {
        Vector2 puntoSpawn = ObtenerPuntoFueraPantalla();
        GameObject nuevoHueso = Instantiate(prefabHueso, puntoSpawn, Quaternion.identity);

        // Reproducir sonido de spawn
        if (sfxSpawn != null) bgmSource.PlayOneShot(sfxSpawn);

        // --- Lógica de Probabilidad Pesada ---
        TipoHueso tipoElegido = SeleccionarTipoAleatorio();

        Color colorAsignado = ObtenerColorSegunTipo(tipoElegido);

        Vector3 dir = (jugador.position - nuevoHueso.transform.position);
        nuevoHueso.GetComponent<HuesoMovimiento>().ConfigurarHueso(tipoElegido, dir, colorAsignado);
    }

    Vector2 ObtenerPuntoFueraPantalla()
    {
        // Obtenemos los bordes de la cámara en el mundo
        float alto = cam.orthographicSize;
        float ancho = alto * cam.aspect;
        float margen = 2f; // Qué tan lejos fuera de la pantalla aparece

        int lado = Random.Range(0, 4); // 0:Arriba, 1:Abajo, 2:Izq, 3:Der
        Vector2 punto = Vector2.zero;

        switch (lado)
        {
            case 0: // Arriba
                punto = new Vector2(Random.Range(-ancho, ancho), alto + margen); break;
            case 1: // Abajo
                punto = new Vector2(Random.Range(-ancho, ancho), -alto - margen); break;
            case 2: // Izquierda
                punto = new Vector2(-ancho - margen, Random.Range(-alto, alto)); break;
            case 3: // Derecha
                punto = new Vector2(ancho + margen, Random.Range(-alto, alto)); break;
        }
        // Sumamos la posición de la cámara por si la cámara se mueve
        return punto + (Vector2)cam.transform.position;
    }

    // --- FUNCIÓN PÚBLICA DE GAME OVER ---
    public void ActivarGameOver()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;
        textoTiempoUI.color = Color.red;

        // Detener música de fondo
        bgmSource.Stop();

        // Reproducir sonido de muerte
        bgmSource.PlayOneShot(sfxMuerte);

        // Calculamos el tiempo final una última vez para el mensaje
        int minutos = Mathf.FloorToInt(tiempoJuego / 60);
        int segundos = Mathf.FloorToInt(tiempoJuego % 60);

        textoTiempoUI.text = string.Format("TIEMPO FINAL - {0:00}:{1:00}", minutos, segundos);

        StartCoroutine(SecuenciaCierre());
    }

    TipoHueso SeleccionarTipoAleatorio()
    {
        int pesoTotal = 0;
        foreach (var p in tablaProbabilidades) pesoTotal += p.peso;

        int randomPoint = Random.Range(0, pesoTotal);

        for (int i = 0; i < tablaProbabilidades.Length; i++)
        {
            if (randomPoint < tablaProbabilidades[i].peso)
            {
                return tablaProbabilidades[i].tipo;
            }
            randomPoint -= tablaProbabilidades[i].peso;
        }
        return TipoHueso.Blanco; // Por si acaso
    }

    Color ObtenerColorSegunTipo(TipoHueso tipo)
    {
        switch (tipo)
        {
            case TipoHueso.Gris: return Color.gray;
            case TipoHueso.Verde: return Color.green;
            case TipoHueso.Azul: return new Color(0, 0.5f, 1);
            case TipoHueso.Naranja: return new Color(1, 0.5f, 0);
            case TipoHueso.Rojo: return Color.red;
            case TipoHueso.Amarillo: return Color.yellow;
            case TipoHueso.Morado: return Color.magenta;
            case TipoHueso.MoradoPequeño: return Color.magenta;
            default: return Color.white;
        }
    }
    IEnumerator SecuenciaCierre()
    {
        // Congelar el tiempo del juego
        Time.timeScale = 0f;

        // Esperar usando tiempo real (porque el tiempo del juego está congelado)
        yield return new WaitForSecondsRealtime(segundosAntesDeCerrar);

        // Cerrar aplicación
        Debug.Log("Cerrando juego...");
        Application.Quit();

        // Nota: Application.Quit no funciona en el editor de Unity.
        // Si quieres probar que funciona, añade esta línea:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
