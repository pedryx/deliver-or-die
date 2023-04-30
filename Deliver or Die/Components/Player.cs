namespace DeliverOrDie.Components;

/// <summary>
/// Tag used for player controlled entity.
/// </summary>
internal struct Player
{
    /// <summary>
    /// Speed of player movement in pixels per second.
    /// </summary>
    public float MoveSpeed;
    /// <summary>
    /// Determine if player is reloading his gun.
    /// </summary>
    public bool Reloading;
    /// <summary>
    /// Time elapsed from the moment player started reloading his gun in seconds.
    /// </summary>
    public float ReloadingElapsed;
    /// <summary>
    /// How long it takes to reload player's gun in seconds.
    /// </summary>
    public float ReloadTime;
    /// <summary>
    /// System uses this field to store current time per frame of other player animations when replaced by time per
    /// frame for reload animation.
    /// </summary>
    public float AnimationTimePerFrame;
    /// <summary>
    /// Time elapsed from the last moment player shoot from gun.
    /// </summary>
    public float ShootingElapsed;
    /// <summary>
    /// How many bullets player can fire per second.
    /// </summary>
    public float ShootingSpeed;
    /// <summary>
    /// Determine if player is currently shooting.
    /// </summary>
    public bool Shooting;
    public int Ammo;
    public int MaxAmmo;
    public float Damage;
}
