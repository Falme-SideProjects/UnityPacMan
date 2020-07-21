using UnityEngine;

public class GhostController : CharacterController
{
    [Header("References")]
    [SerializeField] private GhostAI ghostAI;

    [Header("Data")]
    [SerializeField] private GhostStateDataScriptableObject ghostStateData;
    [SerializeField] private CharacterDataScriptableObject ghostData;

    private bool initialized = false;
    private GhostMovimentation ghostMovimentation;
    private Sprite cachedSprite;
    private SpriteRenderer spriteRenderer;


    private Vector2 cachedGhostPosition;

    private void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.cachedSprite = this.spriteRenderer.sprite;
    }

    void Update()
    {
        if (!initialized) return;

        CheckNearestPosition();
        RefreshCharacter();
    }

    public void InitializeGhost()
    {
        Initialize(new GhostMovimentation(screenData));
        ghostAI.SetGhostCurrentState(GhostState.scatter);
        ghostAI.SetCurrentTarget();

        ghostMovimentation = (GhostMovimentation)characterMovimentation;


        initialized = true;
    }

    private void CheckNearestPosition()
    {
        if (ghostAI.GetScenarioGrid().Count == 0) return;
        
        int _limitY = ghostAI.GetScenarioGrid().Count;
        int _limitX = ghostAI.GetScenarioGrid()[_limitY-1].Count;

        Vector2 startPosition = ghostAI.GetScenarioGrid()[0][0].elementPositionInWorld;
        Vector2 endPosition = ghostAI.GetScenarioGrid()[_limitY-1][_limitX-1].elementPositionInWorld;

        Vector2 _ghostPosition = characterMovimentation.GetPositionInGrid(startPosition, endPosition, new Vector2(_limitX, _limitY));

        CheckNextPosition(_ghostPosition);
    }

    private void CheckNextPosition(Vector2 _ghostPosition)
    {
        if (_ghostPosition != cachedGhostPosition)
        {

            Direction _direction = ghostMovimentation.GetCurrentDirection();
            ghostAI.SetGhostPosition(_ghostPosition);
            ghostAI.SetCurrentTarget();
            ghostMovimentation.SetCurrentDirection(ghostAI.GetNextNearestMove(_ghostPosition, ghostMovimentation.GetCurrentDirection()));
            cachedGhostPosition = _ghostPosition;

            if(_direction != ghostMovimentation.GetCurrentDirection())
                ghostMovimentation.SetPosition(ghostAI.GetScenarioGrid()[(int)_ghostPosition.y][(int)_ghostPosition.x].elementPositionInWorld);
        }

            characterMovimentation.Move(ghostMovimentation.GetCurrentDirection(), Time.deltaTime * ghostData.velocity);
    }

    public void UpdateGhostVisuals()
    {
        for(int a=0; a< ghostStateData.ghostStateInfos.Length; a++)
        {
            if (GetGhostAI().GetGhostCurrentState().Equals(ghostStateData.ghostStateInfos[a].state))
            {
                if (ghostStateData.ghostStateInfos[a].stateSprite == null)
                    this.spriteRenderer.sprite = this.cachedSprite;
                else
                    this.spriteRenderer.sprite = ghostStateData.ghostStateInfos[a].stateSprite;
                
                return;
            }
        }
    }

    public GhostAI GetGhostAI()
    {
        return this.ghostAI;
    }


}
