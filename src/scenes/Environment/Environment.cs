using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary> Holds information about all food positions, ants positions and takes care for grid initialization </summary>
public partial class Environment : Node {
  private List<Vector2I> foodPositions = new();
  public List<Vector2I> FoodPositions { get { return foodPositions; } }

  private List<Vector2I> antPositons = new();
  public  List<Vector2I> AntPositons { get { return antPositons; } }

  private Grid grid = new();
  public Vector2I GridSize { get { return grid.GetGridSize(); } }

  public readonly int CELL_SIZE = 16;

  // Modifiedable fields in editor
  [Export] private ScrollContainer settingsTab;
  [Export] private PackedScene gridScene;
  [Export] private int foodSpawnRate = 3;

  [Export] private int initialFood = 5;
  [Export] private int initialAnts = 3;

  [Export] public int saturationMax = 15;
  [Export] public int saturationRegain = 1;
  [Export] public int saturationLost = 1;

  public override void _Ready() {
    grid.width = GetTree().Root.GetWindow().Size.X / CELL_SIZE;
    grid.height = GetTree().Root.GetWindow().Size.Y / CELL_SIZE;
  }

  public override void _Input(InputEvent @event) {
    if (@event.IsActionPressed("OpenSettingMenu")) {
      OpenSettings();
    }
    if (@event.IsActionPressed("ResetGame")) {
      var gridNode = GetTree().Root.GetNode<Node2D>("Main").GetChild<grid>(0);

      antPositons = new();
      foodPositions = new();

      if (gridNode != null) {
        gridNode.QueueFree();
      }

      var game = gridScene.Instantiate<grid>();
      game.Name = "Grid";
      GetTree().Root.GetNode<Node2D>("Main").AddChild(game);
    }
  }

  public void InitializeGame() {    
    var ants = initialAnts;
    var foods = initialFood;

    while (ants + foods >= grid.GetGridField()) {
      ants -= 1;
      foods -= 1;
    }

    for (int i = 0; i < ants;) {
      var x = (int) (new RandomNumberGenerator().Randi() % grid.width);
      var y = (int) (new RandomNumberGenerator().Randi() % grid.height);

      if (antPositons.Contains(new(x, y))) continue;
      antPositons.Add(new(x, y));
      i++;
    }

    for (int i = 0; i < foods;) { 
      var x = (int) (new RandomNumberGenerator().Randi() % grid.width);
      var y = (int) (new RandomNumberGenerator().Randi() % grid.height);

      if (antPositons.Contains(new(x, y)) || foodPositions.Contains(new(x, y))) continue;
      foodPositions.Add(new(x, y));
      i++;
    }
  }

  public void SpawnFood() {
    var foodToSpawn = this.foodSpawnRate;

    while (foodPositions.Count + antPositons.Count + foodToSpawn >= grid.GetGridField()) {
      foodToSpawn -= 1;
    }


    for (int i = 0; i < foodToSpawn;) {
      var x = (int) (new RandomNumberGenerator().Randi() % grid.width);
      var y = (int) (new RandomNumberGenerator().Randi() % grid.height);

      if (antPositons.Contains(new(x, y)) || foodPositions.Contains(new(x, y))) continue;
      foodPositions.Add(new(x, y));
      i++;
    }
  }

  private struct Grid {
    public int width;
    public int height;
    public int GetGridField() {
      return width * height;
    }

    public Vector2I GetGridSize() {
      return new(width, height);
    }
  }

  private void OpenSettings() {
    if (settingsTab == null) return;

    settingsTab.Visible = !settingsTab.Visible;
  }

  private void OnInitialFoodValueChanged(int value) { initialFood = value; }
  private void OnInitialAntsValueChanged(int value) { initialAnts = value; }
  private void OnSaturationMaxValueChanged(int value) { saturationMax = value; }
  private void OnSaturationLostValueChanged(int value) { saturationLost = value; }
  private void OnSaturationRegainValueChanged(int value) { saturationRegain = value; }
  private void OnFoodRateValueChanged(int value) { foodSpawnRate = value; }
}
