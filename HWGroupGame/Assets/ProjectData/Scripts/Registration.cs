using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Registration : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputField _userNameField;
    [SerializeField] private InputField _userEmailField;
    [SerializeField] private InputField _userPasswordField;
    [SerializeField] private Button _registrationButton;
    [SerializeField] private Text _errorText;

    [Header("UI")]    
    [SerializeField] private GameObject _createAcc;
    [SerializeField] private GameObject _signIn;

    private string _userName;
    private string _userEmail;
    private string _userPassword;

    private void Awake()
    {
        _userNameField.onValueChanged.AddListener(SetUserName); 
        _userEmailField.onValueChanged.AddListener(SetUserEmail);
        _userPasswordField.onValueChanged.AddListener(SetUserPassword);
        _registrationButton.onClick.AddListener(Submit);
    }

    private void SetUserName(string value)
    {
        _userName = value;
    }

    private void SetUserEmail(string value)
    {
        _userEmail = value;
    }

    private void SetUserPassword(string value)
    {
        _userPassword = value;
    }

    private void Submit()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _userName,
            Email = _userEmail,
            Password = _userPassword,
            RequireBothUsernameAndEmail = true
        }, result =>
        {
            _errorText.gameObject.SetActive(false);
            Debug.Log($"User registrated: {result.Username}");
            _createAcc.SetActive(false);
            _signIn.SetActive(true);
        }, error =>
        {
            _errorText.gameObject.SetActive(true);
            _errorText.text = error.ErrorMessage;
            Debug.LogError(error);
        });
    }
}