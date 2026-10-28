using Stylet;
using StyletIoC;
using Serilog;
using Microsoft.Win32;

using System;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using System.Threading.Tasks;

using CobimExplorer.Services.Part;
using CobimExplorer.Commands;
using CobimExplorer.Common.CobimBase;
using CobimExplorer.Common.LogManager;

using CobimExplorer.Data.Models.CobimBase.Explorer;
using CobimExplorer.Interface.Page;
using CobimExplorer.Message;

namespace CobimExplorer.ViewModels.Part.Project.CDE.Explorer
{
    // TODO : 체크인 화면(CheckInPartV.xaml) -> 버튼 "파일열기" 클릭시 팝업화면 "열기" 출력되면서 폴더 및 파일목록이 출력되도록
    //        체크인 뷰모델 (CheckInPartVM.cs)에서 메서드 OpenFile 몸체 안에서 -> 파일 열기 메서드 "OpenFileDialog" 사용해서 구현하기 (2023.11.30 minjae)
    // 참고 URL   - https://rhkdrmfh.tistory.com/119
    // 참고 2 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-C-%ED%8F%B4%EB%8D%94-%EC%97%B4%EA%B8%B0-%E2%80%93-Process-Start
    // 참고 3 URL - https://insurang.tistory.com/entry/WPF-C-OpenFileDialog-%ED%8C%8C%EC%9D%BC%EC%97%B4%EA%B8%B0-%EC%83%81%EC%84%B8-%ED%8C%8C%EC%9D%BC%EC%98%A4%ED%94%88-%EC%98%A4%ED%94%88%EB%8B%A4%EC%9D%B4%EC%96%BC%EB%A1%9C%EA%B7%B8-CustomPlaces-InitialDirectory-Path-%ED%8F%B4%EB%8D%94-folder-Filter-%ED%95%84%ED%84%B0-%ED%99%95%EC%9E%A5%EC%9E%90-%EC%9C%84%EC%B9%98-%EC%A7%80%EC%A0%95-ofdlg-CheckFileExists-FileOk-ShowDialog

    // TODO : 탐색기 화면(ExplorerV.xaml), 뷰모델(ExplorerVM.cs)에서 체크인 Part 화면 구현할 때, PartBase 클래스 구현하기 (2023.11.30 minjae)
    public class CheckInPartVM : PartBase<ExplorerView>, IPage
    {
        #region 프로퍼티 

        // TODO : 체크인 팝업 화면(페이지) 뷰모델 프로퍼티 "checkInPartVM" 필요시 사용 예정 (2023.11.30 minjae)
        /// <summary>
        /// 체크인 팝업 화면(페이지)
        /// </summary>
        public CheckInPartVM checkInPartVM { get; set; }

        // TODO : 파일 열기 Command "OpenFileCommand" 필요시 사용 예정 (2023.12.04 minjae)
        /// <summary>
        /// 파일 열기 Command
        /// </summary>
        // public ICommand OpenFileCommand { get; set; }

        /// <summary>
        /// 체크인 업로드(UpLoad) Command
        /// </summary>
        public ICommand UpLoadCommand { get; set; }

        /// <summary>
        /// 종료 Command
        /// </summary>
        public ICommand ExitCommand { get; set; }

        /// <summary>
        /// 테스트 Command
        /// </summary>
        public ICommand TestCommand { get; set; }

        /// <summary>
        /// 컨테이너 체크인 + 파일 웹서버 업로드 정보
        /// </summary>
        private CheckInPack _CheckInPack { get; set; }

        // TODO : 필요시 프로퍼티 "Container" 사용 예정 (2024.01.04 minjae) 
        //public IContainer Container { get; set; }

        // TODO : 필요시 프로퍼티 "EventAggregator" 사용 예정 (2024.01.04 minjae) 
        //[Inject]
        //public IEventAggregator EventAggregator { get; set; }

        // TODO : 필요시 프로퍼티 "WindowManager" 사용 예정 (2024.01.04 minjae) 
        //[Inject]
        //public IWindowManager WindowManager { get; set; }


        // TODO : 체크인 팝업화면 제목 프로퍼티 "CheckIn_Title" 필요 없을 시 삭제 예정 (2023.11.30 minjae)
        /// <summary>
        /// 체크인 팝업화면 제목
        /// </summary>
        public string CheckIn_Title { get => _CheckIn_Title; set { _CheckIn_Title = value; NotifyOfPropertyChange(); } }
        private string _CheckIn_Title;


        /// <summary>
        /// 체크인 Comment 입력 - Description 
        /// </summary>
        public string TxtComment { get => _TxtComment; set { _TxtComment = value; NotifyOfPropertyChange(); } }
        private string _TxtComment = string.Empty;


        // TODO : 테스트 OpenFileDialog 파일정보 텍스트 프로퍼티 "TxtTestFileInfo" 필요시 사용 예정 (2023.12.04 minjae)
        //#region 테스트

        ///// <summary>
        ///// 테스트 OpenFileDialog 파일정보 텍스트
        ///// </summary>
        //public string TxtTestFileInfo { get => _TxtTestFileInfo; set { _TxtTestFileInfo = value; NotifyOfPropertyChange(); } }
        //private string _TxtTestFileInfo = string.Empty;

        //#endregion 테스트 

        /// <summary>
        /// 종료 (Exit)
        /// </summary>
        public bool IsExit { get => _IsExit; set { _IsExit = value; NotifyOfPropertyChange(nameof(_IsExit)); } }
        private bool _IsExit;

        /// <summary>
        /// 업로드 할 파일 데이터 저장 완료 여부
        /// </summary>
        public bool IsSave { get => _IsSave; set { _IsSave = value; NotifyOfPropertyChange(nameof(IsSave)); } }
        private bool _IsSave;

        #endregion 프로퍼티 

        #region 생성자 

        public CheckInPartVM(IEventAggregator events, IContainer container) : base(container) 
        {
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2023.11.30 minjae)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;

            this.CheckIn_Title   = CobimHelper.CheckIn_Title;

            UpLoadCommand = new ButtonCommand(CobimHelper.AsyncMethodType, UpLoadFileAsync, CanExecuteMethod);
            ExitCommand = new ButtonCommand(CobimHelper.VoidMethodType,  ExitCheckIn, CanExecuteMethod);
            TestCommand = new ButtonCommand(CobimHelper.AsyncMethodType, TestAsync,   CanExecuteMethod);
            // TODO : 파일 열기 Command "OpenFileCommand" 필요시 사용 예정 (2023.12.04 minjae)
            // OpenFileCommand = new ButtonCommand(CobimHelper.MethodType, OpenFile, CanExecuteMethod);
            
        }

        #endregion 생성자 

        #region 컨테이너 체크인 + 파일 웹서버 업로드 정보

        /// <summary>
        /// 컨테이너 체크인 + 파일 웹서버 업로드 정보 값 셋팅 
        /// </summary>
        public CheckInPartVM SetCheckInPack(CheckInPack checkInPack)
        {
            _CheckInPack = checkInPack;
            return this;
        }

        /// <summary>
        /// 컨테이너 체크인 + 파일 웹서버 업로드 정보 가져오기 
        /// </summary>
        public CheckInPack GetCheckInPack()
        {
            return _CheckInPack;
        }

        #endregion 컨테이너 체크인 + 파일 웹서버 업로드 정보

        #region 기본 메소드 

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
            checkInPartVM = this;
        }

        // TODO : 메서드 OnActivate 필요시 사용 예정 (2023.12.04 minjae)
        //protected override void OnActivate()
        //{
        //    base.OnActivate();
        //}

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        // TODO : 메서드 "CanExecuteMethod" 필요시 수정 예정 (2023.10.4 minjae)
        private bool CanExecuteMethod(object obj)
        {
            return true;
        }

        #endregion 기본 메소드 

        #region ShowDialog

        /// <summary>
        /// 체크인 팝업 화면 출력
        /// </summary>
        public void ShowDialog()
        {
            WindowManager.ShowDialog((object)this);
        }

        #endregion ShowDialog

        #region OpenFile

        // TODO : 파일 열기 메서드 "OpenFile" 필요시 구현 예정 (2023.11.30 minjae)
        /// <summary>
        /// 파일 열기
        /// </summary>
        //private void OpenFile(object obj)
        //{
        //    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.11.07 minjae)
        //    var currentMethod = MethodBase.GetCurrentMethod();

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);

        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.6 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/       
        //        // throw;
        //    }
        //}

        #endregion OpenFile

        #region UpLoadFileAsync

        /// <summary>
        /// 컨테이너 체크인 + 업로드
        /// </summary>
        private async Task UpLoadFileAsync(object obj)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : 업로드 할 파일 데이터 저장 메서드 "SaveData" 필요시 구현 예정 (2023.12.05 minjae)
                // IsSave = SaveData();     // 업로드 할 파일 데이터 저장

                // 체크인 Comment 메시지 입력한 경우 
                if(false == string.IsNullOrWhiteSpace(TxtComment))
                {
                    // TODO : 아래 주석친 코드 필요시 사용 예정 (2024.01.05 minjae)
                    // ExplorerView.RequestData.exp_Comment = TxtComment;
                    // InputParamData.exp_Comment = TxtComment;
                    _CheckInPack.SetComment(TxtComment);
                    this.RequestClose(null);
                    // IsSave = true;
                }
                // 체크인 Comment 메시지 공백 또는 null인 경우 
                else MessageBox.Show(CobimHelper.CheckIn_CommentWhiteSpace,
                                     CobimHelper.NoticeTitle);

                // if (IsSave) this.RequestClose(IsSave);
                // else throw new Exception("업로드 실패\r\n담당자에게 문의하세요.");
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
                //IsSave = false;

            }
            return;
        }

        #endregion UpLoadFileAsync

        #region SaveData

        // TODO : 업로드 할 파일 데이터 저장 메서드 "SaveData" 필요시 구현 예정 (2023.12.05 minjae)
        /// <summary>
        /// 업로드 할 파일 데이터 저장
        /// </summary>
        //private bool SaveData()
        //{
        //    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
        //    var currentMethod = MethodBase.GetCurrentMethod();

        //    try
        //    {
        //        // Rest API 11. 컨테이너 체크인 + 업로드 실행시 필요한 요청 데이터 (Body(form-data))
        //        // ExplorerView.RequestData.CheckIn.exp_FolderId    = string.Empty;
        //        // ExplorerView.RequestData.CheckIn.exp_UpLoadFiles = string.Empty;
        //        // ExplorerView.RequestData.CheckIn.exp_FileName    = string.Empty;
        //        // ExplorerView.RequestData.CheckIn.exp_FileSize    = string.Empty;

        //        // ExplorerView.RequestData.exp_Comment = string.Empty;
        //        // ExplorerView.RequestData.exp_Comment = TxtComment;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + "SaveData Error = " + ex.Message);
        //        return false;
        //    }
        //}

        #endregion SaveData

        #region TestAsync

        // TODO : [테스트용 팝업 "열기" 화면 출력 및 컨테이너 체크인 + 업로드 할 파일 열기 구현 보류 - 추후 인재 INC와 소통 필요] 
        // 기존 체크아웃한 파일 말고 새로운 파일도 다중 파일 업로드 기능 필요
        // 컨테이너 1개 -> 파일 A, B, C ... (1 : N 관계) 로 체크인 + 다중 파일 업로드 할 수 있게끔
        // 인재 INC와 소통 진행 필요함. (추후 Rest API 새로 구현 요청)
        // TODO : WPF OpenFileDialog 사용해서 테스트용 팝업 "열기" 화면 출력 및 컨테이너 체크인 + 업로드 할 파일 열기 구현 (2023.12.04 minjae) 
        // 유튜브 참고 URL
        // https://youtu.be/Ks9bzPSx7Vs?si=GkwGbWPvbSYOYOwC

        // 유튜브 참고 2 URL
        // https://youtu.be/2fy6csZb9I0?si=Be3ubDMHW0X4bw5S
        /// <summary>
        /// 테스트용 팝업 "열기" 화면 출력 및 컨테이너 체크인 + 업로드 할 파일 열기
        /// </summary>
        private async Task TestAsync(object obj)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.11.07 minjae)
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();

                // 열고 싶지 않은 파일 필터링(Filter) 처리 (해당 파일 확장자는 화면에 출력 안됨.)
                fileDialog.Filter = "C# Source Files | *.cs";
                //fileDialog.InitialDirectory = "C:\\temp";

                //  테스트용 팝업 "열기" 화면 상단 타이틀바 제목(fileDialog.Title)에 문자열 할당
                fileDialog.Title  = "Please pick CS Source file(s)...";

                // 테스트용 팝업 "열기" 화면에서 여러 개의 파일 선택(Multiselect) 가능(true)
                fileDialog.Multiselect = true;

                // 테스트용 팝업 "열기" 화면에서 여러 개의 파일 선택(Multiselect) 불가능(false)
                // fileDialog.Multiselect = false;

                bool? result = fileDialog.ShowDialog();

                if (result == true)
                {
                    // 테스트용 팝업 "열기" 화면에서 여러 개의 파일 선택(Multiselect) 불가능(false)일 때 사용하는 코드 
                    // 선택한 파일의 전체 파일 경로를 string 클래스 객체 "filePath"에 문자열 할당
                    // string filePath = fileDialog.FileName;
                    // "filePath"에 할당된 문자열을 프로퍼티 "TxtTestFileInfo"에 재할당 및 화면의 TextBox 출력 
                    // TxtTestFileInfo = filePath;

                    // 테스트용 팝업 "열기" 화면에서 여러 개의 파일 선택(Multiselect) 가능(true)할 때 사용하는 코드
                    string[] paths = fileDialog.FileNames;
                    string[] fileNames = fileDialog.SafeFileNames;

                    // 테스트용 팝업 "열기" 화면에서 여러 개의 파일 선택(Multiselect) 가능(true)할 때 사용하는 코드
                    // 선택한 파일의 이름을 string 클래스 객체 "fileName"에 문자열 할당
                    // string fileName = fileDialog.SafeFileName;
                    // "fileName"에 할당된 문자열을 프로퍼티 "TxtTestFileInfo"에 재할당 및 화면의 TextBox 출력 
                    // TxtTestFileInfo = fileName;
                }
                // 아무 것도 열지 않은 경우 
                else
                {

                }

                //MessageBox.Show("테스트");
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
                return;
            }
        }

        #endregion TestAsync

        #region ExitCheckIn

        /// <summary>
        /// 종료 버튼 
        /// </summary>
        private void ExitCheckIn(object obj)
        {
            checkInPartVM.IsExit = true;

            this.RequestClose(checkInPartVM.IsExit);
        }

        #endregion ExitCheckIn

        #region Sample

        // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.10.15 minjae)
        /// <summary>
        /// 컨테이너 체크인 + 업로드
        /// </summary>
        //private async Task UpLoadFileAsync(object obj)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 업로드 할 파일 데이터 저장 메서드 "SaveData" 필요시 구현 예정 (2023.12.05 minjae)
        //        // IsSave = SaveData();     // 업로드 할 파일 데이터 저장

        //        // TODO : 아래 if절 조건절 로직 테스트 후 수정 예정 (2023.12.05 minjae)
        //        // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.12.05 minjae)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        if(TxtComment is not null)
        //        {
        //            // TODO : 아래 주석친 코드 필요시 사용 예정 (2024.01.05 minjae)
        //            // ExplorerView.RequestData.exp_Comment = TxtComment;

        //            InputParamData.exp_Comment = TxtComment;

        //            IsSave = true;
        //            this.RequestClose(IsSave);
        //        }
        //        else
        //        {
        //            IsSave = false;
        //            throw new Exception("업로드 실패\r\n담당자에게 문의하세요.");
        //        }

        //        // if (IsSave) this.RequestClose(IsSave);
        //        // else throw new Exception("업로드 실패\r\n담당자에게 문의하세요.");
        //    }
        //    catch(Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //    return;
        //}

        #endregion Sample
    }
}
