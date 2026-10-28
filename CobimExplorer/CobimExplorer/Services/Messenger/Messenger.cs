using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobimExplorer.Services.Messenger
{
    // TODO : Messenger 클래스 필요시 구현 예정 (2023.08.31 minjae)
    // 유튜브 참고 URL - https://youtu.be/-6Td1iLBXGw?si=_Lq600jpriCve3dQ
    public class Messenger
    {
        //    List<SubScription> _Subscription = new();

        //    public void Send<TMessage>(TMessage message)
        //    {
        //        foreach (var subscription in _Subscription)
        //            _Subscription.Action(message);
        //    }

        //    public void Subscribe<TMessage>(object subscriber, Action<object> action)
        //    {
        //        _Subscription.Add(new SubScription(subscriber, action));
        //    }

        //    public void UnSubscribe<TMessage>(object subscriber)
        //    {
        //        var subscription = _Subscription.FirstOrDefault(x => x.Subscriber == subscriber);
                  // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 minjae)
                  // 참고 URL - https://husk321.tistory.com/405
        //        if (subscription is not null)
        //            _Subscription.Remove(subscription);
        //    }
        //}

        //public record SubScription(object Subscriber, Action<object> Action);
    }
}
