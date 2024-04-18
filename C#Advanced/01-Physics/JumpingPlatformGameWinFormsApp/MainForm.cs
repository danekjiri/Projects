using PhysicsUnitsLib;
using JumpingPlatformGame;

namespace JumpingPlatformGameWinFormsApp {

	// IMPORTANT NOTE: You should NOT change this file as part of your solution! Put you implementation into PhysicsUnitsLib project and Entities.cs.

	public partial class MainForm : Form {
		private const int LabelWidth = 30;
		private const int LabelHeight = 30;

		private List<Entity> _entities = new List<Entity>();
		private List<Label> _entityLabels = new List<Label>();
		private Random _random = new Random();

		public MainForm() {
			InitializeComponent();
		}

		private void RegisterEntity(Entity entity) {
			_entities.Add(entity);

			entity.Location = new WorldPoint {
				X = _random.Next(LabelWidth / 2, worldPanel.Width - LabelWidth / 2).Meters(),
				Y = (LabelWidth / 2).Meters()
			};

			var label = new Label();
			label.AutoSize = false;
			label.Width = LabelWidth;
			label.Height = LabelHeight;
			label.BackColor = entity.Color;
			label.SetLocation(entity, worldPanel.Height);
			_entityLabels.Add(label);
			worldPanel.Controls.Add(label);

			if (entity is MovableEntity movableEntity) {
				movableEntity.Horizontal.LowerBound = (LabelWidth / 2).Meters();
				movableEntity.Horizontal.UpperBound = (worldPanel.Width - LabelWidth / 2).Meters();
				movableEntity.Horizontal.Speed = 160.MeterPerSeconds();
			}

			if (entity is MovableJumpingEntity jumpingEntity) {
				jumpingEntity.Vertical.LowerBound = (LabelHeight / 2).Meters();
				jumpingEntity.Vertical.UpperBound = (worldPanel.Height - LabelHeight / 2).Meters();
				jumpingEntity.Vertical.Speed = (-200).MeterPerSeconds();

				var jumpButton = new Button();
				jumpButton.Text = entity.ToString();
				jumpButton.Tag = entity;
				jumpButton.Click += JumpButton_Click;
				jumpingPanel.Controls.Add(jumpButton);
			}
		}

		private void JumpButton_Click(object? sender, EventArgs e) {
			var entity = (sender as Button)?.Tag as MovableJumpingEntity
				?? throw new ArgumentException($"Must be a Button with Tag set to a {nameof(MovableJumpingEntity)}", nameof(sender));

			entity.Vertical.Speed = Math.Abs(entity.Vertical.Speed.Value).MeterPerSeconds();
		}

		DateTime lastTick = DateTime.Now;

		private void updateTimer_Tick(object sender, EventArgs e) {
			var now = DateTime.Now;
			var deltaSeconds = (now - lastTick).TotalSeconds.Seconds();
			lastTick = now;

			for (int i = 0; i < _entities.Count; i++) {
				_entities[i].Update(deltaSeconds);
				_entityLabels[i].SetLocation(_entities[i], worldPanel.Height);
			}
		}

		private void joeButton_Click(object sender, EventArgs e) => RegisterEntity(new Joe());

		private void janeButton_Click(object sender, EventArgs e) => RegisterEntity(new Jane());

		private void jackButton_Click(object sender, EventArgs e) => RegisterEntity(new Jack());

		private void jillButton_Click(object sender, EventArgs e) => RegisterEntity(new Jill());
	}

	static class ControlExtensions {
		public static void SetLocation(this Control control, Entity entity, int worldHeight) {
			control.Left = (int) entity.Location.X.Value - control.Width / 2;
			control.Top = worldHeight - (int) entity.Location.Y.Value - control.Height / 2;
		}
	}
}
