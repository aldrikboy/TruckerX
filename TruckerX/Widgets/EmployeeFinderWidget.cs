using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Particles;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.State;
using TruckerX.Extensions;
using MonoGame;
using Microsoft.Xna.Framework.Input;

namespace TruckerX.Widgets
{
    class EmployeeFinderWidget : BaseWidget
    {
        public EmployeeState selectedEmployee { get; set; } = null;
        float textBoxHeight = 50;
        private BaseScene scene;
        private Texture2D searchTexture;
        private SmallDetailButtonWidget confirmButton;
        public string text { get; set; } = "";

        bool selected = false;

        public event EventHandler OnEmployeeSelected;

        public EmployeeFinderWidget(BaseScene scene) : base()
        {
            this.scene = scene;
            searchTexture = ContentLoader.GetTexture("search");
            confirmButton = new SmallDetailButtonWidget(scene);
            confirmButton.Text = "Assign";
            confirmButton.OnClick += ConfirmButton_OnClick;
        }

        private void ConfirmButton_OnClick(object sender, EventArgs e)
        {
            OnEmployeeSelected?.Invoke(selectedEmployee, null);
        }

        public void Clear()
        {
            text = "";
            selectedEmployee = null;
        }

        private void DrawTextbox(SpriteBatch batch)
        {
            var font = scene.GetRDFont("main_font_15");
            {
                var str = string.IsNullOrWhiteSpace(text) && !selected ? "E.G. #000001" : text;
                var strSize = font.MeasureString(str);
                float offsetx = this.Position.X + (10 * scene.GetRDMultiplier());
                float offsety = this.Position.Y + (textBoxHeight / 2) - (strSize.Y / 2);
                batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(80, 80, 80, 100));
            }
        }

        private void DrawSelectedEmployeeData(SpriteBatch batch)
        {
            // Eather draw search icon or selected employee.
            if (selectedEmployee == null)
            {
                int iconSize = (int)(this.Size.X / 2.5);
                int iconPosX = (int)(this.Position.X + (this.Size.X / 2) - (iconSize / 2));
                int iconPosY = (int)(this.Position.Y + (this.Size.Y / 2) - (iconSize / 2) + textBoxHeight / 2);
                batch.Draw(searchTexture, new Rectangle(iconPosX, iconPosY, iconSize, iconSize), Color.FromNonPremultiplied(60, 60, 60, 255));
            }
            else
            {
                int pad = 10;
                var font = scene.GetRDFont("main_font_18");
                float textY = 0;
                {
                    var str = selectedEmployee.Name;
                    var strSize = font.MeasureString(str);
                    int offsetx = (int)this.Position.X + pad;
                    int offsety = (int)(this.Position.Y + textBoxHeight + pad);
                    textY += strSize.Y;
                    batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(80, 80, 80, 255));
                }

                font = scene.GetRDFont("main_font_12");
                {
                    var str = selectedEmployee.Id;
                    var strSize = font.MeasureString(str);
                    int offsetx = (int)this.Position.X + pad;
                    int offsety = (int)(this.Position.Y + textBoxHeight + pad + textY);
                    textY += strSize.Y;
                    batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(80, 80, 80, 255));
                }

                font = scene.GetRDFont("main_font_12");
                {
                    var str = "Job: " + selectedEmployee.Job.ToString();
                    int offsetx = (int)this.Position.X + pad;
                    int offsety = (int)(this.Position.Y + textBoxHeight + pad + textY);
                    batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(80, 80, 80, 255));
                }
            }
        }

        internal void SetSelectedEmployee(EmployeeState assignee)
        {
            this.selectedEmployee = assignee;
            this.text = assignee == null ? "" : assignee.Id;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            Primitives2D.FillRectangle(batch, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(230, 230, 230, 255));
            Primitives2D.FillRectangle(batch, new Rectangle(this.Position.ToPoint(), new Point((int)this.Size.X, (int)textBoxHeight)), Color.FromNonPremultiplied(255, 255, 255, 255));
            Primitives2D.DrawRectangle(batch, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(60, 60, 60, 255), 2.0f);
            Primitives2D.DrawRectangle(batch, new Rectangle(this.Position.ToPoint(), new Point((int)this.Size.X, (int)textBoxHeight)), Color.FromNonPremultiplied(60, 60, 60, 255), 2.0f);

            if (selected)
            {
                Primitives2D.FillRectangle(batch, new Rectangle(new Point((int)(this.Position.X), (int)(this.Position.Y + textBoxHeight)), 
                    new Point((int)this.Size.X, 2)), Color.FromNonPremultiplied(255, 173, 123, 255));
            }

            DrawTextbox(batch);
            DrawSelectedEmployeeData(batch);

            confirmButton.Draw(batch, gameTime);
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            this.Size = new Vector2(250, 300) * scene.GetRDMultiplier();
            textBoxHeight = 50 * scene.GetRDMultiplier();

            confirmButton.Position = new Vector2(this.Position.X+1, this.Position.Y + this.Size.Y - confirmButton.Size.Y - (10*scene.GetRDMultiplier()));
            confirmButton.Disabled = selectedEmployee == null;
            confirmButton.Update(scene, gameTime);

            var mouseState = Mouse.GetState();
            if (this.State == WidgetState.MouseDown) selected = true;
            if (!mouseState.Hovering(this) && mouseState.LeftButton == ButtonState.Pressed) selected = false;

            if (selected)
            {
                var keys = Keyboard.GetState().GetPressedKeys();
                foreach(var key in keys)
                {
                    if (key == Keys.Back && KeyboardExtensions.IsKeyPressed(Keys.Back))
                    {
                        if (text.Length != 0) text = text.Remove(text.Length-1, 1);
                    }
                    if (text.Length >= 7) continue;
                    else if (KeyboardExtensions.IsKeyDown(Keys.LeftShift) && key == Keys.D3 && KeyboardExtensions.IsKeyPressed(Keys.D3))
                    {
                        text += "#";
                    }
                    else if (key >= Keys.D0 && key <= Keys.D9 && KeyboardExtensions.IsKeyPressed(key))
                    {
                        text += (char)key;
                    }

                    selectedEmployee = WorldState.GetEmployeeById(text);
                }
            }

            base.Update(scene, gameTime);
        }
    }
}
