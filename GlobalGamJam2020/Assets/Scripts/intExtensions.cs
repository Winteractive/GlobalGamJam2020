
public static class intExtensions
{
    public static UnitAnimator.Character GetCharacterFromID(this int id)
    {
        return id == 1 ? UnitAnimator.Character.Blue : UnitAnimator.Character.Pink;
    }
}
