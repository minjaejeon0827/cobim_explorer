using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using CobimExplorer.Common.LogManager;

namespace CobimExplorer.Common.Extensions
{
    public static class VMNameExtensions
    {
        /// <summary>
        /// static 확장메서드 GetViewModelName 정의
        /// 첫번째 파라미터 - 뷰모델 객체만 사용하도록 지정
        /// 뷰모델 이름이 들어간 마지막 인덱스 구하기 
        /// </summary>
        //public static string GetViewModelName(this object ViewModel)
        //{
        //    int lastIndex = -1;

        //    // TODO: 뷰모델 이름 문자열 구하기 (2023.07.06 jbh)
        //    // 참고 URL - https://sheepone.tistory.com/54
        //    string[] vmstring = ViewModel.GetType().Name.Split('.');
        //    lastIndex = vmstring.Length - 1;   // 뷰모델 이름이 들어간 마지막 인덱스 구하기 

        //    string VMName = vmstring[lastIndex];

        //    return VMName;
        //}

        // TODO : 아래 static 메서드 GetViewModelName 필요시 참고 (2024.08.07 jbh)
        /// <summary>
        /// static 확장메서드 GetViewModelName 정의
        /// 첫번째 파라미터 - 뷰모델 객체만 사용하도록 지정
        /// 뷰모델 이름이 들어간 마지막 인덱스 구하기 
        /// </summary>
        public static string GetViewModelName(this object ViewModel)
        {
            int lastIndex = -1;   // 마지막 인덱스

            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 jbh)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                // TODO: 뷰모델 이름 문자열 구하기 (2023.07.06 jbh)
                // 참고 URL - https://sheepone.tistory.com/54
                string[] vmstring = ViewModel.GetType().Name.Split('.');
                lastIndex = vmstring.Length - 1;   // 뷰모델 이름이 들어간 마지막 인덱스 구하기 

                string VMName = vmstring[lastIndex];

                return VMName;
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.6 jbh)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
                //Log.Error($"Error name = {System.Reflection.Assembly.GetEntryAssembly().GetName().Name}");
            }
        }
    }
}
