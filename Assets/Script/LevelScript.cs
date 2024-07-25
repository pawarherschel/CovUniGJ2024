using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
    public sealed class LevelScript : MonoBehaviour
    {
        private int _totalNoOfEnemies;

        public int noOfRemainingEnemies;

        [SerializeField] private string nextScene;
        
        // Start is called before the first frame update
        private void Start()
        {
            _totalNoOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            print("_totalNoOfEnemies " + _totalNoOfEnemies);
            Debug.Assert(_totalNoOfEnemies != 0);

            noOfRemainingEnemies = _totalNoOfEnemies;

            System.Diagnostics.Debug.Assert(nextScene != null, nameof(nextScene) + " != null");
        }

        // Update is called once per frame
        private void Update()
        {
            if (noOfRemainingEnemies <= 0)
            {
                noOfRemainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            }

            if (noOfRemainingEnemies == 0)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}
