using System;


public class AccountEntity
{
    private int id;
    public virtual int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    private string userName;
    public virtual string UserName
    {
        get
        {
            return userName;
        }

        set
        {
            userName = value;
        }
    }

    private string password;
    public virtual string Password
    {
        get
        {
            return password;
        }

        set
        {
            password = value;
        }
    }

    private int yuanBao;
    public virtual int YuanBao
    {
        get
        {
            return yuanBao;
        }

        set
        {
            yuanBao = value;
        }
    }

    private int lastServerId;
    public virtual int LastServerId
    {
        get
        {
            return lastServerId;
        }

        set
        {
            lastServerId = value;
        }
    }

    private string lastServerName;
    public string LastServerName
    {
        get
        {
            return lastServerName;
        }

        set
        {
            lastServerName = value;
        }
    }

    private DateTime currentTime;
    public DateTime CurrentTime
    {
        get
        {
            return currentTime;
        }

        set
        {
            currentTime = value;
        }
    }

    private DateTime updateTime;
    public DateTime UpdateTime
    {
        get
        {
            return updateTime;
        }

        set
        {
            updateTime = value;
        }
    }


    public override string ToString()
    {
        string info = "";

        info = "ID=" + Id + ", UserName=" + UserName + ", Password=" + Password + ", YuanBao=" + YuanBao + ", LastServerId=" + LastServerId + ", LastServerName=" + LastServerName + ", CurrentTime=" + CurrentTime + ",UpdateTime="+ UpdateTime;

        return info;
    }

}