//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GuideGame {
    public sealed class GuideScript : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI pagText;
        [SerializeField] private GameObject[] pages;
        private int pageIndex;

        private void Start() {
            pageIndex = 0;

            AtualizarUI();
        }

        public void PageUP() {
            pageIndex += 1;
            if (pageIndex >= pages.Length - 1) pageIndex = pages.Length - 1;

            AtualizarUI();
        }

        public void PageDown() {
            pageIndex -= 1;
            if (pageIndex < 0) pageIndex = 0;

            AtualizarUI();
        }

        private void AtualizarUI() {
            for (int i = 0; i < pages.Length; i++) {
                pages[i].SetActive(false);
            }
            pages[pageIndex].SetActive(true);

            pagText.text = string.Format("Page: {0} / {1} ", pageIndex + 1, pages.Length);
        }
    }

}