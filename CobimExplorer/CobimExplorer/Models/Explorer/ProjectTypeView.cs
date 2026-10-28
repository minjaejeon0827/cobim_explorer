using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobimExplorer.Models.Explorer
{
    public class ProjectTypeView
    {
        /// <summary>
        /// 프로젝트 목록 이름 (프로젝트 목록은 파일 탐색기의 상위 드라이브 문자를 가진 저장소인 C 드라이브 또는 D 드라이브와 같은 역할) 
        /// 드라이브 참고 URL - https://namu.wiki/w/C%20%EB%93%9C%EB%9D%BC%EC%9D%B4%EB%B8%8C
        /// </summary>
        //public string ProjectName { get => _ProjectName; set { _ProjectName = value; NotifyOfPropertyChange(); } }
        //private string _ProjectName = string.Empty;
        public string ProjectName { get; set; }
    }
}
