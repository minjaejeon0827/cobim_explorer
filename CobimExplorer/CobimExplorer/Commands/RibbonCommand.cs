using Serilog;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using CobimExplorer.Services.Page;
using CobimExplorer.ViewModels;
using CobimExplorer.Common.CobimBase;
using CobimExplorer.Common.LogManager;
using CobimExplorer.Data.Models.CobimBase.Main;
using CobimExplorer.ViewModels.Windows.Test;

namespace CobimExplorer.Commands
{
    // TODO : RibbonCommand 클래스 필요시 참고 (2024.08.06 jbh)
    // TODO : 구현시 참고 유튜브 영상 URL -  https://youtu.be/2FPFgW0xVB0 (2023.07.12 jbh)
    public class RibbonCommand : Command
    {
        private readonly TestMainVM _mainViewModel;

        //public string vmName { get => _vmName; set { _vmName = value; NotifyOfPropertyChange(); } }
        //private string _vmName = string.Empty;

        public RibbonCommand(TestMainVM mainVM, IContainer container)
        {
            _mainViewModel = mainVM;
        }

        public override void Execute(object parameter)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.11.10 jbh)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                // 아래 리본 버튼 클릭 여부 초기화 로직 필요시 수정 예정 (2023.12.14 jbh)
                // MainView.CommonData.IsRibbonBtnClick = false;  // 리본 버튼 클릭 여부 초기화

                //_mainViewModel.ChangeExplorerViewModel();
                var changeVM = _mainViewModel.Items.Where(it => string.Equals(it.GetType().Name, parameter.ToString())).FirstOrDefault();

                //var changeVM = this.Items.Where(it => it.GetType().Equals(TestViewModel.GetType())).FirstOrDefault();

                // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                if (changeVM is not null)
                {
                    // 리본 버튼 클릭한 경우 로직 필요시 수정 예정 (2023.12.14 jbh) 
                    // 리본 버튼 클릭한 경우
                    MainView.CommonData.BtnClickType = string.Empty;
                    MainView.CommonData.BtnClickType = CobimHelper.RibbonBtnType;
                    // MainView.CommonData.IsRibbonBtnClick = true;   // 리본 버튼 클릭한 경우

                    // changeVM에 할당된 변경할 뷰모델 활성화
                    _mainViewModel.ActivateItem(changeVM);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
            
        }
    }
}
