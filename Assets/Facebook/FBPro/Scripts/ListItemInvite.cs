using UnityEngine;
using UnityEngine.UI;
namespace GS
{
    //Class that Holds items of our dynamic custom Invitable ListView
    public class ListItemInvite : MonoBehaviour
    {
        public Toggle tglBtn; // toggle button to select item
        public string fId, picUrl; //holds id and pic URL returned from server.
        public Text txtName; //Used to Store and Display Name from the server
        public Image imgPic; // Image View to show image of the specified ID

        void Start()
        {
            tglBtn.GetComponent<Toggle>().onValueChanged.AddListener(ToggleClicked);
        }

        private void ToggleClicked(bool state)
        {
            GetComponentInParent<FBManager>().ChangeToggleState(ToggleState.Partial);
        }
        public void AssignValues(string fId, string picUrl, string txtName)
        {
            this.fId = fId;
            this.picUrl = picUrl;
            this.txtName.text = txtName;
        }

        public void SetObjectState(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}