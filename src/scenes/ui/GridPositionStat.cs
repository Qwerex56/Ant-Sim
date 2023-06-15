using Godot;
using System;

public partial class GridPositionStat : StatContainer {
  private void OnAnrGridPositionChanged(Vector2I position) {
    statValueLabel.Text = $"x: {position.X}; y: {position.Y}";
  }
}
