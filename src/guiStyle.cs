using ImGuiNET;
using System.Numerics;

class guiStyle
{
    public static void ApplyStyle()
    {
        var style = ImGui.GetStyle();
        var colors = style.Colors;

        // Matte renk paleti (Hex -> Vector4)
        Vector4 matteDarkBlue = new Vector4(0.16f, 0.21f, 0.34f, 1.00f);    // #293556
        Vector4 matteMediumBlue = new Vector4(0.15f, 0.27f, 0.51f, 1.00f);  // #264583
        Vector4 matteBlue = new Vector4(0.24f, 0.38f, 0.76f, 1.00f);        // #3E60C1
        Vector4 matteLightBlue = new Vector4(0.35f, 0.51f, 0.99f, 1.00f);   // #5983FC

        // Temel stil ayarları
        style.WindowPadding = new Vector2(8, 8);
        style.FramePadding = new Vector2(6, 4);
        style.ItemSpacing = new Vector2(6, 4);
        style.WindowRounding = 4f;
        style.FrameRounding = 3f;

        // Renk atamaları
        colors[(int)ImGuiCol.WindowBg] = matteDarkBlue;              // Ana pencere arkaplan
        colors[(int)ImGuiCol.ChildBg] = matteDarkBlue * 0.9f;        // Alt pencereler
        colors[(int)ImGuiCol.TitleBg] = matteMediumBlue;             // Başlık çubuğu (aktif değil)
        colors[(int)ImGuiCol.TitleBgActive] = matteBlue;             // Aktif başlık çubuğu
        colors[(int)ImGuiCol.FrameBg] = matteMediumBlue * 0.8f;      // Input alanları
        colors[(int)ImGuiCol.Button] = matteBlue;                    // Normal butonlar
        colors[(int)ImGuiCol.ButtonHovered] = matteBlue * 1.1f;      // Hover durumu
        colors[(int)ImGuiCol.ButtonActive] = matteLightBlue;         // Tıklanmış buton
        colors[(int)ImGuiCol.Header] = matteBlue;                    // Seçili item (liste vs.)
        colors[(int)ImGuiCol.Text] = new Vector4(0.95f, 0.96f, 0.98f, 1.00f); // Beyaz metin
    }
}