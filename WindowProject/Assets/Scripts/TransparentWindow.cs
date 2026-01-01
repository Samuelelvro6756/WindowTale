using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
    [SerializeField] private Material windowMaterial;

    // Importamos funciones de la API de Windows
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;
    const int GWL_EXSTYLE = -20;
    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TOPMOST = 0x00000008;
    const uint LWA_COLORKEY = 0x00000001;

    void Start()
    {
        // Solo ejecutar en el Build, no en el Editor de Unity
#if !UNITY_EDITOR
        IntPtr hWnd = GetActiveWindow();

        // Eliminar bordes de la ventana
        SetWindowLong(hWnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);

        // Hacer la ventana "Layered" (capas) y ponerla siempre encima (Topmost)
        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TOPMOST);

        // Extender el área transparente a toda la ventana
        MARGINS margins = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);

        // Establecer la posición (opcional: puedes definir un tamaño fijo aquí)
        SetWindowPos(hWnd, new IntPtr(-1), 0, 0, Screen.width, Screen.height, 0);
#endif
    }
}