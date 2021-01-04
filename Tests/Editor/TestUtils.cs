using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using System;

namespace Tests
{
    public static class TestsAsyncUtils
    {
        private static IEnumerator WaitSeconds(float delayedSeconds, Action onEnd)
        {
            float seconds = 0;
            while (seconds < delayedSeconds)
            {
                seconds = seconds + Time.deltaTime;
                yield return null;
            }
            onEnd.Invoke();
        }


        public static UniTask Delay(float seconds)
        {
            var t = new UniTaskCompletionSource();
            EditorCoroutineUtility.StartCoroutineOwnerless(WaitSeconds(seconds, () => t.TrySetResult()));
            return t.Task;
        }

    }
}