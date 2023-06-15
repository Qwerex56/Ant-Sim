using Godot;
using System;

public partial class SaturationContainer : StatContainer {
  private void OnAntSaturationChanged(int value) {
    statValueLabel.Text = $"val = {value}";
  }
}
