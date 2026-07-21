<p align="center">
  <img src="docs/banner.png" width="100%" alt="WindowTale Banner">
</p>
<h1 align="center">WindowTale</h1>
<p align="center">
<b>Un Bullet Hell donde tu escritorio se convierte en el campo
de batalla.</b>
</p>
<p align="center">
Inspirado en <b>Undertale</b> y <b>Just
Shapes & Beats</b>, WindowTale rompe la cuarta pared utilizando
una ventana transparente que permite jugar directamente sobre el
escritorio de Windows.
</p>
<p align="center">
<img src="https://img.shields.io/badge/Unity-2022+-black?style=for-the-badge&logo=unity">
<img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white">
<img src="https://img.shields.io/badge/2D-Bullet_Hell-blue?style=for-the-badge">
<img src="https://img.shields.io/badge/Platform-Windows-0078D6?style=for-the-badge&logo=windows">
</p>

<br>

# 📑 Índice

-   📖 Descripción
-   ✨ Características
-   🎮 Mecánicas
-   🦴 Tipos de Huesos
-   🎮 Controles
-   🛠 Tecnologías
-   📂 Estructura
-   📸 Capturas
-   🚀 Instalación
-   📈 Estado
-   💡 Roadmap
-   👨‍💻 Autor

<br>

# 📖 Descripción

**WindowTale** es un videojuego **Bullet Hell 2D** desarrollado en
**Unity**, donde el jugador controla un corazón que debe sobrevivir a
oleadas de ataques utilizando movimiento preciso y un dash de esquiva.

Su principal característica es el uso de una **ventana transparente**,
permitiendo que el escritorio del usuario forme parte del escenario y
convirtiendo el sistema operativo en el campo de batalla.

<br>

# ✨ Características

-   🖥️ Juego sobre el escritorio de Windows.
-   ❤️ Sistema de vidas.
-   ⚡ Dash con tiempo de reutilización.
-   🌍 Movimiento continuo entre bordes de pantalla.
-   🦴 Enemigos con mecánicas basadas en colores.
-   📈 Dificultad progresiva.
-   🎨 Estética Pixel Art.

<br>

# 🎮 Mecánicas

### ❤️ Supervivencia

Sobrevive el mayor tiempo posible evitando los ataques.<br><br>
<img src="docs/gifs/survive.gif" width="60%" alt="WindowTale Survive">

### ⚡ Dash

Permite esquivar ataques rápidamente con un breve tiempo de
invulnerabilidad.<br><br>
<img src="docs/gifs/dash.gif" width="60%" alt="WindowTale Dash">

### 🌍 Movimiento entre bordes

Al salir por un borde de la pantalla el jugador aparece por el lado
contrario.<br><br>
<img src="docs/gifs/wrap.gif" width="60%" alt="WindowTale Wrap">

### 📈 Dificultad dinámica

Conforme avanza la partida aumentan la velocidad y la cantidad de
ataques.<br><br>
<img src="docs/gifs/difficult.gif" width="60%" alt="WindowTale Wrap">

<br>

# 🦴 Tipos de Huesos

  | Tipo | Mecánica |
  | -------------------- | --------------------------------- |
  |💿 Gris         |    _No hace nada_          |
  |⚪ Blanco         |    Hace daño al contacto.          |
  |🔵 Azul   |           Solo daña cuando el jugador se mueve.   |            
  |🟠 Naranja      |     Solo daña cuando el jugador permanece quieto.    |             
  |🟢 Verde        |     Recupera una vida al tocarlo.  |
  |🔴 Rojo        |      Rebota continuamente por la pantalla.    |
  |🟡 Amarillo   |       Se desplaza con trayectorias especiales. | 
  |🟣 Morado      |      Al impactar contra un borde se fragmenta en múltiples huesos pequeños.  |                   
  ---------------------------------------------------------------------------

<br>


# 🛠 Tecnologías

  |Categoría |   Tecnología|
  |------------| -------------------|
  |Motor |       Unity|
  |Lenguaje   |  C#|
  |UI           |TextMeshPro, UGUI|
  |Render       |Post Processing|
  |Plataforma  | Windows|
 
<br>

# 📂 Estructura del Proyecto

``` text
WindowProject/
├── Assets/
│   ├── Scripts/
│   ├── Scenes/
│   ├── Prefabs/
│   ├── Sprites/
│   └── Audio/
├── Packages/
├── ProjectSettings/
├── Ejecutable/
└── README.md
```

<br>

# 🚀 Instalación

1.  Abrir carpeta de Ejecutable
2. Ejecutar el archivo WindowTale.exe

<br>

# 📈 Estado

  |Característica            | Estado|
  |-------------------------| --------|
  |Movimiento             |      ✅|
  |Dash                  |       ✅|
  |IA de huesos           |      ✅|
  |Sistema de vidas       |      ✅|
  |Ventana transparente    |    ✅|
  |Fragmentación de huesos   |   ✅|

🟢 **Proyecto en desarrollo activo**

<br>

# 💡 Roadmap

-   🎵 Música dinámica.
-   👾 Nuevos patrones de ataque.
-   🦴 Más tipos de huesos.
-   🏆 Sistema de puntuaciones.
-   🎮 Compatibilidad con mando.

<br>

# 👨‍💻 Autor

**Samuel Durán Cárdenas**

Desarrollador de videojuegos con Unity y C#.


<p align="center">
<b>⭐ Si te gustó el proyecto, considera dejar una estrella en
el repositorio.`</b>`{=html}<br><br> Desarrollado con
❤️ por <b>Samuel Durán</b>
</p>
