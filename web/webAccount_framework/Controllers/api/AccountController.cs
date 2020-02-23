using LitJson;
using Mmcoy.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace webAccount_framework.Controllers.api
{
    public class AccountController : ApiController
    {
        // GET: api/Account
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Account/5
        public string Get(int id)
        {
            return "value";
        }

        //// POST: api/Account
        //public void Post([FromBody]string value)
        //{
        //}

        // POST: api/Account
        public RetValue Post([FromBody] string jsonData)
        {
            RetValue retValue = new RetValue();

            Dictionary<string, object> dict = JsonMapper.ToObject<Dictionary<string, object>>(jsonData);
            int type = dict["type"].ToInt();
            string username = dict["username"].ToString();
            string password = dict["password"].ToString();
            short channelId = (short)dict["channelId"].ToInt();

            //注册
            if (type == 0)
            {
                MFReturnValue<int> mfret = AccountDBModel.Instance.Register(username, password, channelId);
                retValue.HasError = mfret.HasError;
                retValue.ErrorMessage = mfret.Message;
                retValue.Value = mfret.Value;
            }
            //登录
            else
            {
                AccountEntity account = AccountDBModel.Instance.Login(username, password);
                if (account != null)
                {
                    retValue.HasError = false;
                    retValue.ErrorMessage = "登录成功!";
                    retValue.Value = account.Id;
                }
                else
                {
                    retValue.HasError = true;
                    retValue.ErrorMessage = "账户不存在!";
                    retValue.Value = account.Id;
                }
            }

            return retValue;
        }

        // PUT: api/Account/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Account/5
        public void Delete(int id)
        {
        }
    }
}
