using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobimExplorer.Rest.Api.CobimBase.Login;

namespace CobimExplorer.Models.User
{
    // TODO : UserInfoView 클래스 추후 필요시 사용 예정(2023.08.30 minjae)
    public class UserInfoView
    {
        /// <summary>
        /// 로그인 정보 
        /// </summary>
        //public LoginHelper.LoginPack loginPack { get; set; }

        /// <summary>
        /// 로그인 토큰 키 (계속 호출하지 말고 싱글톤으로 사용해야함.)
        /// </summary>
        //[Inject]
        //public LoginHelper.Login_Access_Token Token { get; set; }
        //public LoginHelper.Login_Access_Token token { get; set; }
    }
}
