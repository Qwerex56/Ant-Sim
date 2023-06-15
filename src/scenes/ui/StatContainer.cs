using Godot;
using System;

public partial class StatContainer : HBoxContainer {
  [Export] private string statName;
  public string StatName { get { return statName; } set { statName = value; } }
  [Export] private string statValue;
  public string StatValue { get { return statValue; } set { statValue = value; } }

  protected Label statNameLabel;
  protected Label statValueLabel;

  public override void _Ready() {
    statNameLabel = GetNode<Label>("StatNameLabel");
    statValueLabel = GetNode<Label>("StatValueLabel");

    statNameLabel.Text = statName;
    statValueLabel.Text = statValue;
  }
}
