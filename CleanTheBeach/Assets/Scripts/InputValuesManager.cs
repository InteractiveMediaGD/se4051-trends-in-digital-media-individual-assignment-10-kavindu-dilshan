using UnityEngine;

public static class InputValuesManager
{
    // Static input variables
    public static Vector2 MoveDirection { get; set; }
    public static Vector2 LookRotation { get; set; }
    public static bool IsJumping { get; set; }
    public static bool IsSprinting { get; set; }
}
