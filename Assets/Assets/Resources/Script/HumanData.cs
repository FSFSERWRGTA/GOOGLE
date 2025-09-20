using UnityEngine;

public class HumanData : MonoBehaviour
{
   
    [Header("인간")]
    public Human human;
    public Chapter chepter;
    
    public enum Chapter
    {
        chapter1,
        chapter2,
    }

    public enum Human
    {
        JuIna,
        humen2,
        humen3,
        ghost
    }
}