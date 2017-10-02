using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Text3D {
    public class Text3D : CustomBehaviour {
        private readonly LinkedList<TextCharacter> characters = new LinkedList<TextCharacter>();

        public string Text;
        private string currentText;

        private string RenderableText {
            get { return Text.ToUpper(); }
        }

        public int Count {
            get { return characters.Count; }
        }

        private float currentTracking;
        [Range(-0.5f, 0.5f)] public float Tracking = 0.0f;

        private float currentWhitespaceWidth;
        [Range(0, 1)] public float WhitespaceWidth = 1.0f;

        private void Start() {
            ResetCharacters();
            LayoutCharacters();
            SetCurrentVariables();
        }

        private void Update() {
            if (currentText != Text) {
                ResetCharacters();
                LayoutCharacters();
            }
            else if (currentTracking != Tracking || currentWhitespaceWidth != WhitespaceWidth) {
                LayoutCharacters();
            }

            SetCurrentVariables();
        }

        private void SetCurrentVariables() {
            currentText = Text;
            currentTracking = Tracking;
            currentWhitespaceWidth = WhitespaceWidth;
        }

        private void LayoutCharacters() {
            float left = 0.0f;
            foreach (var textCharacter in characters) {
                if (textCharacter == null) {
                    // whitespace
                    left -= Tracking + WhitespaceWidth;
                    continue;
                }
                textCharacter.transform.localPosition = textCharacter.transform.localPosition.CopySetX(left);
                left -= Tracking + textCharacter.BoxCollider.size.x;
            }
        }

        private void ResetCharacters() {
            foreach (var textCharacter in characters) {
                Destroy(textCharacter);
            }

            characters.Clear();

            foreach (char c in RenderableText) {
                if (c.ToString() == " ") {
                    characters.AddLast((TextCharacter) null);
                    continue;
                }
                if (!Alphabet.Instance.HasCharacter(c)) {
                    continue;
                }
                characters.AddLast(Instantiate(Alphabet.Instance.GetPrefabForCharacter(c), transform));
            }
        }
    }
}
