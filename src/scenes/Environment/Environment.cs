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

  public readonly int cellSize = 16;

  public override void _Ready() {
    grid.width = GetTree().Root.GetWindow().Size.X / cellSize;
    grid.height = GetTree().Root.GetWindow().Size.Y / cellSize;
  }

  public void InitializeGame(int ants, int foods) {
    while (ants + foods >= grid.GetGridField()) {
      ants /= 2;
      foods /= 2;
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
    for (int i = 0; i < 1;) {
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
}
