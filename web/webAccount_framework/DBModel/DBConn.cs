using System;
public static class DBConn
{
    private static string m_DBAccount;

    public static string DBAccount
    {
        get
        {
            if (string.IsNullOrEmpty(m_DBAccount))
            {
                m_DBAccount = System.Configuration.ConfigurationManager.AppSettings["DB"].ToString();
                    
            }
            return m_DBAccount;
        }
    }
}