using System.Configuration;

namespace TheIslandPostManager.Models;

public class AppSettings : ConfigurationSection
{
    private List<string> additionalDirectories;
    #region Directories


    [ConfigurationProperty("inputDirectory",DefaultValue = "")]
	public string InputDirectory
	{
		get { return (string)this["inputDirectory"]; }
		set { this["inputDirectory"] =  value; }
	}

    [ConfigurationProperty("outputDirectory", DefaultValue = "")]
    public string OutputDirectory
    {
        get { return (string)this["outputDirectory"]; }
        set { this["outputDirectory"] = value; }
    }

    [ConfigurationProperty("printerDirectory", DefaultValue = "")]
    public string PrinterDirectory
    {
        get { return (string)this["printerDirectory"]; }
        set { this["printerDirectory"] = value; }
    }

    [ConfigurationProperty("backupDirectory", DefaultValue = "")]
    public string BackupDirectory
    {
        get { return (string)this["backupDirectory"]; }
        set { this["backupDirectory"] = value; }
    }

    [ConfigurationProperty("pendingDirectory", DefaultValue = "")]
    public string PendingDirectory
    {
        get { return (string)this["pendingDirectory"]; }
        set { this["pendingDirectory"] = value; }
    }

    #endregion

    #region Watermark
    [ConfigurationProperty("watermarkDirectory", DefaultValue = "")]
    public string WatermarkDirectory
    {
        get { return (string)this["watermarkDirectory"]; }
        set { this["watermarkDirectory"] = value; }
    }

    [ConfigurationProperty("watermarkPosition", DefaultValue = 4)]
    public int WatermarkPosition
    {
        get { return (int)this["watermarkPosition"]; }
        set { this["watermarkPosition"] = value; }
    }

    [ConfigurationProperty("watermarkBufferW", DefaultValue = 100)]
    public int WatermarkBufferW
    {
        get { return (int)this["watermarkBufferW"]; }
        set { this["watermarkBufferW"] = value; }
    }

    [ConfigurationProperty("watermarkBufferH", DefaultValue = 100)]
    public int WatermarkBufferH
    {
        get { return (int)this["watermarkBufferH"]; }
        set { this["watermarkBufferH"] = value; }
    }

    [ConfigurationProperty("addWwatermark", DefaultValue = true)]
    public bool AddWwatermark
    {
        get { return (bool)this["addWwatermark"]; }
        set { this["addWwatermark"] = value; }
    }
    #endregion

    #region Email

    [ConfigurationProperty("email", DefaultValue = "")]
    public string Email
    {
        get { return (string)this["email"]; }
        set { this["email"] = value; }
    }

    [ConfigurationProperty("password", DefaultValue = "")]
    public string Password
    {
        get { return (string)this["password"]; }
        set { this["password"] = value; }
    }

    [ConfigurationProperty("host", DefaultValue = "")]
    public string Host
    {
        get { return (string)this["host"]; }
        set { this["host"] = value; }
    }

    [ConfigurationProperty("emailTimeout", DefaultValue = 2)]
    public int EmailTimeout
    {
        get { return (int)this["emailTimeout"]; }
        set { this["emailTimeout"] = value; }
    }

    [ConfigurationProperty("companyName", DefaultValue = "")]
    public string CompanyName
    {
        get { return (string)this["companyName"]; }
        set { this["companyName"] = value; }
    }

    [ConfigurationProperty("portNumber", DefaultValue = 0)]
    public int PortNumber
    {
        get { return (int)this["portNumber"]; }
        set { this["portNumber"] = value; }
    }

    [ConfigurationProperty("enableSSL", DefaultValue = true)]
    public bool EnableSSL
    {
        get { return (bool)this["enableSSL"]; }
        set { this["enableSSL"] = value; }
    }


    #endregion

    [ConfigurationProperty("imageQuality", DefaultValue = 90)]
    public int ImageQuality
    {
        get { return (int)this["imageQuality"]; }
        set { this["imageQuality"] = value; }
    }

    [ConfigurationProperty("mySqlUsername", DefaultValue = "root")]
    public string MySqlUsername
    {
        get { return (string)this["mySqlUsername"]; }
        set { this["mySqlUsername"] = value; }
    }

    [ConfigurationProperty("mysqlDatabaseName", DefaultValue = "")]
    public string MysqlDatabaseName
    {
        get { return (string)this["mysqlDatabaseName"]; }
        set { this["mysqlDatabaseName"] = value; }
    }

    [ConfigurationProperty("mysqlDatasource", DefaultValue = "")]
    public string MysqlDatasource
    {
        get { return (string)this["mysqlDatasource"]; }
        set { this["mysqlDatasource"] = value; }
    }

    [ConfigurationProperty("mysqlPassword", DefaultValue = "")]
    public string MysqlPassword
    {
        get { return (string)this["mysqlPassword"]; }
        set { this["mysqlPassword"] = value; }
    }

    [ConfigurationProperty("mysqlPortNumber", DefaultValue = 3306)]
    public int MysqlPortNumber
    {
        get { return (int)this["mysqlPortNumber"]; }
        set { this["mysqlPortNumber"] = value; }
    }

    [ConfigurationProperty("scrollSync", DefaultValue = true)]
    public bool ScrollSync
    {
        get { return (bool)this["scrollSync"]; }
        set { this["scrollSync"] = value; }
    }

    [ConfigurationProperty("additionalDirectories")]

    public string AdditionalDirectories
    {
        get { return (string)this["additionalDirectories"]; }
        set
        {
            this["additionalDirectories"] = value;
        }
    }


}
