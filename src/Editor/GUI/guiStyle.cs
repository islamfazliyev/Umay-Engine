// guiStyle.cs
using ImGuiNET;
using System.Numerics;

class guiStyle
{
    public static void ApplyStyle()
    {
        var style = ImGui.GetStyle();
        var colors = style.Colors;

        // HEX'ten Vector4'e dönüştürülmüş renkler
        Vector4 richBlack = new Vector4(0.11f, 0.04f, 0.00f, 1.00f);       // #1C0A00 (Derin Siyah-Kahve)
        Vector4 mahogany = new Vector4(0.21f, 0.08f, 0.00f, 1.00f);        // #361500 (Maun)
        Vector4 burntSienna = new Vector4(0.38f, 0.21f, 0.00f, 1.00f);     // #603601 (Yanık Toprak)
        Vector4 goldenAmber = new Vector4(0.80f, 0.58f, 0.27f, 1.00f);     // #CC9544 (Altın Amber)

        // Temel stil ayarları (Modern yuvarlak köşeler)
        style.WindowPadding = new Vector2(10, 10);
        style.FramePadding = new Vector2(8, 6);
        style.ItemSpacing = new Vector2(8, 6);
        style.WindowRounding = 8f;
        style.FrameRounding = 6f;
        style.ScrollbarRounding = 4f;

        // Renk atamaları (Dramatik ve lüks bir tema)
        colors[(int)ImGuiCol.WindowBg] = richBlack;            // Ana pencere arkaplan
        colors[(int)ImGuiCol.ChildBg] = mahogany * 0.8f;        // Alt pencereler
        colors[(int)ImGuiCol.TitleBg] = burntSienna;           // Başlık çubuğu (pasif)
        colors[(int)ImGuiCol.TitleBgActive] = goldenAmber;     // Aktif başlık
        colors[(int)ImGuiCol.FrameBg] = mahogany;              // Input alanları
        colors[(int)ImGuiCol.Button] = burntSienna;           // Normal butonlar
        colors[(int)ImGuiCol.ButtonHovered] = goldenAmber;     // Hover efekti (altın vurgu)
        colors[(int)ImGuiCol.ButtonActive] = goldenAmber * 1.2f; // Tıklanmış buton (parlak)
        colors[(int)ImGuiCol.Text] = goldenAmber;              // Metinler (okunabilir)
        colors[(int)ImGuiCol.Border] = goldenAmber * 0.5f;     // Çerçeveler (soft altın)
    }
}