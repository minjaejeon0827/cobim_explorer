using Serilog;
using Stylet;
using StyletIoC;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Collections;
using System.Reflection;
// using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using CobimExplorer.Commands;
using CobimExplorer.Message;
using CobimExplorer.Models.Explorer;
using CobimExplorer.Services.Page;
using CobimExplorer.Interface.Page;
using CobimExplorer.Common.Extensions;
using CobimExplorer.Common.LogManager;
using CobimExplorer.Rest.Api.CobimBase.User;
using CobimExplorer.Rest.Api.CobimBase.Explorer;
using CobimExplorer.ViewModels.Windows.Login;
using CobimExplorer.Models.User;
using CobimExplorer.Settings;

namespace CobimExplorer.ViewModels.Pages
{
    // TODO : TreeView Multi Level (프로젝트 - 팀 - 폴더 - 서브폴더 / 파일) 구현 (2023.09.18 minjae)
    // 참고 URL - https://stackoverflow.com/questions/50760316/wpf-treeview-with-three-levels-in
    public interface ITreeNode
    {
        /// <summary>
        /// 프로젝트, 팀, 폴더, 서브폴더 이름, 연관 컨테이너 이름 (파일 이름 제외)
        /// </summary>
        string exp_TreeName { get; set; }

        /// <summary>
        /// 프로젝트 하위 자식 (팀, 루트 폴더, 서브폴더) 노드
        /// </summary>
        List<ITreeNode> ChildNodes { get; set; }
    }

    /// <summary>
    /// 프로젝트 
    /// </summary>
    public class ProjectView : ITreeNode
    {
        /// <summary>
        /// 프로젝트 이름 
        /// </summary>
        public string exp_TreeName { get; set; }

        /// <summary>
        /// 프로젝트 하위 자식 (팀) 
        /// </summary>
        public List<ITreeNode> ChildNodes
        {
            get
            {
                // 싱글톤 패턴 
                if (_ChildNodes == null)
                {
                    _ChildNodes = new List<ITreeNode>();
                }
                return _ChildNodes;
            }
            set => _ChildNodes = value;
        }
        private List<ITreeNode> _ChildNodes;

        /// <summary>
        /// 프로젝트 아이디
        /// </summary>
        public string exp_ProjectId { get; set; }

        /// <summary>
        /// 프로젝트 목록 리스트
        /// </summary>
        public List<Dictionary<string, object>> ProjectItems { get; set; }

        /// <summary>
        /// 프로젝트 메타 데이터 Dictionary
        /// </summary>
        // public Dictionary<string, object> ProjectDataInfo { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> ProjectDataInfo { get; set; }
    }

    /// <summary>
    /// 팀
    /// </summary>
    public class TeamView : ITreeNode
    {
        /// <summary>
        /// 팀 이름 
        /// </summary>
        public string exp_TreeName { get; set; }

        /// <summary>
        /// 프로젝트 하위 자식 (루트 폴더) 
        /// </summary>
        public List<ITreeNode> ChildNodes
        {
            get
            {
                // 싱글톤 패턴 
                if (_ChildNodes == null)
                {
                    _ChildNodes = new List<ITreeNode>();
                }
                return _ChildNodes;
            }
            set => _ChildNodes = value;
        }
        private List<ITreeNode> _ChildNodes;

        /// <summary>
        /// 팀 아이디
        /// </summary>
        public string exp_TeamId { get; set; }


        // TODO : 프로퍼티 "exp_TeamName" 필요시 사용 예정 (2023.10.23 minjae)
        /// <summary>
        /// 화면 우측 상단 팀 이름 출력할 때 사용할 바인딩 객체 "exp_TeamName"
        /// </summary>
        // public string exp_TeamName { get; set; }

        /// <summary>
        /// 팀 목록 리스트
        /// </summary>
        public List<Dictionary<string, object>> TeamItems { get; set; }

        /// <summary>
        /// 팀 메타 데이터 Dictionary
        /// </summary>
        //public Dictionary<string, object> TeamDataInfo { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> TeamDataInfo { get; set; }

        // TODO : 소속된 팀의 사용자 리스트 필요시 사용 예정 (2023.09.18 minjae)
        /// <summary>
        /// 소속된 팀의 사용자 리스트 
        /// </summary>
        public List<Dictionary<string, object>> TeamUserList { get; set; }
    }

    /// <summary>
    /// 폴더
    /// </summary>
    public class FolderView : ITreeNode
    {
        /// <summary>
        /// (루트 / 서브) 폴더 이름
        /// </summary>
        public string exp_TreeName { get; set; }

        /// <summary>
        /// 폴더가 속한 프로젝트 아이디
        /// </summary>
        public string exp_ProjectId { get; set; }

        /// <summary>
        /// 폴더가 속한 팀 아이디 
        /// </summary>
        public string exp_TeamId { get; set; }

        /// <summary>
        /// 폴더 하위 자식(서브 폴더)
        /// </summary>
        public List<ITreeNode> ChildNodes
        {
            get
            {
                // 싱글톤 패턴 
                if (_ChildNodes == null)
                {
                    _ChildNodes = new List<ITreeNode>();
                }
                return _ChildNodes;
            }
            set => _ChildNodes = value;
        }
        private List<ITreeNode> _ChildNodes;

        /// <summary>
        /// 부모 폴더 아이디
        /// </summary>
        public string exp_TreeParentFolderId { get; set; }

        /// <summary>
        /// 폴더 그룹 아이디
        /// </summary>
        public string exp_TreeFolderGroupId { get; set; }

        /// <summary>
        /// 폴더 아이디
        /// </summary>
        public string exp_TreeFolderId { get; set; }

        /// <summary>
        /// 폴더 레벨
        /// </summary>
        public string exp_TreeFolderLevelNo { get; set; }

        /// <summary>
        /// 폴더 목록 리스트
        /// </summary>
        public List<Dictionary<string, object>> FolderItems { get; set; }

        /// <summary>
        /// 폴더 메타 데이터 Dictionary
        /// </summary>
        // public Dictionary<string, object> FolderDataInfo { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> FolderDataInfo { get; set; }
    }

    /// <summary>
    /// 파일
    /// </summary>
    public class FileView
    {
        /// <summary>
        /// 파일 이름
        /// </summary>
        public string exp_FileName { get; set; }

        // TODO : 파일 하위 자식 "ChildNodes" 필요시 사용 예정 (2023.09.18 minjae)
        /// <summary>
        /// 파일 하위 자식 노드
        /// </summary>
        //public List<ITreeNode> ChildNodes
        //{
        //    get
        //    {
        //        // 싱글톤 패턴 
        //        if (_ChildNodes == null)
        //        {
        //            _ChildNodes = new List<ITreeNode>();
        //        }
        //        return _ChildNodes;
        //    }
        //    set => _ChildNodes = value;
        //}
        //private List<ITreeNode> _ChildNodes;

        /// <summary>
        /// 파일 목록 리스트
        /// </summary>
        public List<Dictionary<string, object>> FileItems { get; set; }

        // TODO : 공유 컨테이너 리스트 "SharedContainerList" 추후 필요시 사용 예정 (2023.09.22 minjae)
        /// <summary>
        /// 공유 컨테이너 리스트
        /// </summary>
        public List<Dictionary<string, object>> SharedContainerList { get; set; }

        /// <summary>
        /// 파일 메타 데이터 Dictionary
        /// </summary>
        // public Dictionary<string, object> FileDataInfo { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> FileDataInfo { get; set; }

        /// <summary>
        /// 파일 유형 정보 Dictionary
        /// </summary>
        public Dictionary<string, object> FileTypeCodeInfo { get; set; }

        /// <summary>
        /// 파일 단계 정보 Dictionary
        /// </summary>
        public Dictionary<string, object> StepCodeInfo { get; set; }

        /// <summary>
        /// 파일 상태 정보 Dictionary
        /// </summary>
        public Dictionary<string, object> StatusCodeInfo { get; set; }

        /// <summary>
        /// 파일 체크 정보 Dictionary
        /// </summary>
        public Dictionary<string, object> CheckCodeInfo { get; set; }

        /// <summary>
        /// 컨테이너 그룹 
        /// </summary>
        public string exp_ContainerGroup { get; set; }

        /// <summary>
        /// 컨테이너 ID (영어 대문자)
        /// </summary>
        public string exp_UpperContainerId { get; set; }

        /// <summary>
        /// 컨테이너 ID (영어 소문자)
        /// </summary>
        public string exp_ContainerId { get; set; }

        /// <summary>
        /// 파일이 속한 상위 프로젝트 아이디
        /// </summary>
        public string exp_ProjectId { get; set; }

        /// <summary>
        /// 파일이 속한 상위 팀 아이디
        /// </summary>
        public string exp_TeamId { get; set; }

        /// <summary>
        /// 파일이 속한 상위 폴더 아이디
        /// </summary>
        public string exp_FolderId { get; set; }

        /// <summary>
        /// 파일 아이디
        /// </summary>
        public string exp_FileId { get; set; }

        /// <summary>
        /// 파일 유형
        /// </summary>
        public string exp_FileTypeCode { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        public string exp_Title { get; set; }

        /// <summary>
        /// 작성자
        /// </summary>
        public string exp_CreateUserId { get; set; }

        /// <summary>
        /// 등록일시
        /// </summary>
        public string exp_CreateDateTime { get; set; }
        // public DateTime exp_CreateDateTime { get; set; }

        /// <summary>
        /// 작성자 + " | " + 등록일시
        /// </summary>
        public string CreateUserIdAndTime { get; set; }

        /// <summary>
        /// 유형 
        /// </summary>
        public string exp_TypeCodeName { get; set; }

        /// <summary>
        /// 단계 
        /// </summary>
        public string exp_StepCodeName { get; set; }

        /// <summary>
        /// 상태 
        /// </summary>
        public string exp_StatusCodeName { get; set; }

        /// <summary>
        /// 버전 
        /// </summary>
        public string exp_RevsnVal { get; set; }
    }

    /// <summary>
    /// 파일 컨테이너 정보
    /// </summary>
    public class ContainerView
    {
        /// <summary>
        /// 작성자
        /// </summary>
        public string exp_CreateUserId { get; set; }

        /// <summary>
        /// 컨테이너 아이디 (영어 소문자)
        /// </summary>
        public string exp_ContainerId { get; set; }


        /// <summary>
        /// 컨테이너 ID (영어 대문자)
        /// </summary>
        // public string exp_UpperContainerId { get; set; }

        /// <summary>
        /// 선택한 특정 컨테이너 ID (영어 대문자)
        /// </summary>
        // public string exp_SelectedContainerId { get; set; }


        /// <summary>
        /// 프로젝트 아이디
        /// </summary>
        public string exp_ProjectId { get; set; }

        /// <summary>
        /// 팀 아이디
        /// </summary>
        public string exp_TeamId { get; set; }


        /// <summary>
        /// 버전
        /// </summary>
        public string exp_RevsnVal { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        public string exp_Title { get; set; }

        /// <summary>
        /// 파일유형 - systemCode
        /// </summary>
        public string exp_FileTypeSystemCode { get; set; }

        /// <summary>
        /// 파일유형 - CodeName
        /// </summary>
        public string exp_FileTypeCodeName { get; set; }

        // TODO : 프로퍼티 "exp_SoftMetaName" 필요시 사용 예정 (2023.10.23 minjae)
        /// <summary>
        /// 소프트웨어 - metaName
        /// </summary>
        // public string exp_SoftMetaName { get; set; }

        /// <summary>
        /// 소프트웨어
        /// </summary>
        public string exp_SoftName { get; set; }

        // TODO : 프로퍼티 "exp_SoftVerMetaName" 필요시 사용 예정 (2023.10.23 minjae)
        /// <summary>
        /// 소프트웨어버전 - metaName
        /// </summary>
        // public string exp_SoftVerMetaName { get; set; }

        /// <summary>
        /// 소프트웨어버전
        /// </summary>
        public string exp_SoftVer { get; set; }

        // TODO : 프로퍼티 "exp_KeywordTagMetaName" 필요시 사용 예정 (2023.10.23 minjae)
        /// <summary>
        /// 키워드/태그 - metaName
        /// </summary>
        // public string exp_KeywordTagMetaName { get; set; }

        /// <summary>
        /// 키워드/태그
        /// </summary>
        public string exp_KeywordTag { get; set; }


        /// <summary>
        /// 마일스톤 - 업무 인덱스
        /// </summary>
        public string exp_TaskDetailIdx { get; set; }

        /// <summary>
        /// 마일스톤 - 업무 유형
        /// </summary>
        public string exp_TaskDetailType { get; set; }

        /// <summary>
        /// 파일명(이름)
        /// </summary>
        public string exp_FileName { get; set; }

        /// <summary>
        /// 파일 아이디
        /// </summary>
        public string exp_FileId { get; set; }

        #region 기본 정보 탭 - Label

        /// <summary>
        /// 공통 (컨테이너 정보, 파일정보) - 구분 
        /// </summary>
        public string exp_DivLabel { get; set; }

        /// <summary>
        /// 공통 (컨테이너 정보, 파일정보) - 내용
        /// </summary>
        public string exp_ContentsLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 작성자
        /// </summary>
        public string exp_CreateUserLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 버전
        /// </summary>
        public string exp_VersionLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 제목
        /// </summary>
        public string exp_TitleLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 소프트웨어
        /// </summary>
        public string exp_SoftLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 소프트웨어버전
        /// </summary>
        public string exp_SoftVerLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 키워드/태그
        /// </summary>
        public string exp_KeywordTagLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 파일유형
        /// </summary>
        public string exp_FileTypeCodeLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 마일스톤
        /// </summary>
        public string exp_TaskDetailLabel { get; set; }

        /// <summary>
        /// 파일정보
        /// </summary>
        public string exp_FileNameLabel { get; set; }

        #endregion 기본 정보 탭 - Label

        /// <summary>
        /// 연관된 부모 컨테이너 정보 리스트
        /// </summary>
        public List<Dictionary<string, object>> LinkedNodeInfoList { get; set; }

        // TODO : 연관된 컨테이너 메타 데이터 리스트 객체 "LinkedNodeMetaDataList" 필요시 사용 예정 (2023.11.01 minjae)
        /// <summary>
        /// 연관된 컨테이너 메타 데이터 리스트 
        /// </summary>
        public List<Dictionary<string, object>> LinkedNodeMetaDataList { get; set; }

        // TODO : 추후 필요시 "컨테이너 정보, 파일정보" 리스트 객체 containerDetailList 구현 예정 (2023.10.17 minjae)
        /// <summary>
        /// 컨테이너 정보, 파일정보 리스트
        /// </summary>
        // public List<Dictionary<string, object>> ContainerDetailList { get; set; }

        /// <summary>
        /// 컨테이너 정보, 파일정보 Dictionary
        /// </summary>
        public Dictionary<string, object> DetailDic { get; set; }

        /// <summary>
        /// 파일유형 Dictionary
        /// </summary>
        public Dictionary<string, object> FileTypeCodeDic { get; set; }

        /// <summary>
        /// 메타데이터 정보 리스트
        /// </summary>
        public List<Dictionary<string, object>> MetaDataList { get; set; }

        // TODO : 추후 필요시 "마일스톤 업무" 리스트 객체 TaskDetailList 구현 예정 (2023.10.17 minjae)
        /// <summary>
        /// 마일스톤 업무 리스트
        /// </summary>
        // public List<Dictionary<string, object>> TaskDetailList { get; set; }

        /// <summary>
        /// 마일스톤 업무 Dictionary
        /// </summary>
        public Dictionary<string, object> TaskDetailDic { get; set; }

        /// <summary>
        /// 마일스톤 - 특정 마일스톤 업무 정보
        /// </summary>
        public Dictionary<string, object> MilestoneTaskInfoDic { get; set; }

        /// <summary>
        /// ComboBox - 파일유형 리스트
        /// </summary>
        public List<FileTypeView> FileTypeList { get; set; }


        /// <summary>
        /// ComboBox - 마일스톤 리스트
        /// </summary>
        public List<TaskDetailView> TaskDetailList { get; set; }

        // TODO : 컨테이너 정보 영역 "파일유형" ComboBox 구현시 바인딩할 BindableCollection 객체 FileType 필요시 사용 예정 (2023.10.20 minjae)
        // 참고 URL - https://www.reddit.com/r/csharp/comments/126sdo6/wpf_binding_a_dictionary_value_to_a_combobox/
        // 참고 2 URL - https://www.codeproject.com/Questions/536820/WPFplusBindplusandpluspopulateplusapluscombobox
        /// <summary>
        /// ComboBox - 파일유형
        /// </summary>
        // public BindableCollection<FileTypeView> FileType { get; set; }

        // TODO : 컨테이너 정보 영역 "마일스톤" ComboBox 구현시 바인딩할 BindableCollection 객체 TaskDetailType 필요시 사용 예정 (2023.10.20 minjae)
        // 참고 URL - https://www.reddit.com/r/csharp/comments/126sdo6/wpf_binding_a_dictionary_value_to_a_combobox/
        // 참고 2 URL - https://www.codeproject.com/Questions/536820/WPFplusBindplusandpluspopulateplusapluscombobox
        /// <summary>
        /// ComboBox - 마일스톤
        /// </summary>
        // public BindableCollection<TaskDetailView> TaskDetailType { get; set; }

        /// <summary>
        /// ComboBox "파일유형" List 생성 메서드 ContainerInfoTypeListCreate 실행시 화면에 출력할 ComboBox 데이터 목록
        /// </summary>
        public Param SelectParam { get; set; } = new Param();
    }

    /// <summary>
    /// 연관 컨테이너 정보
    /// </summary>
    public class LinkedNodeView : ITreeNode
    {
        /// <summary>
        /// 연관 컨테이너 아이디
        /// </summary>
        public string exp_TreeName { get; set; }

        // TODO : 파일 하위 자식 "ChildNodes" 필요시 사용 예정 (2023.09.18 minjae)
        /// <summary>
        /// 파일 하위 자식 노드
        /// </summary>
        public List<ITreeNode> ChildNodes
        {
            get
            {
                // 싱글톤 패턴 
                if (_ChildNodes == null)
                {
                    _ChildNodes = new List<ITreeNode>();
                }
                return _ChildNodes;
            }
            set => _ChildNodes = value;
        }
        private List<ITreeNode> _ChildNodes;

        /// <summary>
        /// 연관 컨테이너 정보(참조관계 - 부모/자식 컨테이너) Dictionary
        /// </summary>
        public Dictionary<string, object> LinkedNodeInfoDic { get; set; }
    }


    // TODO : 뷰(ExplorerV.xaml)에 구현된 ComboBox 파일유형, 마일스톤에서 사용하는 속성 SelectedItem에 바인딩되는
    //        바인딩되는 UpdateParam클래스 객체 Param의 바인딩 객체(FileTypeCodeInfo, taskDetail)에 저장된 값 변경시 
    //        뷰모델 (ExplorerVM.cs)에 해당 바인딩 객체(FileTypeCodeInfo, taskDetail)의 값이 변경되었다는 프로퍼티 체인지 이벤트(NotifyOfPropertyChange();)를
    //        실행하기 위해서 UpdateParam 클래스는 부모 클래스로 ValidatingModelBase를 상속 받도록 구현 (2023.10.23 minjae)
    // 참고 URL - https://kaki104.tistory.com/749
    /// <summary>
    /// ComboBox "파일유형" List 생성 메서드 ContainerInfoTypeListCreate 실행시 ComboBox에 할당될 데이터 목록
    /// </summary>
    public class Param : ValidatingModelBase
    {
        // TODO : 수정 메서드 "UpdateAsync" 실행시 전달할 수정된 메서드 파라미터 필요시 추가 예정 (2023.10.23 minjae)

        /// <summary>
        /// 파일유형
        /// </summary>
        public FileTypeView FileTypeCodeInfo { get => _FileTypeCodeInfo; set { _FileTypeCodeInfo = value; NotifyOfPropertyChange(); } }
        private FileTypeView _FileTypeCodeInfo;

        // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측에서 마일스톤 RestAPI 공유 받은 후 개발 진행 (2023.10.23 minjae)
        /// <summary>
        /// 마일스톤
        /// </summary>
        public TaskDetailView TaskDetail { get => _TaskDetail; set { _TaskDetail = value; NotifyOfPropertyChange(); } }
        private TaskDetailView _TaskDetail;
    }

    /// <summary>
    /// ComboBox - 파일유형
    /// </summary>
    public class FileTypeView
    {
        /// <summary>
        /// systemCode - typmodel, typreprt
        /// </summary>
        public string exp_systemCode { get; set; }

        /// <summary>
        /// codeName - 모델 / 보고서
        /// </summary>
        public string exp_codeName { get; set; }
    }

    // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측에서 마일스톤 RestAPI 공유 받은 후 개발 진행 (2023.10.23 minjae)
    /// <summary>
    /// ComboBox - 마일스톤
    /// </summary>
    public class TaskDetailView
    {
        /// <summary>
        /// 마일스톤 인덱스 (웹서버로 부터 받은 JSON 데이터 중 "taskDetail"에 존재하는 "taskIdx"와 "idx"는 값이 같음.)
        /// </summary>
        // public int exp_taskIdx { get; set; }
        public string exp_idx { get; set; }

        /// <summary>
        /// 마일스톤 타입 title - 관련업무선택, 요구사항정의, 엔터티도출, 요구사항정의서 작성, UI설계서 작성
        /// </summary>
        public string exp_title { get; set; }
    }

    public class ExplorerVM : PageBase, IPageBase, IHandle<ChangeViewModelMsg>
    {
        #region 프로퍼티 

        public IContainer Container { get; set; }

        [Inject]
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// 다운로드 Command
        /// </summary>
        public ICommand DownlLoadCommand { get; set; }

        /// <summary>
        /// 테스트 Command
        /// </summary>
        public ICommand TestCommand { get; set; }

        /// <summary>
        /// 체크아웃 Command
        /// </summary>
        public ICommand CheckOutCommand { get; set; }

        /// <summary>
        /// 체크인 Command
        /// </summary>
        public ICommand CheckInCommand { get; set; }

        /// <summary>
        /// 폴더 목록 조회 Command
        /// </summary>
        public ICommand FolderCommand { get; set; }

        // TODO : CreateCommand - 파일(컨테이너) 신규 등록의 경우 COBIM 웹 사이트(인재 INC)에서만 사용하는 기능이고 CDE 탐색기 응용 프로그램에서는 사용 안 하는 걸로 소통 완료.
        //        하여 해당 파일(컨테이너) 신규 등록 메서드는 주석 처리하고 사용 안 함. (2023.10.19 minjae)
        /// <summary>
        /// 신규등록 Command
        /// </summary>
        // public ICommand CreateCommand { get; set; }

        /// <summary>
        /// 파일(컨테이너) 목록 조회 Command
        /// </summary>
        public ICommand FileCommand { get; set; }

        // TODO : 인터페이스 ICommand 객체 "FileAddCommand" 필요 없을 시 삭제 예정 (2023.09.21 minjae) 
        /// <summary>
        /// 추가 (파일 등록) Command
        /// </summary>
        public ICommand FileAddCommand { get; set; }

        // TODO : 인터페이스 ICommand 객체 "CloserLookCommand" 필요 없을 시 삭제 예정 (2023.09.21 minjae)
        /// <summary>
        /// 보기 (파일 정보 자세히 보기) Command
        /// </summary>
        public ICommand CloserLookCommand { get; set; }

        /// <summary>
        /// 파일 기본정보 - 컨테이너 정보 조회 Command
        /// </summary>
        public ICommand ContainerDetailCommand { get; set; }

        /// <summary>
        /// 검토요청 Command
        /// </summary>
        public ICommand RequestCommand { get; set; }

        /// <summary>
        /// 수정 Command
        /// </summary>
        public ICommand UpdateCommand { get; set; }

        /// <summary>
        /// 탭 "변경" - 컨테이너 변경(컨테이너 안에 저장한 파일 A -> B) 및 저장 Command
        /// </summary>
        public ICommand UploadCommand { get; set; }

        /// <summary>
        /// 연관 컨테이너 Command (참조 관계 - 부모/자식 컨테이너)
        /// </summary>
        public ICommand LinkedNodeInfoCommand { get; set; }

        /// <summary>
        /// 화면 컨트롤러 출력 여부 (Visibility)
        /// </summary>
        //public bool IsVisible { get => _IsVisible; set { _IsVisible = value; NotifyOfPropertyChange(); } }
        //private bool _IsVisible;

        /// <summary>
        /// 화면 셋팅 타입 (Visibility)
        /// </summary>
        public string DisplayType { get => _DisplayType; set { _DisplayType = value; NotifyOfPropertyChange(); } }
        private string _DisplayType;

        /// <summary>
        /// 화면 재실행시 탭 "기본정보" Default 출력
        /// </summary>
        public int SelectedTabIndex { get => _SelectedTabIndex; set { _SelectedTabIndex = value; NotifyOfPropertyChange(); } }
        private int _SelectedTabIndex;

        /// <summary>
        /// 선택한 특정 컨테이너 ID (영어 대문자)
        /// </summary>
        public string SelectedContainerId { get => _SelectedContainerId; set { _SelectedContainerId = value; NotifyOfPropertyChange(); } }
        private string _SelectedContainerId;

        /// <summary>
        /// 화면 재실행시 탭 "기본정보" Default 출력
        /// </summary>
        // public Visibility visibleFirstTabItem { get => _visibleFirstTabItem; set { _visibleFirstTabItem = value; NotifyOfPropertyChange(); } }
        // private Visibility _visibleFirstTabItem = Visibility.Collapsed;

        /// <summary>
        /// Visibility - 검토요청 
        /// </summary>
        public Visibility visibleRequest { get => _visibleRequest; set { _visibleRequest = value; NotifyOfPropertyChange(); } }
        private Visibility _visibleRequest = Visibility.Collapsed;

        /// <summary>
        /// Visibility - 컨테이너 정보  
        /// </summary>
        public Visibility visibleContainerInfo { get => _visibleContainerInfo; set { _visibleContainerInfo = value; NotifyOfPropertyChange(); } }
        private Visibility _visibleContainerInfo = Visibility.Collapsed;

        /// <summary>
        /// Visibility - 파일정보  
        /// </summary>
        public Visibility visibleFileInfo { get => _visibleFileInfo; set { _visibleFileInfo = value; NotifyOfPropertyChange(); } }
        private Visibility _visibleFileInfo = Visibility.Collapsed;

        // TODO : Visibilty 프로퍼티 "visibleLinkedNodeInfo" 필요시 사용 예정 (2023.11.01 minjae)
        /// <summary>
        /// Visibilty - 연관 컨테이너 정보 (참조관계 - 부모/자식 컨테이너)
        /// </summary>
        // public Visibility visibleLinkedNodeInfo { get => _visibleLinkedNodeInfo; set { _visibleLinkedNodeInfo = value; NotifyOfPropertyChange(); } }
        // private Visibility _visibleLinkedNodeInfo = Visibility.Collapsed;

        /// <summary>
        /// 기본정보 - 컨테이너 정보
        /// </summary>
        // public ICommand ContainerInfoCommand { get; set; }

        /// <summary>
        /// HttpClient client (계속 호출하지 말고 싱글톤으로 사용해야함.)
        /// </summary>
        [Inject]
        public HttpClient client { get; set; }

        /// <summary>
        /// 프로젝트(+팀) 정보
        /// </summary>
        public ProjectHelper.ProjectPack ProjectPack { get; set; }

        /// <summary>
        /// 컨테이너 정보 
        /// </summary>
        public ContainerHelper.ContainerPack ContainerPack { get; set; }

        /// <summary>
        /// 다운로드할 파일 정보
        /// </summary>
        public FileHelper.FilePack FilePack { get; set; }

        // TODO : 사용자 정보 프로퍼티 "UserInfo" 필요시 사용 예정 (2023.08.30 minjae)
        /// <sumary>
        /// 사용자 정보
        /// </sumary>
        //[Inject]
        //public UserInfoView UserInfo { get => _UserInfo; set { _UserInfo = value; NotifyOfPropertyChange(); } }
        //private UserInfoView _UserInfo;

        /// <summary>
        /// 로그인 토큰 키 (계속 호출하지 말고 싱글톤으로 사용해야함.)
        /// </summary>
        public LoginHelper.Login_Access_Token Token { get; set; }

        /// <summary>
        /// 로그인 페이지
        /// </summary>
        // public LoginVM loginVM { get; set; }

        // TODO : 프로젝트 목록 ObservableCollection "ProjectTypeCollection" 추후 자료형 타입 ProjectName -> ProjectTypeView(사용자 정의 클래스) 필요시 사용 예정 (2023.08.30 minjae)
        // TODO : 프로젝트 목록 사용자 정의 클래스 "ProjectTypeView" 구현 예정 (2023.08.24 minjae)
        /// <summary>
        /// 프로젝트 목록 ObservableCollection 
        /// </summary>
        //public ObservableCollection<ProjectTypeView> ProjectTypeCollection { get => _ProjectTypeCollection; set { _ProjectTypeCollection = value; NotifyOfPropertyChange(); } }
        //public List<ProjectTypeView> ProjectTypeList 
        //{
        //    get 
        //    { 
        //        // 싱글톤 패턴 
        //        if (_projectTypeList == null)
        //        {
        //            _projectTypeList = new List<ProjectTypeView>();
        //        }

        //        return _projectTypeList;
        //    } 
        //    set => _projectTypeList = value;  
        //}
        //private ObservableCollection<ProjectTypeView> _ProjectTypeCollection = new ObservableCollection<ProjectTypeView>();

        // TODO : 컨테이너 ID (영어 대문자) 프로퍼티 exp_UpperContainerId 필요시 사용 예정(2023.10.23 minjae) 
        /// <summary>
        /// 컨테이너 ID (영어 대문자)
        /// </summary>
        // public string exp_UpperContainerId { get => _exp_UpperContainerId; set { _exp_UpperContainerId = value; NotifyOfPropertyChange(); } }
        // private string _exp_UpperContainerId = string.Empty;


        /// <summary>
        /// 파일 데이터 + 메타데이터 File Collection
        /// </summary>
        public BindableCollection<FileView> FileDatas { get => _FileDatas; set { _FileDatas = value; NotifyOfPropertyChange(); } }
        private BindableCollection<FileView> _FileDatas = new BindableCollection<FileView>();

        public List<FileView> SelectedFileData { get => _SelectedFileData; set { _SelectedFileData = value; NotifyOfPropertyChange(); } }
        private List<FileView> _SelectedFileData = new List<FileView>();

        /// <summary>
        /// 전체 (프로젝트 + 팀 + 폴더 + 서브폴더 + 파일) 데이터 Explorer Collection
        /// </summary>
        public BindableCollection<ProjectView> LevelDatas { get => _LevelDatas; set { _LevelDatas = value; NotifyOfPropertyChange(); } }
        private BindableCollection<ProjectView> _LevelDatas = new BindableCollection<ProjectView>();


        /// <summary>
        /// 파일 컨테이너 정보 + 메타데이터 File Container Collection
        /// </summary>
        public BindableCollection<ContainerView> ContainerDatas { get => _ContainerDatas; set { _ContainerDatas = value; NotifyOfPropertyChange(); } }
        private BindableCollection<ContainerView> _ContainerDatas = new BindableCollection<ContainerView>();

        /// <summary>
        /// 연관 컨테이너 정보 Collection
        /// </summary>
        public BindableCollection<LinkedNodeView> LinkedNodeInfoDatas { get => _LinkedNodeInfoDatas; set { _LinkedNodeInfoDatas = value; NotifyOfPropertyChange(); } }
        private BindableCollection<LinkedNodeView> _LinkedNodeInfoDatas = new BindableCollection<LinkedNodeView>();


        // TODO : 프로젝트 리스트 필요시 수정 예정 (2023.09.18 minjae)
        /// <summary>
        /// 프로젝트 리스트 
        /// </summary>
        public List<ProjectView> ProjectList { get => _ProjectList; set { _ProjectList = value; NotifyOfPropertyChange(); } }
        private List<ProjectView> _ProjectList = new List<ProjectView>();

        /// <summary>
        /// 팀 리스트 
        /// </summary>
        public List<ITreeNode> TeamList { get => _TeamList; set { _TeamList = value; NotifyOfPropertyChange(); } }
        private List<ITreeNode> _TeamList = new List<ITreeNode>();


        /// <summary>
        /// (루트 + 서브) 폴더 리스트 
        /// </summary>
        public List<ITreeNode> FolderList { get => _FolderList; set { _FolderList = value; NotifyOfPropertyChange(); } }
        private List<ITreeNode> _FolderList = new List<ITreeNode>();

        /// <summary>
        /// 파일 리스트
        /// </summary>
        public List<FileView> FileList { get => _FileList; set { _FileList = value; NotifyOfPropertyChange(); } }
        private List<FileView> _FileList = new List<FileView>();

        /// <summary>
        /// 파일 컨테이너 정보 + 메타데이터 리스트
        /// </summary>
        public List<ContainerView> ContainerList { get => _ContainerList; set { _ContainerList = value; NotifyOfPropertyChange(); } }
        private List<ContainerView> _ContainerList = new List<ContainerView>();

        /// <summary>
        /// 연관 컨테이너 정보 리스트
        /// </summary>
        public List<LinkedNodeView> LinkedNodeInfoList { get => _LinkedNodeInfoList; set { _LinkedNodeInfoList = value; NotifyOfPropertyChange(); } }
        private List<LinkedNodeView> _LinkedNodeInfoList = new List<LinkedNodeView>();

        /// <summary>
        /// 수정 메서드 "UpdateAsync" 실행시 전달할 수정된 메서드 파라미터 데이터 목록
        /// </summary>
        public Param UpdateParam { get; set; }

        #endregion 프로퍼티 

        #region 생성자

        public ExplorerVM(IEventAggregator events, IContainer container) : base(container)
        {
            Container = container;

            // TODO : this.EventAggregator = events; 코드 추가 (2023.07.24 minjae)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            this.EventAggregator = events;

            TestCommand             = new ButtonCommand(ExplorerHelper.AsyncMethodType, TestAsync, CanExecuteMethod);
            CheckOutCommand         = new ButtonCommand(ExplorerHelper.AsyncMethodType, CheckOutAsync, CanExecuteMethod);
            CheckInCommand          = new ButtonCommand(ExplorerHelper.AsyncMethodType, CheckInAsync, CanExecuteMethod);

            // TODO : CreateCommand - 파일(컨테이너) 신규 등록의 경우 COBIM 웹 사이트(인재 INC)에서만 사용하는 기능이고 CDE 탐색기 응용 프로그램에서는 사용 안 하는 걸로 소통 완료.
            //        하여 해당 파일(컨테이너) 신규 등록 메서드는 주석 처리하고 사용 안 함. (2023.10.19 minjae)
            // CreateCommand = new ButtonCommand(FileCreateAsync, CanExecuteMethod);
            // FolderListCommand    = new ButtonCommand(FolderListAsync, CanExecuteMethod);
            // FolderCommand        = new ButtonCommand(FolderListAsync, CanExecuteMethod);
            FileCommand             = new ButtonCommand(ExplorerHelper.AsyncMethodType, FileSearchAsync, CanExecuteMethod);
            FileAddCommand          = new ButtonCommand(ExplorerHelper.AsyncMethodType, FileAddAsync, CanExecuteMethod);
            CloserLookCommand       = new ButtonCommand(ExplorerHelper.AsyncMethodType, CloserLookAsync, CanExecuteMethod);
            ContainerDetailCommand  = new ButtonCommand(ExplorerHelper.AsyncMethodType, ContainerInfoSearchAsync, CanExecuteMethod);
            RequestCommand          = new ButtonCommand(ExplorerHelper.AsyncMethodType, RequestAsync, CanExecuteMethod);
            UpdateCommand           = new ButtonCommand(ExplorerHelper.AsyncMethodType, UpdateAsync, CanExecuteMethod);
            DownlLoadCommand        = new ButtonCommand(ExplorerHelper.AsyncMethodType, DownLoadAsync, CanExecuteMethod);

            UploadCommand           = new ButtonCommand(ExplorerHelper.AsyncMethodType, UploadAsync, CanExecuteMethod);
            LinkedNodeInfoCommand   = new ButtonCommand(ExplorerHelper.AsyncMethodType, LinkedNodeInfoSearchAsync, CanExecuteMethod);

        }

        #endregion 생성자 

        #region 기본 메소드


        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();

            // DisplayType = ExplorerHelper.TypeOfOnInitialActivate;
            // IsVisible = false;          // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 숨기도록 비활성화
            // DisplaySetting(DisplayType);
        }

        // TODO : 이벤트 메서드 "OnActivate" 비동기(async)로 구현 (2023.09.18 minjae)
        // 참고 URL - https://vmpo.tistory.com/89
        protected async override void OnActivate()
        {
            base.OnActivate();

            // InitSetting();              // 화면 초기 셋팅  

            DisplayType = ExplorerHelper.TypeOfOnActivate;
            // IsVisible = false;          // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 숨기도록 비활성화
            DisplaySetting(DisplayType);   
            

            // TODO : TreeView 데이터 출력 테스트 코드 추후 삭제 예정 (2023.07.14 minjae)
            // InitTreeViewText();

            await ExplorerListCreateAsync();

            var vm = Container.Get<ExplorerVM>();

            string VMName = vm.GetViewModelName();

            var msg = new ChangeViewModelMsg(this, VMName);
            EventAggregator.PublishOnUIThread(msg);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }



        private bool CanExecuteMethod(object obj)
        {
            return true;
        }


        #endregion 기본 메소드

        #region DisplaySetting

        /// <summary>
        /// 화면셋팅 - 화면에 필요한 컨트롤러(DockPanel, Button 등등...) 출력, 리스트 및 Collection 초기화(Clear)
        /// </summary>
        /// <param name="pIsVisibie">Visibility 프로퍼티 화면 표시 여부</param>
        private void DisplaySetting(string pDisplayType)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                switch (pDisplayType)
                {
                    // case ExplorerHelper.TypeOfOnInitialActivate:
                    case ExplorerHelper.TypeOfOnActivate:
                        // InitSetting();              // 화면 초기 셋팅  

                        // TODO : 탐색기 화면(ExplorerV.xaml) 탭 "기본정보" 제외한 다른 TabItem 선택 후
                        //        화면 종료 또는 다른 화면 이동 후 해당 탐색기 화면 재실행 시
                        //        TabControl의 첫 번째 TabItem="기본정보"로 실행(Default로 지정)하도록
                        //        SelectedIndex = "0" 추가(2023.11.01 minjae) 
                        // 참고 URL - https://stackoverflow.com/questions/20356239/tab-control-selecting-first-tab-by-default 
                        // 참고 2 URL - https://dotnetpattern.com/wpf-tab-control-selection  
                        // 화면 재실행시 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정
                        SelectedTabIndex = 0;

                        // 메서드 "ExplorerListCreateAsync" 사용되는 리스트, Collection
                        ProjectList.Clear();       // 프로젝트 리스트 초기화
                        TeamList.Clear();          // 팀 리스트 초기화
                        FolderList.Clear();        // 폴더 리스트 초기화 
                                                   // LevelDatas.Clear();



                        // 메서드 "FileSearchAsync" 사용되는 리스트
                        FileList.Clear();          // 파일 리스트 초기화
                        FileDatas.Clear();         // 파일 메타 데이터 Collection 초기화

                        // 메서드 "ContainerInfoSearchAsync" 사용되는 리스트
                        ContainerList.Clear();     // 파일 컨테이너 정보 + 메타데이터 리스트 초기화
                                                   // ContainerDatas.Clear();

                        // 메서드 "LinkedNodeInfoCreate" 사용되는 리스트, Collection 초기화
                        LinkedNodeInfoList.Clear();          // 연관 컨테이너 정보 리스트 초기화
                        LinkedNodeInfoDatas.Clear();         // 연관 컨테이너 정보 Collection 초기화

                        // TODO : 컨테이너 ID (영어 대문자) 프로퍼티 exp_UpperContainerId 필요시 사용 예정 (2023.10.23 minjae) 
                        // exp_UpperContainerId = string.Empty;
                        // visibleFirstTabItem = Visibility.Visible;              // 화면 활성화될 때 마다 탭 "기본정보" Default 출력
                        visibleRequest = Visibility.Collapsed;            // 화면 활성화될 때 마다 버튼 "검토요청" 숨김 처리 
                        visibleContainerInfo = Visibility.Collapsed;            // 화면 활성화될 때 마다 DockPanel 영역 "컨테이너 정보" 숨김 처리
                        visibleFileInfo = Visibility.Collapsed;            // 화면 활성화될 때 마다 DockPanel 영역 "파일정보" 숨김 처리
                        // visibleLinkedNodeInfo = Visibility.Collapsed;         // 화면 활성화될 때 마다 DockPanel 영역 "연관 컨테이너 정보 TreeView" 숨김 처리
                        break;
                    case ExplorerHelper.TypeOfContainerInfoSearch:
                        // InitSetting();
                        // 메서드 "FileSearchAsync" 사용되는 리스트
                        // FileList.Clear();          // 파일 리스트 초기화

                        // 메서드 "ContainerInfoSearchAsync" 사용되는 리스트
                        // ContainerList.Clear();     // 파일 컨테이너 정보 + 메타데이터 리스트 초기화


                        // TODO : 탐색기 화면(ExplorerV.xaml) 탭 "기본정보" 제외한 다른 TabItem 선택 후
                        //        컨테이너 ID 클릭 후 메서드 "ContainerInfoSearchAsync" 실행시 
                        //        TabControl의 첫 번째 TabItem="기본정보"로 실행(Default로 지정)하도록
                        //        SelectedIndex = "0" 추가(2023.11.01 minjae) 
                        // 참고 URL - https://stackoverflow.com/questions/20356239/tab-control-selecting-first-tab-by-default 
                        // 참고 2 URL - https://dotnetpattern.com/wpf-tab-control-selection  
                        // 화면 재실행시 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정
                        SelectedTabIndex = 0;

                        visibleRequest = Visibility.Visible;              // 검토요청 활성화 처리
                        visibleContainerInfo = Visibility.Visible;              // 컨테이너 정보 활성화 처리
                        visibleFileInfo = Visibility.Visible;              // 파일정보 활성화 처리
                                                                           // visibleLinkedNodeInfo = Visibility.Visible;           // 연관 컨테이너 정보 TreeView 활성화 처리
                        break;
                    case ExplorerHelper.TypeOfFileSearch:
                        // 메서드 "FileSearchAsync" 사용되는 리스트
                        FileList.Clear();
                        break;
                    case ExplorerHelper.TypeOfLinkedNodeInfoCreate:
                        // 메서드 "LinkedNodeInfoCreate" 사용되는 리스트, Collection 초기화
                        LinkedNodeInfoList.Clear();          // 연관 컨테이너 정보 리스트 초기화
                        LinkedNodeInfoDatas.Clear();         // 연관 컨테이너 정보 Collection 초기화
                        break;
                    default:
                        break;
                }

                // TODO : 아래 테스트 코드 필요시 참고 (2023.11.01 minjae)
                //if (!pIsVisibie)
                //{
                //    // InitSetting();              // 화면 초기 셋팅  

                //    // TODO : 탐색기 화면(ExplorerV.xaml) 탭 "기본정보" 제외한 다른 TabItem 선택 후
                //    //        화면 종료 또는 다른 화면 이동 후 해당 탐색기 화면 재실행 시
                //    //        TabControl의 첫 번째 TabItem="기본정보"로 실행(Default로 지정)하도록
                //    //        SelectedIndex = "0" 추가(2023.11.01 minjae) 
                //    // 참고 URL - https://stackoverflow.com/questions/20356239/tab-control-selecting-first-tab-by-default 
                //    // 참고 2 URL - https://dotnetpattern.com/wpf-tab-control-selection  
                //    // 화면 재실행시 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정
                //    SelectedTabIndex = 0;

                //    // 메서드 "ExplorerListCreateAsync" 사용되는 리스트, Collection
                //    ProjectList.Clear();       // 프로젝트 리스트 초기화
                //    TeamList.Clear();          // 팀 리스트 초기화
                //    FolderList.Clear();        // 폴더 리스트 초기화 
                //                               // LevelDatas.Clear();



                //    // 메서드 "FileSearchAsync" 사용되는 리스트
                //    FileList.Clear();          // 파일 리스트 초기화
                //    FileDatas.Clear();         // 파일 메타 데이터 Collection 초기화

                //    // 메서드 "ContainerInfoSearchAsync" 사용되는 리스트
                //    ContainerList.Clear();     // 파일 컨테이너 정보 + 메타데이터 리스트 초기화
                //                               // ContainerDatas.Clear();

                //    // 메서드 "LinkedNodeInfoCreate" 사용되는 리스트, Collection 초기화
                //    LinkedNodeInfoList.Clear();          // 연관 컨테이너 정보 리스트 초기화
                //    LinkedNodeInfoDatas.Clear();         // 연관 컨테이너 정보 Collection 초기화

                //    // TODO : 컨테이너 ID (영어 대문자) 프로퍼티 exp_UpperContainerId 필요시 사용 예정 (2023.10.23 minjae) 
                //    // exp_UpperContainerId = string.Empty;
                //    // visibleFirstTabItem = Visibility.Visible;              // 화면 활성화될 때 마다 탭 "기본정보" Default 출력
                //    visibleRequest        = Visibility.Collapsed;            // 화면 활성화될 때 마다 버튼 "검토요청" 숨김 처리 
                //    visibleContainerInfo  = Visibility.Collapsed;            // 화면 활성화될 때 마다 DockPanel 영역 "컨테이너 정보" 숨김 처리
                //    visibleFileInfo       = Visibility.Collapsed;            // 화면 활성화될 때 마다 DockPanel 영역 "파일정보" 숨김 처리
                //    // visibleLinkedNodeInfo = Visibility.Collapsed;         // 화면 활성화될 때 마다 DockPanel 영역 "연관 컨테이너 정보 TreeView" 숨김 처리
                //}
                //// 컨테이너 정보 +파일정보 조회 메서드 ContainerInfoSearchAsync 실행 
                //else
                //{
                //    // InitSetting();
                //    // 메서드 "FileSearchAsync" 사용되는 리스트
                //    FileList.Clear();          // 파일 리스트 초기화

                //    // 메서드 "ContainerInfoSearchAsync" 사용되는 리스트
                //    // ContainerList.Clear();     // 파일 컨테이너 정보 + 메타데이터 리스트 초기화


                //    // TODO : 탐색기 화면(ExplorerV.xaml) 탭 "기본정보" 제외한 다른 TabItem 선택 후
                //    //        컨테이너 ID 클릭 후 메서드 "ContainerInfoSearchAsync" 실행시 
                //    //        TabControl의 첫 번째 TabItem="기본정보"로 실행(Default로 지정)하도록
                //    //        SelectedIndex = "0" 추가(2023.11.01 minjae) 
                //    // 참고 URL - https://stackoverflow.com/questions/20356239/tab-control-selecting-first-tab-by-default 
                //    // 참고 2 URL - https://dotnetpattern.com/wpf-tab-control-selection  
                //    // 화면 재실행시 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정
                //    SelectedTabIndex = 0;

                //    visibleRequest        = Visibility.Visible;              // 검토요청 활성화 처리
                //    visibleContainerInfo  = Visibility.Visible;              // 컨테이너 정보 활성화 처리
                //    visibleFileInfo       = Visibility.Visible;              // 파일정보 활성화 처리
                //    // visibleLinkedNodeInfo = Visibility.Visible;           // 연관 컨테이너 정보 TreeView 활성화 처리
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
        }

        #endregion DisplaySetting

        #region InitSetting

        //private void InitSetting()
        //{
        //    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.11.01 minjae)
        //    var currentMethod = MethodBase.GetCurrentMethod();

        //    try
        //    {
        //        // TODO : 탐색기 화면(ExplorerV.xaml) 탭 "기본정보" 제외한 다른 TabItem 선택 후
        //        //        화면 종료 또는 다른 화면 이동 후 해당 탐색기 화면 재실행 시
        //        //        TabControl의 첫 번째 TabItem="기본정보"로 실행(Default로 지정)하도록
        //        //        SelectedIndex = "0" 추가(2023.11.01 minjae) 
        //        // 참고 URL - https://stackoverflow.com/questions/20356239/tab-control-selecting-first-tab-by-default 
        //        // 참고 2 URL - https://dotnetpattern.com/wpf-tab-control-selection  
        //        // 화면 재실행시 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정
        //        SelectedTabIndex = 0;

        //        // 메서드 "ExplorerListCreateAsync" 사용되는 리스트, Collection
        //        ProjectList.Clear();       // 프로젝트 리스트 초기화
        //        TeamList.Clear();          // 팀 리스트 초기화
        //        FolderList.Clear();        // 폴더 리스트 초기화 
        //        // LevelDatas.Clear();
                
                

        //        // 메서드 "FileSearchAsync" 사용되는 리스트
        //        FileList.Clear();          // 파일 리스트 초기화
        //        FileDatas.Clear();         // 파일 메타 데이터 Collection 초기화

        //        // 메서드 "ContainerInfoSearchAsync" 사용되는 리스트
        //        ContainerList.Clear();     // 파일 컨테이너 정보 + 메타데이터 리스트 초기화
        //        // ContainerDatas.Clear();

        //        // 메서드 "LinkedNodeInfoCreate" 사용되는 리스트, Collection 초기화
        //        LinkedNodeInfoList.Clear();          // 연관 컨테이너 정보 리스트 초기화
        //        LinkedNodeInfoDatas.Clear();         // 연관 컨테이너 정보 Collection 초기화
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        throw;
        //    }
        //}

        #endregion InitSetting

        #region ExplorerListCreate

        // TODO : 프로젝트 - 팀 - 폴더 - 서브폴더 생성 해주는 메서드 "ExplorerListCreate" 구현 (2023.09.18 minjae)
        /// <summary>
        /// 프로젝트 - 팀 - 폴더 - 서브폴더 리스트 생성 및 초기화 / 파일 메타 데이터 Collection 초기화 
        /// 메서드 파라미터 List<ITreeNode> pSubFolderFileList 추가 여부 확인 
        /// </summary>
        private async Task ExplorerListCreateAsync()
        {
            string tokenKey = AppSetting.Default.Login.Token.resultData;       // 로그인 토큰 키 

            string testProjectId = AppSetting.Default.Project.TestProjectId;   // 테스트 프로젝트 아이디
            string testTeamId = AppSetting.Default.Project.TestTeamId;         // 테스트 팀 아이디

            string projectId = string.Empty;                                   // JSON 프로젝트 아이디
            string projectName = string.Empty;                                 // JSON 프로젝트 이름 
            
            string teamProjectId = string.Empty;                               // JSON 팀의 상위 프로젝트 아이디
            string teamId = string.Empty;                                      // JSON 팀 아이디
            string teamName = string.Empty;                                    // JSON 팀 이름
                                                                               
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                
                // ProjectList.Clear();       // 프로젝트 리스트 초기화
                // TeamList.Clear();          // 팀 리스트 초기화
                // FolderList.Clear();        // 폴더 리스트 초기화 
                // FileDatas.Clear();         // 파일 메타 데이터 Collection 초기화

                // TODO : ProjectHelper.cs -> ProjectPack 클래스 프로퍼티 "ProjectPack" 구현 및
                //        테스트 프로젝트 아이디(testProjectId), 테스트 팀 아이디(testTeamId) 값 할당 추후 필요시 수정 예정 (2023.09.04 minjae)     
                ProjectPack = new ProjectHelper.ProjectPack
                {
                    projectId = testProjectId,
                    teamId = testTeamId
                };

                // 프로젝트 + 팀 리스트 데이터 받아오기 
                var datas = await ExplorerRestServer.GetProjectTeamListAsync(client, tokenKey);

                // 폴더 리스트 데이터 받아오기
                var folderList = await ExplorerRestServer.GetFolderListAsync(client, ProjectPack, tokenKey);

                foreach (var data in datas)
                {
                    var projectDic = (data[ProjectHelper.projectEntity]).ToDictionary();      // 프로젝트 메타 데이터 Dictionary
                    // Dictionary 객체 projectDic 특정 키(Key) "projectId"에 속하는 값(Value) 문자열로 변환 (2023.09.11 minjae)
                    // 참고 URL - https://developer-talk.tistory.com/694
                    projectId = projectDic[ProjectHelper.projectId].ToString();               // 해당 프로젝트 아이디 
                    projectName = projectDic[ProjectHelper.projectName].ToString();           // 해당 프로젝트 이름 


                    var teamDic = (data[TeamHelper.teamEntity]).ToDictionary();               // 팀 메타 데이터 Dictionary
                    teamProjectId = teamDic[ProjectHelper.projectId].ToString();              // 해당 팀이 소속된 프로젝트 이름 
                    teamId = teamDic[TeamHelper.teamId].ToString();                           // 해당 팀 아이디 
                    teamName = teamDic[TeamHelper.teamName].ToString();                       // 해당 팀 이름 

                    // 프로젝트 - 팀 하위에 속하는 루트 폴더 리스트 데이터 추출
                    var rootFolderList = folderList.Where(folder => folder[FolderHelper.parentFolderId] == null).Select(folder => new FolderView
                    {
                        FolderDataInfo = folder,
                        FolderItems = new List<Dictionary<string, object>> { folder },
                        // TODO : NULL 예외처리 연산자 ?. 사용시 오류 발생으로 인하여 NULL 예외처리 삼항 연산자 사용하는 로직으로 수정 (2023.09.11 minjae)
                        // 참고 URL - https://jellyho.com/blog/54/
                        exp_TreeParentFolderId = (folder[FolderHelper.parentFolderId] == null) ? (string.Empty) : (folder[FolderHelper.parentFolderId].ToString()),
                        exp_TreeFolderGroupId = folder[FolderHelper.folderGroupId].ToString(),
                        exp_TreeFolderId = folder[FolderHelper.folderId].ToString(),
                        exp_TreeName = folder[FolderHelper.folderName].ToString(),
                        exp_TreeFolderLevelNo = folder[FolderHelper.levelNo].ToString(),
                        exp_ProjectId = projectId,
                        exp_TeamId = teamId,
                        // TODO : 파일 리스트 추가 작업시 ChildNodes 사용 예정 (2023.09.18 minjae)
                        // ChildNodes = subNodes
                    });

                    // 루트 폴더 하위 서브 폴더 추가
                    foreach (var rootFolder in rootFolderList)
                    {
                        // (루트 + 서브) 폴더 리스트에 루트 폴더 추가 
                        FolderList.Add(rootFolder);

                        // 서브 폴더 추가
                        await FillFolders(rootFolder, folderList);
                    }

                    // 프로젝트 하위 디렉토리 (팀) 존재하지 않을 경우 새로 생성
                    // TODO : bool 변수 "hasTeamInfo" 구현 (2023.09.18 minjae)
                    // 참고 URL - https://frogand.tistory.com/211
                    bool hasTeamInfo =       !string.IsNullOrWhiteSpace(teamProjectId)
                                          && !string.IsNullOrWhiteSpace(teamId)
                                          && !string.IsNullOrWhiteSpace(teamName);

                    if (hasTeamInfo 
                        && !string.IsNullOrWhiteSpace(projectId) 
                        && projectId.Equals(teamProjectId))
                    {
                        var teamInfo = new TeamView()
                        {
                            TeamDataInfo = teamDic,
                            // TODO : TeamItems값이 NULL인 원인 파악 후 해결하기(2023.09.11 minjae) 
                            // TeamItems = teamArray.Where(key => key["teamId"].Equals(teamIdValue)).Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList(),
                            TeamItems = new List<Dictionary<string, object>> { teamDic },
                            // TODO : 팀에 소속된 사용자 리스트 JSON 데이터 인재 INC 로부터 REST API 공유 받으면 구현 진행 예정 (2023.09.12 minjae)
                            // TeamUserList = new List<Dictionary<string, object>> { teamUserDic },
                            exp_TeamId = teamId,
                            // TODO : 프로퍼티 "exp_TeamName" 필요시 사용 예정(2023.10.23 minjae)
                            // exp_TeamName = teamName,   // 화면 우측 상단 팀 이름 출력할 때 사용할 바인딩 객체 "exp_TeamName"
                            exp_TreeName = teamName,
                            // TestChildNodes = pFolderList

                            // TestChildNodes = TestFolderNodes
                            ChildNodes = FolderList
                        };

                        TeamList.Add(teamInfo);
                    }

                    // 루트 디렉토리 (프로젝트) 존재하지 않을 경우 새로 생성
                    // TODO : bool 변수 ""hasProjectInfo" 구현 (2023.09.18 minjae)
                    // 참고 URL - https://frogand.tistory.com/211
                    bool hasProjectInfo =    !string.IsNullOrWhiteSpace(projectId)
                                          && !string.IsNullOrWhiteSpace(projectName)
                                          && projectId.Equals(teamProjectId);

                    if (hasProjectInfo)
                    {
                        var projectInfo = new ProjectView()
                         {
                             ProjectDataInfo = projectDic,
                             // TODO : ProjectItems값이 NULL인 원인 파악 후 해결하기(2023.09.11 minjae)
                             ProjectItems = new List<Dictionary<string, object>> { projectDic },
                             exp_ProjectId = projectId,
                             exp_TreeName = projectName,
                             ChildNodes = TeamList.Where(team => team.exp_TreeName.Equals(teamName)).ToList()
                         };

                        ProjectList.Add(projectInfo);
                    }
                }

                LevelDatas.Clear();                           // Explorer Collection 초기화
                LevelDatas.AddRange(ProjectList.ToArray());   // Explorer Collection에 전체 데이터(프로젝트 + 팀 + 폴더 + 서브폴더 + 파일) 추가 
                return;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
            return;
        }

        #endregion ExplorerListCreate

        #region FillFolders

        /// <summary>
        /// 서브 폴더 생성 및 부모(루트) 폴더의 자식 노드로 연결
        /// </summary>
        /// <param name="parentFolder">부모(루트) 폴더</param>
        /// <param name="pFolderList">(루트 + 서브)폴더 리스트</param>
        private async Task FillFolders(FolderView parentFolder, List<Dictionary<string, object>> pFolderList)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                // 부모 폴더(루트 또는 서브)에 하위 서브 폴더 데이터 추출 
                var childNodes = pFolderList.Where(folder => parentFolder.exp_TreeFolderId == folder[FolderHelper.parentFolderId]?.ToString());

                if (childNodes.Any() == false) return;

                foreach (var childNode in childNodes)
                {
                    // TODO : Dictionary 특정 키(Key)에 해당하는 값(Value) 구해서 FolderView 클래스에 존재하는 프로퍼티에 값 할당 (2023.09.15 minjae)
                    // 참고 URL - https://developer-talk.tistory.com/694
                    var subFolder = new FolderView {
                                            FolderDataInfo = childNode,
                                            FolderItems = new List<Dictionary<string, object>> { childNode },
                                            exp_TreeParentFolderId = parentFolder.exp_TreeFolderId,
                                            exp_TreeFolderGroupId = childNode[FolderHelper.folderGroupId].ToString(),
                                            exp_TreeFolderId = childNode[FolderHelper.folderId].ToString(),
                                            exp_TreeName = childNode[FolderHelper.folderName].ToString(),
                                            exp_TreeFolderLevelNo = childNode[FolderHelper.levelNo].ToString(),
                                            exp_ProjectId = parentFolder.exp_ProjectId,
                                            exp_TeamId = parentFolder.exp_TeamId,
                                        };

                    // 부모 폴더의 자식 노드로 서브 폴더 추가 
                    parentFolder.ChildNodes.Add(subFolder);

                    // 서브 폴더의 자식 노드로 또 다른 서브 폴더 추가
                    await FillFolders(subFolder, pFolderList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 오류 발생시 예외처리 throw 구현 (2023.10.6 minjae)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }
        }

        #endregion FillFolders

        #region Set 

        // TODO : 추후 필요시 사용 예정 (2023.08.30 minjae)
        //public static void Set(UserInfoView userInfoView)
        //{
        //    try
        //    {
        //        UserInfo = userInfoView;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        #endregion Set

        #region FileCreateAsync

        // TODO : 파일(컨테이너) 신규 등록의 경우 COBIM 웹 사이트(인재 INC)에서만 사용하는 기능이고 CDE 탐색기 응용 프로그램에서는 사용 안 하는 걸로 소통 완료.
        //        하여 해당 파일(컨테이너) 신규 등록 메서드는 주석 처리하고 사용 안 함. (2023.10.19 minjae)
        /// <summary>
        /// 파일(컨테이너) 신규 등록 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //private async Task FileCreateAsync(object obj)
        //{
        //    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
        //    // 참고 URL - https://slaner.tistory.com/73
        //    // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
        //    // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
        //    var currentMethod = MethodBase.GetCurrentMethod();

        //    try
        //    {
        //        MessageBox.Show("파일 신규등록 기능 필요시 구현 예정");
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //    }
        //    return;
        //}

        #endregion FileCreateAsync

        #region FileSearchAsync

        /// <summary>
        /// 파일(컨테이너) 목록 조회
        /// </summary>
        private async Task FileSearchAsync(object obj)
        {
            string tokenKey = AppSetting.Default.Login.Token.resultData;       // 로그인 토큰 키 

            // TODO : Object 배열 객체 obj 리스트로 변환 (2023.09.19 minjae)
            // 참고 URL - https://nonstop-antoine.tistory.com/36
            var fileDatas    = obj as IEnumerable;                             // 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_TreeFolderId") (Object Array -> IEnumerable 형변환) 
            var fileInfos    = new List<string>();                             // fileData 를 리스트로 변환할 때 사용하는 리스트 객체 "fileInfos"
            var fileDataDic  = new Dictionary<string, string>();               // 파일 데이터를 담는 Dictionary 객체 fileDataDic 생성

            string folderId       = string.Empty;                              // 폴더 아이디 
            string createUserId   = string.Empty;                              // 작성자 
            string createDateTime = string.Empty;                              // 등록일시

            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                // FileList.Clear();   // 파일 리스트 초기화
                DisplayType = ExplorerHelper.TypeOfFileSearch;
                // IsVisible = true;            // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 보이도록 활성화
                DisplaySetting(DisplayType);

                // fileDatas 안에 존재하는 파일 정보 반복문으로 방문 -> List 객체 fileInfos에 파일 정보 데이터 추가 
                foreach (var data in fileDatas)
                {
                    fileInfos.Add(data.ToString());
                }

                fileDataDic.Add(ProjectHelper.projectId, fileInfos[0]);
                fileDataDic.Add(TeamHelper.teamId, fileInfos[1]);
                fileDataDic.Add(FolderHelper.folderId, fileInfos[2]);

                folderId = fileDataDic[FolderHelper.folderId];

                // TODO : ProjectHelper.cs -> ProjectPack 클래스 프로퍼티 "ProjectPack" 구현 및
                //        테스트 프로젝트 아이디(testProjectId), 테스트 팀 아이디(testTeamId) 값 할당 추후 필요시 수정 예정 (2023.09.04 minjae)     
                ProjectPack = new ProjectHelper.ProjectPack
                {
                    projectId = fileDataDic[ProjectHelper.projectId],
                    teamId = fileDataDic[TeamHelper.teamId]
                };

                // TODO : 오류 메시지 "Id = 3455, Status = WaitingForActivation, Method = "{null}", Result = "{Not yet computed}"
                //        해결하기 위해 비동기 메서드 호출시 키워드 "await" 추가 (2023.09.20 minjae)
                // 참고 URL - https://www.codeproject.com/Questions/1156449/How-to-call-async-methods-Csharp
                var fileList = await ExplorerRestServer.GetFileListAsync(client, ProjectPack, folderId, tokenKey);

                foreach (var file in fileList)
                {
                    var fileTypeCodeInfoDic = file[FileHelper.fileTypeCodeInfo].ToDictionary();
                    var stepCodeInfoDic = file[FileHelper.stepCodeInfo].ToDictionary();
                    var statusCodeInfoDic = file[FileHelper.statusCodeInfo].ToDictionary();


                    createUserId = file[FileHelper.createUserId].ToString();       // 작성자 

                    // TODO : 특정 문자 'T'를 포함한 등록일시 문자열 (예) "2023-07-25T10:44:45.988+00:00" 을 DateTime 객체 dt로 변환 (2023.09.21 minjae)
                    // 참고 URL - https://stackoverflow.com/questions/14252312/how-to-convert-date-with-t-to-from-string-in-c-sharp
                    var createDT = file[FileHelper.createDateTime].ToString();   // 등록일시

                    // TODO : UTC 협정 세계시로 변환 필요시 DateTimeOffset 클래스 객체 dto 선언 및 초기화 
                    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-C-NET%EC%9D%98-DataTime-%EB%AA%A8%EB%B2%94-%EC%82%AC%EB%A1%80
                    //var dto = DateTimeOffset.Parse(createDT);

                    // TODO : UTC 협정 세계시로 변환할 필요 없이 대한민국 시간(로컬 타임)으로 변환하기 위해 DateTime 클래스 객체 dt 선언 및 초기화(2023.09.26 minjae)
                    // DataTime VS DateTimeOffset 비교 문서
                    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-C-%EC%97%90%EC%84%9C-IronPython-%EC%9D%B4%EC%9A%A9%ED%95%98%EC%97%AC-Python-%EC%97%B0%EB%8F%99
                    // 문자열 ->  DateTime 객체 dt로 형변환
                    // Get the date object from the string. 
                    var dt = DateTime.Parse(createDT);

                    // 문자열 ->  DateTime 객체 dt로 형변환
                    // Get the date object from the string. 
                    //var dt = dto.DateTime;

                    // DateTimeOffset 객체 dto -> 문자열로 형변환
                    // Convert the DateTimeOffSet to string. 
                    // string newVal = dto.ToString("o");

                    // DateTime 객체 dt -> 문자열로 형변환 (대문자 HH - 24시간제 기준 / 소문자 hh - 12시간제 기준(오전 12시, 자정 12시))
                    // 참고 URL - https://wwwi.tistory.com/327
                    createDateTime = dt.ToString("yyyy/MM/dd HH:mm");

                    // var testName = fileTypeCodeInfoDic.Where(k => k.Key.Equals(FileHelper.fileTypeCodeName)).Select(v => v.Value).ToString();

                    //if (folderId.Equals(file[FileHelper.folderId].ToString()))
                    //{
                    //    var fileInfo = new FileView()
                    //    {
                    //        FileDataInfo = file,
                    //        FileItems = new List<Dictionary<string, object>> { file },
                    //        exp_TreeName = file[FileHelper.fileNm].ToString(),
                    //        exp_ContainerId = file[FileHelper.containerId].ToString(),
                    //        exp_TypeCodeName = fileTypeCodeInfoDic[FileHelper.fileTypeCodeName].ToString(),
                    //        exp_StepCodeName = stepCodeInfoDic[FileHelper.stepCodeName].ToString(),
                    //        exp_StatusCodeName = statusCodeInfoDic[FileHelper.statusCodeName].ToString(),
                    //        exp_RevsnVal = file[FileHelper.revsnVal].ToString()
                    //    };

                    //    FileList.Add(fileInfo);
                    //}

                    var fileInfo = new FileView()
                    {
                        FileDataInfo = file,
                        FileItems = new List<Dictionary<string, object>> { file },
                        FileTypeCodeInfo = file[FileHelper.fileTypeCodeInfo].ToDictionary(),
                        StepCodeInfo = file[FileHelper.stepCodeInfo].ToDictionary(),
                        StatusCodeInfo = file[FileHelper.statusCodeInfo].ToDictionary(),
                        CheckCodeInfo = file[FileHelper.checkCodeInfo].ToDictionary(),

                        // TODO : "SharedContainerList" 필요시 구현 예정 (2023.09.22 minjae)
                        SharedContainerList = new List<Dictionary<string, object>>(),

                        CreateUserIdAndTime = createUserId + " | " + createDateTime,
                        exp_FileName = file[FileHelper.fileNm].ToString(),
                        exp_ContainerGroup = file[FileHelper.containerGroup].ToString(),
                        // TODO : 컨테이너 ID(영어 대문자) 프로퍼티 "exp_UpperContainerId" 영어 대문자로 구현(2023.09.22 minjae)
                        //        프로퍼티 "exp_UpperContainerId" 용도 - 탐색기 화면 뷰(ExplorerV.xaml) "ListView" 영역에 바인딩할 컬럼 용도
                        // 참고 URL - https://developer-talk.tistory.com/669
                        exp_UpperContainerId = file[FileHelper.containerId].ToString().ToUpper(),
                        // TODO : 컨테이너 ID(영어 소문자) 프로퍼티 "exp_ContainerId" 영어 대문자로 구현(2023.10.12 minjae)
                        //        프로퍼티 "exp_ContainerId" 용도
                        //        - Rest API 메서드 "ExplorerRestServer.GetContainerDetailListAsync" 파라미터 "ContainerPack"에 속하는
                        //          컨테이너 아이디(containerId)에 영어 소문자로 넘겨줘야 인재 INC 서버 로부터 응답 결과 JSON 데이터를 정상적으로 넘겨 받을 수 있음. 
                        exp_ContainerId = file[FileHelper.containerId].ToString(),
                        exp_ProjectId = file[FileHelper.projectId].ToString(),
                        exp_TeamId = file[FileHelper.teamId].ToString(),
                        
                        exp_FolderId = file[FileHelper.folderId].ToString(),
                        exp_FileId = file[FileHelper.fileId].ToString(),
                        exp_FileTypeCode = file[FileHelper.fileTypeCode].ToString(),

                        exp_Title = file[FileHelper.title].ToString(),
                        exp_CreateUserId = createUserId,
                        exp_CreateDateTime = createDateTime,
                        exp_TypeCodeName = fileTypeCodeInfoDic[FileHelper.fileTypeCodeName].ToString(),
                        exp_StepCodeName = stepCodeInfoDic[FileHelper.stepCodeName].ToString(),
                        exp_StatusCodeName = statusCodeInfoDic[FileHelper.statusCodeName].ToString(),
                        exp_RevsnVal = file[FileHelper.revsnVal].ToString()
                    };

                    FileList.Add(fileInfo);
                }

                // TODO : 추후 필요시 파일리스트에 저장된 컨테이너 아이디 오름차순 정렬 구현하기 (2023.10.27 minjae)
                // FileList.Sort();   

                FileDatas.Clear();
                FileDatas.AddRange(FileList.ToArray());

                MessageBox.Show("파일 리스트 출력 테스트 중입니다.");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                Log.Error(ex.ToString());
                // e.ToString()
            }
            return;
        }

        #endregion FileSearchAsync

        #region FileAddAsync

        /// <summary>
        /// 파일 추가
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task FileAddAsync(object obj)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                MessageBox.Show("파일 추가 개발 예정");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
            return;
        }

        #endregion FileAddAsync

        #region CloserLookAsync

        /// <summary>
        /// 자세히 보기
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task CloserLookAsync(object obj)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                MessageBox.Show("자세히 보기 개발 예정");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
            return;
        }

        #endregion CloserLookAsync

        #region ContainerInfoSearchAsync

        /// <summary>
        /// 컨테이너 정보 + 파일정보 조회
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task ContainerInfoSearchAsync(object obj)
        {
            string tokenKey         = AppSetting.Default.Login.Token.resultData;  // 로그인 토큰 키 
            string inputTenantId    = LoginHelper.inputTenantId;                  // 테넌트 아이디

            // TODO : 컨테이너 정보, 파일정보 리스트 객체 "containerDetailList" 필요시 사용 예정 (2023.10.17 minjae)
            // List<Dictionary<string, object>> containerDetailList = new List<Dictionary<string, object>>();  
            List<Dictionary<string, object>> linkedNodeInfoList  = new List<Dictionary<string, object>>();  // 연관된 부모 컨테이너 정보 리스트

            // TODO : 연관 컨테이너 정보(메타 데이터) 리스트 객체 "linkedNodeMetaDataList" 필요시 사용 예정 (2023.10.17 minjae)
            List<Dictionary<string, object>> linkedNodeMetaDataList = new List<Dictionary<string, object>>();  // 연관 컨테이너 정보(메타 데이터) 리스트

            Dictionary<string, object> detailDic                 = null;          // 컨테이너 정보, 파일정보 Dictionary

            List<Dictionary<string, object>> metaDataList        = new List<Dictionary<string, object>>();  // 메타데이터 정보 리스트

            Dictionary<string, object> fileTypeCodeInfoDic       = null;          // 파일유형 Dictionary

            // TODO : 마일스톤 업무 리스트 객체 "taskDetailList" 필요시 사용 예정 (2023.10.17 minjae)
            // List<Dictionary<string, object>> taskDetailList   = null;          // 마일스톤 업무 리스트
            Dictionary<string, object> taskDetailDic             = null;          // 마일스톤 업무 Dictionary
            Dictionary<string, object> milestoneTaskInfoDic      = null;          // 특정 마일스톤 업무 정보 Dictionary


            string createUserId       = string.Empty;                               // 작성자
            string containerId        = string.Empty;                               // 컨테이너 아이디

            string projectId          = string.Empty;                               // 프로젝트 아이디
            string teamId             = string.Empty;                               // 팀 아이디

            string revsnVal           = string.Empty;                               // 버전
            string title              = string.Empty;                               // 제목
            string fileTypeSystemCode = string.Empty;                               // 파일유형 - systemCode
            string fileTypeCodeNm     = string.Empty;                               // 파일유형 - codeName

            string softMetaNm         = string.Empty;                               // 소프트웨어 metaName
            string softNm             = string.Empty;                               // 소프트웨어
                                      
            string softVerMetaNm      = string.Empty;                               // 소프트웨어버전 metaName
            string softVer            = string.Empty;                               // 소프트웨어버전
                                      
            string keywordMetaNm      = string.Empty;                               // 키워드/태그 metaName
            string keyword            = string.Empty;                               // 키워드/태그

            string taskDetailIdx      = string.Empty;                               // 마일스톤 업무 인덱스
            string taskDetailType     = string.Empty;                               // 마일스톤 업무 유형
                                                                                  
            string fileNm             = string.Empty;                               // 파일명
            string fileId             = string.Empty;                               // 파일 아이디

            // TODO : Object 배열 객체 obj 리스트로 변환 (2023.09.19 minjae)
            // 참고 URL - https://nonstop-antoine.tistory.com/36
            var containerDatas = obj as IEnumerable;                                // 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 
            var containerInfos = new List<string>();                                // containerData 를 리스트로 변환할 때 사용하는 리스트 객체 "containerInfos"
            var containerDataDic = new Dictionary<string, string>();                // 컨테이너 정보 데이터를 담는 Dictionary 객체 containerDataDic 생성

            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                // ContainerList.Clear();            // 파일 컨테이너 정보 + 메타데이터 리스트 초기화

                // TODO : 리스트 객체 "ContainerDetailList", "ContainerDetailList", "TaskDetailList" 필요시 구현 예정(2023.10.12 minjae)
                // ContainerDetailList.Clear();   // 컨테이너 정보, 파일정보 리스트 초기화
                // MetaDataList.Clear();          // 메타데이터 정보 리스트 초기화
                // TaskDetailList.Clear();        // 마일스톤 업무 리스트 초기화

                // containerDatas 안에 존재하는 파일 정보 반복문으로 방문 -> List 객체 containerInfos에 파일 정보 데이터 추가 
                foreach (var data in containerDatas)
                {
                    containerInfos.Add(data.ToString());
                }

                containerDataDic.Add(ProjectHelper.projectId, containerInfos[0]);
                containerDataDic.Add(TeamHelper.teamId, containerInfos[1]);
                containerDataDic.Add(ContainerHelper.containerId, containerInfos[2]);

                // TODO : ContainerHelper.cs -> ContainerPack 클래스 프로퍼티 "ContainerPack" 구현 (2023.10.12 minjae)  
                ContainerPack = new ContainerHelper.ContainerPack
                {
                    projectId = containerDataDic[ProjectHelper.projectId],
                    teamId = containerDataDic[TeamHelper.teamId],
                    containerId = containerDataDic[ContainerHelper.containerId]
                };

                // ExplorerV.xaml 화면 우측 하단에 할당할 선택한 특정 컨테이너 아이디 값 영어 대문자로 할당
                SelectedContainerId = ContainerPack.containerId.ToUpper();

                var total_Info_List = await ExplorerRestServer.GetContainerDetailListAsync(client, ContainerPack, tokenKey, inputTenantId);


                // TODO : 아래 Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)는
                //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                //        문서 정리하여 공유 받고 추가 구현 예정 (2023.11.01 minjae)
                // 연관 컨테이너 리스트(메타데이터) Rest API로 통신해서 받아오기 (Rest API 메서드 파라미터 rootContainerId로 전달)
                // var MetaDataList = await ExplorerRestServer.GetLinkedNodeInfoListAsync(client, ContainerPack, tokenKey, inputTenantId);


                // 데이터 타입 리스트 객체 "infoTypeList" 생성 (ContainerHelper.detail, ContainerHelper.metaDataList, ContainerHelper.taskDetail)
                // var infoTypeList = total_Info_List.SelectMany(container => container.Keys).ToList();

                foreach (var infoDic in total_Info_List)
                {
                    // TODO : Dictionary 객체 "infoDic"에 존재하는 value 값 가져와서 리스트로 구현 (2023.10.18 minjae)
                    var containerList = infoDic.SelectMany(container => container.Value).ToList();

                    // TODO : Dictionary<string, List<Dictionary<string, object>>> 클래스 객체 "infoDic"에 존재하는 Key 값 가져와서 리스트로 변환 (2023.10.18 minjae)
                    // 참고 URL - https://www.techiedelight.com/ko/get-list-of-keys-and-values-in-a-dictionary-in-csharp/
                    var infoTypeList = new List<string>(infoDic.Keys);

                    // TODO : 리스트 객체 "infoTypeList"의 첫 번째 요소 값을 반환 하도록 메서드 "FirstOfDefault" 사용 (2023.10.18 minjae)
                    // 참고 URL - https://developer-talk.tistory.com/294
                    var infoType = infoTypeList.FirstOrDefault();

                    foreach (var data in containerList)
                    {
                        switch (infoType)
                        {
                            // 연관된 부모 컨테이너 정보 
                            case ContainerHelper.linkedNodeInfo:
                                linkedNodeInfoList.Add(data);
                                // TODO : 아래 프로퍼티 "linkedNodeMetaDataList"에 할당 해야할 메타 데이터 리스트 "MetaDataList"의 경우 
                                //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                                //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                                //        문서 정리하여 공유 받고 구현 예정 (2023.11.01 minjae)
                                // linkedNodeMetaDataList = MetaDataList;
                                break;

                            // 컨테이너 정보, 파일정보
                            case ContainerHelper.detail:
                                // TODO : 추후 필요시 "컨테이너 정보, 파일정보" 객체 containerDetailList 구현 예정 (2023.10.17 minjae)
                                // containerDetailList = infoList;
                                detailDic = data;
                                createUserId = data[ContainerHelper.createUserId].ToString();                   // 작성자 
                                containerId = data[ContainerHelper.containerId].ToString();                     // 컨테이너 아이디

                                projectId = data[ContainerHelper.projectId].ToString();       // 프로젝트 아이디
                                teamId    = data[ContainerHelper.teamId].ToString();          // 팀 아이디

                                revsnVal = data[ContainerHelper.revsnVal].ToString();                           // 버전
                                title = data[ContainerHelper.title].ToString();                                 // 제목
                                fileNm = data[ContainerHelper.fileNm].ToString();                               // 파일명
                                fileId = data[ContainerHelper.fileId].ToString();                               // 파일 아이디 

                                // 파일유형 Dictionary
                                // var fileTypeCodeInfo = data[ContainerHelper.fileTypeCodeInfo].ToDictionary();
                                // fileTypeCodeNm = fileTypeCodeInfo[ContainerHelper.codeName].ToString();      // 파일유형
                                fileTypeCodeInfoDic = data[ContainerHelper.fileTypeCodeInfo].ToDictionary();
                                fileTypeSystemCode = fileTypeCodeInfoDic[ContainerHelper.systemCode].ToString();// 파일유형 - systemCode
                                fileTypeCodeNm = fileTypeCodeInfoDic[ContainerHelper.codeName].ToString();      // 파일유형 - codeName
                                break;

                            // 메타데이터 정보
                            case ContainerHelper.metaDataList:
                                metaDataList.Add(data);

                                var metaCode = data[ContainerHelper.metaCode].ToString();

                                // "metaCode": "soft-ifo" 일 경우 
                                if (metaCode.Equals(ContainerHelper.soft_ifo))
                                {
                                    var parentMetaCode = data[ContainerHelper.parentMetaCode].ToDictionary();   // parentMetaCode Dictionary
                                    softMetaNm = parentMetaCode[ContainerHelper.metaName].ToString();           // 소프트웨어 metaName
                                    softNm = data[ContainerHelper.metaComment].ToString();                      // 소프트웨어
                                }

                                // "metaCode": "soft-ver" 일 경우 
                                if (metaCode.Equals(ContainerHelper.soft_ver))
                                {
                                    var parentMetaCode = data[ContainerHelper.parentMetaCode].ToDictionary();   // parentMetaCode Dictionary
                                    softVerMetaNm = parentMetaCode[ContainerHelper.metaName].ToString();        // 소프트웨어버전 metaName
                                    softVer = data[ContainerHelper.metaComment].ToString();                     // 소프트웨어버전
                                }

                                // "metaCode": "find-tag" 일 경우 
                                if (metaCode.Equals(ContainerHelper.find_tag))
                                {
                                    var parentMetaCode = data[ContainerHelper.parentMetaCode].ToDictionary();   // parentMetaCode Dictionary
                                    keywordMetaNm = parentMetaCode[ContainerHelper.metaName].ToString();        // 키워드/태그 metaName
                                    keyword = data[ContainerHelper.metaComment].ToString();                     // 키워드/태그
                                }

                                break;

                            // 마일스톤 업무
                            case ContainerHelper.taskDetail:
                                taskDetailDic = data;
                                milestoneTaskInfoDic = taskDetailDic == null ? null : taskDetailDic[ContainerHelper.milestoneTaskInfo].ToDictionary();
                                // taskDetailDic.Where(x => x.Key.Equals("title")).Select(value => value).ToString();


                                taskDetailIdx = milestoneTaskInfoDic == null ? ContainerHelper.taskIdxNone : milestoneTaskInfoDic[ContainerHelper.idx].ToString();

                                // TODO : 마일스톤 업무 리스트 객체 "taskDetailList" 필요시 사용 예정 (2023.10.17 minjae)
                                // taskDetailList = infoDic[key];
                                // 마일스톤 - 업무 유형 데이터 마일스톤 관련 Rest API를 통해 데이터를 받아와야 하는 경우 아래 소스코드 수정 예정(2023.10.17 minjae)
                                // taskDetailType = taskDetailDic == null ? ContainerHelper.taskDetailTypeNone : taskDetailDic.Values.ToString();
                                taskDetailType = milestoneTaskInfoDic == null ? ContainerHelper.taskTypeNone : milestoneTaskInfoDic[ContainerHelper.title].ToString();
                                // taskDetailType = milestoneTaskInfoDic[ContainerHelper.title].ToString();

                                break;
                        }
                    }
                }

                var containerInfo = new ContainerView()
                {
                    LinkedNodeInfoList = linkedNodeInfoList,

                    // TODO : 아래 프로퍼티 "LinkedNodeMetaDataList"에 할당 해야할 메타 데이터 리스트 "linkedNodeMetaDataList"의 경우 
                    //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                    //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                    //        문서 정리하여 공유 받고 구현 예정 (2023.11.01 minjae)
                    // LinkedNodeMetaDataList = linkedNodeMetaDataList,

                    // TODO : 추후 필요시 "컨테이너 정보, 파일정보" 객체 containerDetailList 구현 예정 (2023.10.17 minjae)
                    // ContainerDetailList = containerDetailList,
                    DetailDic = detailDic,

                    FileTypeCodeDic = fileTypeCodeInfoDic,

                    MetaDataList = metaDataList,

                    // TODO : 추후 필요시 "마일스톤 업무" 리스트 객체 "TaskDetailList" 필요시 사용 예정 (2023.10.17 minjae)
                    // TaskDetailList = taskDetailList,
                    TaskDetailDic = taskDetailDic,
                    MilestoneTaskInfoDic = milestoneTaskInfoDic,

                    // Detail
                    exp_CreateUserId = createUserId,
                    exp_ContainerId  = containerId,
                    // exp_SelectedContainerId = ContainerPack.containerId.ToUpper(),   
                    // exp_UpperContainerId = containerId.ToUpper(),                 // ExplorerV.xaml 화면 우측 하단에 할당할 컨테이너 아이디 값 영어 대문자로 할당
      
                    exp_ProjectId = projectId,
                    exp_TeamId = teamId, 

                    exp_RevsnVal = revsnVal,
                    exp_Title = title,
                    exp_FileTypeSystemCode = fileTypeSystemCode,
                    exp_FileTypeCodeName = fileTypeCodeNm,

                    // 메타 데이터 
                    // TODO : 프로퍼티 "exp_SoftMetaName" 필요시 사용 예정 (2023.10.23 minjae)
                    // exp_SoftMetaName = softMetaNm,
                    exp_SoftName = softNm,

                    // TODO : 프로퍼티 "exp_SoftVerMetaName" 필요시 사용 예정 (2023.10.23 minjae)
                    // exp_SoftVerMetaName = softVerMetaNm,
                    exp_SoftVer  = softVer,

                    // TODO : 프로퍼티 "exp_KeywordTagMetaName" 필요시 사용 예정 (2023.10.23 minjae)
                    //exp_KeywordTagMetaName = keywordMetaNm,
                    exp_KeywordTag = keyword,

                    // 마일스톤
                    exp_TaskDetailIdx  = taskDetailIdx,
                    exp_TaskDetailType = taskDetailType,

                    // Detail - 파일명(이름)
                    exp_FileName = fileNm,

                    // Detail - 파일 아이디
                    exp_FileId   = fileId,

                    // 기본정보 탭 - Label (구분 항목)
                    exp_DivLabel = ContainerHelper.divLabel,
                    exp_ContentsLabel = ContainerHelper.contentsLabel,
                    exp_CreateUserLabel = ContainerHelper.createUserLabel,
                    exp_VersionLabel = ContainerHelper.versionLabel,
                    exp_TitleLabel = ContainerHelper.titleLabel,
                    exp_SoftLabel = softMetaNm,
                    exp_SoftVerLabel = softVerMetaNm,
                    exp_KeywordTagLabel = keywordMetaNm,
                    exp_FileTypeCodeLabel = ContainerHelper.fileTypeCodeLabel,
                    exp_TaskDetailLabel = ContainerHelper.taskDetailLabel,
                    exp_FileNameLabel = ContainerHelper.fileName
                };

                // exp_UpperContainerId = containerId.ToUpper();   // ExplorerV.xaml 화면 우측 하단에 할당할 컨테이너 아이디 값 영어 대문자로 할당


                // TODO : 메서드 "ContainerInfoTypeListCreate" 호출시 전달 인자로 containerInfo 설정할 때
                //        ref(기존 변수 containerInfo안에 들어있는 값을 메서드 "ContainerInfoTypeListCreate" 실행시 수정)로 구현 (2023.10.23 minjae)
                // 참고 URL - https://yeko90.tistory.com/entry/c-%EA%B8%B0%EC%B4%88-ref-vs-out-%EC%B0%A8%EC%9D%B4
                // 참고 2 URL - https://kdsoft-zeros.tistory.com/142
                // 참고 3 URL - https://codingcoding.tistory.com/56
                ContainerInfoTypeListCreate(ref containerInfo, fileTypeCodeInfoDic, taskDetailDic);   // ComboBox "파일유형", "마일스톤" List 생성 

                // TODO : 아래 비동기 메서드 "LinkedNodeInfoCreate"에 전달할 메서드 파라미터 메타 데이터 리스트 "MetaDataList"의 경우 
                //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
                // await LinkedNodeInfoCreate(linkedNodeInfoList, linkedNodeMetaDataList); // 연관 컨테이너 정보 Tree 생성 
                await LinkedNodeInfoCreate(linkedNodeInfoList); // 연관 컨테이너 정보 Tree 생성 

                // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측으로 마일스톤 목록 조회 RestAPI 요청 후 개발 진행 (2023.10.20 minjae)
                // "마일스톤" List 생성 
                // var taskDetailList = await ExplorerRestServer.GetTaskDetailListAsync();
                // containerInfo.TaskDetailType = await ExplorerRestServer.GetTaskDetailListAsync();

                ContainerList.Clear();
                ContainerList.Add(containerInfo);

                ContainerDatas.Clear();
                ContainerDatas.AddRange(ContainerList.ToArray());

                DisplayType = ExplorerHelper.TypeOfContainerInfoSearch;
                // IsVisible = true;            // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 보이도록 활성화
                DisplaySetting(DisplayType);

                // TODO : 테스트 코드 - 강제로 오류 발생하도록 Exception 생성 코드 필요시 사용 예정 (2023.10.24 minjae)
                // 참고 URL - https://morm.tistory.com/187
                // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
                // throw new Exception("테스트 오류 코드");

                MessageBox.Show("파일 기본정보 - 컨테이너 정보 테스트");
                return;

                // TODO : 아래 테스트 코드 필요시 참고 (2023.10.18 minjae)
                // foreach (var infoDic in total_Info_List)
                // {                  
                // 
                //     // TODO : infoDic.Keys의 값이 하나가 아니라 여러개일 경우 아래 리스트 객체 "keyList"와 foreach문 사용 예정 (2023.10.17 minjae)
                //     // TODO : Dictionary<string, List<Dictionary<string, object>>> 클래스 객체 "infoDic"에 존재하는 Key 값 가져와서 리스트로 변환 (2023.10.16 minjae)
                //     // 참고 URL - https://www.techiedelight.com/ko/get-list-of-keys-and-values-in-a-dictionary-in-csharp/
                //     var keyList = new List<string>(infoDic.Keys);
                // 
                //     foreach (var key in keyList)
                //     {
                //         var list = infoDic[key];
                //         foreach (var dic in list)
                //         {
                // 
                //         }
                //         switch (key)
                //         {
                //             // 컨테이너 정보, 파일정보
                //             case ContainerHelper.detail:
                //                 // var detailDic = infoDic[key].Select(value => value);
                //                 // var detailDic = infoDic[key].Select(list => list).Select(dic => dic).ToDictionary(); 
                //                 // containerDetailList = new List<Dictionary<string, object>> { detailDic };
                //                 // containerDetailList = new List<Dictionary<string, object>>();
                //                 containerDetailList.Clear();
                //                 containerDetailList = infoDic[key];
                // 
                //                 //foreach (var container in containerDetailList)
                //                 //{
                // 
                //                 //}
                //                 // var detailDic = containerDetailList.Where(detail => detail.Keys.Equals(ContainerHelper.containerId).Equals(ContainerPack.containerId)));
                // 
                //                 // ContainerDetailDatas
                //                 // infoDic[key].Where(detail => detail.Keys.Equals(ContainerHelper.createUserId))
                //                 // createUserId = detailDic[ContainerHelper.createUserId].ToString();      // 작성자 
                //                 // revsnVal = detailDic[ContainerHelper.revsnVal].ToString();              // 버전
                //                 // title = detailDic[ContainerHelper.title].ToString();                    // 제목
                //                 // fileNm = detailDic[ContainerHelper.fileNm].ToString();                  // 파일명
                // 
                //                 // 파일유형 Dictionary
                //                 // var fileTypeCodeInfo = detailDic[ContainerHelper.fileTypeCodeInfo].ToDictionary();
                //                 // fileTypeCodeNm = fileTypeCodeInfo[ContainerHelper.codeName].ToString();   // 파일유형
                // 
                //                 break;
                //             // 메타데이터 정보
                //             case ContainerHelper.metaDataList:
                //                 metaDataList = infoDic[key];
                // 
                //                 // 소프트웨어 Dictionary
                //                 var soft_ifoDic = metaDataList.Where(meta => meta.Values.Equals(ContainerHelper.soft_ifo)).Select(value => value); // .Select(value => value); // .ToDictionary();
                // 
                //                 // var test = metaDataList.FindAll(metaDic => metaDic.Values.Equals(ContainerHelper.soft_ifo)).;
                // 
                // 
                //                 // 소프트웨어버전 Dictionary
                //                 var soft_verDic = metaDataList.Where(meta => meta.Values.Equals(ContainerHelper.soft_ver)).Select(value => value); // .ToDictionary();
                //                 // 키워드/태그 Dictionary
                //                 var keywordDic = metaDataList.Where(meta => meta.Values.Equals(ContainerHelper.find_tag)).Select(value => value); // .ToDictionary();
                // 
                //                 // softNm = soft_ifoDic[ContainerHelper.metaComment].ToString();             // 소프트웨어
                //                 // softVersion = soft_verDic[ContainerHelper.metaComment].ToString();        // 소프트웨어버전
                //                 // keyword = keywordDic[ContainerHelper.metaComment].ToString();             // 키워드/태그
                // 
                //                 break;
                //             // 마일스톤 업무
                //             case ContainerHelper.taskDetail:
                //                 // var  = new List<string>(infoDic.Keys);
                //                 var taskDetailDic = infoDic[key].Select(value => value).ToDictionary();
                //                 taskDetailList = infoDic[key];
                //                 // 마일스톤 - 업무 유형 데이터 마일스톤 관련 Rest API를 통해 데이터를 받아와야 하는 경우 아래 소스코드 수정 예정(2023.10.17 minjae) 
                //                 taskDetailType = taskDetailDic == null ? ContainerHelper.taskDetailTypeNone : taskDetailDic.Values.ToString();
                // 
                //                 break;
                // 
                //             default:
                //                 MessageBox.Show($"데이터 입력 오류 {infoDic[key]} 확인 요망!");
                //                 Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + $"데이터 입력 오류 {infoDic[key]} 확인 요망!");
                //                 break;
                //         }
                //     }
                // }

                //var containerInfo = new ContainerView()
                //{
                //    ContainerDetailList = containerDetailList,
                //    MetaDataList = metaDataList,
                //    TaskDetailList = taskDetailList,

                //    exp_CreateUserId = createUserId,
                //    exp_RevsnVal = revsnVal,
                //    exp_Title = title,
                //    exp_FileTypeCodeName = fileTypeCodeNm,

                //    exp_SoftName = softNm,
                //    exp_SoftVersion = softVersion,
                //    exp_Keyword = keyword,

                //    exp_TaskDetailType = taskDetailType,

                //    exp_FileName = fileNm,
                //};

                //ContainerList.Add(containerInfo);

                //ContainerDatas.Clear();
                //ContainerDatas.AddRange(ContainerList.ToArray());

                // var containerDetailDic = total_Info_List.Where(detail => detail.Values.Equals());

                // total_Info_List.Where(list => list.)

                // var fileList = await ExplorerRestServer.GetExplorerFileListAsync(client, ProjectPack, folderId, tokenKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);

                // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.StackTrace - 오류 발생한 파일 경로 포함) 구현 (2023.10.19 minjae) 
                // 참고 URL - https://morm.tistory.com/187
                // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
                // 참고 3 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.exception.stacktrace?view=net-7.0
                Log.Error(ex.StackTrace);

                // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.ToString() - 오류 발생한 파일 경로 포함) 구현 (2023.10.19 minjae) 
                // 참고 URL - https://morm.tistory.com/187
                // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
                // 참고 3 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.exception.stacktrace?view=net-7.0
                Log.Error(ex.ToString());
            }
            return;
        }

        #endregion ContainerInfoSearchAsync

        #region ContainerInfoTypeListCreate

        // TODO : 메서드 "ContainerInfoTypeListCreate" 호출시 메서드 파라미터로 pContainerInfo 설정할 때
        //        ref(기존 변수 pContainerInfo안에 들어있는 값을 메서드 "ContainerInfoTypeListCreate" 실행시 수정)로 구현 (2023.10.23 minjae)
        // 참고 URL - https://yeko90.tistory.com/entry/c-%EA%B8%B0%EC%B4%88-ref-vs-out-%EC%B0%A8%EC%9D%B4
        // 참고 2 URL - https://kdsoft-zeros.tistory.com/142
        // 참고 3 URL - https://codingcoding.tistory.com/56
        /// <summary>
        /// ComboBox "파일유형", "마일스톤" List 생성 메서드 ContainerInfoTypeListCreate
        /// </summary>
        /// <param name="pContainerInfo"></param>
        /// <param name="pFileTypeCodeInfoDic"></param>
        /// <param name="pMilestoneTaskInfoDic"></param>
        private void ContainerInfoTypeListCreate(ref ContainerView pContainerInfo, Dictionary<string, object> pFileTypeCodeInfoDic, Dictionary<string, object> pTaskDetailDic)
        {
            string fileTypeSystemCode = string.Empty;   // 파일유형 - systemCode
            string fileTypeCodeNm     = string.Empty;   // 파일유형 - codeName

            string taskIdx            = string.Empty;   // 마일스톤 - 업무 인덱스                             
            string taskType           = string.Empty;   // 마일스톤 - 업무 유형

            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod         = MethodBase.GetCurrentMethod();

            // var ContainerInfo = pContainerInfo;

            try
            {
                fileTypeSystemCode = pContainerInfo.exp_FileTypeSystemCode;   // 파일유형 - systemCode
                fileTypeCodeNm     = pContainerInfo.exp_FileTypeCodeName;     // 파일유형 - codeName

                taskIdx            = pContainerInfo.exp_TaskDetailIdx;        // 마일스톤 - 업무 인덱스
                taskType           = pContainerInfo.exp_TaskDetailType;       // 마일스톤 - 업무 유형

                // TODO : pContainerInfo.FileTypeList.Clear(); 소스코드 실행시 FileTypeList 값이 null이므로 null Exception 발생해서 사용 안함.(2023.10.23 minjae)
                // pContainerInfo.FileTypeList.Clear();    // ComboBox - 파일유형 리스트 초기화

                // TODO : ComboBox "파일유형" 리스트 생성 구현 (2023.10.24 minjae)
                pContainerInfo.FileTypeList = new List<FileTypeView>
                {
                    new FileTypeView { exp_systemCode = ContainerHelper.systemCodeModel, exp_codeName = ContainerHelper.codeNameModel},
                    new FileTypeView { exp_systemCode = ContainerHelper.systemCodeReprt, exp_codeName = ContainerHelper.codeNameReprt}
                };

                // TODO : ComboBox "마일스톤" 리스트 생성 구현 (2023.10.24 minjae)
                pContainerInfo.TaskDetailList = new List<TaskDetailView>
                {
                    new TaskDetailView { exp_idx = ContainerHelper.taskIdxNone,     exp_title = ContainerHelper.taskTypeNone },       // 관련업무선택
                    new TaskDetailView { exp_idx = ContainerHelper.taskIdxDefine,   exp_title = ContainerHelper.taskTypeDefine },     // 요구사항정의            
                    new TaskDetailView { exp_idx = ContainerHelper.taskIdxEntity,   exp_title = ContainerHelper.taskTypeEntity },     // 엔터티도출
                    new TaskDetailView { exp_idx = ContainerHelper.taskIdxCreation, exp_title = ContainerHelper.taskTypeCreation },   // 요구사항정의서 작성
                    new TaskDetailView { exp_idx = ContainerHelper.taskIdxUI,       exp_title = ContainerHelper.taskTypeUI },         // UI설계서 작성
                };


                // TODO : 리스트 객체 "pContainerInfo.FileTypeList"의 조건을 만족하는 첫 번째 요소 값을 반환 하도록 메서드 "FirstOfDefault" 사용 (2023.10.18 minjae)
                // 참고 URL - https://developer-talk.tistory.com/294
                // TODO : 리스트 객체 "pContainerInfo.FileTypeList"의 FirstOrDefault 메서드 사용할 때, Contains 메서드 사용해서 조건 1, 조건 2 만족하는 데이터 있는지 찾기 (2023.10.23 minjae) 
                // 참고 URL - https://developer-talk.tistory.com/585 
                // FirstOrDefault (where 조건절과 하는 역할 비슷하고 조건을 만족하는 요소가 존재하는지 체크 기능)
                // 조건 1 - pContainerInfo.FileTypeList에 할당된 (파일유형 - systemCode - fileType.exp_systemCode) 값과 fileTypeSystemCode에 할당된 (컨테이너 정보 파일유형 - systemCode) 가 동일해야 한다.
                // 조건 2 - pContainerInfo.FileTypeList에 할당된 (파일유형 - codeName - fileType.exp_codeName) 값과 fileTypeCodeNm에 할당된 (컨테이너 정보 파일유형 - codeName) 가 동일해야 한다. 
                pContainerInfo.SelectParam.FileTypeCodeInfo = pContainerInfo.FileTypeList.FirstOrDefault(fileType => fileType.exp_systemCode.Contains(fileTypeSystemCode)
                                                                                                                  && fileType.exp_codeName.Contains(fileTypeCodeNm));

                // TODO : 리스트 객체 "pContainerInfo.TaskDetailList"의 조건을 만족하는 첫 번째 요소 값을 반환 하도록 메서드 "FirstOfDefault" 사용 (2023.10.24 minjae)
                pContainerInfo.SelectParam.TaskDetail = pContainerInfo.TaskDetailList.FirstOrDefault(taskDetailType => taskDetailType.exp_idx.Contains(taskIdx)
                                                                                                                    && taskDetailType.exp_title.Contains(taskType));



                // Param.FileTypeCodeInfo = 

                // TODO : ComboxBox - 파일유형에 바인딩할 BindableCollection에 값 할당 및 서버로부터 받은 값을 화면에 출력 할 수 있도록
                //        메서드 파라미터 "pContainerInfo", "pFileTypeCodeInfoDic" 활용하기 (2023.10.20 minjae)
                // pContainerInfo.FileType
                // exp_systemCode 
                // exp_codeName 

                // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측에서 마일스톤 RestAPI 공유 받은 후 개발 진행 (2023.10.23 minjae)
                //pContainerInfo.TaskDetailType = pMilestoneTaskInfoDic.Where().Select()).
                //pContainerInfo.TaskDetailType.Title
                // var result = await ExplorerRestServer.GetTaskDetailListAsync();


                // TODO : ComboxBox - 파일유형에 바인딩할 BindableCollection에 값 할당 및 서버로부터 받은 값을 화면에 출력 할 수 있도록
                //        메서드 파라미터 "pContainerInfo", "pFileTypeCodeInfoDic" 활용하기 (2023.10.20 minjae)
                //        컨테이너 정보 영역 "마일스톤" ComboBox 구현시 바인딩할 BindableCollection 객체 TaskDetailType 구현(2023.10.20 minjae)
                // pContainerInfo.FileType
                // exp_systemCode 
                // exp_codeName 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);

                // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.StackTrace - 오류 발생한 파일 경로 포함) 구현 (2023.10.19 minjae) 
                // 참고 URL - https://morm.tistory.com/187
                // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
                // 참고 3 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.exception.stacktrace?view=net-7.0
                Log.Error(ex.StackTrace);

                // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.ToString() - 오류 발생한 파일 경로 포함) 구현 (2023.10.19 minjae) 
                // 참고 URL - https://morm.tistory.com/187
                // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
                // 참고 3 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.exception.stacktrace?view=net-7.0
                Log.Error(ex.ToString());
                throw;
            }
        }

        #endregion ContainerInfoTypeListCreate

        #region LinkedNodeInfoCreate

        // TODO : 아래 비동기 메서드 "LinkedNodeInfoCreate"에 전달할 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우 
        //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
        /// <summary>
        /// 연관 컨테이너 정보 Tree 생성 
        /// </summary>
        /// <returns></returns>
        // private async Task LinkedNodeInfoCreate(List<Dictionary<string, object>> pLinkedNodeInfoList, List<Dictionary<string, object>> pLinkedNodeMetaDataList)
        private async Task LinkedNodeInfoCreate(List<Dictionary<string, object>> pLinkedNodeInfoList)
        {
            string rootContainerId = string.Empty;                                  // 루트 컨테이너 아이디

            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.27 minjae)
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                // LinkedNodeInfoList.Clear();          // 연관 컨테이너 정보 리스트 초기화
                // LinkedNodeInfoDatas.Clear();         // 연관 컨테이너 정보 Collection 초기화
                DisplayType = ExplorerHelper.TypeOfLinkedNodeInfoCreate;
                DisplaySetting(DisplayType);

                // 루트 노드 컨테이너 아이디 찾기
                // 부모 컨테이너 아이디(ContainerHelper.targetId)는 존재하지 않고(== null),
                // 자식 컨테이너 아이디(또는 자기 자신 컨테이너 ID)(ContainerHelper.sourceId)만 존재하는 경우(!= null)
                // rootContainerId = pLinkedNodeInfoList.Where(node => node[ContainerHelper.targetId] == null
                //                                                  && node[ContainerHelper.sourceId] != null)
                //                                      .Select(node => node[ContainerHelper.sourceId].ToString())
                //                                      .FirstOrDefault();

                // 루트 노드(컨테이너) 찾기
                // 부모 컨테이너 아이디(ContainerHelper.targetId)는 존재하지 않고(== null),
                // 자식 컨테이너 아이디(또는 자기 자신 컨테이너 ID)(ContainerHelper.sourceId)만 존재하는 경우(!= null)
                LinkedNodeView rootContainer = pLinkedNodeInfoList.Where(node => node[ContainerHelper.targetId] == null
                                                                              && node[ContainerHelper.sourceId] != null)
                                                                  .Select(node => new LinkedNodeView
                                                                                      {
                                                                                          // TreeView 영역 루트 노드 이름 프로퍼티 "exp_TreeName" 영어 대문자로 출력 (2023.11.01 minjae)
                                                                                          exp_TreeName = node[ContainerHelper.sourceId].ToString().ToUpper()
                                                                                          // LinkedNodeInfoDic = linkedNodeInfoList.Where(node => node.Values.Equals())
                                                                                      })
                                                                  .FirstOrDefault();



                // TODO : 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
                //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
                // TODO : 루트 컨테이너 "rootContainer"에 루트 컨테이너 아이디 "rootContainerId"에 속하는 메타데이터를
                //        메타데이터 리스트 객체 "pLinkedNodeMetaDataList"에서 Linq로 찾아서 "new LinkedNodeView" 프로퍼티 "LinkedNodeInfoDic"에 할당하기 (2023.11.01 minjae)
                // LinkedNodeView rootContainer = pLinkedNodeMetaDataList.Where(meta => meta[ContainerHelper.containerId].ToString().Equals(rootContainerId))
                //                                                       .Select(meta => new LinkedNodeView
                //                                                                           {
                //                                                                               exp_TreeName = rootContainerId,
                //                                                                               LinkedNodeInfoDic = meta
                //                                                                           })
                //                                                       .FirstOrDefault();

                // 루트 노드(컨테이너)가 존재하지 않을 경우 리턴 
                if (rootContainer == null) return;

                LinkedNodeInfoList.Add(rootContainer);

                // TODO : 비동기 메서드 "FillLinkedNodes"에 속하는 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
                //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
                // 루트 컨테이너(노드) 하위 참조관계에 속한 자식 컨테이너(노드) 추가
                // await FillLinkedNodes(rootContainer, pLinkedNodeInfoList, pLinkedNodeMetaDataList);
                await FillLinkedNodes(rootContainer, pLinkedNodeInfoList);

                LinkedNodeInfoDatas.Clear();   // 연관 컨테이너 정보 Collection 초기화
                LinkedNodeInfoDatas.AddRange(LinkedNodeInfoList.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);

                // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.StackTrace - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
                Log.Error(ex.StackTrace);

                // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.ToString() - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
                Log.Error(ex.ToString());
                throw;
            }
            return;
        }

        #endregion LinkedNodeInfoCreate

        #region FillLinkedNodes

        // TODO : 비동기 메서드 "FillLinkedNodes"에 속하는 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
        //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
        /// <summary>
        /// 루트 컨테이너(노드) 하위 참조관계에 속한 자식 컨테이너(노드) 추가
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="pLinkedNodeInfoList"></param>
        /// <returns></returns>
        // private async Task FillLinkedNodes(LinkedNodeView parentContainer, List<Dictionary<string, object>> pLinkedNodeInfoList, List<Dictionary<string, object>> pLinkedNodeMetaDataList)
        private async Task FillLinkedNodes(LinkedNodeView parentContainer, List<Dictionary<string, object>> pLinkedNodeInfoList)
        {
            string subContainerId = string.Empty;   // 서브 컨테이너 아이디

            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.27 minjae)
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                // TODO : pLinkedNodeInfoList의 WHERE 조건절 -> TreeView 영역 부모 노드 이름 프로퍼티 " parentContainer.exp_TreeName" 영어 대문자 -> 소문자로 변환(ToLower()) 후
                //        node[ContainerHelper.targetId]?.ToString() 문자열(영어 소문자) 값과 비교하여 같은 값만 추출하기 (2023.11.01 minjae)
                // 부모 컨테이너(루트 또는 서브)에 하위 참조관계에 속한 자식 컨테이너 데이터 추출 
                var childNodes = pLinkedNodeInfoList.Where(node => parentContainer.exp_TreeName.ToLower() == node[ContainerHelper.targetId]?.ToString());

                if (childNodes.Any() == false) return;

                foreach (var childNode in childNodes)
                {
                    // TODO : TreeView 영역 부모 노드 하위 자식 노드(childNode) 이름 프로퍼티 "exp_TreeName" 영어 대문자로 출력 (2023.11.01 minjae)
                    subContainerId = childNode[ContainerHelper.sourceId].ToString().ToUpper();

                    // TODO : 참조 컨테이너 "subContainer"에 참조 컨테이너 아이디 "subContainer"에 속하는 메타데이터를 "new LinkedNodeView" 프로퍼티 "LinkedNodeInfoDic"에 할당하기 (2023.10.30 minjae)
                    // 참조관계에 속한 자식 컨테이너(subContainer) 생성 및 아이디 값 할당 
                    var subContainer = new LinkedNodeView
                                           {
                                               exp_TreeName = childNode[ContainerHelper.sourceId].ToString().ToUpper(),
                                               // LinkedNodeInfoDic
                                           };

                    // TODO : 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
                    //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                    //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                    //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
                    // var subContainer = pLinkedNodeMetaDataList.Where(meta => meta[ContainerHelper.containerId].ToString().Equals(subContainerId))
                    //                                           .Select(meta => new LinkedNodeView 
                    //                                                               { 
                    //                                                                   exp_TreeName = subContainerId,
                    //                                                                   LinkedNodeInfoDic = meta
                    //                                                               })
                    //                                           .FirstOrDefault();

                    // 부모 컨테이너(루트 또는 서브)의 자식 노드로 subContainer 추가 
                    parentContainer.ChildNodes.Add(subContainer);

                    // TODO : 비동기 메서드 "FillLinkedNodes"에 속하는 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
                    //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                    //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                    //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
                    // 서브 컨테이너의 자식 노드로 또 다른 서브 컨테이너 추가 
                    // await FillLinkedNodes(subContainer, pLinkedNodeInfoList, pLinkedNodeMetaDataList);
                    await FillLinkedNodes(subContainer, pLinkedNodeInfoList);
                }

            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);

                // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.StackTrace - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
                Log.Error(ex.StackTrace);

                // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.ToString() - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
                Log.Error(ex.ToString());
                throw;
            }
        }

        #endregion FillLinkedNodes

        #region LinkedNodeInfoSearchAsync

        private async Task LinkedNodeInfoSearchAsync(object obj)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.27 minjae)
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                MessageBox.Show("연관 컨테이너(참조관계 - 부모/자식 컨테이너) 메타데이터 정보 출력 기능 필요시 구현 예정");
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);

                // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.StackTrace - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
                // Log.Error(e.StackTrace);

                // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.ToString() - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
                // Log.Error(e.ToString());
                // throw;
            }
            return;
        }

        #endregion LinkedNodeInfoSearchAsync

        #region RequestAsync

        /// <summary>
        /// 검토요청
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task RequestAsync(object obj)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.18 minjae)
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                MessageBox.Show("승인 / 검토 요청 필요시 구현 예정");
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
            return;
        }

        #endregion RequestAsync

        #region UpdateAsync

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task UpdateAsync(object obj)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.18 minjae)
            var currentMethod = MethodBase.GetCurrentMethod();

            var upContainerDatas = obj as IEnumerable;

            // string upSystemCode = string.Empty;   // 수정할 systemCode - typmodel, typreprt

            // string upCodeName = string.Empty;     // 수정할 codeName - 모델 / 보고서

            try 
            {
                foreach (ContainerView data in upContainerDatas)
                {
                    // TODO : 아래 소스코드에 들어갈 데이터가 추가로 필요할 시 소스코드 수정 예정 (2023.10.23 minjae)
                    //        UpdateParam 프로퍼티 필요 없어도 변경된 데이터(data.SelectParam)들이 정상적으로 서버로 넘어갈 수 있으면 UpdateParam 삭제 예정 (2023.10.23 minjae)
                    UpdateParam = new Param();
                    UpdateParam = data.SelectParam;
                }

                // var test = ContainerDatas.(containerInfo => containerInfo.SelectParam).Object;
                // UpdateParam = obj as IEnumerable; // .Select(s => s.SelectParam.FileTypeCodeInfo);

                // UpdateAsync 메서드 실행시 ComboBox 파일유형에서 변경된 값을 가져오도록 Linq 식으로 구현하기 (2023.10.23 minjae) 
                // var obj = null;
                // FileTypeView fileTypeCodeInfo = ContainerDatas.Select(x => x.Param.FileTypeCodeInfo);

                // upSystemCode = fileTypeCodeInfo
                //Select(x => x.Param.FileTypeCodeInfo);

                MessageBox.Show("수정 기능 필요시 구현 예정");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
        }

        #endregion UpdateAsync

        #region DownlLoadAsync

        /// <summary>
        /// 다운로드
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task DownLoadAsync(object obj)
        {
            string inputTenantId = LoginHelper.inputTenantId;                  // 테넌트 아이디
            string tokenKey      = AppSetting.Default.Login.Token.resultData;  // 로그인 토큰 키 

            // TODO : Object 배열 객체 obj 리스트로 변환 (2023.09.19 minjae)
            // 참고 URL - https://nonstop-antoine.tistory.com/36
            var FileDatas        = obj as IEnumerable;                         // 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 
            var FileInfos        = new List<string>();                         // FileDatas 를 리스트로 변환할 때 사용하는 리스트 객체 "FileInfos"
            var FileDataDic      = new Dictionary<string, string>();           // 컨테이너 정보 데이터를 담는 Dictionary 객체 FileDataDic 생성

            string fileId        = string.Empty;                               // 파일 아이디
            string fileName      = string.Empty;                               // 파일명(이름)

            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                MessageBox.Show("다운로드 테스트 중입니다.");

                // containerDatas 안에 존재하는 파일 정보 반복문으로 방문 -> List 객체 containerInfos에 파일 정보 데이터 추가 
                foreach (var data in FileDatas)
                {
                    FileInfos.Add(data.ToString());
                }

                FileDataDic.Add(ProjectHelper.projectId, FileInfos[0]);
                FileDataDic.Add(TeamHelper.teamId, FileInfos[1]);
                FileDataDic.Add(ContainerHelper.containerId, FileInfos[2]);
                FileDataDic.Add(ContainerHelper.fileId, FileInfos[3]);
                FileDataDic.Add(ContainerHelper.fileNm, FileInfos[4]);

                // TODO : ProjectHelper.cs -> ProjectPack 클래스 프로퍼티 "ProjectPack" 구현 및 테스트 프로젝트 아이디(testProjectId), 테스트 팀 아이디(testTeamId) 값 할당 (2023.09.04 minjae)
                //        추후 필요시 수정 예정
                ContainerPack = new ContainerHelper.ContainerPack
                {
                    projectId = FileDataDic[ProjectHelper.projectId],
                    teamId = FileDataDic[TeamHelper.teamId],
                    containerId = FileDataDic[ContainerHelper.containerId]
                };

                FilePack = new FileHelper.FilePack 
                {
                    fileId = FileDataDic[ContainerHelper.fileId],       // 파일 아이디
                    fileName = FileDataDic[ContainerHelper.fileNm]      // 파일명(이름)
                };

                // 파일 다운로드 진행
                await ExplorerRestServer.GetFileDownLoadAsync(client, ContainerPack, FilePack, tokenKey, inputTenantId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
            finally
            {
                // TODO : 추후 필요 시 finally문에 코드 추가 예정 (2023.08.24 minjae)
            }
            // return;
        }

        #endregion DownlLoadAsync

        #region UploadAsync

        /// <summary>
        /// 탭 "변경" - 컨테이너 변경(컨테이너 안에 저장한 파일 A -> B) 및 저장 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task UploadAsync(object obj)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
        }


        #endregion UploadAsync

        // TODO : 마우스 오른쪽 클릭 이벤트 메서드 "Btn_MouseRightButtonDown" 필요시 뷰모델에 구현 예정 (2023.11.02 minjae)
        // #region Btn_MouseRightButtonDown

        // TODO : 마우스 오른쪽 클릭 이벤트 메서드 "Btn_MouseRightButtonDown" 구현 (2023.11.02 minjae)
        // 참고 URL - https://yeko90.tistory.com/entry/c-wpf-%EB%A7%88%EC%9A%B0%EC%8A%A4-%EC%9D%B4%EB%B2%A4%ED%8A%B8%EB%8D%94%EB%B8%94%ED%81%B4%EB%A6%AD-%EC%98%A4%EB%A5%B8%EC%AA%BD%ED%81%B4%EB%A6%AD-%EC%99%BC%EC%AA%BD-%ED%81%B4%EB%A6%AD
        // 참고 2 URL - https://poorman.tistory.com/263
        // private void Btn_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        // {
        //     MessageBox.Show("마우스 오른쪽 클릭 : 향후 체크인/체크아웃 항목 구현 예정");
        // }

        // #endregion Btn_MouseRightButtonDown

        #region TestAsync

        /// <summary>
        /// 테스트
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task TestAsync(object obj)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                MessageBox.Show("테스트 중입니다.");

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
            finally
            {
                // TODO : 추후 필요 시 finally문에 코드 추가 예정 (2023.08.24 minjae)
            }
            return;
        }

        #endregion TestAsync

        #region CheckOutAsync

        /// <summary>
        /// 체크아웃
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task CheckOutAsync(object obj)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                MessageBox.Show("체크아웃 테스트 중입니다.");

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
            finally
            {
                // TODO : 추후 필요 시 finally문에 코드 추가 예정 (2023.08.24 minjae)
            }
            return;
        }

        #endregion CheckOutAsync

        #region CheckInAsync

        /// <summary>
        /// 체크인
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task CheckInAsync(object obj)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                MessageBox.Show("체크인 테스트 중입니다.");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
            }
            finally
            {
                // TODO : 추후 필요 시 finally문에 코드 추가 예정 (2023.08.24 minjae)
            }
            return;
        }

        #endregion CheckInAsync

        #region FolderListAsync

        // TODO : 폴더 리스트 데이터 받아오기 메서드 "FolderListAsync" 필요 시 구현 예정 (2023.09.19 minjae)
        //private async Task FolderListAsync(object obj)
        //{
        //    string projectId = ProjectHelper.testProjectId;   // 프로젝트 아이디 
        //    string teamId    = ProjectHelper.testTeamId;      // 팀 아이디
        //    string tokenKey = AppSetting.Default.Login.Token.resultData;   // 로그인 토큰 키 
        //    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
        //    // 참고 URL - https://slaner.tistory.com/73
        //    // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
        //    // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
        //    var currentMethod = MethodBase.GetCurrentMethod();

        //    try
        //    {
        //        // 폴더 리스트 데이터 받아오기 
        //        // FolderList = await ExplorerRestServer.GetExplorerFolderListAsync(client, tokenKey, projectId, teamId);


        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage +  ex.Message);
        //    }
        //}


        #endregion FolderListAsync

        #region Handle
        public void Handle(ChangeViewModelMsg message)
        {
            // throw new NotImplementedException();
        }

        #endregion Handle

        #region Sample

        #endregion Sample
    }
}
