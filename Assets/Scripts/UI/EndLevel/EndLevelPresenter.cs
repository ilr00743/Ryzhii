using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class EndLevelPresenter : MonoBehaviour
{
    [SerializeField] private EndLevelView _view;
    [SerializeField] private PlayerColliderHandler _playerCollider;
    [SerializeField] private FinishLine _finishLine;
    [SerializeField] private MoneyHolder _playerMoney;
    [SerializeField] private MoneyCollector _moneyCollector;
    private bool _canLoadNextLevel;
    private void Start()
    {
        Advertisement.Initialize("4746828", false);
        Advertisement.Load("Rewarded_Android");
    }
    
    private void OnEnable()
    {
        _view.MultiplyButtonClicked += OnMultiplyButtonClicked;
        _view.SkipButtonClicked += OnSkipButtonClicked;
        _playerCollider.Hit += OnHit;
        _finishLine.Finished += OnFinished;
    }

    private void OnDisable()
    {
        _view.MultiplyButtonClicked -= OnMultiplyButtonClicked;
        _view.SkipButtonClicked -= OnSkipButtonClicked;
        _playerCollider.Hit -= OnHit;
        _finishLine.Finished -= OnFinished;
    }

    private void OnSkipButtonClicked()
    {
        StartCoroutine(LoadLevel(0.2f));
    }

    private void OnMultiplyButtonClicked()
    {
        if (_moneyCollector.CollectedCoin == 0)
        {
            StartCoroutine(LoadLevel(1f));
        }
        else
        {
            Advertisement.Show("Rewarded_Android");
            _playerMoney.AddMoney(_moneyCollector.CollectedCoin);
            StartCoroutine(LoadLevel(2f));
        }
    }

    private void OnHit()
    {
        _canLoadNextLevel = false;
        StartCoroutine(ShowPanel(1.5f));
    }

    private void OnFinished()
    {
        _canLoadNextLevel = true;
        StartCoroutine(ShowPanel(2.5f));
    }

    private IEnumerator ShowPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        _view.gameObject.SetActive(true);
    }

    private IEnumerator LoadLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_canLoadNextLevel == true)
        {
            if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
