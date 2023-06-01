using Godot;
using System;
using System.Collections.Generic;

public partial class AntCommunicationComponent : Node {
  private HashSet<Vector2I> foodPlaces = new();
  public HashSet<Vector2I> FoodPlaces {get { return foodPlaces; } }

  private HashSet<Vector2I> antPositions = new();
  public HashSet<Vector2I> AntPositions {get { return antPositions; } }

  private HashSet<DeathMesasge> deathMassages = new();
  public HashSet<DeathMesasge> DeathMassages {get { return deathMassages; } }

  public enum CommuniqueTypeEnum {
    Food,
    Position,
    Death
  }

  public void PushMassage(CommuniqueTypeEnum type, params object[] args) {
    var argsList = new List<object>(args);

    Vector2I[] positions = (Vector2I[])argsList.Find((obj) => obj.GetType() == typeof(Vector2I[]));
    Vector2I position = (Vector2I)argsList.Find((obj) => obj.GetType() == typeof(Vector2I));
    Node2D ant = (Node2D)argsList.Find((obj) => obj.GetType() == typeof(Node2D));
    int round = (int)argsList.Find((obj) => obj.GetType() == typeof(int));

    switch (type) {
      case CommuniqueTypeEnum.Food:
        PushFood(positions);
      break;
      
      case CommuniqueTypeEnum.Position:
        PushPosition(position);
      break;
      
      case CommuniqueTypeEnum.Death:
        PushDeath(position, round);
      break;
      
      default:
      break; 
    }
  }

  public void PopMassage(CommuniqueTypeEnum type, Vector2I position) {
    switch (type) {
      case CommuniqueTypeEnum.Food:
        foodPlaces.Remove(position);
      break;
      
      case CommuniqueTypeEnum.Position:
        antPositions.Remove(position);
      break;

      case CommuniqueTypeEnum.Death:

      break;
    }
  }

  public void PushFood(Vector2I[] where) {
    foodPlaces.UnionWith(where);
  }

  public void PushPosition(Vector2I where) {
    antPositions.Add(where);
  }

  public void PushDeath(Vector2I where, int when) {
    deathMassages.Add(new(where, when));
  }

  public struct DeathMesasge {
    public DeathMesasge(Vector2I position, int when) {
      place = position;
      round = when;
    }

    Vector2I place;
    int round;
  }
}
