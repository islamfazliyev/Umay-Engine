using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImGuiNET;

public class FileBrowser
{
    private string _currentDirectory;
    private List<string> _directories = new List<string>();
    private List<string> _files = new List<string>();
    private string _selectedPath = "";
    private string[] _allowedExtensions;

    public string SelectedPath => _selectedPath;

    // Kurucu Metod (Extensions örnek: "png", "jpg")
    public FileBrowser(string initialPath = null, string[] allowedExtensions = null)
    {
        _allowedExtensions = allowedExtensions ?? Array.Empty<string>();
        _currentDirectory = initialPath ?? Directory.GetCurrentDirectory();
        RefreshDirectory();
    }

    // Dizin içeriğini yenile
    public void RefreshDirectory()
    {
        _directories = Directory.GetDirectories(_currentDirectory)
            .Select(Path.GetFileName)
            .ToList();

        _files = Directory.GetFiles(_currentDirectory)
            .Where(file => _allowedExtensions.Length == 0 || 
                   _allowedExtensions.Contains(Path.GetExtension(file).TrimStart('.')))
            .Select(Path.GetFileName)
            .ToList();
    }

    // GUI'yi çiz
    public bool Draw()
    {
        bool fileSelected = false;
        
        ImGui.Begin("File Browser");

        // Geri düğmesi (Üst dizine çık)
        if (ImGui.Button("←") && Directory.GetParent(_currentDirectory) != null)
        {
            _currentDirectory = Directory.GetParent(_currentDirectory).FullName;
            RefreshDirectory();
        }

        ImGui.SameLine();
        ImGui.Text(_currentDirectory);

        // Dizinler
        foreach (var dir in _directories)
        {
            if (ImGui.Selectable($"📁 {dir}"))
            {
                _currentDirectory = Path.Combine(_currentDirectory, dir);
                RefreshDirectory();
            }
        }

        // Dosyalar
        foreach (var file in _files)
        {
            if (ImGui.Selectable($"X {file}"))
            {
                _selectedPath = Path.Combine(_currentDirectory, file);
                fileSelected = true;
            }
        }

        ImGui.End();
        
        return fileSelected;
    }
}