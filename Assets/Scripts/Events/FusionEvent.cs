using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace FusionUtilsEvents
{
    /// <summary>
    /// Fusion에서 발생한 이벤트들을 옵저버 패턴으로 관리하기 위한 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu]
    public class FusionEvent : ScriptableObject
    {
        public List<Action<PlayerRef, NetworkRunner>> Responses = new List<Action<PlayerRef, NetworkRunner>>();

        public void Raise(PlayerRef player = default, NetworkRunner runner = null)
        {
            for (int i = 0; i < Responses.Count; i++)
            {
                Responses[i].Invoke(player, runner);
            }
        }

        public void RegisterResponse(Action<PlayerRef, NetworkRunner> response)
        {
            Responses.Add(response);
        }

        public void RemoveResponse(Action<PlayerRef, NetworkRunner> response)
        {
            if (Responses.Contains(response))
                Responses.Remove(response);
        }
    }
}