using ImGuiNET;
using Raylib_cs;

public static class InputManager
{
    public static bool IsMouseAvailable()
    {
        // ImGui fareyi kullanmıyorsa true döner
        return !ImGui.GetIO().WantCaptureMouse && !ImGui.GetIO().WantCaptureKeyboard;
    }
}