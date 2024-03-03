using Cinemachine;
using UnityEngine;

namespace Battle.MyCamera
{
    public class CameraZoom : CinemachineExtension
    {
        [SerializeField] private float minOffset, maxOffset;
        [SerializeField] private float minTargetDistance, maxTargetDistance;

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage,
            ref CameraState state,
            float deltaTime)
        {
            if (stage != CinemachineCore.Stage.Aim)
                return;

            var distance = GetDistance(vcam.AbstractFollowTargetGroup);
            var clampedDistance = Mathf.Clamp(distance, minTargetDistance, maxTargetDistance);

            var normalizedDistance = (clampedDistance - minTargetDistance) / (maxTargetDistance - minTargetDistance);
            
            var offset = maxOffset - minOffset;
            offset *= normalizedDistance;
            offset += minOffset;

            var pos = state.RawPosition;
            pos.z = -offset;
            state.RawPosition = pos;
        }

        private float GetDistance(ICinemachineTargetGroup targetGroup)
        {
            var targets = ((CinemachineTargetGroup)targetGroup).m_Targets;

            var maxDistance = 0f;

            for (int i = 0; i < targets.Length; i++)
            {
                for (int j = i + 1; j < targets.Length; j++)
                {
                    var distance = Vector3.Distance(targets[i].target.position, targets[j].target.position);
                    if (distance > maxDistance)
                        maxDistance = distance;
                }
            }

            return maxDistance;
        }
    }
}