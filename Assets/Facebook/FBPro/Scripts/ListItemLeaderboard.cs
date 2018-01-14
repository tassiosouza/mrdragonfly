using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace GS
{
    //Class that holds items for Our Custom dynamic Leaderboard ListView
    public class ListItemLeaderboard : MonoBehaviour
    {
        public string fId, picUrl; // holds id and pic URL returned from server.
        public Text txtName, txtScore, txtRank; //Used to Store and Display Name and Score from the server
        public Image imgPic; // Image View to show image of the specified ID
        
        public void AssignValues(string fId, string txtName, string txtScore, string rank)
        {
            this.fId = fId;
            this.txtName.text = txtName;
            this.txtScore.text = txtScore;
            txtRank.text = rank;
        }

        public void SetObjectState(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}