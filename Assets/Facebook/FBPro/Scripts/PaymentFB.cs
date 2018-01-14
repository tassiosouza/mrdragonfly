using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
namespace GS
{
    // enum of our product offerings
    public enum CoinPackage { Hundred = 100, TwoFifty = 250, EightHundred = 800 };
    public class PaymentFB : MonoBehaviour
    {
        // We are using Facebook payment objects with static pricing hosted on the game server
        // See: https://developers.facebook.com/docs/payments/product

        // Note: In this git repo, these objects are located at X
        // Note2: Use the Open Graph Object Debugger to force scrape your open graph objects after updating: https://developers.facebook.com/tools/debug/og/object/

        private static readonly string PaymentObjectURL = "https://www.curioerp.com/plugins/fbpro/payments/{0}.php";
        private static readonly Dictionary<CoinPackage, string> PaymentObjects = new Dictionary<CoinPackage, string>
    {
        { CoinPackage.Hundred, "100coins" },
        { CoinPackage.TwoFifty, "250coins" },
        { CoinPackage.EightHundred, "800coins" }
    };

        // Prompt the user to purchase a virtual item with the Facebook Pay Dialog
        // See: https://developers.facebook.com/docs/payments/reference/paydialog
        public static void BuyCoins(CoinPackage cPackage)
        {
            FBManager fbM = FindObjectOfType<FBManager>();
            // Format payment URL
            string paymentURL = string.Format(PaymentObjectURL, PaymentObjects[cPackage]);

            // https://developers.facebook.com/docs/unity/reference/current/FB.Canvas.Pay
            FB.Canvas.Pay(paymentURL,
                          "purchaseitem",
                          1,
                          null, null, null, null, null,
                          (IPayResult result) =>
                          {
                              fbM.PrintLog("PayCallback");
                              if (result.Error != null)
                              {
                                  Debug.LogError(result.Error);
                                  return;
                              }
                              fbM.PrintLog(result.RawResult);

                              object payIdObj;
                              if (result.ResultDictionary.TryGetValue("payment_id", out payIdObj))
                              {
                                  string payID = payIdObj.ToString();
                                  fbM.PrintLog("Payment complete");
                                  fbM.PrintLog("Payment id:" + payID);

                                  // Verify payment before awarding item
                                  if (VerifyPayment(payID))
                                  {
                                      ConstantData.userCoinCount += (int)cPackage;
                                      fbM.PrintLog("Purchase Complete");
                                      fbM.UpdateCoins();
                                  }
                              }
                              else
                              {
                                  fbM.PrintLog("Payment error");
                              }
                          });
        }

        // Verify payment with Facebook
        // See: https://developers.facebook.com/docs/payments/implementation-guide/order-fulfillment
        //
        // Reminder: It is important to do this payment verification server-to-server
        // See more: https://developers.facebook.com/docs/payments/realtimeupdates
        //
        private static bool VerifyPayment(string paymentID)
        {
            // Payment verification is not implemented in this sample Example
            return true;
        }
    }
}
