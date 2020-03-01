using Godot;

public abstract class Powerup : RigidBody2D
{
    protected Node2D Parent { get; private set; }

    protected Vector2 screenSize;
    protected bool isDropped = false;

    public override void _Ready()
    {
        this.Hide();
        this.screenSize = GetViewport().Size;
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("Disabled", true);
    }

    public void DropPowerup(Node2D node)
    {
        this.Parent = node;
        this.Show();
        ApplyCentralImpulse(new Vector2(0, 200));
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("Disabled", false);
        this.isDropped = true;
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        if (isDropped)
        {
            var bodies = GetCollidingBodies();
            foreach (Node2D node in bodies)
            {
                if (node is Player || Position.y > screenSize.y)
                {
                    EmitSignal(nameof("AddPowerUp"), this);
                    GetParent().QueueFree();
                }
            }
        }
    }
}


