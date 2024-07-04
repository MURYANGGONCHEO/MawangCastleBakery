using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeStarter : MonoBehaviour
{
    [SerializeField] private EpisodeData _episodeData;

    private void Start ()   
    {
        if (_episodeData.isAlreadyLook) return;

        EpisodeManager.Instanace.StartEpisode(_episodeData);
    }
}