using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using CobimNet;
using CobimExplorer.Common.LogManager;

namespace CobimExplorer.Settings
{
    public class AppSetting : SettingPartBase
    {
        // private static AppSetting _default;
        // TODO : UiSettingPart.cs 필요시 구현 예정 (2023.08.30 jbh)
        //private UiSettingPart ui;
        // private LoginSetting login;

        // TODO : AppSetting.cs -> 응용 프로그램 기본 설정 프로퍼티 "Default" 필요시 수정 예정 (2023.09.24 jbh)
        /// <summary>
        /// 응용 프로그램 기본 설정
        /// </summary>
        public static AppSetting Default
        {
            get => AppSetting._Default ?? (AppSetting._Default = new AppSetting());
            set
            {
                AppSetting._Default = value;
                BindableBase.StaticChanged(nameof(Default));
            }
        }
        private static AppSetting _Default;

        // TODO : UiSettingPart.cs 필요시 구현 예정 (2023.08.30 jbh)
        //public UiSettingPart UI
        //{
        //    get => this.ui ?? (this.ui = new UiSettingPart());
        //    set => this.SetAndNotify<UiSettingPart>(ref this.ui, value, nameof(UI));
        //}


        /// <summary>
        /// 로그인 기본 설정 
        /// </summary>
        public LoginSetting LoginBase
        {
            get => _LoginBase ?? (_LoginBase = new LoginSetting());
            set
            {
                _LoginBase = value;
                this.Changed(nameof(LoginBase));
            }
        }
        private LoginSetting _LoginBase;

        /// <summary>
        /// 디렉토리(폴더) 기본 설정
        /// </summary>
        public DirectorySetting DirectoryBase
        {
            get => _DirectoryBase ?? (_DirectoryBase = new DirectorySetting());
            set
            {
                _DirectoryBase = value;
                this.Changed(nameof(DirectoryBase));
            }
        }
        private DirectorySetting _DirectoryBase;

        /// <summary>
        /// 프로젝트 테스트 설정
        /// </summary>
        public ProjectSetting Project
        {
            get => this._Project ?? (this._Project = new ProjectSetting());
            set 
            {
                this._Project = value;
                this.Changed(nameof(Project));
            }
        }
        private ProjectSetting _Project;


        public static void LoadDefaultFromFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            //if (!fileInfo.Exists)
            if (false == fileInfo.Exists)
                AppSetting.SaveDefaultToFile(path);
            StreamReader streamReader = fileInfo.OpenText();
            string end = streamReader.ReadToEnd();
            streamReader.Dispose();
            AppSetting.Default = AppSetting.Deserialize(end);
        }

        public static void SaveDefaultToFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            fileInfo.Directory.Create();
            StreamWriter text = fileInfo.CreateText();
            text.Write(AppSetting.Serialize(AppSetting.Default));
            text.Dispose();
        }

        public static string Serialize(AppSetting setting) => JsonSerializer.Serialize<AppSetting>(setting, new JsonSerializerOptions()
        {
            WriteIndented = true
        });

        public static AppSetting Deserialize(string txt)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 jbh)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                return JsonSerializer.Deserialize<AppSetting>(txt, new JsonSerializerOptions());
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                return new AppSetting();
            }
        }
    }
}
