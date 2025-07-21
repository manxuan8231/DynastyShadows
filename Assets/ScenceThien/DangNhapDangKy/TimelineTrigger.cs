using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public PlayableDirector director;
    public TimelineDone timelineDone;

    void Start()
    {
        director.stopped += OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector obj)
    {
        timelineDone.OnTimelineFinished();
    }
}
