using System;
public static class DBConn
{
    private static string rpg_gameServer;

    public static string RPG_GameServer
    {
        get
        {
            if (string.IsNullOrEmpty(rpg_gameServer))
            {
                //rpg_gameServer = System.Configuration.ConfigurationManager.AppSettings["DB"].ToString();
                
            }
            return rpg_gameServer;
        }
    }
}