using System;
using System.Collections.Generic;

namespace Calchemy.Core
{
    /// <summary>
    /// 효과 로직 데이터 스키마
    /// Antigravity YAML 데이터의 'singleLogic', 'bingoLogic'에 대응
    /// </summary>
    [Serializable]
    public class EffectLogic
    {
        public string action; // 명령 (예: "TRANSFORM", "ADD_SHIELD")
        public List<EffectParameter> parameters; // 매개변수 리스트 (Dictionary 대신 Serialized 위해 List 사용)

        public EffectLogic()
        {
            parameters = new List<EffectParameter>();
        }
    }

    [Serializable]
    public class EffectParameter
    {
        public string key;
        public float value;
    }
}
