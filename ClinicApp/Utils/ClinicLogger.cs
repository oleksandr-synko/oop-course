using System;
using System.IO;
using System.Text;
using System.Linq;

namespace ClinicApp.Utils;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    private void WriteLog(string level, string message)
    {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
        File.AppendAllText(_logPath, logEntry + Environment.NewLine, Encoding.UTF8);
    }

    public void LogInfo(string message)
    {
        WriteLog("INFO", message);
    }

    public void LogWarning(string message)
    {
        WriteLog("WARN", message);
    }

    public void LogError(string message)
    {
        WriteLog("ERROR", message);
    }

    public string[] ReadLastLines(int count)
    {
        if (!File.Exists(_logPath))
            return Array.Empty<string>();

        string[] lines = File.ReadAllLines(_logPath);

        return lines.TakeLast(count).ToArray();
    }

    public void Clear()
    {
        File.WriteAllText(_logPath, string.Empty);
    }
}