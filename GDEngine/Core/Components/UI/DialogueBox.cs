using System;
using System.Text;
using GDEngine.Core.Components;
using GDEngine.Core.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Depths_of_Elune.UI
{

    public class DialogueBox : UIRenderer
    {
        #region Fields
        //Different fonts for name and dialogue
        private SpriteFont _nameFont;
        private SpriteFont _dialogueFont;
        private Texture2D _portraitTexture;
        private Rectangle _portraitRect;
        private Dictionary<string, Texture2D> _portraitLookup;
        private bool _isVisible;
        //Initialize with nothing in it
        private string _currentText = "";
        private string _speakerName = "";
        //Box Texture
        private Texture2D _backgroundTexture;
        //Box Size
        private Rectangle _boxSize;
        #endregion

        #region Properties
        //Keep a reference of if it is visible for testing

        public bool isVisible { get; set; } = false;
        #endregion

        #region Constructor
        public DialogueBox(SpriteFont nameFont, SpriteFont dialogueFont, Texture2D dialogueTexture, GraphicsDevice graphicsDevice, Rectangle boxSize, Dictionary<string, Texture2D> portraitLookup)
        {
            _nameFont = nameFont;
            _dialogueFont = dialogueFont;
            _boxSize = boxSize;
            _graphicsDevice = graphicsDevice;
            _backgroundTexture = dialogueTexture;

            _portraitLookup = portraitLookup;

            LayerDepth = UILayer.HUD;
        }
        #endregion

        #region Methods
        //Initialize
        public void Show(string speakerName, string text)
        {
            _speakerName = speakerName;
            _currentText = text;
            _isVisible = true;

            // Auto-assign portrait based on speaker
            if (_portraitLookup != null && _portraitLookup.ContainsKey(speakerName))
            {
                _portraitTexture = _portraitLookup[speakerName];

                int portraitSize = 120;
                _portraitRect = new Rectangle(
                    _boxSize.X + 20,
                    _boxSize.Y + _boxSize.Height - portraitSize - 20,
                    portraitSize,
                    portraitSize
                );
            }
            else
            {
                _portraitTexture = null;
            }
        }

        //To hide everything when not speaking
        public void Hide()
        {
            _isVisible = false;
            _currentText = "";
            _speakerName = "";
        }
        #endregion

        #region Draw
        public override void Draw(GraphicsDevice device, Camera camera)
        {
            if (!_isVisible)
            {
                return;
            }

            if (_spriteBatch == null)
            {
                return;
            }
            //Picture in bottom left
            if (_portraitTexture != null)
            {
                _spriteBatch.Draw(
                    _portraitTexture,
                    _portraitRect,
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    UILayer.HUD
                );
            }

            // Dialogue box
            _spriteBatch.Draw(_backgroundTexture, _boxSize, null, Color.White,
                0f, Vector2.Zero, SpriteEffects.None, UILayer.Background);

            _spriteBatch.DrawString(_nameFont, "Press F to continue", new Vector2(_boxSize.Width-200, _boxSize.Height+470), Color.MediumPurple,
                0f, Vector2.Zero, 1f, SpriteEffects.None, UILayer.HUD);

            // NPC name
            if (!string.IsNullOrEmpty(_speakerName))
            {
                Vector2 namePos = new Vector2(_boxSize.X + 40, _boxSize.Y + 10);
                if(_speakerName.Equals("Celeste"))
                {
                    _spriteBatch.DrawString(_nameFont, _speakerName, namePos, Color.LimeGreen,
                    0f, Vector2.Zero, 1f, SpriteEffects.None, Before(LayerDepth));
                }
                else if(_speakerName.Equals("Khaslana"))
                {
                    _spriteBatch.DrawString(_nameFont, _speakerName, namePos, Color.Gold,
                   0f, Vector2.Zero, 1f, SpriteEffects.None, UILayer.HUD);
                }
                else if(_speakerName.Equals("Elysia"))
                {
                    _spriteBatch.DrawString(_nameFont, _speakerName, namePos, Color.Lavender,
                   0f, Vector2.Zero, 1f, SpriteEffects.None, UILayer.HUD);
                }
            }

            // Dialogue text
            if (!string.IsNullOrEmpty(_currentText))
            {
                Vector2 textPos = new Vector2(_boxSize.X + 170, _boxSize.Y + 60);
                _currentText = WrapText(_dialogueFont, _currentText, _boxSize.Width - 250);
                _spriteBatch.DrawString(_dialogueFont, _currentText, textPos, Color.White,
                    0f, Vector2.Zero, 1f, SpriteEffects.None, UILayer.HUD);
            }
        }

        private string WrapText(SpriteFont font, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            string wrappedText = "";
            string line = "";

            foreach (var word in words)
            {
                // Measure the line if this word is added
                string testLine = (line.Length == 0) ? word : line + " " + word;
                float lineWidth = font.MeasureString(testLine).X;

                if (lineWidth > maxLineWidth)
                {
                    // Commit the current line and start a new one
                    wrappedText += line + "\n";
                    line = word;
                }
                else
                {
                    line = testLine;
                }
            }

            // Add the final line
            wrappedText += line;

            return wrappedText;
        }
        #endregion

    }
}