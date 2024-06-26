using System;
using System.Collections.Generic;
using Battle.Character.Enemy;
using Battle.MyCamera;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create StageObjectHolder", fileName = "StageObjectHolder", order = 0)]
    public class StageObjectHolder : SerializedScriptableObject
    {
        [OdinSerialize] private readonly Dictionary<string, StageObject> _stageObjects;
        public IReadOnlyDictionary<string, StageObject> StageObjects => _stageObjects;

        [Serializable]
        public class StageObject
        {
            [SerializeField] private BossBase boss;
            [SerializeField] private BackGroundCameraRoot backGroundCameraRoot;


            public BossBase Boss => boss;

            public BackGroundCameraRoot BackGroundCameraRoot => backGroundCameraRoot;
        }
    }
}