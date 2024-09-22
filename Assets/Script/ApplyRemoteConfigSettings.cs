using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Services.RemoteConfig;
using Unity.Services.Core;
using Unity.Services.Authentication;

using System.Threading.Tasks;

using UnityEngine.UI;

// This script handles the set-up and fetching of the Remote Configuration settings
public class ApplyRemoteConfigSettings : MonoBehaviour
{
    public static ApplyRemoteConfigSettings Instance {get; private set;}
  
    // We'll be using these variables throughout the project
    public bool color = false;
    public string _name = "Cube";
    public float characterSize = 1.0f;
    public float characterSpeed = 1.0f;
    public int activeHat = 0;

    // References to Start and Store text components
    public GameObject StartButtonText;
    public GameObject StoreButtonText;


    public Cube cube;

    public struct userAttributes
    {
        // Optionally declare variables for any custom user attributes:
        // These variables can be updated as the game progresses and then used in Campaign Audience Targeting!
        public int score;

        // items inventory[]
        // levelsCompleted
    }

    public struct appAttributes
    {
        // Optionally declare variables for any custom app attributes:

        // i.e
        // public int level;
        // public string appVersion;
    }
    
    //Much like coroutines and the yield statement, async methods 
    //and await methods can be paused (waiting for result from an asynchronous call) and then resumed. 
    //The key difference, however, is async methods can return data.
    async Task InitializeRemoteConfigAsync()
    {
        // initialize handlers for Unity game services
        await UnityServices.InitializeAsync();

        // As of Remote Config 3.0.0 SDK,
        // * Remote Config now requires Authentication for managing environment information and retrieving Settings *
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
    
    // Simple Instance set-up
    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    // Async start() function
    async void Start()
    {
        // call with await keyword our async Task function
        await InitializeRemoteConfigAsync();

        // Set-up for how to send a custom Struct value
        userAttributes uaStruct = new userAttributes();

        // Can also be saved via Cloud Save or a Server 
        uaStruct.score = 10;

        RemoteConfigService.Instance.FetchCompleted += RemoteConfigLoaded;
        // Fetch the Dashboard Remote Config from RemoteConfigManager    
        // We also append the userAttributes and appAttributes struct in our Fetch request
        RemoteConfigService.Instance.FetchConfigs<userAttributes, appAttributes>(uaStruct, new appAttributes(){});

        // Optional Settings
        // Set the user’s unique ID:
        // RemoteConfigService.Instance.SetCustomUserID("some-user-id");

        // Set the environment ID: 
        // Defaults to Production, unless Development Build is Checked
        // RemoteConfigService.Instance.SetEnvironmentID("951304dd-2b96-421c-ace2-a944d56b2948");

        
    }

    // Subscribed event function that provides information on what the ConfigResponse was
    private void RemoteConfigLoaded(ConfigResponse configResponse)
    {
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");

                // Fetch and set Remote Config value for Language
                color = RemoteConfigService.Instance.appConfig.GetBool("color");

                // Call the SetLocalization Function passing in the language string as a parameter
                //SetColor();

                // Assign the rest of the local variables to the Remote Config Setting equivalents

                _name = RemoteConfigService.Instance.appConfig.GetString("name");

                //activeHat = RemoteConfigService.Instance.appConfig.GetInt("ActiveHat");

                characterSize = RemoteConfigService.Instance.appConfig.GetFloat("size");

                //characterSpeed = RemoteConfigService.Instance.appConfig.GetFloat("CharacterSpeed");

                //cube.SetLocalize();
                Debug.Log("Fetch Load");
                // End Case
                break;
        }
    }

    private void OnDestroy()
    {
        RemoteConfigService.Instance.FetchCompleted -= RemoteConfigLoaded;
    }

    // Public function for access outside of the class
    public void FetchConfigs()
    {
        RemoteConfigService.Instance.FetchConfigs<userAttributes, appAttributes>(new userAttributes(){}, new appAttributes(){});

        RemoteConfigService.Instance.FetchCompleted += RemoteConfigLoaded;
    }
    
}

