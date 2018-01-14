using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.Linq;
using System.IO;

namespace GS
{
    // Main Class Responsible for Every Facebook Method and Work
    public class FBManager : MonoBehaviour
    {
        #region Initialization and Declarations

        public Sprite[] stateSprites;
        [HideInInspector]
        public ToggleState tglStateSlctAll = ToggleState.Unchecked;
        public Button btnSlctAll, btnInviteDialog, btnPostScoreDialog, btnBuyCoinDialog, btnShareGraphDialog;


        public Text playerName, playerScore, countFriends, countInstalledFriends, countCoins;
        public Image playerDp;

        // List of the invite and leaderboard list items
        List<ListItemInvite> listInvites = new List<ListItemInvite>();
        List<ListItemLeaderboard> listLeaderboard = new List<ListItemLeaderboard>();
        List<GameScoreItem> listGameScore = new List<GameScoreItem>();
        List<GameScoreItem> listUserFriends = new List<GameScoreItem>();

        // List containers that list Items - (Dynamically Increasing ListView <Custom>)
        public GameObject inviteParent, leaderParent, allGamesScoresParent, userFriendsParent;

        //Prefabs that holds items that will be places in the containers.
        public ListItemInvite itemInvitePref;
        public ListItemLeaderboard itemLeaderPref;
        public GameScoreItem gameScorePref, userFriendsPrefab;

        List<string> readPermission = new List<string>() { "public_profile", "user_friends", "user_games_activity" },
            publishPermission = new List<string>() { "publish_actions" };

        //strings that let you get JSON from the Facebook API calls.
        string getLeaderboardString = "app/scores",
            getInvitableFriendsString = "me/invitable_friends?limit=150",
            getUserPicString = "me?fields=picture.height(256)",
            getNameString = "me",
            getAllScoresString = "me/scores?fields=application,score",
            getFriendsInfoString = "me/friends";
        public static string appID;

        //public GameObject viewLeaderboardFullScreen, viewInvite;
        public Button btnInit, btnLogin, btnLogout, btnNativeInvite, btnCustomInvite,
            btnLeaderboard, btnGetName, btnSubmitScore, btnName, btnLoadDpFromServer,
            btnSaveDp, btnLoadDpFromLocal, btnGetScore, btnGetAllScores, btnShare, btnTakeScreenshotNShare,
            btnGetFriends, btnGetDeepLink, btnUnlockAchievement, btnCanvasPay, btnShareViaDialog, btnShareViaGraph;

        public Text logTxt;
        delegate void LoadPictureCallback(Texture2D texture, int index);

        public GameObject[] dialogs;
        public GameObject[] loaders;

        public InputField inpInvSearcher, inpLeadSearcher, inpSubmitScore, inpPostGraph;

		public GameObject dialog;

		public Button increase;
		public Text score;
		private int scoreValue = 0;
		public void increaseScore()
		{
			scoreValue++;
			score.text = "" + scoreValue;
		}

        void Awake()
        {
            //print(logTxt.text.Length);
            SetButtonsListeners();
            SetFBItems(false);
            btnLogin.interactable = false;
        }

        void SetButtonsListeners()
        {
            // Main Buttons Listeners
            btnInit.onClick.AddListener(() =>
            {
                InitFB();
            });
            btnLogin.onClick.AddListener(() =>
            {
                LoginFB();
            });
            btnLogout.onClick.AddListener(() =>
            {
                LogoutFB();
            });

            btnNativeInvite.onClick.AddListener(() =>
            {
                NativeInviteFriendsFB();
            });
            btnCustomInvite.onClick.AddListener(() =>
            {
                ShowHideDialog(0, true);
                btnSlctAll.GetComponent<Image>().sprite = stateSprites[0];
                LoadInvitableFriends();
            });
            btnLeaderboard.onClick.AddListener(() =>
            {
                ShowHideDialog(1, true);
                LoadLeaderboard();
            });

            btnSubmitScore.onClick.AddListener(() =>
            {
                ShowHideDialog(2, true);
                inpSubmitScore.text = "";
            });

            btnName.onClick.AddListener(() =>
            {
                LoadPlayerName();
            });
            btnLoadDpFromServer.onClick.AddListener(() =>
            {
                LoadPlayerPic();
            });
            btnSaveDp.onClick.AddListener(() =>
            {
                LoadPlayerPic(true);
            });

            btnLoadDpFromLocal.onClick.AddListener(() =>
            {
                LoadDPifExists();
            });
            btnGetScore.onClick.AddListener(() =>
            {
                GetAppIDNScore();
            });
            btnGetAllScores.onClick.AddListener(() =>
            {
                ShowHideDialog(3, true);
                GetAllScores();
            });

            btnShare.onClick.AddListener(() =>
            {
                ShareOnFB();
            });

            btnGetFriends.onClick.AddListener(() =>
            {
                ShowHideDialog(4, true);
                GetFriendsInfo();
            });

            btnTakeScreenshotNShare.onClick.AddListener(() =>
            {
                TakeScreenshotNShare();
            });
            btnGetDeepLink.onClick.AddListener(() =>
            {
                GetDeepLink();
            });

            btnCanvasPay.onClick.AddListener(() =>
            {
                UpdateCoins();
                ShowHideDialog(5, true);
            });
            btnUnlockAchievement.onClick.AddListener(() =>
            {
                PostAchievement();
            });

            btnShareViaDialog.onClick.AddListener(() =>
            {
                ShareViaDialog();
            });

            btnShareViaGraph.onClick.AddListener(() =>
            {
                ShowHideDialog(6, true);
            });

            //Dialog Buttons Listeners
            btnSlctAll.onClick.AddListener(() =>
            {
                TglSelectAllClickHandler();
            });
            btnInviteDialog.onClick.AddListener(() =>
            {
                SendInvites();
            });
            btnPostScoreDialog.onClick.AddListener(() =>
            {
                PostScore();
            });
            btnBuyCoinDialog.onClick.AddListener(() =>
            {
                BuyCoins();
            });

            btnShareGraphDialog.onClick.AddListener(() =>
            {
                ShareViaGraph();
            });

        }
        void ShowHideDialog(int dialogID, bool state)
        {
            dialogs[dialogID].SetActive(state);
        }

        void ShowHideLoader(int loaderId, bool state)
        {
            loaders[loaderId].SetActive(state);
        }
        public void PrintLog(string msg)
        {
            if (msg.Length > 3000)
            {
                print(msg);
                logTxt.text = "Huge Data, can't be Displayed in Log View! Please see Console!";
            }
            else
            {
                print(msg);
                logTxt.text = msg;
            }
        }

        #endregion

        #region Get and Post Current User Score- User's All games' Score
        public void PostScore()
        {
            /*
            If you don't have facebook publish permission already, Ask for it
            Note! this is not going to work if your publish_actions permission is not approved by facebook.
            Each time you'll post score, It'll prompt user to grant publish_actions unless your app is 
            approved by facebook for publish actions.
            */
            if (!AccessToken.CurrentAccessToken.Permissions.Contains(publishPermission[0]))
            {
                // As A good Practice, You should tell your users that why you need publish permission so
                // show a dialog telling about it. or else simply go to facebook permission prompt.
                //sm.publish_permissionDialog.SetActive(true);
                GetPublishPermission();
            }
            else
            {
                PostOnlyIfPermitted();
            }
        }

        void PostOnlyIfPermitted()
        {
			var scoreData = new Dictionary<string, string>() { { "score", ""+ PlayerPrefs.GetInt ("Score") } };
            FB.API("/me/scores", HttpMethod.POST, delegate (IGraphResult r)
            {
                PrintLog("Result: " + r.RawResult);
				
				//Load scores after posting
				ShowHideDialog(1, true);
				LoadLeaderboard();

            }, scoreData);
        }

        public void GetPublishPermission()
        {
            FB.LogInWithPublishPermissions(publishPermission,
            delegate (ILoginResult loginResult)
            {
                if (AccessToken.CurrentAccessToken.Permissions.Contains(publishPermission[0]))
                {
                    if (string.IsNullOrEmpty(loginResult.Error) && !loginResult.Cancelled)
                    {
                        PostOnlyIfPermitted();
                    }
					else
					{
						dialog.SetActive (false);
						loader.SetActive (false);
						LogoutFB();
					}
                }
                else
                {
                    PrintLog("No publish_actions! permission");
						dialog.SetActive (false);
						loader.SetActive (false);
                }

            });
        }

        void GetAppIDNScore()
        {
            if (string.IsNullOrEmpty(appID))
            {
                FB.API("app", HttpMethod.GET, delegate (IGraphResult result)
                {
                    if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
                    {
                        appID = result.ResultDictionary["id"] as string;
                        PrintLog("Current App Id is " + appID + "Now getting Score !");
                        GetFBScoreInternal();
                    }
                    else
                    {
                        PrintLog("Failed to Get Current App ID! You can try again!");
                    }
                });
            }
            else
            {
                GetFBScoreInternal();
            }

        }

        void GetFBScoreInternal()
        {
            FB.API(getAllScoresString, HttpMethod.GET, delegate (IGraphResult result)
            {
                if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
                {
                    List<object> data = result.ResultDictionary["data"] as List<object>;

                    for (int i = 0; i < data.Count; i++)
                    {
                        Dictionary<string, object> appData = ((Dictionary<string, object>)data[i])["application"]
                        as Dictionary<string, object>;
                        string gameId = Convert.ToString(appData["id"]);

                        if (gameId == appID)
                        {
                            string score = Convert.ToString(((Dictionary<string, object>)data[i])["score"]);
                            playerScore.text = score;
                            PrintLog(string.Format("Current Player Score is {0} For Current AppID {1}", score, appID));
                            break;
                        }
                    }
                }
                else
                {
                    PrintLog("Failed to Get Current App Score! You can try again!");
                }
            });
        }

        void GetAllScores()
        {
            ClearGameScores();
            FB.API(getAllScoresString, HttpMethod.GET, delegate (IGraphResult result)
            {
                if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
                {
                    List<object> data = result.ResultDictionary["data"] as List<object>;

                    for (int i = 0; i < data.Count; i++)
                    {

                        string score = Convert.ToString(((Dictionary<string, object>)data[i])["score"]);
                        Dictionary<string, object> appData = ((Dictionary<string, object>)data[i])["application"] as Dictionary<string, object>;

                        string gameName = Convert.ToString(appData["name"]);

                        GameScoreItem tempItem = Instantiate(gameScorePref) as GameScoreItem;
                        tempItem.AssignValues(gameName, score);
                        tempItem.transform.SetParent(allGamesScoresParent.transform, false);
                        listGameScore.Add(tempItem);
                    }
                }
                ShowHideLoader(2, false);
            });

        }

        #endregion

        #region Get Friends Info
        void GetFriendsInfo()
        {
            ClearUserFriendsData();
            FB.API(getFriendsInfoString, HttpMethod.GET, delegate (IGraphResult result)
            {
                if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
                {
                    List<object> data = result.ResultDictionary["data"] as List<object>;

                    for (int i = 0; i < data.Count; i++)
                    {
                        string fName = Convert.ToString(((Dictionary<string, object>)data[i])["name"]);
                        string fId = Convert.ToString(((Dictionary<string, object>)data[i])["id"]);

                        GameScoreItem tempItem = Instantiate(userFriendsPrefab) as GameScoreItem;
                        tempItem.AssignValues(fName, fId);
                        tempItem.transform.SetParent(userFriendsParent.transform, false);
                        listUserFriends.Add(tempItem);
                    }
                    countInstalledFriends.text = data.Count.ToString();
                    IDictionary summary = result.ResultDictionary["summary"] as IDictionary;
                    countFriends.text = summary["total_count"].ToString();

                    PrintLog(string.Format("Friends Who Installed {0}, Total Friends", data.Count, countFriends.text));
                }
                else
                {
                    PrintLog("Failed to Get Current App Score! You can try again!");
                }

                ShowHideLoader(3, false);
            });
        }
        #endregion

        #region Leaderboard
        //Method to load leaderboard
        public void LoadLeaderboard()
        {
            ClearLeaderboard();
            FB.API(getLeaderboardString, HttpMethod.GET, CallBackLoadLeaderboard);
        }
        //callback of from Facebook API when the leaderboard data from the server is loaded.
        void CallBackLoadLeaderboard(IGraphResult result)
        {
            if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
            {
                //Dictionary<string, object> JSON = Json.Deserialize(result.RawResult) as Dictionary<string, object>;

                List<object> data = result.ResultDictionary["data"] as List<object>;//JSON["data"] as List<object>;
                for (int i = 0; i < data.Count; i++)
                {
                    string fScore;
                    try
                    {
                        fScore = Convert.ToString(((Dictionary<string, object>)data[i])["score"]);
                    }
                    catch (Exception)
                    {
                        fScore = "0";
                    }
                    Dictionary<string, object> UserInfo = ((Dictionary<string, object>)data[i])["user"] as Dictionary<string, object>;
                    string name = Convert.ToString(UserInfo["name"]);
                    string id = Convert.ToString(UserInfo["id"]);
                    CreateListItemLeaderboard(id, name, fScore, i + 1);
                    LoadFriendsAvatar(i);
                }
            }

			loader.SetActive (false);
            PrintLog(result.RawResult);
            inpLeadSearcher.text = "";
        }

        public void OnValueChangeLeaderSeacher()
        {
            string friendsName = inpLeadSearcher.text;

            PrintLog("Friend's Name is " + friendsName);
            if (!string.IsNullOrEmpty(friendsName))
            {
                foreach (var item in listLeaderboard)
                {
                    string currName = item.txtName.text;

                    if (friendsName.Length <= currName.Length &&
                    string.Equals(friendsName, currName.Substring(0, friendsName.Length), StringComparison.OrdinalIgnoreCase))
                    {
                        item.SetObjectState(true);
                    }
                    else
                    {
                        item.SetObjectState(false);
                    }
                }
            }
            else
            {
                PrintLog("No query It's blank !");
                foreach (var item in listLeaderboard)
                {
                    item.SetObjectState(true);
                }
            }
        }
        // Method to load Friends Profile Pictures
        void LoadFriendsAvatar(int index)
        {
            FB.API(Util.GetPictureURL(listLeaderboard[index].fId), HttpMethod.GET, result =>
         {
             if (result.Error != null)
             {
                 PrintLog(result.Error);
                 return;
             }
             listLeaderboard[index].picUrl = Util.DeserializePictureURLString(result.RawResult);
             StartCoroutine(LoadFPicRoutine(listLeaderboard[index].picUrl, PicCallBackLeaderboard, index));
         });
        }

        //Method to all items to the leaderboard dynamically scrollable list
        void CreateListItemLeaderboard(string id, string fName, string fScore = "", int rank = 0)
        {
            ListItemLeaderboard tempItem = Instantiate(itemLeaderPref) as ListItemLeaderboard;

            tempItem.AssignValues(id, fName, fScore, rank.ToString());
            tempItem.transform.SetParent(leaderParent.transform, false);
            listLeaderboard.Add(tempItem);
        }

        private void PicCallBackLeaderboard(Texture2D texture, int index)
        {
            if (texture == null)
            {
                StartCoroutine(LoadFPicRoutine(listLeaderboard[index].picUrl, PicCallBackLeaderboard, index));
                return;
            }
            Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            listLeaderboard[index].imgPic.sprite = sp;
        }

        //Coroutine to load Picture from the specified URL
        IEnumerator LoadFPicRoutine(string url, LoadPictureCallback Callback, int index)
        {
            WWW www = new WWW(url);
            yield return www;
            Callback(www.texture, index);
        }
        #endregion

        #region Custom and Native Invite
        // Method that Proceeds with the Invitable Friends
        //Click Handler of Select All Buttons
        public void TglSelectAllClickHandler()
        {
            switch (tglStateSlctAll)
            {
                case ToggleState.Partial:
                case ToggleState.Unchecked:
                    foreach (var item in listInvites)
                    {
                        item.tglBtn.isOn = true;
                    }
                    tglStateSlctAll = ToggleState.Checked;
                    ChangeToggleState(ToggleState.Checked);
                    break;
                case ToggleState.Checked:
                    foreach (var item in listInvites)
                    {
                        item.tglBtn.isOn = false;
                    }
                    ChangeToggleState(ToggleState.Unchecked);
                    break;
            }
        }
        //Method to change Toggle State On the Fly
        public void ChangeToggleState(ToggleState state)
        {
            switch (state)
            {
                case ToggleState.Unchecked:
                    tglStateSlctAll = state;
                    btnSlctAll.GetComponent<Image>().sprite = stateSprites[0];
                    break;
                case ToggleState.Partial:
                    bool flagOn = false, flagOff = false;
                    foreach (var item in listInvites)
                    {
                        if (item.tglBtn.isOn)
                        {
                            flagOn = true;
                        }
                        else
                        {
                            flagOff = true;
                        }
                    }
                    if (flagOn && flagOff)
                    {
                        tglStateSlctAll = state;
                        btnSlctAll.GetComponent<Image>().sprite = stateSprites[1];
                        //Debug.Log("Partial");
                    }
                    else if (flagOn && !flagOff)
                    {
                        ChangeToggleState(ToggleState.Checked);
                        //Debug.Log("Checked");
                    }
                    else if (!flagOn && flagOff)
                    {
                        ChangeToggleState(ToggleState.Unchecked);
                        //Debug.Log("Unchecked");
                    }
                    break;
                case ToggleState.Checked:
                    tglStateSlctAll = state;
                    btnSlctAll.GetComponent<Image>().sprite = stateSprites[2];
                    break;
            }
        }

        //ClickHandling Method that Sends Backend Facebook Native App request (Invitable)Calls
        void SendInvites()
        {
            List<string> lstToSend = new List<string>();
            foreach (var item in listInvites)
            {
                if (item.tglBtn.isOn)
                {
                    lstToSend.Add(item.fId);
                }
            }
            int dialogCount = (int)Mathf.Ceil(lstToSend.Count / 50f);
            CallInvites(lstToSend, dialogCount);
        }
        //Helping method that will be recursive if you'll have to sent invites to more than 50 Friends.
        private void CallInvites(List<string> lstToSend, int dialogCount)
        {
            if (dialogCount > 0)
            {
                string[] invToSend = (lstToSend.Count >= 50) ? new string[50] : new string[lstToSend.Count];

                for (int i = 0; i < invToSend.Length; i++)
                {
                    try
                    {
                        if (lstToSend[i] != null)
                        {
                            invToSend[i] = lstToSend[i];
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }
                lstToSend.RemoveRange(0, invToSend.Length);
                FB.AppRequest(
                    ConstantData.invMessage, invToSend, null, null, null, null, ConstantData.invTitle,
                    callback: delegate (IAppRequestResult result)
                    {
                        if (--dialogCount > 0)
                        {
                            CallInvites(lstToSend, dialogCount);
                        }
                    }
                );
            }
        }
        void LoadInvitableFriends()
        {
            ClearInvite();
            FB.API(getInvitableFriendsString, HttpMethod.GET, CallBackLoadInvitableFriends);
        }
        //Callback of Invitable Friends API Call
        void CallBackLoadInvitableFriends(IGraphResult result)
        {
            //Deserializing JSON returned from server
            //Dictionary<string, object> JSON = Json.Deserialize(result.RawResult) as Dictionary<string, object>;
            List<object> data = result.ResultDictionary["data"] as List<object>;//JSON["data"] as List<object>;
            //Loop to traverse and process all the items returned from the server.

            for (int i = 0; i < data.Count; i++)
            {
                string id = Convert.ToString(((Dictionary<string, object>)data[i])["id"]);
                string name = Convert.ToString(((Dictionary<string, object>)data[i])["name"]);
                Dictionary<string, object> picInfo = ((Dictionary<string, object>)data[i])["picture"] as Dictionary<string, object>;
                string url = Util.DeserializePictureURLObject(picInfo);
                CreateListItemInvite(id, name, url);
                StartCoroutine(LoadFPicRoutine(url, PicCallBackInvitable, i));
            }
            PrintLog(result.RawResult);
            ShowHideLoader(0, false);
            inpInvSearcher.text = "";
        }
        //Method to add item to the custom invitable dynamically scrollable list
        void CreateListItemInvite(string id, string fName, string url = "")
        {
            ListItemInvite tempItem = Instantiate(itemInvitePref) as ListItemInvite;
            tempItem.AssignValues(id, url, fName);
            tempItem.transform.SetParent(inviteParent.transform, false);
            listInvites.Add(tempItem);
        }
        //Callback of Invitable Friend API call
        void PicCallBackInvitable(Texture2D texture, int index)
        {
            if (texture == null)
            {
                StartCoroutine(LoadFPicRoutine(listInvites[index].picUrl, PicCallBackInvitable, index));
                return;
            }
            listInvites[index].imgPic.sprite = Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
        }
        // Native Invite!
        void NativeInviteFriendsFB()
        {
            FB.AppRequest(
                ConstantData.inviteDialogMsg, null, null, null, null, null, ConstantData.inviteDialogTitle,
                callback: delegate (IAppRequestResult result)
                {
                    PrintLog(result.RawResult);
                });
        }

        public void OnValueChangeInvSearcher()
        {
            string friendsName = inpInvSearcher.text;

            PrintLog("Friend's Name is " + friendsName);
            if (friendsName != null && friendsName.Length != 0)
            {
                //print("There is a query");
                foreach (var item in listInvites)
                {
                    string currName = item.txtName.text;

                    //print("This Item Name is " + currName);
                    if (friendsName.Length <= currName.Length &&
                    string.Equals(friendsName, currName.Substring(0, friendsName.Length), StringComparison.OrdinalIgnoreCase))
                    {
                        item.SetObjectState(true);
                    }
                    else
                    {
                        item.SetObjectState(false);
                    }
                }
            }
            else
            {
                PrintLog("No query It's blank !");
                foreach (var item in listInvites)
                {
                    item.SetObjectState(true);
                }
            }
        }


        #endregion
		public GameObject loader;
        #region FB Init Login and Logout
        public void InitFB()
        {
			loader.SetActive (true);
            if (!FB.IsInitialized)
            {
                FB.Init(InitCallback, onHideUnity);
            }
            else
            {
                FB.ActivateApp();
                PrintLog("Initialized !");
                //btnInit.interactable = false;
                btnLogin.interactable = true;

				LoginFB ();

            }
        }
        // Perform Unity Tasks When App is Connecting To Facebook 
        private void onHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }
        // Method that will Get called After Facebook Initialization Method Call!
        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                //btnInit.interactable = false;
                btnLogin.interactable = true;
                PrintLog("Initialized !");

				//Log in

				LoginFB ();

            }
            else
            {
                PrintLog("Failed to Initialize the Facebook SDK!");
                //InitFB();//Try Again!
				dialog.SetActive (false);
				loader.SetActive (false);
            }
        }

        void LoginFB()
        {
            if (FB.IsLoggedIn)
            {
                SetFBItems(true);
                PrintLog("Logged In !");

				//write score and show leaderboarder
				PostScore();
            }
            else
            {
                FB.ActivateApp();
                FB.LogInWithReadPermissions(readPermission, LoginCallback);
//				FB.LogInWithPublishPermissions (
//					new List<string> (){ "publish_actions" },
//					LoginCallback);
            }
        }

        //Callback method of login
        void LoginCallback(ILoginResult result)
        {
            if (FB.IsLoggedIn)
            {
                // AccessToken class will have session details
                var aToken = AccessToken.CurrentAccessToken;
                foreach (string perm in aToken.Permissions)
                {
                    PrintLog(perm);
                }
                PrintLog("Logged In Successfully!");
                SetFBItems(true);

				//write score and show leaderboarder
				PostScore();
            }
            else
            {
                PrintLog("User cancelled login");
				dialog.SetActive (false);
				loader.SetActive (false);
            }
        }
        public void LogoutFB()
        {
            FB.LogOut();
            PrintLog("Logged Out !");
            SetFBItems(false);
            ClearOldData();
			dialog.SetActive (false);
			loader.SetActive (false);

        }
        void ClearOldData()
        {
            ClearLeaderboard();
            ClearInvite();
            ClearGameScores();
            ClearUserFriendsData();
        }
        void ClearInvite()
        {
            listInvites.Clear();
            ShowHideLoader(0, true);
            for (int i = 0; i < inviteParent.transform.childCount; i++)
            {
                Destroy(inviteParent.transform.GetChild(i).gameObject);
            }
        }
        void ClearLeaderboard()
        {
            listLeaderboard.Clear();
            ShowHideLoader(1, true);
            for (int i = 0; i < leaderParent.transform.childCount; i++)
            {
                Destroy(leaderParent.transform.GetChild(i).gameObject);
            }
        }
        void ClearGameScores()
        {
            listGameScore.Clear();
            ShowHideLoader(2, true);
            for (int i = 0; i < allGamesScoresParent.transform.childCount; i++)
            {
                Destroy(allGamesScoresParent.transform.GetChild(i).gameObject);
            }
        }
        void ClearUserFriendsData()
        {
            listUserFriends.Clear();
            ShowHideLoader(3, true);
            for (int i = 0; i < userFriendsParent.transform.childCount; i++)
            {
                Destroy(userFriendsParent.transform.GetChild(i).gameObject);
            }
        }
        void SetFBItems(bool isLogin)
        {
            btnLogin.interactable = !isLogin;

            btnLogout.interactable = isLogin;
            btnCustomInvite.interactable = isLogin;
            btnSlctAll.interactable = isLogin;
            btnNativeInvite.interactable = isLogin;
            btnCustomInvite.interactable = isLogin;
            btnLeaderboard.interactable = isLogin;
            btnSubmitScore.interactable = isLogin;
            btnName.interactable = isLogin;
            btnLoadDpFromServer.interactable = isLogin;
            btnSaveDp.interactable = isLogin;
            btnLoadDpFromLocal.interactable = isLogin;
            btnShare.interactable = isLogin;
            btnUnlockAchievement.interactable = isLogin;
            btnTakeScreenshotNShare.interactable = isLogin;
            btnGetFriends.interactable = isLogin;
            btnGetScore.interactable = isLogin;
            btnGetAllScores.interactable = isLogin;
            btnCanvasPay.interactable = isLogin;
            btnGetDeepLink.interactable = isLogin;
            btnShareViaDialog.interactable = isLogin;
            btnShareViaGraph.interactable = isLogin;
        }

        #endregion

        #region User FB Name, Picture - Saving for Offline Access
#if !UNITY_WEBGL
        string FILE_NAME = "userpic.jpg";
        string GetPath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
#endif
        void LoadPlayerPic(bool needToSave = false)
        {
            FB.API(getUserPicString, HttpMethod.GET,
                delegate (IGraphResult result)
                {
                    if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
                    {
                        IDictionary picData = result.ResultDictionary["picture"] as IDictionary;
                        IDictionary data = picData["data"] as IDictionary;
                        string picURL = data["url"] as string;
                        StartCoroutine(GetProfilePicRoutine(picURL, needToSave));

                    }
                    PrintLog(result.RawResult);
                });
        }

        void LoadDPifExists()
        {
            if (File.Exists(GetPath(FILE_NAME)))
            {
#if !UNITY_WEBGL
                byte[] fileData = File.ReadAllBytes(GetPath(FILE_NAME));
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                LoadOrSavePicture(tex, true);
                PrintLog("Dp Loaded From Local Storage! Address => " + GetPath(FILE_NAME));
#else
                PrintLog("No Storage on WebGL!");
#endif
            }
            else
            {
                PrintLog("Nothing Stored Locally Yet!");
            }

        }
        void LoadOrSavePicture(Texture2D tex, bool needToSave)
        {
            playerDp.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

#if !UNITY_WEBGL
            if (needToSave)
            {
                byte[] bytes = tex.EncodeToJPG();
                File.WriteAllBytes(GetPath(FILE_NAME), bytes);
            }
#endif
        }

        private IEnumerator GetProfilePicRoutine(string url, bool needToSave = false)
        {
            WWW www = new WWW(url);
            yield return www;
            LoadOrSavePicture(www.texture, needToSave);
        }
        void LoadPlayerName()
        {
            FB.API(getNameString, HttpMethod.GET,
                delegate (IGraphResult result)
                {
                    if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
                    {
                        playerName.text = result.ResultDictionary["name"] as string;
                        PrintLog("Name Loaded !");
                    }
                    else
                    {
                        PrintLog("Failed! Try Again!");
                    }
                });
        }
        #endregion

        #region Screenshot

        void TakeScreenshotNShare()
        {
            StartCoroutine(TakeScreenshot());
        }
        private IEnumerator TakeScreenshot()
        {
            yield return new WaitForEndOfFrame();

            var width = Screen.width;
            var height = Screen.height;
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            // Read screen contents into the texture
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            byte[] screenshot = tex.EncodeToPNG();

            PrintLog("Screenshot Taken! Now Started Posting to Facebook");
            var wwwForm = new WWWForm();
            wwwForm.AddBinaryData("image", screenshot, "Screenshot.png");

            FB.API("me/photos", HttpMethod.POST,
                delegate (IGraphResult result)
                {
                    if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
                    {
                        PrintLog("Post Successfully !" + result.RawResult);
                    }
                    else
                    {
                        PrintLog("Error Occured ! See Details =>" + result.RawResult);
                    }

                }, wwwForm);
        }

        #endregion

        #region Get DeepLink

        void GetDeepLink()
        {
            FB.GetAppLink(
                delegate (IAppLinkResult result)
                {
                    if (!string.IsNullOrEmpty(result.Url))
                    {
                        var index = (new Uri(result.Url)).Query.IndexOf("request_ids");
                        if (index != -1)
                        {
                            // ...have the user interact with the friend who sent the request,
                            // perhaps by showing them the gift they were given, taking them
                            // to their turn in the game with that friend, etc.
                        }
                    }
                    PrintLog(result.RawResult);
                });

        }
        #endregion

        #region Share On Facebook
        public void ShareOnFB()
        {
            if (FB.IsLoggedIn)
            {
                FB.ShareLink(
                    contentURL: ConstantData.fbShareURI,
                    contentTitle: ConstantData.shareDialogTitle,
                    contentDescription: ConstantData.shareDialogMsg,
                    photoURL: ConstantData.fbSharePicURI,
                    callback: delegate (IShareResult result)
                    {
                        if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
                        {
                            PrintLog("Story Posted Successfully!");
                        }
                        else
                        {
                            PrintLog("Error Occured!");
                        }
                    }
                    );
            }
        }

        #endregion

        #region Canvas Payment

        void BuyCoins()
        {
            PaymentFB.BuyCoins(CoinPackage.Hundred);
        }

        public void UpdateCoins()
        {
            countCoins.text = ConstantData.userCoinCount.ToString();
        }

        #endregion

        #region Achievements

        void PostAchievement()
        {
            var data = new Dictionary<string, string>() { { "achievement", ConstantData.achURL } };
            FB.API("me/achievements",
                    HttpMethod.POST,
                    delegate (IGraphResult result)
                    {
                        if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
                        {
                            PrintLog("Success! ");
                        }
                        else
                        {
                            PrintLog(result.RawResult);
                        }

                        FB.ShareLink(
                        contentURL: new Uri(ConstantData.achURL),
                        callback: delegate (IShareResult shareRes)
                        {
                            if (string.IsNullOrEmpty(shareRes.Error) && !shareRes.Cancelled)
                                PrintLog("Achievement Posted Successfully!");
                            else
                                PrintLog("Posting Unsuccessful!");
                        }
                        );

                    },
                    data);
        }
        #endregion

        #region Share Graph API

        void ShareViaDialog()
        {
            // This method is for Life and In Game gifts.
            //FB.AppRequest("Here, take this life!", // A message for the user
            //          OGActionType.SEND, // Can be .Send or .AskFor depending on what you want to do with the object.
            //          "1761710260726741", // Here we put the object id we got as a result before.		             
            //          null, // The id of the sender.
            //          "Life is Good!", // Here you can put in any data you want
            //          "Send a life to your friend", // A title
            //          callback: delegate (IAppRequestResult shareRes)
            //          {
            //              if (string.IsNullOrEmpty(shareRes.Error) && !shareRes.Cancelled)
            //                  PrintLog("Story Posted Successfully!");
            //              else
            //                  PrintLog("Posting Unsuccessful!");
            //          });

            FB.ShareLink(
                    contentURL: new Uri(ConstantData.openGraphObjURL),
                    callback: delegate (IShareResult shareRes)
                    {
                        if (string.IsNullOrEmpty(shareRes.Error) && !shareRes.Cancelled)
                            PrintLog("Story Posted Successfully!");
                        else
                            PrintLog("Posting Unsuccessful!");
                    }
                    );
        }

        void ShareViaGraph()
        {
            var data = new Dictionary<string, string>() {
                { "gsfbtest", ConstantData.openGraphObjURL},
                {"fb:explicitly_shared" , "true" },
                {"message", inpPostGraph.text }
                };
            FB.API("me/gametestfeatures:test",
                    HttpMethod.POST,
                    delegate (IGraphResult result)
                    {
                        if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
                        {
                            PrintLog("Success! Posted");
                        }
                        else
                        {
                            PrintLog("Error!");
                        }
                        PrintLog(result.RawResult);
                    },
                    data);
        }

        #endregion
    }
    public enum ToggleState
    {
        Unchecked,
        Partial,
        Checked
    };
}

