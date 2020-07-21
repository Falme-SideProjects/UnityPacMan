using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] protected ScreenDataScriptableObject screenData;

    protected CharacterMovimentation characterMovimentation;

    protected void Initialize(CharacterMovimentation _characterMovimentation)
    {
        characterMovimentation = _characterMovimentation;

        characterMovimentation.SetInitialPosition(transform.position);
        characterMovimentation.ResetPosition();
    }

    public CharacterMovimentation GetCharacterMovimentation()
    {
        return this.characterMovimentation;
    }

    protected void RefreshCharacter()
    {
        this.transform.position =
            new Vector3(characterMovimentation.GetPosition().x,
                        characterMovimentation.GetPosition().y,
                        transform.position.z);
    }
}
