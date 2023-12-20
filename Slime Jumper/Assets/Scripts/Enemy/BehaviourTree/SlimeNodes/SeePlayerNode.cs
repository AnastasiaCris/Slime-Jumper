using UnityEngine;

public class SeePlayerNode : Node
{
    private SlimeBehaviour _slime;

    public SeePlayerNode(SlimeBehaviour slime)
    {
        this._slime = slime;
    }
    public override NodeState Evaluate()
    {
        if (!SeesPlayer())
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
    /// <summary>
    /// Creates 2 rays that will check for the player in the direction the enemy looks
    /// </summary>
    /// <returns></returns>
    private bool SeesPlayer()
    {
        Vector2 rayPosOnTop = new Vector2(_slime.transform.position.x, _slime.transform.position.y + 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(_slime.transform.position, -_slime.transform.right, 100, layerMask: 1<<7);
        RaycastHit2D hit1 = Physics2D.Raycast(rayPosOnTop, -_slime.transform.right, 100, layerMask: 1<<7);
        Debug.DrawRay(_slime.transform.position, -_slime.transform.right * 24, Color.red);
        Debug.DrawRay(rayPosOnTop, -_slime.transform.right * 24, Color.red);

        return hit.collider != null || hit1.collider != null; //if either hit turn true
    }
    
}
