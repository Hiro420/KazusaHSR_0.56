using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KazusaHSR.Utils;
public class Config
{
    public GameServerInfo GameServer { get; set; } = new();
    public WebServerInfo WebServer { get; set; } = new();
    public AccountDataBaseInfo AccountDataBase { get; set; } = new();
    public KeyStoreInfo KeyStore { get; set; } = new();
    public LogOptionInfo LogOption { get; set; } = new();

    public static Config Load(string filePath = "config.json")
    {
        if (!System.IO.File.Exists(filePath))
        {
            var defaultConfig = new Config();
            System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(defaultConfig, Formatting.Indented));
            Console.WriteLine("Config file created. Please edit the config file and restart the server.");
            Environment.Exit(0);
        }

        return JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText(filePath))!;
    }
}

public class LogOptionInfo
{
    public bool Commands { get; set; } = true;
    public bool Connections { get; set; } = true;
    public bool Packets { get; set; } = true;
}

public class GameServerInfo
{
    public string ServerIP { get; set; } = "127.0.0.1";
    public int ServerPort { get; set; } = 6969;
    public bool AutoCreateAccount { get; set; } = true; // unused for now
}

public class WebServerInfo
{
    public string ServerIP { get; set; } = "127.0.0.1";
    public int ServerPort { get; set; } = 3000;
    public bool UseSSL { get; set; } = false;
}

// still not used //

public class AccountDataBaseInfo
{
    public string Uri { get; set; } = "mongodb://localhost:27017";
    public string Collection { get; set; } = "KazusaHSR";
    public bool UseInternal { get; set; } = true;
}

public class KeyStoreInfo
{
    public string Path { get; set; } = "./keystore.p12";
    public string Password { get; set; } = "Kazusa";
}