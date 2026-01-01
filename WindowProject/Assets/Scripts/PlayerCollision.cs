using UnityEngine;

public class JugadorColision : MonoBehaviour
{
    private PlayerDashMovement scriptMovimiento;

    void Start()
    {
        scriptMovimiento = GetComponent<PlayerDashMovement>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bone"))
        {
            ProcesarContacto(collision.GetComponent<HuesoMovimiento>());
        }
    }

    // Usamos Stay para los huesos Azul y Naranja (que dependen de si te mueves)
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bone"))
        {
            ProcesarContacto(collision.GetComponent<HuesoMovimiento>());
        }
    }

    void ProcesarContacto(HuesoMovimiento hueso)
    {
        if (hueso == null) return;

        // 1. El hueso VERDE siempre cura, incluso en dash
        if (hueso.tipo == TipoHueso.Verde)
        {
            GameManager.instancia.ModificarVida(1);
            Destroy(hueso.gameObject);
            return;
        }

        // 2. Si estamos en DASH, ignoramos todos los demás huesos (invulnerabilidad)
        if (scriptMovimiento != null && scriptMovimiento.estaEnDash) return;

        // 3. Lógica de daño según el tipo de hueso
        float movH = Input.GetAxisRaw("Horizontal");
        float movV = Input.GetAxisRaw("Vertical");
        bool seMueve = (movH != 0 || movV != 0);

        switch (hueso.tipo)
        {
            case TipoHueso.Blanco:
            case TipoHueso.Rojo:
            case TipoHueso.Amarillo:
                GameManager.instancia.ModificarVida(-1);
                break;
            case TipoHueso.Morado:
            case TipoHueso.MoradoPequeño:
                GameManager.instancia.ModificarVida(-1);
                break;
            case TipoHueso.Azul:
                if (seMueve) GameManager.instancia.ModificarVida(-1);
                break;

            case TipoHueso.Naranja:
                if (!seMueve) GameManager.instancia.ModificarVida(-1);
                break;
        }
    }
}