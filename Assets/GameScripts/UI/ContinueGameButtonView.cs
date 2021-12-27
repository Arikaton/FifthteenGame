using System;
using GameScripts.Game;
using UnityEngine;

namespace GameScripts.UI
{
    public class ContinueGameButtonView : MonoBehaviour
    {
        private void Start()
        {
            UpdateState();
        }

        public void UpdateState()
        {
            gameObject.SetActive(FieldModel.HasSavedInstance);
        }
    }
}